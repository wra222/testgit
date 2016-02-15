using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using IMES.Query.DB;
using IMES.Infrastructure;
using log4net;
using System.Reflection;
using System.Data;
using IMES.Entity.Repository.Meta.IMESSKU;
using IMES.Entity.Infrastructure.Framework;
using IMES.Entity.Infrastructure.Interface;

namespace IMES.Query.Implementation
{
   public class SA_SMTDashboard :MarshalByRefObject,ISA_SMTDashboard
    {
       private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       public IRepository<SMT_Dashboard_Line_Time_Qty> SMTDashBoardRep = new Repository<SMT_Dashboard_Line_Time_Qty>("HPIMES_Rep_DB");
       public IRepository<SMT_Dashboard_Result> SMTDashBoardResultRep = new Repository<SMT_Dashboard_Result>("HPIMES_Rep_DB");
       public IRepository<SMT_Dashboard_Line_RefreshTime_Station> SMTLineRefreshTimeStationtRep = new Repository<SMT_Dashboard_Line_RefreshTime_Station>("HPIMES_Rep_DB");
       public List<DataTable> GetQueryResult(string Connection, string line)
       {
            string methodName = MethodBase.GetCurrentMethod().Name;
            
            BaseLog.LoggingBegin(logger, methodName);
            List<DataTable> dts = new List<DataTable>();
            try
            {
                using(UnitOfWork uow =new UnitOfWork())
               {
                string SQLText = @"select DurTime, Family, Output, StandardOutput, OutputRate, DefectQty, Rate, LocationTop3
                                    from SMT_Dashboard_Result where Line =@Line";
                

                DataTable dt1= SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@Line", line));
                string defectCause = @"if (GETDATE()<=cast((CONVERT(char(10),GETDATE(),23)  +' 08:00')as datetime))
                                        begin 
	                                        select CONVERT(varchar(5), StartTime, 108)+'--'+CONVERT(varchar(5), EndTime, 108),Family,isnull(SrcLocation,'') as Location,ISNULL(Cause,'') as Cause,ISNULL(Remark,'') as Remark 
                                            from Alarm where Cdt between cast((CONVERT(char(10), DATEADD(D,-1,GETDATE()),23)  +' 08:00')as datetime)
	                                        and cast((CONVERT(char(10),GETDATE(),23)  +' 08:00')as datetime)
                                        end 
                                        else begin 
	                                        select CONVERT(varchar(5), StartTime, 108)+'--'+CONVERT(varchar(5), EndTime, 108),Family,isnull(SrcLocation,'') as Location,ISNULL(Cause,'') as Cause,ISNULL(Remark,'') as Remark 
                                            from Alarm where Cdt between cast((CONVERT(char(10),GETDATE(),23)  +' 08:00')as datetime)
	                                        and cast((CONVERT(char(10),dateadd(D,1,GETDATE()),23)  +' 00:00')as datetime)
                                        end ";
                DataTable dt2 = SQLHelper.ExecuteDataFill(Connection,
                                             System.Data.CommandType.Text,
                                             defectCause);

                string refreshTimeAndStation = @"select RefreshTime,Station from SMT_Dashboard_Line_RefreshTime_Station where Line=@line";


                DataTable dt3 = SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 refreshTimeAndStation,
                                                 SQLHelper.CreateSqlParameter("@line", line));
                    dts.Add(dt1);
                    dts.Add(dt2);
                    dts.Add(dt3);
                    return dts;
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

       public List<SMT_DashBoard_Line> GetQueryLine()
       {
           string methodName = MethodBase.GetCurrentMethod().Name;
           logger.DebugFormat("BEGIN: {0}()", methodName);
           List<SMT_DashBoard_Line> lines = new List<SMT_DashBoard_Line>();
           try
           {
               using (UnitOfWork uow = new UnitOfWork())
               {
                   lines = (from SMTDashboard in SMTLineRefreshTimeStationtRep.Query()
                            select new SMT_DashBoard_Line { line = SMTDashboard.Line }).Distinct().ToList();
                   return lines;
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

       public IList<SMT_DashBoard_MantainInfo> GetSMTDashBoardLineListByCondition(SMT_DashBoard_MantainInfo condition)
       {
           string methodName = MethodBase.GetCurrentMethod().Name;

           BaseLog.LoggingBegin(logger, methodName);

           IList<SMT_DashBoard_MantainInfo> ret = new List<SMT_DashBoard_MantainInfo>();
           try
           {

               using (UnitOfWork uow = new UnitOfWork())
               {
                       ret = (from SMTDashboard in SMTDashBoardRep.Query()
                              where SMTDashboard.Line==condition.line
                              select new SMT_DashBoard_MantainInfo { id = SMTDashboard.ID, line = SMTDashboard.Line, durTime = SMTDashboard.DurTime, standardOutput = SMTDashboard.Qty,editor=SMTDashboard.Editor,StandardOutputCdt=SMTDashboard.Cdt,StandardOutputUdt=SMTDashboard.Udt}).OrderBy(x=>x.line).Distinct().ToList();
               }

               return ret;
           }
           catch (Exception)
           {
               throw;
           }
           finally
           {
               BaseLog.LoggingEnd(logger, methodName);
           }
       }

       public IList<SMT_DashBoard_MantainInfo> GetSMTDashBoardLineListById(SMT_DashBoard_MantainInfo condition)
       {
           string methodName = MethodBase.GetCurrentMethod().Name;

           BaseLog.LoggingBegin(logger, methodName);

           IList<SMT_DashBoard_MantainInfo> ret = new List<SMT_DashBoard_MantainInfo>();
           try
           {

               using (UnitOfWork uow = new UnitOfWork())
               {
                       ret = (from SMTDashboard in SMTDashBoardRep.Query()
                              where SMTDashboard.ID ==Convert.ToInt32(condition.id)
                              select new SMT_DashBoard_MantainInfo { id = SMTDashboard.ID, line = SMTDashboard.Line, durTime = SMTDashboard.DurTime, standardOutput = SMTDashboard.Qty }).Distinct().ToList();
                   
               }

               return ret;
           }
           catch (Exception)
           {
               throw;
           }
           finally
           {
               BaseLog.LoggingEnd(logger, methodName);
           }
       }
       
       public void AddSMTDashboardInfo(SMT_DashBoard_MantainInfo conditicon)
       {
           string methodName = MethodBase.GetCurrentMethod().Name;
           logger.DebugFormat("BEGIN: {0}()", methodName);
           try
           {
               using (UnitOfWork uow = new UnitOfWork())
               {
                   SMT_Dashboard_Line_Time_Qty smtDashBoardMaintain = new SMT_Dashboard_Line_Time_Qty();
                   SMT_Dashboard_Result smtDashboardResult = new SMT_Dashboard_Result();
                   smtDashBoardMaintain.Line = conditicon.line;
                   smtDashBoardMaintain.Qty = conditicon.standardOutput;
                   smtDashBoardMaintain.DurTime = "08:00--10:00";
                   smtDashBoardMaintain.Editor = conditicon.editor;
                   smtDashBoardMaintain.Cdt = DateTime.Now;
                   smtDashBoardMaintain.Udt = DateTime.Now;
                   smtDashboardResult.Line = conditicon.line;
                   smtDashboardResult.DurTime = "08:00--10:00";
                   SMTDashBoardRep.Insert(smtDashBoardMaintain); // 将数据塞入Maintain 表中
                   SMTDashBoardResultRep.Insert(smtDashboardResult);//将Line和时间段塞入 结果表中
                   uow.Commit();
                   smtDashBoardMaintain.DurTime = "10:00--12:00";
                   smtDashboardResult.DurTime = "10:00--12:00";
                   SMTDashBoardRep.Insert(smtDashBoardMaintain);
                   SMTDashBoardResultRep.Insert(smtDashboardResult);
                   uow.Commit();
                   smtDashBoardMaintain.DurTime = "12:00--14:00";
                   smtDashboardResult.DurTime = "12:00--14:00";
                   SMTDashBoardRep.Insert(smtDashBoardMaintain);
                   SMTDashBoardResultRep.Insert(smtDashboardResult);
                   uow.Commit();
                   smtDashBoardMaintain.DurTime = "14:00--16:00";
                   smtDashboardResult.DurTime = "14:00--16:00";
                   SMTDashBoardRep.Insert(smtDashBoardMaintain);
                   SMTDashBoardResultRep.Insert(smtDashboardResult);
                   uow.Commit();
                   smtDashBoardMaintain.DurTime = "16:00--18:00";
                   smtDashboardResult.DurTime = "16:00--18:00";
                   SMTDashBoardRep.Insert(smtDashBoardMaintain);
                   SMTDashBoardResultRep.Insert(smtDashboardResult);
                   uow.Commit();
                   smtDashBoardMaintain.DurTime = "18:00--20:30";
                   smtDashboardResult.DurTime = "18:00--20:30";
                   SMTDashBoardRep.Insert(smtDashBoardMaintain);
                   SMTDashBoardResultRep.Insert(smtDashboardResult);
                   uow.Commit();
                   smtDashBoardMaintain.DurTime = "20:30--22:00";
                   smtDashboardResult.DurTime = "20:30--22:00";
                   SMTDashBoardRep.Insert(smtDashBoardMaintain);
                   SMTDashBoardResultRep.Insert(smtDashboardResult);
                   uow.Commit();
                   smtDashBoardMaintain.DurTime = "22:00--00:00";
                   smtDashboardResult.DurTime = "22:00--00:00";
                   SMTDashBoardRep.Insert(smtDashBoardMaintain);
                   SMTDashBoardResultRep.Insert(smtDashboardResult);
                   uow.Commit();
                   smtDashBoardMaintain.DurTime = "00:00--02:00";
                   smtDashboardResult.DurTime = "00:00--02:00";
                   SMTDashBoardRep.Insert(smtDashBoardMaintain);
                   SMTDashBoardResultRep.Insert(smtDashboardResult);
                   uow.Commit();
                   smtDashBoardMaintain.DurTime = "02:00--04:00";
                   smtDashboardResult.DurTime = "02:00--04:00";
                   SMTDashBoardRep.Insert(smtDashBoardMaintain);
                   SMTDashBoardResultRep.Insert(smtDashboardResult);
                   uow.Commit();
                   smtDashBoardMaintain.DurTime = "04:00--06:00";
                   smtDashboardResult.DurTime = "04:00--06:00";
                   SMTDashBoardRep.Insert(smtDashBoardMaintain);
                   SMTDashBoardResultRep.Insert(smtDashboardResult);
                   uow.Commit();
                   smtDashBoardMaintain.DurTime = "06:00--08:00";
                   smtDashboardResult.DurTime = "06:00--08:00";
                   SMTDashBoardRep.Insert(smtDashBoardMaintain);
                   SMTDashBoardResultRep.Insert(smtDashboardResult);
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
       
       public void UpdateSMTDashBoardInfo(SMT_DashBoard_MantainInfo conditicon)
       {
           string methodName = MethodBase.GetCurrentMethod().Name;
           logger.DebugFormat("BEGIN: {0}()", methodName);
           SMT_Dashboard_Line_Time_Qty smtDashBoardMaintain = new SMT_Dashboard_Line_Time_Qty();
           try
           {
               using (UnitOfWork uow = new UnitOfWork())
               {

                   smtDashBoardMaintain.ID = conditicon.id;
                   smtDashBoardMaintain.Line = conditicon.line;
                   smtDashBoardMaintain.Qty = conditicon.standardOutput;
                   smtDashBoardMaintain.DurTime = conditicon.durTime;
                   smtDashBoardMaintain.Editor = conditicon.editor;
                   smtDashBoardMaintain.Cdt = conditicon.StandardOutputCdt;
                   smtDashBoardMaintain.Udt = DateTime.Now;
                   SMTDashBoardRep.Update(smtDashBoardMaintain);
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

       public IList<SMT_DashBoard_MantainInfo> GetSMTDashBoardRefreshTimeAndStationByLine(SMT_DashBoard_MantainInfo condition)
       {
           string methodName = MethodBase.GetCurrentMethod().Name;

           BaseLog.LoggingBegin(logger, methodName);

           IList<SMT_DashBoard_MantainInfo> ret = new List<SMT_DashBoard_MantainInfo>();
           try
           {

               using (UnitOfWork uow = new UnitOfWork())
               {
                   ret = (from SMTRefreshTimeStation in SMTLineRefreshTimeStationtRep.Query()
                          where SMTRefreshTimeStation.Line ==condition.line
                          select new SMT_DashBoard_MantainInfo { line = SMTRefreshTimeStation.Line, refreshTime = SMTRefreshTimeStation.RefreshTime, station = SMTRefreshTimeStation.Station }).Distinct().ToList();

               }

               return ret;
           }
           catch (Exception)
           {
               throw;
           }
           finally
           {
               BaseLog.LoggingEnd(logger, methodName);
           }
       }

       public void AddOrUpDateSMTDashboardRefreshTimeAndStation(SMT_DashBoard_MantainInfo condition, string DBConnection)
       {
           string methodName = MethodBase.GetCurrentMethod().Name;

           BaseLog.LoggingBegin(logger, methodName);

           try
           {

               string SQL = @"if exists(select * from SMT_Dashboard_Line_RefreshTime_Station where Line=@line)
                                           Update SMT_Dashboard_Line_RefreshTime_Station Set RefreshTime=@refreshtime,
                                                  Station=@station,Editor=@editor,Udt=getdate()
                                            where Line=@line
                                         else
                                           Insert SMT_Dashboard_Line_RefreshTime_Station
                                             Values(@line,@refreshtime,@station,@editor,getdate(),getdate())";

               SQLHelper.ExecuteNonQuery(DBConnection,
                                               System.Data.CommandType.Text,
                                               SQL,
                                               SQLHelper.CreateSqlParameter("@line", 1024, condition.line),
                                               SQLHelper.CreateSqlParameter("@refreshtime", condition.refreshTime),
                                               SQLHelper.CreateSqlParameter("@station", 1024, condition.station),
                                               SQLHelper.CreateSqlParameter("@editor", 1024, condition.editor)
                                               );

           }
           catch (Exception)
           {
               throw;
           }
           finally
           {
               BaseLog.LoggingEnd(logger, methodName);
           }
       
       }
   }
}
