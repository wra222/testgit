using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPH.Interface;
using UPH.DB;
using log4net;
using System.Reflection;
using System.Data;
using UPH.Entity.Infrastructure.Framework;
using UPH.Entity.Infrastructure.Interface;
using UPH.Entity.Repository.Meta.IMESSKU;

namespace UPH.Implementation
{
    public class AlarmMaintain:MarshalByRefObject,IAlarmMaintain
    {
        public List<string> GetAlarmline(string Process)
        {
            List<string> list =new List<string>();
            StringBuilder st=new StringBuilder();
            if (Process == "ALL")
            {
                st.AppendLine("SELECT DISTINCT PdLine FROM PdLine");
                DataTable dt = new DataTable();
                string Connection = SQLHelper.ConnectionString_ONLINE(0);
                dt = SQLHelper.ExecuteDataFill(Connection,
                    System.Data.CommandType.Text,
                    st.ToString()
                    );
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString());
                }
                return list;
            }
            else
            {
                st.AppendLine("SELECT DISTINCT PdLine FROM PdLine where Process=@Process");
                DataTable dt = new DataTable();
                string Connection = SQLHelper.ConnectionString_ONLINE(0);
                dt = SQLHelper.ExecuteDataFill(Connection,
                    System.Data.CommandType.Text,
                    st.ToString(),
                    SQLHelper.CreateSqlParameter("@Process", Process.ToString())
                    );
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString());
                }
                return list;
            }
        }
        public List<string> GetAlarmProcess()
        {
            List<string> list = new List<string>();
            StringBuilder st = new StringBuilder();
            st.AppendLine("SELECT DISTINCT Process FROM AlarmStatus");
            DataTable dt = new DataTable();
            string Connection = SQLHelper.ConnectionString_ONLINE(0);
            dt = SQLHelper.ExecuteDataFill(Connection,
                System.Data.CommandType.Text,
                st.ToString()
                );
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr[0].ToString());
            }
            return list;
        }
        //public List<string> GetAlarmAllLine()
        //{
        //    List<string> list = new List<string>();
        //    StringBuilder st = new StringBuilder();
        //    st.AppendLine("SELECT DISTINCT PdLine FROM PdLine order by PdLine");
        //    DataTable dt = new DataTable();
        //    string Connection = SQLHelper.ConnectionString_ONLINE(0);
        //    dt = SQLHelper.ExecuteDataFill(Connection,
        //        System.Data.CommandType.Text,
        //        st.ToString()
        //        );
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        list.Add(dr[0].ToString());
        //    }
        //    return list;
        //}

        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IList<AlarmInfo> GetAllAlarmInfo()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            IList<AlarmInfo> ret = new List<AlarmInfo>();
            try
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IRepository<Alarm> ecoModelRep = new Repository<Alarm>("UPHDBServer");//连接字符串 查询ProductUPH 表的数据
                    ret = (from q in ecoModelRep.Query()
                           select new AlarmInfo
                           {
                               ID = q.ID,
                               process = q.Process,
                               Class = q.Class,
                               PdLine = q.PdLine,
                               BeginTime = q.BeginTime,
                               EndTime = q.EndTime,
                               Status = q.Status,
                               Remark = q.Remark,
                               Editor = q.Editor,
                               Cdt = q.Cdt,
                               Udt = q.Udt
                           }).ToList();
                }

                return ret;

            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        //public IList<AlarmInfo> GetAlarmInfoList(DateTime fromtime, DateTime totime)
        //{
        //    string methodName = MethodBase.GetCurrentMethod().Name;
        //    BaseLog.LoggingBegin(logger, methodName);
        //    IList<AlarmInfo> ret = new List<AlarmInfo>();
        //    try
        //    {
        //        using (UnitOfWork uow = new UnitOfWork())
        //        {
        //            IRepository<Alarm> ecoModelRep = new Repository<Alarm>("UPHDBServer");//连接字符串 查询ProductUPH 表的数据
        //            ret = (from q in ecoModelRep.Query()
        //                   where q.Date >= fromtime
        //                       && q.Date <= totime
        //                   select new AlarmInfo
        //                   {
        //                       ID = q.ID,
        //                       Date = q.Date,
        //                       Line = q.Line,
        //                       Lesson = q.Lesson,
        //                       TimeRange = q.TimeRange,
        //                       Family = q.Family,
        //                       ProductRatio = q.ProductRatio
        //                   }).ToList();
        //        }

        //        return ret;

        //    }
        //    catch (Exception e)
        //    {
        //        BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
        //        throw;
        //    }
        //    finally
        //    {
        //        BaseLog.LoggingEnd(logger, methodName);
        //    }

        //}


        public void AddAlarmInfo(AlarmInfo itemui)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {

                Alarm items = new Alarm
                {
                    Process = itemui.process,
                    Class = itemui.Class,
                    PdLine = itemui.PdLine,
                    BeginTime = itemui.BeginTime,
                    EndTime = itemui.EndTime,
                    Status = itemui.Status,
                    Remark = itemui.Remark,
                    Editor = itemui.Editor,
                    Cdt = itemui.Cdt,
                    Udt = itemui.Udt
                };
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IRepository<Alarm> ecoModelRep = new Repository<Alarm>("UPHDBServer");
                    ecoModelRep.Insert(items);
                    uow.Commit();
                }



            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }

        }


        public void DelAlarmInfo(AlarmInfo itemui)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                Alarm items = new Alarm
                {
                    ID=itemui.ID
                };
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IRepository<Alarm> ecoModelRep = new Repository<Alarm>("UPHDBServer");
                    ecoModelRep.Delete(items, true);//删掉记录 根据设置的条件
                    uow.Commit();
                }

            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        public void UpdateAlarmInfo(AlarmInfo itemui)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                Alarm items = new Alarm
                {
                    Process = itemui.process,
                    Class = itemui.Class,
                    PdLine = itemui.PdLine,
                    BeginTime = itemui.BeginTime,
                    EndTime = itemui.EndTime,
                    Status = itemui.Status,
                    Remark = itemui.Remark,
                    Editor = itemui.Editor,
                    Cdt = itemui.Cdt,
                    Udt = itemui.Udt
                };
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IRepository<Alarm> ecoModelRep = new Repository<Alarm>("UPHDBServer");
                    ecoModelRep.Update(items);
                    uow.Commit();
                }

            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        public void AddAlarmlog(AlarmInfoLog itemui)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                AlarmLog items = new AlarmLog
                {
                    Process = itemui.process,
                    Class = itemui.Class,
                    PdLine = itemui.PdLine,
                    BeginTime = itemui.BeginTime,
                    EndTime = itemui.EndTime,
                    Status = itemui.Status,
                    Remark = itemui.Remark,
                    Editor = itemui.Editor,
                    Cdt = itemui.Cdt,
                };
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IRepository<AlarmLog> ecoModelRep = new Repository<AlarmLog>("UPHDBServer");
                    ecoModelRep.Insert(items);
                    uow.Commit();
                }

            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }
        //public bool CheckDuplicateData(AlarmInfo item)
        //{
        //    bool re = false;
        //    string methodName = MethodBase.GetCurrentMethod().Name;
        //    BaseLog.LoggingBegin(logger, methodName);
        //    IList<AlarmInfo> ret = new List<AlarmInfo>();
        //    try
        //    {
        //        IRepository<Alarm> ecoModelRep = new Repository<Alarm>("UPHDBServer");//连接字符串 查询ProductUPH 表的数据
        //        ret = (from q in ecoModelRep.Query()
        //               where q.Date == item.Date
        //                   && q.Line == item.Line
        //               select new ProductUPHInfo
        //               {
        //                   Date = q.Date,
        //                   Line = q.Line,
        //                   Lesson = q.Lesson,
        //                   TimeRange = q.TimeRange,
        //                   Family = q.Family,
        //                   ProductRatio = q.ProductRatio
        //               }).ToList();
        //        if (ret.Count != 0)
        //        {
        //            re = true;
        //            //throw new Exception("Duplicate AstCode!");

        //        }
        //        return re;
        //    }
        //    catch (Exception e)
        //    {
        //        BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
        //        throw;
        //    }
        //    finally
        //    {
        //        BaseLog.LoggingEnd(logger, methodName);
        //    }


        //}

        /// <summary>
        /// SQL
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="Station"></param>
        /// <param name="QueryType"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="Line"></param>
        /// <param name="List"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataTable GetLocRinDown(string Connection, string Station, string QueryType, string FromDate, string ToDate, string Line, string List, IList<string> model)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            try
            {


                StringBuilder sb = new StringBuilder();
                sb.AppendLine("exec sp_Query_FA_LocQuery @Station,@QueryType,@BeginTime,@EndTime,@Line,@list,@Model ");
                DataTable dt = new DataTable();
                dt = SQLHelper.ExecuteDataFill(Connection,
                                             System.Data.CommandType.Text,
                                             sb.ToString(),
                                              SQLHelper.CreateSqlParameter("@Station", Station.Trim()),
                                              SQLHelper.CreateSqlParameter("@QueryType", QueryType.Trim()),
                                                 SQLHelper.CreateSqlParameter("@BeginTime", FromDate),
                                                    SQLHelper.CreateSqlParameter("@EndTime", ToDate),
                                                     SQLHelper.CreateSqlParameter("@Line", Line),
                                                     SQLHelper.CreateSqlParameter("@list", List),
                                                     SQLHelper.CreateSqlParameter("@Model", string.Join("','", model.ToArray()))
                                              );

                return dt;

            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        public DataTable GetAlarmALL()
        {
            string sqlCMD = "SELECT*FROM AlarmStatus ORDER BY PdLine,Cdt";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD
                                           );
            return dt;
        }

        public DataTable GetAlarm(string Process)
        {
            string sqlCMD = "SELECT*FROM AlarmStatus where Process=@Process ORDER BY PdLine,Cdt";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,
                                           SQLHelper.CreateSqlParameter("Process", Process.Trim())
                                           );
            return dt;
        }

        public DataTable GetAlarmC(string Process,string Class)
        {
            string sqlCMD = "SELECT*FROM AlarmStatus where Process=@Process and Class=@Class ORDER BY PdLine,Cdt";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,
                                           SQLHelper.CreateSqlParameter("Process", Process.Trim()),
                                           SQLHelper.CreateSqlParameter("Class", Class.Trim())
                                           );
            return dt;
        }

        public DataTable GetAlarmPd(string PdLine)
        {
            string sqlCMD = "SELECT*FROM AlarmStatus where PdLine=@PdLine ORDER BY PdLine,Cdt";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,
                                           SQLHelper.CreateSqlParameter("PdLine", PdLine.Trim())
                                           );
            return dt;
        }
        public DataTable GetAlarmCALL(string Class)
        {
            string sqlCMD = "SELECT*FROM AlarmStatus where Class=@Class ORDER BY PdLine,Cdt";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,
                                           SQLHelper.CreateSqlParameter("Class", Class.Trim())
                                           );
            return dt;
        }

        public DataTable DelAlarm(int id)
        {

            string sqlCMD = "delete AlarmStatus where ID=@ID";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,
                                           SQLHelper.CreateSqlParameter("ID", id)
                                           );
            return dt;

        }
        public DataTable DelAlarmLog(int id)
        {
            string sqlCMDLog = "INSERT dbo.AlarmLog SELECT Process,Class,PdLine,BeginTime,EndTime,Status,'Delete！ '+Remark,Editor,GETDATE() FROM AlarmStatus WHERE ID=@ID";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMDLog,
                                           SQLHelper.CreateSqlParameter("ID", id)
                                           );
            return dt;
        }
        public DataTable UpdateAlarm_ID(int id, string BeginTime, string EndTime, string Status, string Remark, string Editor, DateTime Udt)
        {
            string sqlCMD = "UPDATE AlarmStatus SET BeginTime=@BeginTime,EndTime=@EndTime,Status=@Status,Remark=@Remark,Editor=@Editor,Udt=@Udt  where ID=@ID";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,
                                           SQLHelper.CreateSqlParameter("ID", id),
                                           SQLHelper.CreateSqlParameter("BeginTime", BeginTime),
                                           SQLHelper.CreateSqlParameter("EndTime", EndTime),
                                           SQLHelper.CreateSqlParameter("Status", Status),
                                           SQLHelper.CreateSqlParameter("Editor", Editor),
                                           SQLHelper.CreateSqlParameter("Udt", Udt),
                                               SQLHelper.CreateSqlParameter("Remark", Remark)
                                           );
            return dt;

        }
        public DataTable UpdateAlarmLog_ID(int id)
        {
            string sqlCMDLog = "INSERT dbo.AlarmLog SELECT Process,Class,PdLine,BeginTime,EndTime,Status,'Update！ '+Remark,Editor,GETDATE() FROM AlarmStatus WHERE ID=@ID";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMDLog,
                                           SQLHelper.CreateSqlParameter("ID", id)
                                           );
            return dt;
        }
    }
}
