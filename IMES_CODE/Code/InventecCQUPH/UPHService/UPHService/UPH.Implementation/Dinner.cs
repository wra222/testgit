﻿using System;
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
    public class Dinner : MarshalByRefObject, IDinner
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetAllDinnerTime()
        {
            DataTable dinner = null;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM DinnerTime");
            string Connection = SQLHelper.ConnectionString_ONLINE(0);
            dinner = SQLHelper.ExecuteDataFill(Connection,
                                            System.Data.CommandType.Text,
                                              sb.ToString());
            return dinner;
        }

        public DataTable GetAllDinnerProcess(string process)
        {
            DataTable dinner = null;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM DinnerTime where Process=@process");
            string Connection = SQLHelper.ConnectionString_ONLINE(0);
            dinner = SQLHelper.ExecuteDataFill(Connection,
                                            System.Data.CommandType.Text,
                                            sb.ToString(),
                                            SQLHelper.CreateSqlParameter("@process", process));
            return dinner;
        }

        public List<string> GetAllProcess()
        {
            List<string> ret = new List<string>();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT distinct Process FROM PdLine order by Process ");
            DataTable dt = new DataTable();
            string Connection = SQLHelper.ConnectionString_ONLINE(0);
            dt = SQLHelper.ExecuteDataFill(Connection,
                                            System.Data.CommandType.Text,
                                              sb.ToString());
            foreach (DataRow dr in dt.Rows)
            {
                string process = dr["Process"].ToString();
                ret.Add(process);
            }
            return ret;
        }

        public DataTable GetAllDinnerProcessClass(string process, string Class, string line)
        {
            DataTable dinner = null;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM DinnerTime where Process=@process and Class=@Class and PdLine=@line");
            string Connection = SQLHelper.ConnectionString_ONLINE(0);
            dinner = SQLHelper.ExecuteDataFill(Connection,
                                            System.Data.CommandType.Text,
                                            sb.ToString(),
                                            SQLHelper.CreateSqlParameter("@process", process),
                                            SQLHelper.CreateSqlParameter("@Class", Class),
                                            SQLHelper.CreateSqlParameter("@line", line));
            return dinner;
        }

        public DataTable GetAllDinnerLine(string process, string line)
        {
            DataTable dinner = null;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM DinnerTime where Process=@process and PdLine=@line");
            string Connection = SQLHelper.ConnectionString_ONLINE(0);
            dinner = SQLHelper.ExecuteDataFill(Connection,
                                            System.Data.CommandType.Text,
                                            sb.ToString(),
                                            SQLHelper.CreateSqlParameter("@process", process),
                                            SQLHelper.CreateSqlParameter("@line", line));
            return dinner;
        }

        public List<string> GetAllLine(string process)
        {
            List<string> ret = new List<string>();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT distinct PdLine FROM PdLine where Process=@process order by PdLine ");
            DataTable dt = new DataTable();
            string Connection = SQLHelper.ConnectionString_ONLINE(0);
            dt = SQLHelper.ExecuteDataFill(Connection,
                                            System.Data.CommandType.Text,
                                              sb.ToString(),
                                              SQLHelper.CreateSqlParameter("@process", process));
            foreach (DataRow dr in dt.Rows)
            {
                string line = dr["PdLine"].ToString();
                ret.Add(line);
            }
            return ret;
        }

        public void AddDinnerTimeInfo(DinnerTimeInfo q)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                DinnerTime items = new DinnerTime
                {
                    ID = q.ID,
                    Process = q.Process,
                    Type = q.Type,
                    Class = q.Class,
                    PdLine = q.PdLine,
                    BeginTime = q.BeginTime,
                    EndTime = q.EndTime,
                    Remark = q.Remark,
                    Editor = q.Editor,
                    Cdt = q.Cdt,
                    Udt = q.Udt
                };
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IRepository<DinnerTime> ecoModelRep = new Repository<DinnerTime>("UPHDBServer");
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

        public void AddDinnerLogInfo(DinnerLogInfo q)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                DinnerLog items = new DinnerLog
                {
                    ID = q.ID,
                    Process = q.Process,
                    Type = q.Type,
                    Class = q.Class,
                    PdLine = q.PdLine,
                    BeginTime = q.BeginTime,
                    EndTime = q.EndTime,
                    Remark = q.Remark,
                    Editor = q.Editor,
                    Cdt = q.Cdt
                };
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IRepository<DinnerLog> ecoModelRep = new Repository<DinnerLog>("UPHDBServer");
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

        public DataTable DelDinnerTime(int id)
        {
            DataTable dinner = null;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("delete DinnerTime where ID=@id");
            string Connection = SQLHelper.ConnectionString_ONLINE(0);
            dinner = SQLHelper.ExecuteDataFill(Connection,
                                            System.Data.CommandType.Text,
                                              sb.ToString(),
                                              SQLHelper.CreateSqlParameter("@id", id));
            return dinner;
        }

        public void UpdateDinnerTime(int id, string begintime, string endtime, string remark, string editor)
        {
            string sqlCMD = "UPDATE DinnerTime SET BeginTime=@BeginTime,EndTime=@EndTime,Remark=@Remark,Editor=@Editor,Udt=getdate()  where ID=@ID";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,
                                           SQLHelper.CreateSqlParameter("ID", id),
                                           SQLHelper.CreateSqlParameter("BeginTime", begintime),
                                           SQLHelper.CreateSqlParameter("EndTime", endtime),
                                           SQLHelper.CreateSqlParameter("Remark", remark),
                                           SQLHelper.CreateSqlParameter("Editor", editor)
                                           );
        }

        public void ADDDinnerLog(int id, string remark, string editor)
        {
            string sqlCMDLog = "INSERT dbo.DinnerLog SELECT Process,Type,Class,PdLine,BeginTime,EndTime,@Remark,@Editor,GETDATE() FROM DinnerTime WHERE ID=@ID";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMDLog,
                                           SQLHelper.CreateSqlParameter("ID", id),
                                           SQLHelper.CreateSqlParameter("Remark", remark),
                                           SQLHelper.CreateSqlParameter("Editor", editor)
                                           );
        }
    }
}

