using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using log4net;
using System.Reflection;
using System.Configuration;
using System.Data.Linq;

namespace UTL.SQL
{
    public static class SQLHelper
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string TimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        #region create input SQL parameter
        static public SqlParameter createInputSqlParameter(string name,
                                                                                 int size,
                                                                                    string value)
        {
            
            SqlParameter param = new SqlParameter(name, SqlDbType.VarChar, size);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            logger.DebugFormat("SQLParameter: {0} = {1}", name, value);
            return param;
        }

        static public SqlParameter createInputSqlParameter(  string name,
                                                                                  string value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.VarChar);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            logger.DebugFormat("SQLParameter: {0} = {1}", name, value);
            return param;
        }

        static public SqlParameter createInputSqlParameter(string name,
                                                              DateTime value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.DateTime);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            logger.DebugFormat("SQLParameter: {0} = {1}", name, value.ToString(TimeFormat));
            return param;
        }

        static public SqlParameter createInputSqlParameter(string name,
                                                              int size,
                                                              DateTime value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.DateTime);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            logger.DebugFormat("SQLParameter: {0} = {1}", name, value.ToString(TimeFormat));
            return param;
        }

        static public SqlParameter createInputSqlParameter(SqlCommand cmd,
                                                              string name,
                                                              int size,
                                                              int value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Int);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            logger.DebugFormat("SQLParameter: {0} = {1}", name, value.ToString());
            return param;
        }
        static public SqlParameter createInputSqlParameter(SqlCommand cmd,
                                                              string name,
                                                               int value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Int);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            logger.DebugFormat("SQLParameter: {0} = {1}", name, value.ToString());
            return param;
        }
        static public SqlParameter createInputSqlParameter( string name,
                                                                     int size,
                                                                     float value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Float);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            logger.DebugFormat("SQLParameter: {0} = {1}", name, value.ToString());
            return param;
        }

        static public SqlParameter createInputSqlParameter(string name,
                                                                    List<string> value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Structured);
            param.Direction = ParameterDirection.Input;
            param.TypeName = "TbStringList";
            param.Value = SQLReader.ToDataTable(value);
           
            logger.DebugFormat("SQLParameter: {0} = {1}", name, string.Join(",", value.ToArray()));
            return param;
        }

        static public SqlParameter createInputSqlParameter(string name,
                                                                    List<int> value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Structured);
            param.Direction = ParameterDirection.Input;
            param.TypeName = "TbIntList";
            param.Value = SQLReader.ToDataTable(value);

            logger.DebugFormat("SQLParameter: {0} = {1}", name, string.Join(",", (value.Select(x=>x.ToString())).ToArray()));
            return param;
        }
        #endregion

        #region execute SQL


        public static DataTable ExecuteDataFill(string connectionString,
                                                                       CommandType cmdType,
                                                                       string cmdText,
                                                                       params SqlParameter[] cmdParms)
        {
            //Vincent add for read uncommitted data
            logger.Debug("Connect String :" + connectionString);
            if (cmdType == CommandType.Text)
                cmdText = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; \n" + cmdText;

            int timeout = ConfigurationManager.AppSettings["SQLCmdTimeOut"] == null ? 30 : int.Parse(ConfigurationManager.AppSettings["SQLCmdTimeOut"]);

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
                cmd.CommandTimeout = timeout;
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                    {
                        cmd.Parameters.Add(parm);
                        //logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + (parm.Value == null ? "" : parm.Value.ToString()) + " [Direction]:" + parm.Direction.ToString());
                    }
                }
                sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
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


        public static int ExecuteNonQuery(string connectionString,
                                                                CommandType cmdType,
                                                                string cmdText,
                                                                params SqlParameter[] cmdParms)
        {
            logger.Debug("Connect String :" + connectionString);
            logger.Debug("SQL Statement :" + cmdText);
            int timeout = ConfigurationManager.AppSettings["SQLCmdTimeOut"] == null ? 30 : int.Parse(ConfigurationManager.AppSettings["SQLCmdTimeOut"]);
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
                    }
                }
                return cmd.ExecuteNonQuery();
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

        public static int ExecuteNonQuery(SqlTransaction trans,
                                                                CommandType cmdType,
                                                                string cmdText,
                                                                params SqlParameter[] cmdParms)
        {
            //logger.Debug("Connect String :" + connectionString);
            logger.Debug("SQL Statement :" + cmdText);
            int timeout = ConfigurationManager.AppSettings["SQLCmdTimeOut"] == null ? 30 : int.Parse(ConfigurationManager.AppSettings["SQLCmdTimeOut"]);
            SqlCommand cmd = new SqlCommand();
            //SqlConnection conn = new SqlConnection(connectionString);
            try
            {

                //if (conn.State != ConnectionState.Open)
                //    conn.Open();
                cmd.Transaction = trans;
                cmd.Connection = trans.Connection;
                cmd.CommandTimeout = timeout;
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                    {
                        cmd.Parameters.Add(parm);
                        //logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + (parm.Value == null ? "" : parm.Value.ToString()) + " [Direction]:" + parm.Direction.ToString());
                    }
                }
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                logger.Error(e.StackTrace);
                throw;
            }
            finally
            {
                //conn.Close();
            }
        }

        public static SqlDataReader ExecuteReader(SqlConnection conn,
                                                                                 CommandType cmdType,
                                                                                 string cmdText,
                                                                                params SqlParameter[] cmdParms)
        {

            //Vincent add for read uncommitted data
            //logger.Debug("Connect String :" + connectionString);
            if (cmdType == CommandType.Text)
                cmdText = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; \n" + cmdText;

            int timeout = ConfigurationManager.AppSettings["SQLCmdTimeOut"] == null ? 30 : int.Parse(ConfigurationManager.AppSettings["SQLCmdTimeOut"]);

            logger.Debug("SQL Statement :" + cmdText);

            SqlCommand cmd = new SqlCommand();
            //SqlConnection conn = new SqlConnection(connectionString);

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
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
                    }
                }


                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception e)
            {
                conn.Close();
                logger.Error(e.Message);
                logger.Error(e.StackTrace);
                throw;
            }
            finally
            {

            }
        }

       
        #endregion
    }

}
