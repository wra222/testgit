using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace IMES.BartenderPrinter
{
    /// <summary>
    /// ADO.NET使用帮助类
    /// </summary>
    public abstract class SqlHelper
    {
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["OnlineDBServer"].ToString();
    

        private static string GetCataLog(string connectionStringX)
        {
            string[] toTrim = ConnectionString.Split(new string[] { "{0}" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in toTrim)
            {
                connectionStringX = connectionStringX.Replace(str, string.Empty);
            }
            return connectionStringX;
        }

        /// <summary>
        /// 数据库连接字符串 DB_IMES
        /// </summary>
        public static string ConnectionString_IMES
        {
            get
            {
                return string.Format(ConnectionString, DB_IMES);
            }
        }    
        /// <summary>
        /// DB Catalog名称 DB_IMES
        /// </summary>
        public static readonly string DB_IMES = ConfigurationManager.AppSettings["DB_IMES"].ToString();
       

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

       
        public static SqlDataReader ExecuteReader(string connectionString, 
                                                                            CommandType cmdType, 
                                                                            string cmdText, 
                                                                             params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {   
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
            finally
            {
                
            }
        }

     
        

       

        
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

               
                cmd.CommandType = cmdType;

                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                    {
                        if (parm.Value == null)
                            parm.Value = DBNull.Value;
                        if (forOutParam && (parm.Direction == ParameterDirection.Output || parm.Direction == ParameterDirection.ReturnValue || parm.Direction == ParameterDirection.InputOutput))
                        {
                            cmd.Parameters.Add(parm);
                        }
                        else
                        {
                            SqlParameter clonedParameter = (SqlParameter)((ICloneable)parm).Clone();
                            cmd.Parameters.Add(clonedParameter);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #region Create SQL parameter
        static public SqlParameter CreateSqlParameter(string name,
                                                                                       int size,
                                                                                      string value,
                                                                                      ParameterDirection direction)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.VarChar, size);
            param.Direction = direction;
            if (direction == ParameterDirection.Input ||
              direction == ParameterDirection.InputOutput)
            {
                param.Value = value;
            }
            return param;
        }
        static public SqlParameter CreateSqlParameter(string name,
                                                                                              DateTime value,
                                                                                              ParameterDirection direction)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.DateTime);
            param.Direction = direction;
            if (direction == ParameterDirection.Input ||
               direction == ParameterDirection.InputOutput)
            {
                param.Value = value;
            }
            return param;
        }

        static public SqlParameter CreateSqlParameter(string name,
                                                                                               int value,
                                                                                               ParameterDirection direction)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Int);
            param.Direction = direction;
            if (direction == ParameterDirection.Input ||
               direction == ParameterDirection.InputOutput)
            {
                param.Value = value;
            }
            return param;
        }

        static public SqlParameter CreateSqlParameter(string name,
                                                                                              long value,
                                                                                              ParameterDirection direction)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.BigInt);
            param.Direction = direction;
            if (direction == ParameterDirection.Input ||
               direction == ParameterDirection.InputOutput)
            {
                param.Value = value;
            }
            return param;
        }

        static public SqlParameter CreateSqlParameter(string name,
                                                                                              float value,
                                                                                             ParameterDirection direction)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Float);
            param.Direction = direction;
            if (direction == ParameterDirection.Input ||
                direction == ParameterDirection.InputOutput)
            {
                param.Value = value;
            }
            return param;

        }
        #endregion


        #region Create Input SQL parameter

        static public SqlParameter CreateSqlParameter(string name,
                                                                                     int size,
                                                                                    string value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.VarChar, size);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            return param;
        }

        static public SqlParameter CreateSqlParameter(string name,
                                                                                       string value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.VarChar);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            return param;
        }

        static public SqlParameter CreateSqlParameter(string name,
                                                                                              DateTime value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.DateTime);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            return param;
        }

        static public SqlParameter CreateSqlParameter(string name,
                                                                                               int value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Int);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            return param;
        }

        static public SqlParameter CreateSqlParameter(string name,
                                                                                              long value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.BigInt);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            return param;
        }

        static public SqlParameter CreateSqlParameter(string name,
                                                                                       float value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Float);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            return param;

        }
        #endregion
        /// <summary>
        /// 获得统一时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDateTime()
        {
            return DateTime.Now;
        }
    }
}
