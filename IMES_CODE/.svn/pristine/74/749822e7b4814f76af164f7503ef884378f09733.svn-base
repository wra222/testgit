using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using log4net;
using System.Reflection;
using System.Configuration;

namespace UTL.SQL
{

    public static class SQLHelper
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string TimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        #region create input SQL parameter
        static public void createInputSqlParameter(SqlCommand cmd,
                                                                                string name,
                                                                                 int size,
                                                                                    string value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.VarChar, size);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            logger.DebugFormat("SQLParameter: {0} = {1}", name, value);
        }

        static public void createInputSqlParameter(SqlCommand cmd,
                                                                              string name,
                                                                                  string value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.VarChar);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            logger.DebugFormat("SQLParameter: {0} = {1}", name, value);
        }

        static public void createInputSqlParameter(SqlCommand cmd,
                                                              string name,
                                                              DateTime value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.DateTime);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            logger.DebugFormat("SQLParameter: {0} = {1}", name, value.ToString(TimeFormat));
        }

        static public void createInputSqlParameter(SqlCommand cmd,
                                                              string name,
                                                              int size,
                                                              DateTime value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.DateTime);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            logger.DebugFormat("SQLParameter: {0} = {1}", name, value.ToString(TimeFormat));
        }

        static public void createInputSqlParameter(SqlCommand cmd,
                                                              string name,
                                                              int size,
                                                              int value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.Int);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            logger.DebugFormat("SQLParameter: {0} = {1}", name, value.ToString());
        }
        static public void createInputSqlParameter(SqlCommand cmd,
                                                              string name,
                                                               int value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.Int);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            logger.DebugFormat("SQLParameter: {0} = {1}", name, value.ToString());
        }
        static public void createInputSqlParameter(SqlCommand cmd,
                                                                    string name,
                                                                     int size,
                                                                     float value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.Float);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            logger.DebugFormat("SQLParameter: {0} = {1}", name, value.ToString());
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

        public static SqlDataReader ExecuteReader(string connectionString,
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

        public static DataTable ToDataTable<T>(this IList<T> data)
        {

            DataTable table = new DataTable();
            //special handling for value types and string
            if (typeof(T).IsValueType || typeof(T).Equals(typeof(string)))
            {

                DataColumn dc = new DataColumn("data");

                table.Columns.Add(dc);

                foreach (T item in data)
                {
                    DataRow dr = table.NewRow();
                    dr[0] = item;

                    table.Rows.Add(dr);
                }
            }
            else
            {
                //無法確定MetadataToken 順序, 確認可以使用
                IList<PropertyInfo> properties = typeof(T).GetProperties().OrderBy(x => x.MetadataToken).ToList();
                //IList<PropertyInfo> properties = typeof(T).GetProperties().OrderBy(p => ((META.OrderAttribute)p.GetCustomAttributes(typeof(META.OrderAttribute), false)[0]).Order).ToList();

                foreach (PropertyInfo prop in properties)
                {
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }

                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyInfo prop in properties)
                    {
                        try
                        {
                            row[prop.Name] = prop.GetValue(item, null) ?? DBNull.Value;
                        }
                        catch (Exception ex)
                        {
                            row[prop.Name] = DBNull.Value;
                        }

                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }
        #endregion
    }
}
