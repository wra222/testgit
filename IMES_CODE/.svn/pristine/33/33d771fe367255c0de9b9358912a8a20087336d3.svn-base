using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.PCBVersion;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Util;
using System.Data;
using IMES.Infrastructure.UnitOfWork;
using System.Reflection;
using IMES.Infrastructure.Repository._Metas;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
using IMES.DataModel;

namespace IMES.Infrastructure.Repository.PCA
{
    public class PCBVersionRepository : BaseRepository<PCBVersion>, IPCBVersionRepository
    {
        #region Overrides of BaseRepository<PCBVersion>

        protected override void PersistNewItem(PCBVersion item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                switch (tracker.GetState(item))
                {
                    case DataRowState.Added:
                        if (this.CheckExist(item))
                        {
                            this.PersistUpdatePCBVersion(item);
                        }
                        else
                        {
                             this.PersistInsertPCBVersion(item);
                        }
                        break;
                    case DataRowState.Modified:
                        if (this.CheckExist(item))
                        {
                            this.PersistUpdatePCBVersion(item);
                        }
                        else
                        {
                             this.PersistInsertPCBVersion(item);
                        }
                        break;                   
                }
               
            }
            finally
            {
                tracker.Clear();
            }

        }

        protected override void PersistUpdatedItem(PCBVersion item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                switch (tracker.GetState(item))
                {
                    case DataRowState.Added:
                        if (this.CheckExist(item))
                        {
                            this.PersistUpdatePCBVersion(item);
                        }
                        else
                        {
                            this.PersistInsertPCBVersion(item);
                        }
                        break;
                    case DataRowState.Modified:
                        if (this.CheckExist(item))
                        {
                            this.PersistUpdatePCBVersion(item);
                        }
                        else
                        {
                            this.PersistInsertPCBVersion(item);
                        }
                        break;
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(PCBVersion item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                switch (tracker.GetState(item))
                {
                    case DataRowState.Deleted:
                    case DataRowState.Detached:
                            this.PersistDeletePCBVersion(item);
                            break;
                }                
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<PCBVersion>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override PCBVersion Find(object key)
        {
            try
            {
                PCBVersion ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select Family, MBCode, PCBVer, CTVer, Supplier, 
                                                                       Remark, Editor, Cdt, Udt
                                                                 from    PCBVersion
                                                                 where   Family=@Family and
                                                                         MBCode =@MBCode and
                                                                         PCBVer = @PCBVer";
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("MBCode", new SqlParameter("@MBCode", SqlDbType.VarChar));
                        sqlCtx.AddParam("PCBVer", new SqlParameter("@PCBVer", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                string[] keys = (string[])key;
                sqlCtx.Param("Family").Value = keys[0];
                sqlCtx.Param("MBCode").Value = keys[1];
                sqlCtx.Param("PCBVer").Value = keys[2];

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = SQLData.ToObject<PCBVersion>(sqlR);
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
        public override IList<PCBVersion> FindAll()
        {
            try
            {
                IList<PCBVersion> ret = new List<PCBVersion>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select Family, MBCode, PCBVer, CTVer, Supplier, 
                                                                       Remark, Editor, Cdt, Udt
                                                            from    PCBVersion
                                                           order by Family, MBCode, PCBVer";

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
                        PCBVersion item = SQLData.ToObject<PCBVersion>(sqlR);
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
        public override void Add(PCBVersion item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(PCBVersion item, IUnitOfWork uow)
        {
            base.Remove(item, uow); 
            //throw new Exception("Not Allow Delete Data");
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(PCBVersion item, IUnitOfWork uow)
        {
            base.Update(item, uow);

        }

        #endregion

        #region . Inners .

        private void PersistInsertPCBVersion(PCBVersion item)
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
                        sqlCtx.Sentence = @"insert PCBVersion(Family, MBCode, PCBVer, CTVer, Supplier, 
                                                                                          Remark, Editor, Cdt, Udt)
                                                                        values(@Family, @MBCode, @PCBVer, @CTVer, @Supplier, 
                                                                               @Remark, @Editor, GETDATE(), GETDATE())";
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("MBCode", new SqlParameter("@MBCode", SqlDbType.VarChar));
                        sqlCtx.AddParam("PCBVer", new SqlParameter("@PCBVer", SqlDbType.VarChar));
                        sqlCtx.AddParam("CTVer", new SqlParameter("@CTVer", SqlDbType.VarChar));
                        sqlCtx.AddParam("Supplier", new SqlParameter("@Supplier", SqlDbType.VarChar));

                        sqlCtx.AddParam("Remark", new SqlParameter("@Remark", SqlDbType.VarChar));
                        
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Family").Value = item.Family;
                sqlCtx.Param("MBCode").Value = item.MBCode;
                sqlCtx.Param("PCBVer").Value = item.PCBVer;
                sqlCtx.Param("CTVer").Value = item.CTVer;
                sqlCtx.Param("Supplier").Value = item.Supplier;

                sqlCtx.Param("Remark").Value = item.Remark;
               

                sqlCtx.Param("Editor").Value = item.Editor;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA,
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

        private void PersistUpdatePCBVersion(PCBVersion item)
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
                        sqlCtx.Sentence = @"update  PCBVersion
                                                                   set  CTVer=@CTVer,
                                                                          Supplier=@Supplier,
                                                                          Remark=@Remark,
                                                                          Editor = @Editor,
                                                                          Udt = GETDATE()
                                                                where   Family=@Family and
                                                                             MBCode =@MBCode and
                                                                             PCBVer = @PCBVer ";
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("MBCode", new SqlParameter("@MBCode", SqlDbType.VarChar));
                        sqlCtx.AddParam("PCBVer", new SqlParameter("@PCBVer", SqlDbType.VarChar));
                        sqlCtx.AddParam("CTVer", new SqlParameter("@CTVer", SqlDbType.VarChar));
                        sqlCtx.AddParam("Supplier", new SqlParameter("@Supplier", SqlDbType.VarChar));

                        sqlCtx.AddParam("Remark", new SqlParameter("@Remark", SqlDbType.VarChar));

                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Family").Value = item.Family;
                sqlCtx.Param("MBCode").Value = item.MBCode;
                sqlCtx.Param("PCBVer").Value = item.PCBVer;
                sqlCtx.Param("CTVer").Value = item.CTVer;
                sqlCtx.Param("Supplier").Value = item.Supplier;

                sqlCtx.Param("Remark").Value = item.Remark;


                sqlCtx.Param("Editor").Value = item.Editor;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA,
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

        private void PersistDeletePCBVersion(PCBVersion item)
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
                        sqlCtx.Sentence = @"Delete PCBVersion
                                                           where   Family=@Family and
                                                                         MBCode =@MBCode and
                                                                         PCBVer = @PCBVer";
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("MBCode", new SqlParameter("@MBCode", SqlDbType.VarChar));
                        sqlCtx.AddParam("PCBVer", new SqlParameter("@PCBVer", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Family").Value = item.Family;
                sqlCtx.Param("MBCode").Value = item.MBCode;
                sqlCtx.Param("PCBVer").Value = item.PCBVer;


                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA,
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

        private bool CheckExist(PCBVersion item)
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
                        sqlCtx.Sentence = @"select Family
                                                                from PCBVersion
                                                               where   Family=@Family and
                                                                            MBCode =@MBCode and
                                                                            PCBVer = @PCBVer";
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("MBCode", new SqlParameter("@MBCode", SqlDbType.VarChar));
                        sqlCtx.AddParam("PCBVer", new SqlParameter("@PCBVer", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Family").Value = item.Family;
                sqlCtx.Param("MBCode").Value = item.MBCode;
                sqlCtx.Param("PCBVer").Value = item.PCBVer;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
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

        #endregion


        #region implement IPCBVersionRepository
        public IList<PCBVersion> GetPCBVersion(string family)
        {
            try
            {
                IList<PCBVersion> ret = new  List<PCBVersion>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select Family, MBCode, PCBVer, CTVer, Supplier, 
                                                                       Remark, Editor, Cdt, Udt
                                                                 from    PCBVersion
                                                                 where   Family=@Family";
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                       // sqlCtx.AddParam("MBCode", new SqlParameter("@MBCode", SqlDbType.VarChar));
                        //sqlCtx.AddParam("PCBVer", new SqlParameter("@PCBVer", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
               
                sqlCtx.Param("Family").Value = family;
               // sqlCtx.Param("MBCode").Value = keys[1];
              //  sqlCtx.Param("PCBVer").Value = keys[2];

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PCBVersion item = SQLData.ToObject<PCBVersion>(sqlR);
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
        public IList<PCBVersion> GetPCBVersion(string family, string mbCode)
        {
            try
            {
                IList<PCBVersion> ret = new List<PCBVersion>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select Family, MBCode, PCBVer, CTVer, Supplier, 
                                                                       Remark, Editor, Cdt, Udt
                                                                 from    PCBVersion
                                                                 where   Family=@Family and
                                                                              MBCode=@MBCode";
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("MBCode", new SqlParameter("@MBCode", SqlDbType.VarChar));
                        //sqlCtx.AddParam("PCBVer", new SqlParameter("@PCBVer", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Family").Value = family;
                sqlCtx.Param("MBCode").Value = mbCode;
                //  sqlCtx.Param("PCBVer").Value = keys[2];

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PCBVersion item = SQLData.ToObject<PCBVersion>(sqlR);
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
        #endregion

        
    }
}
