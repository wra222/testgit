﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.UnitOfWork;
using log4net;

namespace IMES.Infrastructure.Repository._Schema
{
    /// <summary>
    /// ADO.NET使用帮助类
    /// </summary>
    public abstract class SqlHelper
    {
        //private static readonly string DBServer = ConfigurationManager.AppSettings["DBServer"].ToString();
        private readonly static ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly static string MsgSQL = "{0} SQL Statement:\r\n{1}\r\nSQL Parameter:{2}";
        private readonly static string MethodExecuteNonQuery = "ExecuteNonQuery";
        private readonly static string MethodExecuteNonQueryConsiderOutParams = "ExecuteNonQueryConsiderOutParams";
        private readonly static string MethodExecuteReader = "ExecuteReader";
        private readonly static string MethodExecuteReader_OnTrans = "ExecuteReader_OnTrans";
        private readonly static string MethodExecuteDataFill = "ExecuteDataFill";
        private readonly static string MethodExecuteDataFillConsiderOutParams = "ExecuteDataFillConsiderOutParams";
        private readonly static string MethodExecSPorSql = "ExecSPorSql";
        private readonly static string MethodExecuteScalar = "ExecuteScalar";
        private readonly static string MsgSqlParam = " [Name]:{0} [Value]:{1} [DBType]:{2} [Direction]:{3}";
        
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DBServer"].ToString();
        //public static readonly string ConnectionString_Remote = ConfigurationManager.ConnectionStrings["DBRemote"].ToString();

        public static string GetCataLog(string connectionStringX)
        {
            string[] toTrim = ConnectionString.Split(new string[] { "{0}" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in toTrim)
            {
                connectionStringX = connectionStringX.Replace(str, string.Empty);
            }
            return connectionStringX;
        }

        /// <summary>
        /// 数据库连接字符串 BOM
        /// </summary>
        public static string ConnectionString_BOM
        {
            get
            {
                return string.Format(ConnectionString, DB_BOM);
            }
        }
        /// <summary>
        /// 数据库连接字符串 GetData
        /// </summary>
        public static string ConnectionString_GetData
        {
            get
            {
                return string.Format(ConnectionString, DB_GetData);
            }
        }
        /// <summary>
        /// 数据库连接字符串 PCA
        /// </summary>
        public static string ConnectionString_PCA
        {
            get
            {
                return string.Format(ConnectionString, DB_PCA);
            }
        }
        /// <summary>
        /// 数据库连接字符串 FA
        /// </summary>
        public static string ConnectionString_FA
        {
            get
            {
                return string.Format(ConnectionString, DB_FA);
            }
        }
        /// <summary>
        /// 数据库连接字符串 PAK
        /// </summary>
        public static string ConnectionString_PAK
        {
            get
            {
                return string.Format(ConnectionString, DB_PAK);
            }
        }
        /// <summary>
        /// 数据库连接字符串 Dashboard
        /// </summary>
        public static string ConnectionString_Dashborad
        {
            get
            {
                return string.Format(ConnectionString, DB_Dashborad);
            }
        }
        /// <summary>
        /// 数据库连接字符串 KIT
        /// </summary>
        public static string ConnectionString_KIT
        {
            get
            {
                return string.Format(ConnectionString, DB_KIT);
            }
        }
        /// <summary>
        /// 数据库连接字符串 DOA
        /// </summary>
        public static string ConnectionString_DOA
        {
            get
            {
                return string.Format(ConnectionString, DB_DOA);
            }
        }
        /// <summary>
        /// 数据库连接字符串 KIT
        /// </summary>
        public static string ConnectionString_HP_EDI
        {
            get
            {
                return string.Format(ConnectionString, DB_HP_EDI);
            }
        }

        /// <summary>
        /// 数据库连接字符串 Docking
        /// </summary>
        public static string ConnectionString_Docking
        {
            get
            {
                return string.Format(ConnectionString, DB_Docking);
            }
        }

        /// <summary>
        /// 数据库连接字符串 SIEGetData
        /// </summary>
        //public static string ConnectionString_SIEGetData
        //{
        //    get
        //    {
        //        return string.Format(ConnectionString, DB_SIEGetData);
        //    }
        //}


        /// <summary>
        /// DB Catalog名称 BOM
        /// </summary>
        public static readonly string DB_BOM = ConfigurationManager.AppSettings["DB_BOM"].ToString();
        /// <summary>
        /// DB Catalog名称 GetData
        /// </summary>
        public static readonly string DB_GetData = ConfigurationManager.AppSettings["DB_GetData"].ToString();
        /// <summary>
        /// DB Catalog名称 PCA
        /// </summary>
        public static readonly string DB_PCA = ConfigurationManager.AppSettings["DB_PCA"].ToString();
        /// <summary>
        /// DB Catalog名称 FA
        /// </summary>
        public static readonly string DB_FA = ConfigurationManager.AppSettings["DB_FA"].ToString();
        /// <summary>
        /// DB Catalog名称 PAK
        /// </summary>
        public static readonly string DB_PAK = ConfigurationManager.AppSettings["DB_PAK"].ToString();
        /// <summary>
        /// DB Catalog名称 Dashboard
        /// </summary>
        public static readonly string DB_Dashborad = ConfigurationManager.AppSettings["DB_Dashborad"].ToString();
        /// <summary>
        /// DB Catalog名称 KIT
        /// </summary>
        public static readonly string DB_KIT = ConfigurationManager.AppSettings["DB_KIT"].ToString();
        /// <summary>
        /// DB Catalog名称 DOA
        /// </summary>
        public static readonly string DB_DOA = ConfigurationManager.AppSettings["DB_DOA"].ToString();
        /// <summary>
        /// DB Catalog名称 HP_EDI
        /// </summary>
        public static readonly string DB_HP_EDI = ConfigurationManager.AppSettings["DB_HP_EDI"].ToString();
        /// <summary>
        /// DB Catalog名称 Docking
        /// </summary>
        public static readonly string DB_Docking = ConfigurationManager.AppSettings["DB_Docking"].ToString();
        /// <summary>
        /// DB Catalog名称 GetData for SIE Replicate DB
        /// </summary>
        //public static readonly string DB_SIEGetData = ConfigurationManager.AppSettings["DB_SIEGetData"].ToString();

        public static int SqlCommandTimeout
        {
            get 
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["SqlCommandTimeout"].ToString());
                }
                catch(Exception)
                {
                    return 30;
                }
            }
        }

        //// Hashtable to store cached parameters
        //private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        //// Hashtable to store cached sql sentences
        //private static Hashtable sqlCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlConnection conn = null;
            try
            {
                SqlCommand cmd = new SqlCommand();
                if (Logger.IsDebugEnabled)
                {
                    LogSql(MethodExecuteNonQuery, cmdText, commandParameters);
                 }
                conn = SqlTransactionManager.GetSqlConnection(connectionString);
                //using (SqlConnection conn = new SqlConnection(connectionString))
                //{
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    if (SqlTransactionManager.inScopeTag)
                        SqlTransactionManager.ChangeCataLog(GetCataLog(connectionString));
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                //}
            }
            catch
            {
                if (!Logger.IsDebugEnabled)
                {
                    LogSql(MethodExecuteNonQuery, cmdText, commandParameters);
                }   
                throw;
            }
            finally
            {
                SqlTransactionManager.CloseSqlConnection(conn);
            }
        }

        public static int ExecuteNonQueryConsiderOutParams(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlConnection conn = null;
            try
            {
                SqlCommand cmd = new SqlCommand();
                if (Logger.IsDebugEnabled)
                {
                    LogSql(MethodExecuteNonQueryConsiderOutParams, cmdText, commandParameters);                   
                }
                conn = SqlTransactionManager.GetSqlConnection(connectionString);
                //using (SqlConnection conn = new SqlConnection(connectionString))
                //{
                PrepareCommandConsiderOutParams(cmd, conn, null, cmdType, cmdText, commandParameters);
                if (SqlTransactionManager.inScopeTag)
                    SqlTransactionManager.ChangeCataLog(GetCataLog(connectionString));
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
                //}
            }
            catch
            {
                if (!Logger.IsDebugEnabled)
                {
                    LogSql(MethodExecuteNonQueryConsiderOutParams, cmdText, commandParameters);                      
                }   
                throw;
            }
            finally
            {
                SqlTransactionManager.CloseSqlConnection(conn);
            }
        }

        ///// <summary>
        ///// Execute a SqlCommand (that returns no resultset) against an existing database connection 
        ///// using the provided parameters.
        ///// </summary>
        ///// <remarks>
        ///// e.g.:  
        /////  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        ///// </remarks>
        ///// <param name="conn">an existing database connection</param>
        ///// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ///// <param name="commandText">the stored procedure name or T-SQL command</param>
        ///// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ///// <returns>an int representing the number of rows affected by the command</returns>
        //public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        //{
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand();
        //        PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
        //        int val = cmd.ExecuteNonQuery();
        //        cmd.Parameters.Clear();
        //        return val;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
        ///// using the provided parameters.
        ///// </summary>
        ///// <remarks>
        ///// e.g.:  
        /////  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        ///// </remarks>
        ///// <param name="trans">an existing sql transaction</param>
        ///// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ///// <param name="commandText">the stored procedure name or T-SQL command</param>
        ///// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ///// <returns>an int representing the number of rows affected by the command</returns>
        //public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        //{
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand();
        //        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
        //        int val = cmd.ExecuteNonQuery();
        //        cmd.Parameters.Clear();
        //        return val;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the results</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            if (Logger.IsDebugEnabled)
            {
                LogSql(MethodExecuteReader, cmdText, commandParameters);               
            }
            //Vincent 2015-07-14 check transaction or not, if yes, join into transaction
            if (SqlTransactionManager.inScopeTag)
            {
                return ExecuteReader_OnTrans(connectionString, cmdType, cmdText, commandParameters);
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                SqlConnection conn = new SqlConnection(connectionString);

                // we use a try/catch here because if the method throws an exception we want to 
                // close the connection throw code, because no datareader will exist, hence the 
                // commandBehaviour.CloseConnection will not work
                try
                {
                    SqlTransactionManager.Suppress();

                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return rdr;
                }
                catch
                {
                    conn.Close();
                    if (!Logger.IsDebugEnabled)
                    {
                        LogSql(MethodExecuteReader, cmdText, commandParameters); 
                    }                      
                    throw;
                }
                finally
                {
                    SqlTransactionManager.Recover();
                }
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
        /// using the provided parameters.(On transaction)
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the results</returns>
        public static SqlDataReader ExecuteReader_OnTrans(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlConnection conn = null;
            try
            {
                //SqlTransactionManager.Begin();
                if (Logger.IsDebugEnabled)
                {
                    LogSql(MethodExecuteReader_OnTrans, cmdText, commandParameters);    
                }

                SqlCommand cmd = new SqlCommand();

                conn = SqlTransactionManager.GetSqlConnection(connectionString);

                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                if (SqlTransactionManager.inScopeTag)
                    SqlTransactionManager.ChangeCataLog(GetCataLog(connectionString));

                SqlDataReader rdr = SqlTransactionManager.inScopeTag ? cmd.ExecuteReader() : cmd.ExecuteReader(CommandBehavior.CloseConnection);

                cmd.Parameters.Clear();

                //SqlTransactionManager.Commit();

                return rdr;
            }
            catch
            {
                //SqlTransactionManager.Rollback();
                if (!Logger.IsDebugEnabled)
                {
                    LogSql(MethodExecuteReader_OnTrans, cmdText, commandParameters);                   
                }   
                throw;
            }
            finally
            {
                //SqlTransactionManager.Dispose();
                //SqlTransactionManager.End();
            }
        }

        //public static SqlDataReader ExecuteReader_OnTrans(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        //{
        //    SqlCommand cmd = new SqlCommand();

        //    try
        //    {
        //        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
        //        SqlDataReader rdr = cmd.ExecuteReader();
        //        cmd.Parameters.Clear();
        //        return rdr;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Execute a SqlCommand that returns a datatable.
        /// </summary>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the results</returns>
        public static DataTable ExecuteDataFill(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            //SqlCommand cmd = new SqlCommand();
            //SqlConnection conn = new SqlConnection(connectionString);
            //SqlDataAdapter sda = null;            
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                if (Logger.IsDebugEnabled)
                {
                    LogSql(MethodExecuteDataFill, cmdText, commandParameters);                    
                }
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            cmd.Parameters.Clear();
                            return dt;
                        }
                    }
                }
            }
            catch
            {
                if (!Logger.IsDebugEnabled)
                {
                    LogSql(MethodExecuteDataFill, cmdText, commandParameters);                    
                }   
                throw;
            }
            finally
            {
                //conn.Close();
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns a datatable. (Not copy parameters, for output parameters.)
        /// </summary>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the results</returns>
        public static DataTable ExecuteDataFillConsiderOutParams(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            //SqlCommand cmd = new SqlCommand();
            //SqlConnection conn = new SqlConnection(connectionString);
            //SqlDataAdapter sda = null;            
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                if (Logger.IsDebugEnabled)
                {
                    LogSql(MethodExecuteDataFillConsiderOutParams, cmdText, commandParameters);                     
                }
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommandConsiderOutParams(cmd, conn, null, cmdType, cmdText, commandParameters);
                        //sda = new SqlDataAdapter(cmd);
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            cmd.Parameters.Clear();
                            return dt;
                        }
                    }
                }
            }
            catch
            {
                if (!Logger.IsDebugEnabled)
                {
                    LogSql(MethodExecuteDataFillConsiderOutParams, cmdText, commandParameters);                     
                }                   
                throw;
            }
            finally
            {
                //conn.Close();
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns a datatable. (Not copy parameters, for output parameters.)
        /// </summary>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the results</returns>
        public static DataSet ExecSPorSql(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            //SqlCommand cmd = new SqlCommand();
           // SqlConnection conn = new SqlConnection(connectionString);
            //SqlDataAdapter sda = null;            
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                if (Logger.IsDebugEnabled)
                {
                    LogSql(MethodExecSPorSql, cmdText, commandParameters);                   
                }
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommandConsiderOutParams(cmd, conn, null, cmdType, cmdText, commandParameters);
                        //sda = new SqlDataAdapter(cmd);
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet dt = new DataSet();
                            sda.Fill(dt);
                            cmd.Parameters.Clear();
                            return dt;
                        }
                    }
                }
            }
            catch
            {
                if (!Logger.IsDebugEnabled)
                {
                    LogSql(MethodExecSPorSql, cmdText, commandParameters);                   
                }   
                throw;
            }
            finally
            {
                //conn.Close();
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlConnection connection = null;
            try
            {
                SqlCommand cmd = new SqlCommand();
                if (Logger.IsDebugEnabled)
                {
                    LogSql(MethodExecuteScalar, cmdText, commandParameters);                   
                }
                connection = SqlTransactionManager.GetSqlConnection(connectionString);
                //using (SqlConnection connection = new SqlConnection(connectionString))
                //{
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                    if (SqlTransactionManager.inScopeTag)
                        SqlTransactionManager.ChangeCataLog(GetCataLog(connectionString));
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                //}
            }
            catch
            {
                if (!Logger.IsDebugEnabled)
                {
                    LogSql(MethodExecuteScalar, cmdText, commandParameters); 
                }               
                throw;
            }
            finally
            {
                SqlTransactionManager.CloseSqlConnection(connection);
            }
        }

        public static int ExecuteScalarForAquireIdInsert(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            return Convert.ToInt32(ExecuteScalar(connectionString, cmdType, cmdText, commandParameters));
        }

        public static long ExecuteScalarForAquireLongIdInsert(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            return Convert.ToInt64(ExecuteScalar(connectionString, cmdType, cmdText, commandParameters));
        }

        public static int ExecuteScalarForAquireIdInsertWithTry(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            object obj = ExecuteScalar(connectionString, cmdType, cmdText, commandParameters);
            int ret = -1;
            try
            {
                ret = Convert.ToInt32(obj);
            }
            catch (Exception)
            {
                ret = -1;
            }
            return ret;
        }

        ///// <summary>
        ///// Execute a SqlCommand that returns the first column of the first record against an existing database connection 
        ///// using the provided parameters.
        ///// </summary>
        ///// <remarks>
        ///// e.g.:  
        /////  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        ///// </remarks>
        ///// <param name="conn">an existing database connection</param>
        ///// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ///// <param name="commandText">the stored procedure name or T-SQL command</param>
        ///// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ///// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        //public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        //{
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand();

        //        PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
        //        object val = cmd.ExecuteScalar();
        //        cmd.Parameters.Clear();
        //        return val;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public static object ExecuteScalar(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        //{
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand();

        //        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
        //        object val = cmd.ExecuteScalar();
        //        cmd.Parameters.Clear();
        //        return val;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// add parameter array to the cache
        ///// </summary>
        ///// <param name="cacheKey">Key to the parameter cache</param>
        ///// <param name="cmdParms">an array of SqlParamters to be cached</param>
        //public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        //{
        //    try
        //    {
        //        parmCache[cacheKey] = commandParameters;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Retrieve cached parameters
        ///// </summary>
        ///// <param name="cacheKey">key used to lookup parameters</param>
        ///// <returns>Cached SqlParamters array</returns>
        //public static SqlParameter[] GetCachedParameters(string cacheKey)
        //{
        //    try
        //    {
        //        SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

        //        if (cachedParms == null)
        //            return null;

        //        SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

        //        for (int i = 0, j = cachedParms.Length; i < j; i++)
        //        {
        //            clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();
        //            clonedParms[i].Value = null;
        //        }

        //        return clonedParms;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="cacheKey"></param>
        ///// <param name="sql"></param>
        //public static void CacheSql(string cacheKey, string sql)
        //{
        //    try
        //    {
        //        sqlCache[cacheKey] = sql;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="cacheKey"></param>
        ///// <returns></returns>
        //public static string GetSql(string cacheKey)
        //{
        //    try
        //    {
        //        string sql = (string)sqlCache[cacheKey];

        //        if (null == sql)
        //            return null;

        //        return sql;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            try
            {
                PrepareCommand_Inner(cmd, conn, trans, cmdType, cmdText, cmdParms, false);
            }
            catch
            {
                throw;
            }
        }

        private static void PrepareCommandConsiderOutParams(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            try
            {
                PrepareCommand_Inner(cmd, conn, trans, cmdType, cmdText, cmdParms, true);
            }
            catch
            {
                throw;
            }
        }

        private static void PrepareCommand_Inner(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms, bool forOutParam)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                cmd.CommandTimeout = SqlCommandTimeout;

                if (SqlTransactionManager.inScopeTag)
                {
                    SqlTransaction trn = SqlTransactionManager.GetTransaction();
                    if (trn != null)
                        cmd.Transaction = trn;
                }
                else
                {
                    if (trans != null)
                        cmd.Transaction = trans;
                }

                cmd.CommandType = cmdType;

                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                    {
                        if (parm.Value == null)
                            parm.Value = DBNull.Value;
                        //Vincent 2015-11-08 don't need copy SqlParameter due to PeerTheSQL was copy new SQLParameter 
                        cmd.Parameters.Add(parm);
                        //if (forOutParam && (parm.Direction == ParameterDirection.Output || parm.Direction == ParameterDirection.ReturnValue || parm.Direction == ParameterDirection.InputOutput))
                        //{
                        //    cmd.Parameters.Add(parm);
                        //}
                        //else
                        //{                            
                        //    SqlParameter clonedParameter = (SqlParameter)((ICloneable)parm).Clone();
                        //    cmd.Parameters.Add(clonedParameter);                            
                        //}
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 获得统一时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        private static void LogSql(string methodName, string sqlText,SqlParameter[] sqlParams)
        {
            try
            {
                string paramStr = string.Empty;
                if (sqlParams != null)
                {              
                    foreach (SqlParameter param in sqlParams)
                    {
                        paramStr = paramStr + Environment.NewLine + string.Format(MsgSqlParam,
                                                            param.ParameterName,
                                                            param.Value == null ? string.Empty : param.Value.ToString(),
                                                            param.SqlDbType.ToString(),
                                                            param.Direction.ToString());
                    }                   
                }
                if (Logger.IsDebugEnabled)
                {
                    Logger.DebugFormat(MsgSQL, methodName, sqlText, paramStr);
                }
                else
                {
                    Logger.ErrorFormat(MsgSQL, methodName, sqlText, paramStr);
                }
            }
            catch (Exception e)
            {
                Logger.Error("LogSql", e);
            }
            finally
            {
            }

        }        
    }
}
