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
using System.Data.SqlClient;
namespace IMES.Query.Implementation
{
    public class QueryCommon : MarshalByRefObject, IQueryCommon, IDefect, ICause        
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetStationDescr(string station, string DBConnection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
          
            try
            {

                string SQL = "select Station,Name,Descr from Station ";

                if (station != "")
                {
                    //SQL += " AND rtrim(a.Status)+' '+rtrim(a.Line) = '" + station + "'";
                    SQL += " where Station like '" + station + "%' ";
                }

                return SQLHelper.ExecuteDataFill(DBConnection,
                                                 System.Data.CommandType.Text,
                                                 SQL, SQLHelper.CreateSqlParameter("@station", 32, station));
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
        //public DataTable GetStation(List<string> stationType)
        //{
        //    string methodName = MethodBase.GetCurrentMethod().Name;
        //    BaseLog.LoggingBegin(logger, methodName);
        //    string stationList="";

        //    try
        //    { 
        //        List<SqlParameter> lstSqlPar = new List<SqlParameter>();
        //        StringBuilder sb = new StringBuilder("Select Station,Name from Station where 1=1 ");
        //        if (stationType != null)
        //        {
        //            stationList = string.Join("','", stationType.ToArray());
        //            sb.Append(" and StationType in ('"+stationList+"')");
        //        }
             


        //        return SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
        //                                         System.Data.CommandType.Text,
        //                                         sb.ToString(), lstSqlPar.ToArray());

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

        public DataTable GetStation(List<string> stationType, string DBConnection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            string stationList = "";
            try
            {
                List<SqlParameter> lstSqlPar = new List<SqlParameter>();
                StringBuilder sb = new StringBuilder("Select Station,Name from Station where 1=1 ");
                if (stationType != null)
                {
                    stationList = string.Join("','", stationType.ToArray());
                    sb.Append(" and StationType in ('" + stationList + "')");
                }
                sb.Append("union Select Station,Name from Station where Station in ('CR01','CR02','CR05','CR10','CR15','CR32')");
                return SQLHelper.ExecuteDataFill(DBConnection,
                                                 System.Data.CommandType.Text,
                                                 sb.ToString(), lstSqlPar.ToArray());

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

        public DataTable GetStationName(List<string> station, string DBConnection)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                StringBuilder sb = new StringBuilder("SELECT Station,Name FROM Station WHERE 1=1 ");
                if (station.Count > 0)
                {
                    sb.AppendFormat("AND Station IN ('{0}')", string.Join("','", station.ToArray()));                    
                }
                sb.AppendLine("ORDER BY Station");

                return SQLHelper.ExecuteDataFill(DBConnection,
                                                 System.Data.CommandType.Text,
                                                 sb.ToString());

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
        public DataTable GetLine(IList<string> lstProcess, string customer, bool IsWithoutShift, string DBConnection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                //  StringBuilder sb = new StringBuilder("SELECT Line,Descr FROM Line WHERE 1=1 ");
                string SQLText = "";
                if (IsWithoutShift)
                {
                    SQLText = "select distinct SUBSTRING(Line,1,1) AS Line, 'Line-' + SUBSTRING(Line,1,1) AS Descr FROM Line WHERE 1=1 ";

                }
                else
                {
                    SQLText = "SELECT Line,Descr FROM Line WHERE 1=1 ";
                }
                //    SQLText += string.Format(" AND Stage IN ('{0}')", string.Join("','", lstProcess.ToArray()));
                //  SQLText += " ORDER BY 1";


                if (lstProcess != null && lstProcess.Count > 0)
                {
                    SQLText += string.Format(" AND Stage IN ('{0}')", string.Join("','", lstProcess.ToArray()));

                }
                DataTable dt = null;
                if (!string.IsNullOrEmpty(customer))
                {
                    SQLText += " AND CustomerID=@customer";
                    SQLText += " ORDER BY 1";
                    dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                 System.Data.CommandType.Text, SQLText,
                                                   SQLHelper.CreateSqlParameter("@customer", 32, customer, ParameterDirection.Input));

                }
                else
                {
                    SQLText += " ORDER BY 1";
                    SQLHelper.ExecuteDataFill(DBConnection,
                                                     System.Data.CommandType.Text, SQLText);
                }

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

        public DataTable GetFixtureID(string DBConnection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string SQLText = @"select distinct a.FixtureID from PCBTestLog a where a.FixtureID <> '' AND a.FixtureID is not null order by FixtureID ";
                return SQLHelper.ExecuteDataFill(DBConnection,System.Data.CommandType.Text,SQLText);

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
        public DataTable GetOP(string DBConnection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string SQLText = @"select distinct a.Editor from PCBTestLog a where a.Station IN('10','ICT','15','10A') AND a.Editor is not null order by Editor  ";
                return SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text, SQLText);

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

        public DataTable GetFamily(string DBConnection, DateTime fromDate, DateTime toDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {


                string SQLText = @"select distinct LEFT(a.PCBNo,2)as family  from PCBTestLog a where Cdt between @fromDate and @toDate order by family ";
                return SQLHelper.ExecuteDataFill(DBConnection, 
                                                        System.Data.CommandType.Text, 
                                                        SQLText, 
                                                        SQLHelper.CreateSqlParameter("@fromDate", fromDate),
                                                        SQLHelper.CreateSqlParameter("@toDate", toDate) 
                                                        );


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
        //public DataTable GetLine(IList<string> lstProcess, string customer, bool IsWithoutShift)
        //{
        //    string methodName = MethodBase.GetCurrentMethod().Name;
        //    BaseLog.LoggingBegin(logger, methodName);

        //    try
        //    {

        //        //  StringBuilder sb = new StringBuilder("SELECT Line,Descr FROM Line WHERE 1=1 ");
        //        string SQLText = "";
        //        if (IsWithoutShift)
        //        {
        //            SQLText = "select distinct SUBSTRING(Line,1,1) AS Line, 'Line-' + SUBSTRING(Line,1,1) AS Descr FROM Line WHERE 1=1 ";

        //        }
        //        else
        //        {
        //            SQLText = "SELECT Line,Descr FROM Line WHERE 1=1 ";
        //        }
        //        //    SQLText += string.Format(" AND Stage IN ('{0}')", string.Join("','", lstProcess.ToArray()));
        //        //  SQLText += " ORDER BY 1";


        //        if (lstProcess != null && lstProcess.Count > 0)
        //        {
        //            SQLText += string.Format(" AND Stage IN ('{0}')", string.Join("','", lstProcess.ToArray()));

        //        }
        //        DataTable dt = null;
        //        if (!string.IsNullOrEmpty(customer))
        //        {
        //            SQLText += " AND CustomerID=@customer";
        //            SQLText += " ORDER BY 1";
        //            dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
        //                                         System.Data.CommandType.Text, SQLText,
        //                                           SQLHelper.CreateSqlParameter("@customer", 32, customer, ParameterDirection.Input));

        //        }
        //        else
        //        {
        //            SQLText += " ORDER BY 1";
        //            SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
        //                                             System.Data.CommandType.Text, SQLText);
        //        }

        //        return dt;
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
        public DataTable GetDNList(string DBConnection, DateTime fromDate, DateTime toDate)
        { 
         string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                //DataTable dt = null;  [sp_Query_PAK_NonBulkPackingRpt_Detail]
                //return dt;
                string SQLText = @"select DeliveryNo from Delivery where ShipDate between @fromDate and @toDate ";

                return SQLHelper.ExecuteDataFill(DBConnection,
                                                      System.Data.CommandType.Text,
                                                      SQLText,
                                                      SQLHelper.CreateSqlParameter("@fromDate", fromDate),
                                                      SQLHelper.CreateSqlParameter("@toDate", toDate)
                                                      );


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
        public DataTable GetDefect(List<string> type,string DBConnection) {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                StringBuilder sb = new StringBuilder("SELECT Defect, Type, Descr FROM DefectCode ");
                if (type.Count > 0){
                    sb.AppendLine(string.Format("WHERE Type IN ('{0}') ", string.Join("','", type.ToArray())));
                }
                sb.AppendLine("ORDER BY Defect");

                return SQLHelper.ExecuteDataFill(DBConnection,
                                                 System.Data.CommandType.Text,
                                                 sb.ToString());

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
        public DataTable GetDefectInfo(List<string> type, string customer, string DBConnection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                if (customer == null ||customer == "")
                {
                    throw new Exception("NO customer");
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("SELECT  Code, Description, Type FROM DefectInfo WHERE CustomerID = '{0}' ", customer));
                if (type.Count > 0)
                {
                    sb.AppendLine(string.Format("AND Type IN ('{0}') ", string.Join("','", type.ToArray())));
                }
                sb.AppendLine("ORDER BY Code");

                return SQLHelper.ExecuteDataFill(DBConnection,
                                                 System.Data.CommandType.Text,
                                                 sb.ToString());

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
        public void UpdateSysSetting(string Name, string Value, string DBConnection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
           BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string SQL = @"if exists(select * from SysSetting where Name=@Name)
                                           Update SysSetting Set Value=@Value where Name=@Name
                                         else
                                           Insert SysSetting
                                         Values(@Name,@Value,@Name)";

                 SQLHelper.ExecuteNonQuery(DBConnection,
                                                 System.Data.CommandType.Text,
                                                 SQL,
                                                 SQLHelper.CreateSqlParameter("@Name", 1024, Name),
                                                 SQLHelper.CreateSqlParameter("@Value", 1024, Value)
                                                 );

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
        public DataTable GetSysSetting(List<string> Name,string DBConnection )
        {
            List<string> ValueList = new List<string>();
            string methodName = MethodBase.GetCurrentMethod().Name;
            string nameLst = string.Join(",", Name.ToArray());

            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string SQL = @"select Name,Value from SysSetting a
                                        join
                                         ( select value from dbo.fn_split(@Name,','))
                                         b on a.Name=b.value";

                return SQLHelper.ExecuteDataFill(DBConnection,
                                                 System.Data.CommandType.Text,
                                                 SQL,
                                                 SQLHelper.CreateSqlParameter("@Name", 2048,nameLst)
                                                 );

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

        #region Implementation of IDefect
        /// <summary>
        /// 取得Defect列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>Defect信息列表</returns>
        public IList<DefectInfo> GetDefectList(string type)
        {

            IList<DefectInfo> retLst = new List<DefectInfo>();

            try
            {

                if (!String.IsNullOrEmpty(type))
                {
                      string sqlStr =@"SELECT Defect, Type, Descr
                                       FROM DefectCode
                                       WHERE Type=@Type";

                    DataTable dt= SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                               System.Data.CommandType.Text,
                                               sqlStr, 
                                               SQLHelper.CreateSqlParameter("@Type",32,type));
                    foreach (DataRow dr in dt.Rows)
                    {
                        DefectInfo item = new DefectInfo();
                        item.id = dr["Defect"].ToString().Trim();
                        item.friendlyName = dr["Descr"].ToString().Trim();
                        item.description = dr["Descr"].ToString().Trim();
                        retLst.Add(item);
                    }
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 取得Defect信息
        /// </summary>
        /// <param name="defectId">Defect标识</param>
        /// <returns>Defect信息</returns>
        public DefectInfo GetDefectInfo(string defectId)
        {

            DefectInfo defectInfo = new DefectInfo();
            try
            {

                if (!String.IsNullOrEmpty(defectId))
                {
                    string sqlStr = @"SELECT Defect, Type, Descr
                                      FROM DefectCode
                                      WHERE Defect=@Defect";

                    DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                               System.Data.CommandType.Text,
                                               sqlStr,
                                               SQLHelper.CreateSqlParameter("@Defect", 32, defectId));
                    foreach (DataRow dr in dt.Rows)
                    {                      
                        defectInfo.id = dr["Defect"].ToString().Trim();
                        defectInfo.friendlyName = dr["Descr"].ToString().Trim();
                        defectInfo.description = dr["Descr"].ToString().Trim();
                    }
                }
                return defectInfo;

            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 根据type,customer获取对应的Defect信息列表
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="customer">customer</param>
        /// <returns>Defect信息列表</returns>
        public IList<DefectInfo> GetDefectInfoByTypeAndCustomer(string type, string customer)
        {
            IList<DefectInfo> retLst = new List<DefectInfo>(); 

            try
            {
                string sqlStr = @"SELECT Code, Description, Type
                                  FROM DefectInfo
                                  WHERE Type=@Type AND CustomerID= @CustomerID";

                DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                           System.Data.CommandType.Text,
                                           sqlStr,
                                           SQLHelper.CreateSqlParameter("@Type", 32, type),
                                           SQLHelper.CreateSqlParameter("@CustomerID", 32, customer));
                foreach (DataRow dr in dt.Rows)
                {
                    DefectInfo item = new DefectInfo();
                    item.id = dr["Code"].ToString().Trim();
                    item.friendlyName = dr["Descr"].ToString().Trim();
                    item.description = dr["Descr"].ToString().Trim();
                    retLst.Add(item);
                }

                return retLst;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion



        #region Implementation of ICause

        /// <summary>
        /// 取得Cause信息列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Cause信息列表</returns>
        public IList<CauseInfo> GetCauseList(string customerId,string stage)
        {
            IList<CauseInfo> retLst = new List<CauseInfo>();

            try
            {

                if (!String.IsNullOrEmpty(customerId))
                {
                    
                    string sqlStr = @"SELECT Code, Description, Type
                                      FROM DefectInfo
                                      WHERE Type=@Type AND CustomerID= @CustomerID";

                    DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                               System.Data.CommandType.Text,
                                               sqlStr,
                                               SQLHelper.CreateSqlParameter("@Type", 32, stage+DefectInfoType.Cause),
                                               SQLHelper.CreateSqlParameter("@CustomerID", 32, customerId));
                    foreach (DataRow dr in dt.Rows)
                    {
                        CauseInfo item = new CauseInfo();
                        item.id = dr["Code"].ToString().Trim();
                        item.friendlyName = dr["Description"].ToString().Trim();                       
                        retLst.Add(item);
                    }
                }
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public DataTable GetWithdrawTest(DateTime shipDate, string model, string status,DataTable dt, string DBConnection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
             
                if (dt == null)
                {
                    return SQLHelper.ExecuteDataFill(DBConnection,
                                                  System.Data.CommandType.StoredProcedure,
                                                  "IMES_ShipmentCombined_Query", //@ShipDate Datetime,@Process varchar(32),@Line varchar(512),@Model varchar(512)
                                                 SQLHelper.CreateSqlParameter("@Shipdate",shipDate),
                                                 SQLHelper.CreateSqlParameter("@Model", 32, model),
                                                 SQLHelper.CreateSqlParameter("@Status", 16, status));
                
                }
                else
                {//@ShipDate Datetime,@Model varchar(32),@dtDnPallet dbo.TbShipmentCombineStatus ReadOnly
                
                    DataTable dtR= SQLHelper.ExecuteDataFill(DBConnection,
                                                  System.Data.CommandType.StoredProcedure,
                                                  "sp_Query_WithdrawTest", //@ShipDate Datetime,@Process varchar(32),@Line varchar(512),@Model varchar(512)
                                                SQLHelper.CreateSqlParameter("@Shipdate",shipDate),
                                                    SQLHelper.CreateSqlParameter("@Model", 32, model),
                                                 SQLHelper.CreateSqlParameter("@dtDnPallet",dt));
                    return dtR;
                
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

 public DataTable GetSMTLine(string DBConnection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {


                string SQLText = @"select distinct le.Line as Line,l.Descr as Descr from LineEx le ,Line l  
                                    where le.Line=l.Line and AliasLine like 'AOI%' order by Line ";
                return SQLHelper.ExecuteDataFill(DBConnection,
                                                        System.Data.CommandType.Text,
                                                        SQLText
                                                        );


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

        public DataTable GetSMTRefrshTimeAndStationByLine(string DBConnection, string line)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {


                string SQLText = @"select * from SMT_Dashboard_Line_RefreshTime_Station WHERE Line=@line";
                return SQLHelper.ExecuteDataFill(DBConnection,
                                                        System.Data.CommandType.Text,
                                                        SQLText,
                                                        SQLHelper.CreateSqlParameter("@line", line)
                                                        );


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
