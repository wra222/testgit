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


namespace IMES.Query.Implementation
{
    public class PAK_WipTracking : MarshalByRefObject, IPAK_WipTracking
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public int[] GetDNShipQty_Docking(string Connection, DateTime ShipDate,string Model)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string subSql = "";
                if (!string.IsNullOrEmpty(Model))
                {
                    subSql += " and Model in (select value from dbo.fn_split (@Model,',')) ";
                }

                DataSet ds = new DataSet();


               string SQLText = @"declare @s varchar(256)
                                              select @s=Value from SysSetting where Name='PAKStation'
                                               select ISNULL(SUM(Qty),0) as Qty
                                                from Delivery (NOLOCK) where ShipDate=@ShipDate  {0} 
                                              and   dbo.GetDNType(DeliveryNo)='Docking'
                                               union ALL 
                                            select COUNT(*) from ProductStatus  WITH(NOLOCK) 
                                              where ProductID in
                                              (
                                               select ProductID from Product  WITH(NOLOCK) where 
                                               DeliveryNo in
                                               (
                                               select DeliveryNo from Delivery	 WITH(NOLOCK) where ShipDate=@ShipDate 
                                                 and   dbo.GetDNType(DeliveryNo)='Docking'
                                               )  {0}
                                              )
                                             and Station in
                                               (select value from dbo.fn_split (@s,','))  ";


//                string SQLText = @" declare @s varchar(256)
//                                                select @s=Value from SysSetting where Name='PAKStation'
//                                                select ISNULL(SUM(Qty),0) as Qty
//                                                 from Delivery (NOLOCK) where ShipDate=@ShipDate  {0} 
//                                                 and   dbo.GetDNType(DeliveryNo)='Docking'
//                                                 union ALL 
//                                              
//                                              select COUNT(*) as Qty from view_wip_station_NoLog
//                                               where ShipDate=@ShipDate and Station in
//                                               (select value from dbo.fn_split (@s,','))  and Model not like 'PC%'  {0}";

                SQLText = string.Format(SQLText, subSql);
                DataTable dt = SQLHelper.ExecuteDataFill(Connection,
                                                  System.Data.CommandType.Text,
                                                  SQLText,
                                                  SQLHelper.CreateSqlParameter("@ShipDate", ShipDate),
                                                  SQLHelper.CreateSqlParameter("@Model", 1024, Model));

                int[] inArr = new int[2];
                inArr[0] = int.Parse(dt.Rows[0][0].ToString());
                inArr[1] = int.Parse(dt.Rows[1][0].ToString());
                return inArr;

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
        public int[] GetDNShipQty_TEST(string Connection, string ShipDate, string Model, string PrdType)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string subSql = "and dbo.CheckModelCategory(Model,'{0}')='Y'";
                subSql = string.Format(subSql, PrdType);
                string stationList = PrdType == "PC" ? "PAKStation" : "PAKStation_RCTO";
                if (!string.IsNullOrEmpty(Model))
                {
                    subSql += " and Model in (select value from dbo.fn_split (@Model,',')) ";
                }

                DataSet ds = new DataSet();

                string SQLText = @" declare @s varchar(256)
                                                select @s=Value from SysSetting  WITH(NOLOCK) where Name='{1}'
                                                select ISNULL(SUM(Qty),0) as Qty from Delivery (NOLOCK) 
                                            where ShipDate=@ShipDate  {0} 
                                                 union ALL 
                                              
                                              select COUNT(*) as Qty from view_wip_station_PAK3
                                               where ShipDate=@ShipDate and Station in
                                               (select value from dbo.fn_split (@s,','))   {0}";

                SQLText = string.Format(SQLText, subSql, stationList);
                DataTable dt = SQLHelper.ExecuteDataFill(Connection,
                                                  System.Data.CommandType.Text,
                                                  SQLText,
                                                  SQLHelper.CreateSqlParameter("@ShipDate",64, ShipDate),
                                                  SQLHelper.CreateSqlParameter("@Model", 1024, Model));

                int[] inArr = new int[2];
                inArr[0] = int.Parse(dt.Rows[0][0].ToString());
                inArr[1] = int.Parse(dt.Rows[1][0].ToString());
                return inArr;

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
        public int[] GetDNShipQty_TEST(string Connection, DateTime ShipDate, string Model, string PrdType)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string subSql = "and dbo.CheckModelCategory(Model,'{0}')='Y'";
                subSql = string.Format(subSql, PrdType);
                string stationList = PrdType == "PC" ? "PAKStation" : "PAKStation_RCTO";
                if (!string.IsNullOrEmpty(Model))
                {
                    subSql += " and Model in (select value from dbo.fn_split (@Model,',')) ";
                }

                DataSet ds = new DataSet();

                string SQLText = @" declare @s varchar(256)
                                                select @s=Value from SysSetting  WITH(NOLOCK) where Name='{1}'
                                                select ISNULL(SUM(Qty),0) as Qty from Delivery (NOLOCK) 
                                            where ShipDate='2012-12-11'  {0} 
                                                 union ALL 
                                              
                                              select COUNT(*) as Qty from view_wip_station_PAK3
                                               where ShipDate='2012-12-11' and Station in
                                               (select value from dbo.fn_split (@s,','))   {0}";

                SQLText = string.Format(SQLText, subSql, stationList);
                DataTable dt = SQLHelper.ExecuteDataFill(Connection,
                                                  System.Data.CommandType.Text,
                                                  SQLText,
                                                //  SQLHelper.CreateSqlParameter("@ShipDate", ShipDate),
                                                   SQLHelper.CreateSqlParameter("@Model", 1024, Model));

                int[] inArr = new int[2];
                inArr[0] = int.Parse(dt.Rows[0][0].ToString());
                inArr[1] = int.Parse(dt.Rows[1][0].ToString());
                return inArr;

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

        public int[] GetDNShipQty(string Connection, DateTime ShipDate, string Model, string PrdType)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string subSql = "and dbo.CheckModelCategory(Model,'{0}')='Y'";
                subSql = string.Format(subSql, PrdType);
                string stationList = PrdType == "PC" ? "PAKStation" : "PAKStation_RCTO";
                if (!string.IsNullOrEmpty(Model))
                {
                    subSql += " and Model in (select value from dbo.fn_split (@Model,',')) ";
                }

                DataSet ds = new DataSet();

                string SQLText = @" declare @s varchar(256)
                                                select @s=Value from SysSetting  WITH(NOLOCK) where Name='{1}'
                                                select ISNULL(SUM(Qty),0) as Qty from Delivery (NOLOCK) 
                                            where ShipDate=@ShipDate  {0} 
                                                 union ALL 
                                              
                                              select COUNT(*) as Qty from view_wip_station_PAK3
                                               where ShipDate=@ShipDate and Station in
                                               (select value from dbo.fn_split (@s,','))   {0}";

                SQLText = string.Format(SQLText, subSql,stationList);
                DataTable dt;
                if(!string.IsNullOrEmpty(Model))
                {
                    dt=SQLHelper.ExecuteDataFill(Connection,
                                                  System.Data.CommandType.Text,
                                                  SQLText,
                                                  SQLHelper.CreateSqlParameter("@ShipDate", ShipDate),
                                                   SQLHelper.CreateSqlParameter("@Model",Model));}
                else
                {
                    dt=SQLHelper.ExecuteDataFill(Connection,
                                                  System.Data.CommandType.Text,
                                                  SQLText,
                                                  SQLHelper.CreateSqlParameter("@ShipDate", ShipDate));}
             

                int[] inArr = new int[2];
                inArr[0] = int.Parse(dt.Rows[0][0].ToString());
                inArr[1] = int.Parse(dt.Rows[1][0].ToString());
                return inArr;

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
        public DataTable WipTrackingByDN_GetNullTable(string Connection, string ShipDate, string Model, string Line, string Process, string Type, string DBName)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
               
                    return SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.StoredProcedure,
                                                "sp_Query_PAK_WipByDN_Ext_TEST",
                                               SQLHelper.CreateSqlParameter("@ShipDate", 64, ShipDate),
                                               SQLHelper.CreateSqlParameter("@Process", 1024, Process),
                                               SQLHelper.CreateSqlParameter("@Line", 1024, Line),
                                               SQLHelper.CreateSqlParameter("@Model", 1024, Model));
            
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
     
        public DataTable WipTrackingByDN_PAK_Ex(string Connection, string ShipDate, string Model, string Line, string Process, string Type)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                if (Type == "Model")
                {
                    return SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.StoredProcedure,
                                                "sp_Query_PAK_WipByDN_Ext",
                                               SQLHelper.CreateSqlParameter("@ShipDate", 64, ShipDate),
                                               SQLHelper.CreateSqlParameter("@Process", 1024, Process),
                                               SQLHelper.CreateSqlParameter("@Line", 1024, Line),
                                               SQLHelper.CreateSqlParameter("@Model", 1024, Model));
                }
                else
                {
                    return SQLHelper.ExecuteDataFill(Connection,
                                                    System.Data.CommandType.StoredProcedure,
                                                    "sp_Query_PAK_WipByDN_Ext_ByModelLine",
                                                   SQLHelper.CreateSqlParameter("@ShipDate", 64, ShipDate),
                                                   SQLHelper.CreateSqlParameter("@Process", 1024, Process),
                                                   SQLHelper.CreateSqlParameter("@Line", 1024, Line),
                                                   SQLHelper.CreateSqlParameter("@Model", 1024, Model));
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
        public DataTable WipTrackingByDN_FA_Ex(string Connection, string ShipDate, string Model, string Line, string Process, string Type, string DBName)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                if (Type == "Model")
                {
                    return SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.StoredProcedure,
                                                "sp_Query_FA_WipByDN_Ext",
                                               SQLHelper.CreateSqlParameter("@ShipDate", 64, ShipDate),
                                               SQLHelper.CreateSqlParameter("@Process", 1024, Process),
                                               SQLHelper.CreateSqlParameter("@Line", 1024, Line),
                                               SQLHelper.CreateSqlParameter("@Model", 1024, Model));
                }
                else
                {
                    return SQLHelper.ExecuteDataFill(Connection,
                                                    System.Data.CommandType.StoredProcedure,
                                                    "sp_Query_FA_WipByDN_Ext_ByModelLine",
                                                   SQLHelper.CreateSqlParameter("@ShipDate", 64, ShipDate),
                                                   SQLHelper.CreateSqlParameter("@Process", 1024, Process),
                                                   SQLHelper.CreateSqlParameter("@Line", 1024, Line),
                                                   SQLHelper.CreateSqlParameter("@Model", 1024, Model));
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


        public DataTable WipTrackingByDN_PAK_Ex(string Connection, string ShipDate, string Model, string Line, string Process, string Type, string PrdType,string DBName)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
          
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                return GetWipTrackingData(Connection, ShipDate, Model, Line, Process, Type, PrdType, DBName);

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
        public DataTable WipTrackingByDN_PAK_Ex(string Connection, string ShipDate, string Model, string Line, string Process, string Type,string PrdType,string DBName, out int[] CountDNQty)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            //if (string.Equals(DBName, "HPDocking", StringComparison.CurrentCultureIgnoreCase))
            if (DBName != null && DBName.ToUpper().IndexOf("HPDOCKING") >= 0)
            { CountDNQty = GetDNShipQty_Docking(Connection, DateTime.Parse(ShipDate), Model); }
            else
            { CountDNQty = GetDNShipQty(Connection, DateTime.Parse(ShipDate), Model, PrdType); }
        
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                return GetWipTrackingData(Connection, ShipDate, Model, Line, Process, Type, PrdType, DBName);
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
        private DataTable GetWipTrackingData(string Connection, string ShipDate, string Model, string Line, string Process, string Type, string PrdType, string DBName)
        {
            string spName = Type == "Model" ? "sp_Query_PAK_WipByDN_Ext" : "sp_Query_PAK_WipByDN_Ext_ByModelLine";
            //if (string.Equals(DBName, "HPDocking", StringComparison.CurrentCultureIgnoreCase))
            if (DBName != null && DBName.ToUpper().IndexOf("HPDOCKING") >= 0)
            {
                return SQLHelper.ExecuteDataFill(Connection,
                                          System.Data.CommandType.StoredProcedure,
                                         spName,
                                         SQLHelper.CreateSqlParameter("@ShipDate", 64, ShipDate),
                                         SQLHelper.CreateSqlParameter("@Process", 1024, Process), //@PrdType
                                         SQLHelper.CreateSqlParameter("@Line", 1024, Line),
                                         SQLHelper.CreateSqlParameter("@Model", 1024, Model));
            }
            else
            {
                return SQLHelper.ExecuteDataFill(Connection,
                                          System.Data.CommandType.StoredProcedure,
                                         spName,
                                         SQLHelper.CreateSqlParameter("@ShipDate", 64, ShipDate),
                                         SQLHelper.CreateSqlParameter("@Process", 1024, Process), //@PrdType
                                         SQLHelper.CreateSqlParameter("@PrdType", 64, PrdType), //@PrdType
                                         SQLHelper.CreateSqlParameter("@Line", 1024, Line),
                                         SQLHelper.CreateSqlParameter("@Model", 1024, Model));
            }
          
        }
        public DataTable WipTrackingByDN_FA_Ex(string Connection, string ShipDate, string Model, string Line, string Process, string Type, string DBName, out int[] CountDNQty)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            CountDNQty = GetDNShipQty(Connection, DateTime.Parse(ShipDate), DBName, Model);
            BaseLog.LoggingBegin(logger, methodName);
            try
            {

                if (Type == "Model")
                {
                    return SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.StoredProcedure,
                                                "sp_Query_FA_WipByDN_Ext",
                                               SQLHelper.CreateSqlParameter("@ShipDate", 64, ShipDate),
                                               SQLHelper.CreateSqlParameter("@Process", 1024, Process),
                                               SQLHelper.CreateSqlParameter("@Line", 1024, Line),
                                               SQLHelper.CreateSqlParameter("@Model", 1024, Model));
                }
                else
                {
                    return SQLHelper.ExecuteDataFill(Connection,
                                                    System.Data.CommandType.StoredProcedure,
                                                    "sp_Query_FA_WipByDN_Ext_ByModelLine",
                                                   SQLHelper.CreateSqlParameter("@ShipDate", 64, ShipDate),
                                                   SQLHelper.CreateSqlParameter("@Process", 1024, Process),
                                                   SQLHelper.CreateSqlParameter("@Line", 1024, Line),
                                                   SQLHelper.CreateSqlParameter("@Model", 1024, Model));
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
        public DataTable WipTrackingByDN_PAK(string Connection, string ShipDate, string FAStation, string PAKStation, string Model,
                                                                 string Line, string IsShiftLine, string grpType, string DBName, out int[] CountDNQty)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            CountDNQty = GetDNShipQty(Connection, DateTime.Parse(ShipDate), DBName, Model);
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string process = "";
                if (FAStation != "" && PAKStation != "")
                { process = "ALL"; }
                else if (FAStation != "")
                { process = "FA"; }
                else
                { process = "PAK"; }

                if (grpType == "Model")
                {
                    return SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.StoredProcedure,
                                                "sp_Query_PAK_WipByDN_Ext", //@ShipDate Datetime,@Process varchar(32),@Line varchar(512),@Model varchar(512)
                                               SQLHelper.CreateSqlParameter("@ShipDate", 64, ShipDate),
                                               SQLHelper.CreateSqlParameter("@Process", 1024, process),
                                               SQLHelper.CreateSqlParameter("@Line", 1024, Line),
                                               SQLHelper.CreateSqlParameter("@Model", 1024, Model));
                                           
                }
                else
                {
                    return SQLHelper.ExecuteDataFill(Connection,
                                                    System.Data.CommandType.StoredProcedure,
                                                    "sp_Query_PAK_WipByDN2",
                                                   SQLHelper.CreateSqlParameter("@Shipdate", 64, ShipDate),
                                                    SQLHelper.CreateSqlParameter("@FAStation", 1024, FAStation),
                                                    SQLHelper.CreateSqlParameter("@PAKStation", 1024, PAKStation),
                                                    SQLHelper.CreateSqlParameter("@Model", 1024, Model),
                                                    SQLHelper.CreateSqlParameter("@Line", 1024, Line),
                                                    SQLHelper.CreateSqlParameter("@IsShiftLine", 32, IsShiftLine));
                }

                //               @Shipdate varchar(64),@FAStation varchar(max),@PAKStation varchar(max),@Model varchar(MAX),
                //@Line varchar(255),@IsShiftLine varchar(16)
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
         public DataTable GetDetailForWipTracking(string Connection, string Model, string Line, string Station,string DN)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            DateTime DShipDate = DateTime.Now;
            try
            {
                

              DataTable  dt = SQLHelper.ExecuteDataFill(Connection,
                                                       System.Data.CommandType.StoredProcedure,
                                                       "sp_Query_PAK_WipByDN_GetDetail",
                                                       SQLHelper.CreateSqlParameter("@Station", 32, Station),
                                                       SQLHelper.CreateSqlParameter("@DN", 32, DN),
                                                       SQLHelper.CreateSqlParameter("@Model", 32, Model),
                                                       SQLHelper.CreateSqlParameter("@Line", 32, Line));
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
    
       

    }
}
