using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using log4net;
using System.Text.RegularExpressions;
using SQLHelp;

namespace SQLHelp
{
    public static class SQLHelper
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum DBConnectionCategory
        {
            Online,
            History
        }
    

        public static string GetCataLog(string connectionStringX)
        {
            Regex re = new Regex(@"(Initial Catalog=|Database=){1}([\w-]+)[;]?");
            return re.Match(connectionStringX).Groups[2].Value;
        }

        //private static string GetDBConnectionString(DBConnectionCategory category, string dbName)
        //{
        //    if (category == DBConnectionCategory.Online)
        //    {
        //        return string.Format(OnLineConnectionString, dbName);

        //    }
        //      return string.Format(HistoryConnectionString, dbName);
        //}


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
                                                                                      string value,
                                                                                      ParameterDirection direction)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.VarChar);
            param.Direction = direction;
            if (direction == ParameterDirection.Input ||
              direction == ParameterDirection.InputOutput)
            {
                param.Value = value;
            }
            return param;
        }
        static public SqlParameter CreateSqlParameter(string name,
                                                                               string value,
                                                                               bool isUnicode,     
                                                                                ParameterDirection direction)
        {
            SqlParameter param = new SqlParameter(name, isUnicode? SqlDbType.NVarChar:SqlDbType.VarChar);
            param.Direction = direction;
            if (direction == ParameterDirection.Input ||
              direction == ParameterDirection.InputOutput)
            {
                param.Value = value;
            }
            return param;
        }

        static public SqlParameter CreateSqlParameter(string name,
                                                                               string tableName,
                                                                               DataTable value,
                                                                                ParameterDirection direction)
        {
            SqlParameter param = new SqlParameter(name,SqlDbType.Structured);
            param.Direction = direction;
            param.TypeName = tableName;
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

        static public SqlParameter CreateSqlParameter(string name,
                                                                             TimeSpan value,
                                                                            ParameterDirection direction)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Time);
            param.Direction = ParameterDirection.Input;
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
            SqlParameter param = new SqlParameter(name, SqlDbType.NVarChar, size);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            return param;
        }

        static public SqlParameter CreateSqlParameter(string name,
                                                                               string value,
                                                                                bool isUnicode)
        {
            SqlParameter param = new SqlParameter(name, isUnicode ? SqlDbType.NVarChar : SqlDbType.VarChar);
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
        static public SqlParameter CreateSqlParameter(string name,
                                                                                     int size,
                                                                                    byte value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.VarChar, size);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            return param;
        }

        static public SqlParameter CreateSqlParameter(string name,
                                                                               string tableName,
                                                                               DataTable value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Structured);
            param.Direction =  ParameterDirection.Input;
            param.TypeName = tableName;          
            param.Value = value;
          
            return param;
        }

        static public SqlParameter CreateSqlParameter(string name,
                                                                              TimeSpan value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Time);
            param.Direction = ParameterDirection.Input;            
            param.Value = value;

            return param;
        }

        static public SqlParameter CreateSqlReturnParameter(string name)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.VarChar, 1024);
            param.Direction = ParameterDirection.ReturnValue;
            return param;
        }
        #endregion

        public static DataSet ExecuteDataSetFill(string connectionString,
                                                                        CommandType cmdType,
                                                                        string cmdText,
                                                                        params SqlParameter[] cmdParms)
        {
            logger.Debug("SQL Statement :" + cmdText);

            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlDataAdapter sda = null;

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                    {
                        cmd.Parameters.Add(parm);
                        logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + parm.Value.ToString() + " [Direction]:" + parm.Direction.ToString());
                    }
                }
                sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }


        public static DataTable ExecuteDataFill(string connectionString,
                                                                        CommandType cmdType,
                                                                        string cmdText,
                                                                        params SqlParameter[] cmdParms)
        {
            logger.Debug("SQL Statement :" + cmdText);

            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlDataAdapter sda = null;

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                    {
                        cmd.Parameters.Add(parm);
                        logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + parm.Value.ToString() + " [Direction]:" + parm.Direction.ToString());
                    }
                }
                sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }


        public static int ExecuteNonQuery(string connectionString,
                                                                CommandType cmdType,
                                                                string cmdText,
                                                                params SqlParameter[] cmdParms)
        {
            logger.Debug("SQL Statement :" + cmdText);
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                    {
                        cmd.Parameters.Add(parm);
                        writeSqlParameterLog(parm);
                        //logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + parm.Value.ToString() + " [Direction]:" + parm.Direction.ToString());
                    }
                }
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public static DataTable ExecuteTxnDataFill(string connectionString,
                                                                        CommandType cmdType,
                                                                        string cmdText,
                                                                        params SqlParameter[] cmdParms)
        {
            logger.Debug("SQL Statement :" + cmdText);

            SqlConnection conn = null;


            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                SqlCommand cmd = new SqlCommand();
                conn = SQLTxnManager.GetSqlConnection(connectionString);

                SqlDataAdapter sda = null;
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlTransaction trn = SQLTxnManager.GetTransaction();
                if (trn != null)
                    cmd.Transaction = trn;
                if (SQLTxnManager.inScopeTag)
                    SQLTxnManager.ChangeCataLog(GetCataLog(connectionString));

                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                    {
                        cmd.Parameters.Add(parm);
                        writeSqlParameterLog(parm);
                        //logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + parm.Value.ToString() + " [Direction]:" + parm.Direction.ToString());
                    }
                }
                sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                SQLTxnManager.CloseSqlConnection(conn);
            }
        }


        public static int ExecuteTxnNonQuery(string connectionString,
                                                                CommandType cmdType,
                                                                string cmdText,
                                                                params SqlParameter[] cmdParms)
        {
            logger.Debug("SQL Statement :" + cmdText);
            SqlConnection conn = null;

            try
            {
                SqlCommand cmd = new SqlCommand();

                conn = SQLTxnManager.GetSqlConnection(connectionString);
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlTransaction trn = SQLTxnManager.GetTransaction();
                if (trn != null)
                    cmd.Transaction = trn;
                if (SQLTxnManager.inScopeTag)
                    SQLTxnManager.ChangeCataLog(GetCataLog(connectionString));

                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                    {
                        cmd.Parameters.Add(parm);
                        writeSqlParameterLog(parm);
                        //logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + parm.Value.ToString() + " [Direction]:" + parm.Direction.ToString());
                    }
                }
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                SQLTxnManager.CloseSqlConnection(conn);
            }
        }
        public static object ExecuteScalar(string connectionString,
                                                                    CommandType cmdType,
                                                                    string cmdText,
                                                                    params SqlParameter[] cmdParms)
        {
            logger.Debug("SQL Statement :" + cmdText);
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                    {
                        cmd.Parameters.Add(parm);
                        writeSqlParameterLog(parm);
                        //logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + parm.Value.ToString() + " [Direction]:" + parm.Direction.ToString());
                    }
                }
                return cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }


        public static List<T> ExecuteReader<T>(string connectionString,
                                                                  CommandType cmdType,
                                                                  string cmdText,
                                                                  params SqlParameter[] cmdParms) where T : new()
        {
           
            logger.Debug("Connect String :" + connectionString);
          

            int timeout = ConfigurationManager.AppSettings["SQLCmdTimeOut"] == null ? 30 : int.Parse(ConfigurationManager.AppSettings["SQLCmdTimeOut"]);

            logger.Debug("SQL Statement :" + cmdText);

            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandTimeout = timeout;
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                    {
                        cmd.Parameters.Add(parm);
                        //logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + (parm.Value == null ? "" : parm.Value.ToString()) + " [Direction]:" + parm.Direction.ToString());
                        writeSqlParameterLog(parm);
                    }
                }

                List<T> res = new List<T>();
                using (SqlDataReader rs = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (rs!=null&& rs.Read())
                    {
                        res.Add(SQLData.ToObject<T>(rs));
                    }
                }
                return res;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                logger.Error(e.StackTrace);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }



        private static void writeSqlParameterLog(SqlParameter parm)
        {
            if (parm.Direction == ParameterDirection.Output || parm.Direction == ParameterDirection.ReturnValue)
            {
                logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + " [Direction]:" + parm.Direction.ToString());
            }
            else
            {
                logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + parm.Value.ToString() + " [Direction]:" + parm.Direction.ToString());
            }
        }

    }
}