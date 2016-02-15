using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPH.Interface;
using log4net;
using System.Reflection;
using UPH.DB;
using UPH.Entity.Infrastructure.Framework;
using UPH.Entity.Infrastructure.Interface;
using UPH.Entity.Repository.Meta.IMESSKU;
using System.Data;

namespace UPH.Implementation
{
    public class UPH_Family : MarshalByRefObject, IUPH_Family
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetUPHFamily()
        {
            string sqlCMD = "SELECT DISTINCT Family FROM [10.96.183.28].HPIMES.dbo.Model";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD
                                           );
            return dt;
        }
        public void AddProductUPHInfo(UPHInfo itemui)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {

                Entity.Repository.Meta.IMESSKU.UPH items = new Entity.Repository.Meta.IMESSKU.UPH
                {
                    Process = itemui.Process,
                    Attend_normal = itemui.Attend_normal,
                    Family = itemui.Family,
                    ST = itemui.ST,
                    NormalUPH = itemui.NormalUPH,
                    Cycle = itemui.Cycle,
                    Remark = itemui.Remark,
                    Special = itemui.Special,
                    Editor = itemui.Editor,
                    Cdt = itemui.Cdt,
                    Udt = itemui.Udt

                };
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IRepository<Entity.Repository.Meta.IMESSKU.UPH> ecoModelRep = new Repository<Entity.Repository.Meta.IMESSKU.UPH>("UPHDBServer");
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
        public void DelProductUPHInfo(UPHInfo itemui)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                Entity.Repository.Meta.IMESSKU.UPH items = new Entity.Repository.Meta.IMESSKU.UPH
                {
                    Process = itemui.Process,
                    Family = itemui.Family,
                    Special = itemui.Remark,


                };


                using (UnitOfWork uow = new UnitOfWork())
                {
                    IRepository<Entity.Repository.Meta.IMESSKU.UPH> ecoModelRep = new Repository<Entity.Repository.Meta.IMESSKU.UPH>("UPHDBServer");
                    var ret = from q in ecoModelRep.Query()
                              where q.Process == itemui.Process
                          && q.Family == itemui.Family
                          && q.Special == itemui.Special
                              select new UPHInfo
                              {
                                  ID = q.ID,
                                  Process = q.Process,
                                  Attend_normal = q.Attend_normal,
                                  Family = q.Family,
                                  ST = q.ST.ToString(),
                                  NormalUPH = q.NormalUPH,
                                  Cycle = q.Cycle.ToString(),
                                  Remark = q.Remark,
                                  Special = q.Special,
                                  Editor = q.Editor,
                                  Cdt = q.Cdt,
                                  Udt = q.Udt
                              };
                    var first = ret.First();
                    items.ID = first.ID;
                    items.Process = first.Process;
                    items.Attend_normal = first.Attend_normal;
                    items.ST = first.ST;
                    items.NormalUPH = first.NormalUPH;
                    items.Cycle = first.Cycle;
                    items.Remark = first.Remark;
                    items.Special = first.Special;
                    items.Editor = first.Editor;
                    items.Cdt = first.Cdt;
                    items.Udt = first.Udt;


                    //IRepository<Entity.Repository.Meta.IMESSKU.UPH> ecoModelRep = new Repository<Entity.Repository.Meta.IMESSKU.UPH>("UPHDBServer");
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
        public void UpdateProductUPHInfo(UPHInfo itemui)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                IRepository<Entity.Repository.Meta.IMESSKU.UPH> ecoModelRep = new Repository<Entity.Repository.Meta.IMESSKU.UPH>("UPHDBServer");

                var ret = from q in ecoModelRep.Query()
                          where q.Process == itemui.Process
                      && q.Family == itemui.Family
                      && q.Special == itemui.Special
                          select new UPHInfo
                          {
                              ID = q.ID,
                              Process = q.Process,
                              Attend_normal = q.Attend_normal,
                              Family = q.Family,
                              ST = q.ST.ToString(),
                              NormalUPH = q.NormalUPH,
                              Cycle = q.Cycle.ToString(),
                              Remark = q.Remark,
                              Special = q.Remark,
                              Editor = q.Editor,
                              Cdt = q.Cdt,
                              Udt = q.Udt
                          };
                var first = ret.First();

                Entity.Repository.Meta.IMESSKU.UPH items = new Entity.Repository.Meta.IMESSKU.UPH
                {
                    ID = first.ID,
                    Process = itemui.Process,
                    Attend_normal = itemui.Attend_normal,
                    Family = itemui.Family,
                    ST = itemui.ST,
                    NormalUPH = itemui.NormalUPH,
                    Cycle = itemui.Cycle,
                    Remark = itemui.Remark,
                    Special = itemui.Special,
                    Editor = itemui.Editor,
                    Cdt = first.Cdt,
                    Udt = itemui.Udt

                };
                using (UnitOfWork uow = new UnitOfWork())
                {
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
        public bool CheckDuplicateData(UPHInfo item)
        {
            bool re = false;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            IList<UPHInfo> ret = new List<UPHInfo>();
            try
            {
                IRepository<Entity.Repository.Meta.IMESSKU.UPH> ecoModelRep = new Repository<Entity.Repository.Meta.IMESSKU.UPH>("UPHDBServer");//连接字符串 查询ProductUPH 表的数据
                using (UnitOfWork uow = new UnitOfWork()) { }
                ret = (from q in ecoModelRep.Query()
                       where q.Process == item.Process
                           && q.Family == item.Family
                           && q.Special == item.Special
                       select new UPHInfo
                       {
                           Process = q.Process,
                           Attend_normal = q.Attend_normal,
                           Family = q.Family,
                           ST = q.ST.ToString(),
                           NormalUPH = q.NormalUPH,
                           Cycle = q.Cycle.ToString(),
                           Remark = q.Remark,
                           Special = q.Special,
                           Editor = q.Editor,
                           Cdt = q.Cdt,
                           Udt = q.Udt
                       }).ToList();

                if (ret.Count != 0)
                {
                    re = true;
                    //throw new Exception("Duplicate AstCode!");

                }
                return re;
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
        public IList<UPHInfo> GetProductUPHInfoList(UPHInfo item)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            IList<UPHInfo> ret = new List<UPHInfo>();
            try
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IRepository<Entity.Repository.Meta.IMESSKU.UPH> ecoModelRep = new Repository<Entity.Repository.Meta.IMESSKU.UPH>("UPHDBServer");//连接字符串 查询ProductUPH 表的数据
                    ret = (from q in ecoModelRep.Query()
                           where q.Process == item.Process
                           && q.Family == item.Family
                           && q.Special == item.Special
                           select new UPHInfo
                           {
                               Process = q.Process,
                               Attend_normal = q.Attend_normal,
                               Family = q.Family,
                               ST = q.ST.ToString(),
                               NormalUPH = q.NormalUPH,
                               Cycle = q.Cycle.ToString(),
                               Remark = q.Remark,
                               Special = q.Special,
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

        public void AddUPHLog(UPHInfo itemui)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {

                Entity.Repository.Meta.IMESSKU.UPHLog items = new Entity.Repository.Meta.IMESSKU.UPHLog
                {
                    Process = itemui.Process,
                    Attend_normal = itemui.Attend_normal,
                    Family = itemui.Family,
                    ST = itemui.ST,
                    NormalUPH = itemui.NormalUPH,
                    Cycle = itemui.Cycle,
                    Remark = itemui.Remark,
                    Special = itemui.Special,
                    Editor = itemui.Editor,
                    Cdt = itemui.Cdt,

                };
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IRepository<Entity.Repository.Meta.IMESSKU.UPHLog> ecoModelRep = new Repository<Entity.Repository.Meta.IMESSKU.UPHLog>("UPHDBServer");
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
        public DataTable GetAlarmALL()
        {
            string sqlCMD = "SELECT *FROM dbo.UPH ORDER BY Cdt ";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD
                                           );
            return dt;
        }
        public DataTable GetUPHA(string Process)
        {
            string sqlCMD = "SELECT *FROM dbo.UPH WHERE Process=@Process  ORDER BY Family";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,
                                           SQLHelper.CreateSqlParameter("Process", Process.Trim())
                                           );
            return dt;
        }
        public DataTable GetUPHH(string Process, string Family)
        {
            string sqlCMD = "SELECT *FROM dbo.UPH WHERE Process=@Process and Family=@Family  ORDER BY Cdt";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,
                                           SQLHelper.CreateSqlParameter("Process", Process.Trim()),
                                           SQLHelper.CreateSqlParameter("Family", Family.Trim())

                                           );
            return dt;
        }
        public DataTable InsertUPH(string Process, string Family, string Special, string Editor)
        {
            string sqlCMD = "INSERT INTO dbo.UPHLog( Process , Family ,Attend_normal , ST ,Cycle , NormalUPH ,Special ,Remark ,Editor ,Cdt)SELECT Process ,Family ,Attend_normal ,ST , Cycle ,NormalUPH ,Special , Remark , @Editor ,GETDATE()  FROM dbo.UPH WHERE Process=@Process AND Family=@Family AND Special=@Special";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,
                                           SQLHelper.CreateSqlParameter("Process", Process.Trim()),
                                           SQLHelper.CreateSqlParameter("Family", Family.Trim()),
                                           SQLHelper.CreateSqlParameter("Special", Special.Trim()),
                                           SQLHelper.CreateSqlParameter("Editor", Editor.Trim())

                                           );
            return dt;
        }
        public DataTable GetUPHZ(string Process, string Family, string Special)
        {
            string sqlCMD = "SELECT *FROM dbo.UPH WHERE Process=@Process and Family=@Family and Special=@Special   ORDER BY Cdt";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,
                                           SQLHelper.CreateSqlParameter("Process", Process.Trim()),
                                           SQLHelper.CreateSqlParameter("Family", Family.Trim()),
                                           SQLHelper.CreateSqlParameter("Special", Special.Trim())


                                           );
            return dt;
        }
        public DataTable Del()
        {
            string sqlCMD = "DELETE FROM  dbo.UPH WHERE ID NOT IN( SELECT a.ID FROM (SELECT TOP 100000 Process,MAX(ID) AS ID,Family,Special FROM   dbo.UPH WHERE Process IN('FA','PA','SA','SMT')AND Family IN(SELECT DISTINCT Family  FROM dbo.UPH)AND Special IN('DIS','RCTO','UMA','') GROUP BY Process,dbo.UPH.Family,Special ORDER BY dbo.UPH.Process) AS a)";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD
                                           );
            return dt;
        }

    }
}
