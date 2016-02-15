using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Data.SqlClient;
using System.Data;

using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Util;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.Repository._Metas;
using IMES.FisObject.Common.Model;



namespace IMES.Infrastructure.Repository.Common
{
    public class OSWINRepository : BaseRepository<OSWIN>, IOSWINRepository
    {
        #region Overrides of BaseRepository<OSWIN>

        protected override void PersistNewItem(OSWIN item)
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

        protected override void PersistUpdatedItem(OSWIN item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdate(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(OSWIN item)
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
        public override OSWIN Find(object key)
        {
            try
            {
                OSWIN ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, Family, Zmode, OS, AV, 
                                                                        Image, Editor, Cdt, Udt
                                                                from OSWIN
                                                                where ID =@ID";
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
                        ret = SQLData.ToObject<OSWIN>(sqlR);
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
        public override IList<OSWIN> FindAll()
        {
            try
            {
                IList<OSWIN> ret = new List<OSWIN>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, Family, Zmode, OS, AV, 
                                                                        Image, Editor, Cdt, Udt
                                                                from OSWIN
                                                             order by Family, Zmode";
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
                        OSWIN item = SQLData.ToObject<OSWIN>(sqlR);
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
        public override void Add(OSWIN item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(OSWIN item, IUnitOfWork uow)
        {
            base.Remove(item, uow);

        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(OSWIN item, IUnitOfWork uow)
        {
            base.Update(item, uow);

        }



        #endregion

        #region . Inners .

        private void PersistInsert(OSWIN item)
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
                        sqlCtx.Sentence = @"insert OSWIN (Family, Zmode, OS, AV,Image, 
                                                                                      Editor, Cdt, Udt)
                                                           Values(@Family, @Zmode, @OS, @AV,@Image, 
                                                                               @Editor, GETDATE(), getdate())";
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("Zmode", new SqlParameter("@Zmode", SqlDbType.VarChar));
                        sqlCtx.AddParam("OS", new SqlParameter("@OS", SqlDbType.VarChar));
                        sqlCtx.AddParam("AV", new SqlParameter("@AV", SqlDbType.VarChar));
                        sqlCtx.AddParam("Image", new SqlParameter("@Image", SqlDbType.VarChar));
                                                
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Family").Value = item.Family;
                sqlCtx.Param("Zmode").Value = item.Zmode;
                sqlCtx.Param("OS").Value = item.OS;
                sqlCtx.Param("AV").Value = item.AV;
                sqlCtx.Param("Image").Value = item.Image;
                
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

        private void PersistUpdate(OSWIN item)
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
                        sqlCtx.Sentence = @"update OSWIN
                                                            set Family=@Family, Zmode=@Zmode, OS=@OS, AV=@AV,Image=@Image, 
                                                                Editor=@Editor, Udt=getdate()
                                                            where ID=@ID  ";
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("Zmode", new SqlParameter("@Zmode", SqlDbType.VarChar));
                        sqlCtx.AddParam("OS", new SqlParameter("@OS", SqlDbType.VarChar));
                        sqlCtx.AddParam("AV", new SqlParameter("@AV", SqlDbType.VarChar));
                        sqlCtx.AddParam("Image", new SqlParameter("@Image", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        sqlCtx.AddParam("ID", new SqlParameter("@ID", SqlDbType.Int));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Family").Value = item.Family;
                sqlCtx.Param("Zmode").Value = item.Zmode;
                sqlCtx.Param("OS").Value = item.OS;
                sqlCtx.Param("AV").Value = item.AV;
                sqlCtx.Param("Image").Value = item.Image;              
                sqlCtx.Param("Editor").Value = item.Editor;
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

        private void PersistRemove(OSWIN item)
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
                        sqlCtx.Sentence = @"Delete from OSWIN
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

        #region Implement IOSWINRepository
        public IList<OSWIN> GetOSWIN(string family)
        {
            try
            {
                IList<OSWIN> ret = new List<OSWIN>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, Family, Zmode, OS, AV, 
                                                                        Image, Editor, Cdt, Udt
                                                                from OSWIN
                                                             where Family=@Family
                                                             order by Zmode";
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Family").Value = family;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        OSWIN item = SQLData.ToObject<OSWIN>(sqlR);
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

        public OSWIN GetOSWINByZmode(string family, string zmode)
        {
            try
            {
                OSWIN ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, Family, Zmode, OS, AV, 
                                                                        Image, Editor, Cdt, Udt
                                                                from OSWIN
                                                             where Family=@Family and
                                                                        Zmode=@Zmode";
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("Zmode", new SqlParameter("@Zmode", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Family").Value = family;
                sqlCtx.Param("Zmode").Value = zmode;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = SQLData.ToObject<OSWIN>(sqlR);
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
        /// select a.*
        ///     from OSWIN a
        ///     inner join ModelBOM b on  b.Component like '%' +a.Zmode
        ///      where b.Material = @model 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OSWIN GetOSWINByModelBOM(string model)
        {
            try
            {
                OSWIN ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select a.ID, a.Family, a.Zmode, a.OS, a.AV, 
                                                                       a.Image, a.Editor, a.Cdt, a.Udt, 
                                                                        b.Component as BomZmode
                                                               from OSWIN a
                                                              inner join ModelBOM b on  b.Component like '%' +a.Zmode
                                                            where b.Material = @Model";
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
                        ret = SQLData.ToObject<OSWIN>(sqlR);
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

        public IList<string> GetOSWINFamily()
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
                        sqlCtx.Sentence = @"select distinct Family
                                                                from OSWIN                                                            
                                                             order by Family";
                       
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
