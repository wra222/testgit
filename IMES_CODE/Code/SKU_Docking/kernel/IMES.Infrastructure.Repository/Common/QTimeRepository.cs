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
using IMES.FisObject.Common.QTime;
using IMES.Infrastructure.Repository._Metas;
using IMES.DataModel;


namespace IMES.Infrastructure.Repository.Common
{
    class QTimeRepository : BaseRepository<QTime>, IQTimeRepository
    {
        #region Overrides of BaseRepository<QTime>

        protected override void PersistNewItem(QTime item)
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

        protected override void PersistUpdatedItem(QTime item)
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

        protected override void PersistDeletedItem(QTime item)
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

        #region Implementation of IRepository<QTime>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override QTime Find(object key)
        {
            try
            {
                QTime ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select Line, Station, Family, Category, TimeOut, StopTime, 
                                                                       DefectCode, HoldStation, HoldStatus,ExceptStation, Editor, Cdt, 
                                                                       Udt 
                                                                from QTime
                                                                where Line=@Line and
                                                                           Station=@Station and
                                                                           Family =@Family";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                string[] inData = (string[])key;
                sqlCtx.Param("Line").Value = inData[0];
                sqlCtx.Param("Station").Value = inData[1];
                sqlCtx.Param("Family").Value = inData[2];


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = SQLData.ToObjectWithAttribute<QTime>(sqlR);
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
        public override IList<QTime> FindAll()
        {
            try
            {
                IList<QTime> ret = new List<QTime>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select Line, Station,Family, Category, TimeOut, StopTime, 
                                                                       DefectCode, HoldStation, HoldStatus, ExceptStation, Editor, Cdt, 
                                                                       Udt
                                                                from QTime";
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
                        QTime item = SQLData.ToObjectWithAttribute<QTime>(sqlR);
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
        public override void Add(QTime item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(QTime item, IUnitOfWork uow)
        {
            base.Remove(item, uow);

        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(QTime item, IUnitOfWork uow)
        {
            base.Update(item, uow);

        }



        #endregion

        #region . Inners .

        private void PersistInsert(QTime item)
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
                        sqlCtx.Sentence = @"insert QTime(Line, Station,Family,Category, TimeOut, StopTime, 
                                                                                   DefectCode, HoldStation, HoldStatus, ExceptStation, Editor, Cdt, 
                                                                                   Udt)
                                                                            Values(@Line, @Station,@Family, @Category, @TimeOut, @StopTime, 
                                                                                   @DefectCode, @HoldStation, @HoldStatus, @ExceptStation,@Editor, getdate(), 
                                                                                   getdate())  ";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("Category", new SqlParameter("@Category", SqlDbType.VarChar));
                        sqlCtx.AddParam("TimeOut", new SqlParameter("@TimeOut", SqlDbType.Int));

                        sqlCtx.AddParam("StopTime", new SqlParameter("@StopTime", SqlDbType.Int));
                        sqlCtx.AddParam("DefectCode", new SqlParameter("@DefectCode", SqlDbType.VarChar));
                        sqlCtx.AddParam("HoldStation", new SqlParameter("@HoldStation", SqlDbType.VarChar));
                        sqlCtx.AddParam("HoldStatus", new SqlParameter("@HoldStatus", SqlDbType.Int));
                        sqlCtx.AddParam("ExceptStation", new SqlParameter("@ExceptStation", SqlDbType.VarChar));

                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Line").Value = item.Line;
                sqlCtx.Param("Station").Value = item.Station;
                sqlCtx.Param("Family").Value = item.Family;
                sqlCtx.Param("Category").Value = item.Category.ToString();
                sqlCtx.Param("TimeOut").Value = item.TimeOut;

                sqlCtx.Param("StopTime").Value = item.StopTime;
                sqlCtx.Param("DefectCode").Value = item.DefectCode;
                sqlCtx.Param("HoldStation").Value = item.HoldStation;
                sqlCtx.Param("HoldStatus").Value = (int) item.HoldStatus;
                sqlCtx.Param("ExceptStation").Value = item.ExceptStation;

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



        private void PersistUpdate(QTime item)
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
                        sqlCtx.Sentence = @"update  QTime
                                                             set  Category=@Category, TimeOut =@TimeOut, StopTime=@StopTime, DefectCode=@DefectCode, 
                                                                     HoldStation=@HoldStation, HoldStatus=@HoldStatus, ExceptStation=@ExceptStation,Editor=@Editor,  
                                                                     Udt=getdate()
                                                             where Line=@Line and
                                                                        Station=@Station and
                                                                        Family=@Family";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("Category", new SqlParameter("@Category", SqlDbType.VarChar));
                        sqlCtx.AddParam("TimeOut", new SqlParameter("@TimeOut", SqlDbType.Int));

                        sqlCtx.AddParam("StopTime", new SqlParameter("@StopTime", SqlDbType.Int));
                        sqlCtx.AddParam("DefectCode", new SqlParameter("@DefectCode", SqlDbType.VarChar));
                        sqlCtx.AddParam("HoldStation", new SqlParameter("@HoldStation", SqlDbType.VarChar));
                        sqlCtx.AddParam("HoldStatus", new SqlParameter("@HoldStatus", SqlDbType.Int));
                        sqlCtx.AddParam("ExceptStation", new SqlParameter("@ExceptStation", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Line").Value = item.Line;
                sqlCtx.Param("Station").Value = item.Station;
                sqlCtx.Param("Family").Value = item.Family;
                sqlCtx.Param("Category").Value = item.Category.ToString();
                sqlCtx.Param("TimeOut").Value = item.TimeOut;

                sqlCtx.Param("StopTime").Value = item.StopTime;
                sqlCtx.Param("DefectCode").Value = item.DefectCode;
                sqlCtx.Param("HoldStation").Value = item.HoldStation;
                sqlCtx.Param("HoldStatus").Value = (int)item.HoldStatus;
                sqlCtx.Param("ExceptStation").Value = item.ExceptStation;

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


        private void PersistRemove(QTime item)
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
                        sqlCtx.Sentence = @"delete from  QTime
                                                             where Line=@Line and
                                                                        Station=@Station and
                                                                        Family=@Family";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Line").Value = item.Line;
                sqlCtx.Param("Station").Value = item.Station;
                sqlCtx.Param("Family").Value = item.Family;

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


        #region implementation IQTimeRepository
        public IList<QTime> GetQTime(string line, string station)
        {
            try
            {
                IList<QTime> ret = new List<QTime>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select Line, Station, Family, Category, TimeOut, StopTime, 
                                                                       DefectCode, HoldStation, HoldStatus, ExceptStation,Editor, Cdt, 
                                                                       Udt 
                                                                from QTime
                                                                where Line=@Line and
                                                                           Station=@Station 
                                                                 order by Family desc";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));                     
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
               
                sqlCtx.Param("Line").Value = line;
                sqlCtx.Param("Station").Value = station;
                


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                         QTime item = SQLData.ToObjectWithAttribute<QTime>(sqlR);
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


        public QTime GetPriorityQTime(string line, string station,
                                                string productId, string model, string family)
        {
            try
            {
                QTime ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select top 1 Line, Station, Family, Category, TimeOut, StopTime, 
                                                                       DefectCode, HoldStation, HoldStatus,  ExceptStation, Editor, Cdt, 
                                                                       Udt, (case when  Family =@ProductID then 1
                                                                                        when  Family =@Model then 2
                                                                                        when  Family =@Family then 3
                                                                                         else 4
                                                                                end) as Priority           
                                                                from QTime
                                                                where Line=@Line and
                                                                           Station=@Station and
                                                                           (Family = @ProductID or
                                                                            Family = @Model or
                                                                            Family = @Family or
                                                                            Family = '')               
                                                                 order by Priority";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("ProductID", new SqlParameter("@ProductID", SqlDbType.VarChar));
                        sqlCtx.AddParam("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Line").Value = line;
                sqlCtx.Param("Station").Value = station;
                sqlCtx.Param("ProductID").Value = productId;
                sqlCtx.Param("Model").Value = model;
                sqlCtx.Param("Family").Value = family;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = SQLData.ToObjectWithAttribute<QTime>(sqlR);
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

        public IList<QTime> GetQTimeByLine(string line)
        {
            try
            {
                IList<QTime> ret = new List<QTime>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select Line, Station, Family, Category, TimeOut, StopTime, 
                                                                       DefectCode, HoldStation, HoldStatus, ExceptStation, Editor, Cdt, 
                                                                       Udt 
                                                                from QTime
                                                                where Line=@Line
                                                                 order by Station,Family desc";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Line").Value = line;
                //sqlCtx.Param("Station").Value = station;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        QTime item = SQLData.ToObjectWithAttribute<QTime>(sqlR);
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
        public IList<QTime> GetQTimeByStation(string station)
        {
            try
            {
                IList<QTime> ret = new List<QTime>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select Line, Station, Family, Category, TimeOut, StopTime, 
                                                                       DefectCode, HoldStation, HoldStatus, ExceptStation, Editor, Cdt, 
                                                                       Udt 
                                                                from QTime
                                                                where Station=@Station
                                                              order by Line,Family desc";
                        //sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                //sqlCtx.Param("Line").Value = line;
                sqlCtx.Param("Station").Value = station;



                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        QTime item = SQLData.ToObjectWithAttribute<QTime>(sqlR);
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


        public void RemoveQTime(string line, string station, string family)
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
                        sqlCtx.Sentence = @"delete from  QTime
                                                             where Line= @Line and
                                                                        Station= @Station and
                                                                        Family= @Family ";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Line").Value = line;
                sqlCtx.Param("Station").Value = station;
                sqlCtx.Param("Family").Value = family;

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

        public void RemoveQTimeDefered(IUnitOfWork uow, string line, string station, string family)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), line, station, family);
        }

        public LineStationLastProcessTime GetLastProcessTime(string line, string station)
        {
            try
            {
                LineStationLastProcessTime ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select Line, Station, ProductID, ProcessTime, Editor, 
                                                                        GetDate() as Now, 
                                                                        Isnull(DATEDIFF(Second, ProcessTime, GetDate()),0) as SpeedTime
                                                             from  LineStationLastProcessTime
                                                              where Line=@Line and
                                                                         Station=@Station ";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Line").Value = line;
                sqlCtx.Param("Station").Value = station;
                
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if  (sqlR != null && sqlR.Read())
                    {
                        ret = SQLData.ToObject<LineStationLastProcessTime>(sqlR);                       
                    }
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void UpdateLineStationLastProcessTime(LineStationLastProcessTime time)
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
                        sqlCtx.Sentence = @"merge  LineStationLastProcessTime as T
                                                             using (select @Line as Line,
			                                                               @Station as Station,
			                                                               @ProductID as ProductID,
			                                                               @ProcessTime as ProcessTime,
			                                                               @Editor as Editor) as S
                                                             on (T.Line = S.Line and T.Station=S.Station)
                                                             When MATCHED  Then
                                                                  Update 
                                                                  Set ProductID= S.ProductID,
                                                                      ProcessTime = S.ProcessTime,
                                                                      Editor = S.Editor
                                                              When NOT MATCHED  Then
                                                                  Insert (Line, Station, ProductID, ProcessTime, Editor)
                                                                  values(S.Line, S.Station, S.ProductID, S.ProcessTime, S.Editor);";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("ProductID", new SqlParameter("@ProductID", SqlDbType.VarChar));
                        sqlCtx.AddParam("ProcessTime", new SqlParameter("@ProcessTime", SqlDbType.DateTime));                        
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Line").Value = time.Line;
                sqlCtx.Param("Station").Value = time.Station;
                sqlCtx.Param("ProductID").Value = time.ProductID;
                sqlCtx.Param("ProcessTime").Value = time.ProcessTime;

                sqlCtx.Param("Editor").Value = time.Editor;

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
        public void UpdateLineStationLastProcessTimeDefered(IUnitOfWork uow, LineStationLastProcessTime time)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), time);
        }

        public void AddLineStationStopPeriodLog(LineStationStopPeriodLog log)
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
                        sqlCtx.Sentence = @"insert  LineStationStopPeriodLog(Line, Station, StartTime, EndTime, Editor)
                                                            values(@Line, @Station, @StartTime, @EndTime, @Editor) ";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("StartTime", new SqlParameter("@StartTime", SqlDbType.DateTime));
                        sqlCtx.AddParam("EndTime", new SqlParameter("@EndTime", SqlDbType.DateTime));    
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Line").Value = log.Line;
                sqlCtx.Param("Station").Value = log.Station;
                sqlCtx.Param("StartTime").Value = log.StartTime;
                sqlCtx.Param("EndTime").Value = log.EndTime;
                sqlCtx.Param("Editor").Value = log.Editor;
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

        public void AddLineStationStopPeriodLogDefered(IUnitOfWork uow, LineStationStopPeriodLog log)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), log);
        }

        public void RemoveStationStopPeriodLog(string line, string station, int remainDays)
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
                        sqlCtx.Sentence = @"delete from  LineStationStopPeriodLog
                                                             where Line=@Line and
                                                                        Station=@Station and
                                                                        EndTime <= @EndTime";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("EndTime", new SqlParameter("@EndTime", SqlDbType.DateTime));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Line").Value = line;
                sqlCtx.Param("Station").Value = station;                
                sqlCtx.Param("EndTime").Value =DateTime.Now.AddDays((double)(-remainDays));

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

        public void RemoveStationStopPeriodLogDefered(IUnitOfWork uow, string line, string station, int remainDays)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), line, station,remainDays);
        }

        public IList<int> CalLineStopTime(string line, string station, DateTime startTime, DateTime endTime)
        {
            try
            {
                IList<int> ret = new List<int>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"with cte as (
	                                                                select ID, DATEDIFF(second, StartTime, EndTime) as TotalTime
	                                                                  from LineStationStopPeriodLog
	                                                                where Line=@Line and 
		                                                                  Station =@Station and 
		                                                                  StartTime between  @StartTime and @EndTime and
		                                                                  EndTime between  @StartTime and @EndTime
                                                                    union 
                                                                     select ID, DATEDIFF(second, StartTime, @EndTime) as TotalTime
	                                                                  from LineStationStopPeriodLog
	                                                                where Line=@Line and 
		                                                                  Station =@Station and 
		                                                                  @EndTime between  StartTime and EndTime and
		                                                                  StartTime > @StartTime   
                                                                    union 
                                                                     select ID, DATEDIFF(second, @StartTime, EndTime) as TotalTime
	                                                                  from LineStationStopPeriodLog
	                                                                where Line=@Line and 
		                                                                  Station =@Station and 
		                                                                  @StartTime between  StartTime and EndTime and
		                                                                  @EndTime > EndTime
	                                                                union 
                                                                     select ID, DATEDIFF(second, StartTime, EndTime) as TotalTime
	                                                                  from LineStationStopPeriodLog
	                                                                where Line=@Line and 
		                                                                  Station =@Station and 
		                                                                  @StartTime between  StartTime and EndTime and
		                                                                  @EndTime between  StartTime and EndTime	                                                                		                         
                                                                )
                                                                select isnull(sum(TotalTime),0) as  StopTime, 
                                                                            DATEDIFF(second, @StartTime, @EndTime) as TimeOut
                                                                from cte";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("StartTime", new SqlParameter("@StartTime", SqlDbType.DateTime));
                        sqlCtx.AddParam("EndTime", new SqlParameter("@EndTime", SqlDbType.DateTime));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
               
                sqlCtx.Param("Line").Value = line;
                sqlCtx.Param("Station").Value = station;
                sqlCtx.Param("StartTime").Value = startTime;
                sqlCtx.Param("EndTime").Value = endTime;
                
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret.Add(sqlR.GetInt32(0));
                        ret.Add(sqlR.GetInt32(1));
                    }
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<int> CalLineStopTime(string line, string station, DateTime startTime)
        {
            try
            {
                IList<int> ret = new List<int>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"declare @EndTime datetime=getdate();
                                                            with cte as (
	                                                                select ID, DATEDIFF(second, StartTime, EndTime) as TotalTime
	                                                                  from LineStationStopPeriodLog
	                                                                where Line=@Line and 
		                                                                  Station =@Station and 
		                                                                  StartTime between  @StartTime and @EndTime and
		                                                                  EndTime between  @StartTime and @EndTime
                                                                    union all
                                                                     select ID, DATEDIFF(second, StartTime, @EndTime) as TotalTime
	                                                                  from LineStationStopPeriodLog
	                                                                where Line=@Line and 
		                                                                  Station =@Station and 
		                                                                  @EndTime between  StartTime and EndTime and
		                                                                  StartTime > @StartTime   
                                                                    union all
                                                                     select ID, DATEDIFF(second, @StartTime, EndTime) as TotalTime
	                                                                  from LineStationStopPeriodLog
	                                                                where Line=@Line and 
		                                                                  Station =@Station and 
		                                                                  @StartTime between  StartTime and EndTime and
		                                                                  @EndTime > EndTime
	                                                                union all
                                                                     select ID, DATEDIFF(second, StartTime, EndTime) as TotalTime
	                                                                  from LineStationStopPeriodLog
	                                                                where Line=@Line and 
		                                                                  Station =@Station and 
		                                                                  @StartTime between  StartTime and EndTime and
		                                                                  @EndTime between  StartTime and EndTime	                                                                		                         
                                                                )
                                                                select isnull(sum(TotalTime),0) as  StopTime, 
                                                                            DATEDIFF(second, @StartTime, @EndTime) as TimeOut
                                                                from cte";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("StartTime", new SqlParameter("@StartTime", SqlDbType.DateTime));
                        //sqlCtx.AddParam("EndTime", new SqlParameter("@EndTime", SqlDbType.DateTime));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Line").Value = line;
                sqlCtx.Param("Station").Value = station;
                sqlCtx.Param("StartTime").Value = startTime;
                //sqlCtx.Param("EndTime").Value = endTime;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret.Add( sqlR.GetInt32(0));
                        ret.Add(sqlR.GetInt32(1));
                    }
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IList<LineStopLogInfo> CalLineStopMillionSecond(string line, string station, DateTime startTime, DateTime endTime)
        {
            try
            {
                IList<LineStopLogInfo> ret = new List<LineStopLogInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"with cte as (
	                                                                select ID as ID, StartTime as StartTime, EndTime  as EndTime
	                                                                  from LineStationStopPeriodLog
	                                                                where Line=@Line and 
		                                                                  Station =@Station and 
		                                                                  StartTime between  @StartTime and @EndTime and
		                                                                  EndTime between  @StartTime and @EndTime
                                                                    union 
                                                                     select ID as ID, StartTime as StartTime, @EndTime as EndTime
	                                                                  from LineStationStopPeriodLog
	                                                                where Line=@Line and 
		                                                                  Station =@Station and 
		                                                                  @EndTime between  StartTime and EndTime and
		                                                                  StartTime > @StartTime   
                                                                    union 
                                                                     select ID as ID,  @StartTime as StartTime, EndTime as EndTime
	                                                                  from LineStationStopPeriodLog
	                                                                where Line=@Line and 
		                                                                  Station =@Station and 
		                                                                  @StartTime between  StartTime and EndTime and
		                                                                  @EndTime > EndTime
	                                                                union 
                                                                     select ID as ID, StartTime as StartTime, EndTime as EndTime
	                                                                  from LineStationStopPeriodLog
	                                                                where Line=@Line and 
		                                                                  Station =@Station and 
		                                                                  @StartTime between  StartTime and EndTime and
		                                                                  @EndTime between  StartTime and EndTime	                                                                		                         
                                                                )
                                                                select *
                                                                from cte";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("StartTime", new SqlParameter("@StartTime", SqlDbType.DateTime));
                        sqlCtx.AddParam("EndTime", new SqlParameter("@EndTime", SqlDbType.DateTime));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Line").Value = line;
                sqlCtx.Param("Station").Value = station;
                sqlCtx.Param("StartTime").Value = startTime;
                sqlCtx.Param("EndTime").Value = endTime;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        LineStopLogInfo item = SQLData.ToObject<LineStopLogInfo>(sqlR);
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