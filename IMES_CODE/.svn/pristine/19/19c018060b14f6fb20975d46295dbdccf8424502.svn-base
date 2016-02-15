using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using log4net;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace IMES.LD
{
    public  static class SqlHelper
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region disable code
        //public static string connStr = ConfigurationManager.ConnectionStrings["OnlineDBServer"].ConnectionString;

        //public static DataTable ExecuteDataTable(CommandType cmdType, string cmdText,
        //                                                                 params SqlParameter[] parameters)
        //{

        //    logger.Debug("Connect String :" + connStr);
        //    logger.Debug("SQL Statement :" + cmdText);

        //    int timeout = ConfigurationManager.AppSettings["SQLCmdTimeOut"] == null ? 30 : int.Parse(ConfigurationManager.AppSettings["SQLCmdTimeOut"]);

        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandTimeout = timeout;
        //            cmd.CommandText = cmdText;
        //            cmd.CommandType = cmdType;
        //            cmd.Parameters.AddRange(parameters);

        //            DataSet dataset = new DataSet();
        //            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //            adapter.Fill(dataset);
        //            return dataset.Tables[0];
        //        }
        //    }
        //}

        //public static int ExecuteNonQuery(CommandType cmdType, string cmdText,
        //                                         params SqlParameter[] parameters)
        //{
        //    logger.Debug("Connect String :" + connStr);
        //    logger.Debug("SQL Statement :" + cmdText);

        //    int timeout = ConfigurationManager.AppSettings["SQLCmdTimeOut"] == null ? 30 : int.Parse(ConfigurationManager.AppSettings["SQLCmdTimeOut"]);

        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandTimeout = timeout;
        //            cmd.CommandText = cmdText;
        //            cmd.CommandType = cmdType;
        //            cmd.Parameters.AddRange(parameters);

        //            int str = cmd.ExecuteNonQuery();
        //            return str;
        //        }
        //    }
        //}
        #endregion
        #region execute SQL
        public static DataTable ExecuteDataTable(string dbConnectionStringName,
                                                                        CommandType cmdType, 
                                                                        string cmdText,
                                                                        params SqlParameter[] parameters)
        {

            string dbConnStr = ConfigurationManager.ConnectionStrings[dbConnectionStringName].ConnectionString;

             logger.Debug("Connect String :" + dbConnStr);
            logger.Debug("SQL Statement :" + cmdText);

            int timeout = ConfigurationManager.AppSettings["SQLCmdTimeOut"] == null ? 30 : int.Parse(ConfigurationManager.AppSettings["SQLCmdTimeOut"]);

            using (SqlConnection conn = new SqlConnection(dbConnStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandTimeout = timeout;
                    cmd.CommandText = cmdText;
                    cmd.CommandType = cmdType;
                    cmd.Parameters.AddRange(parameters);

                    DataSet dataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dataset);
                    return dataset.Tables[0];
                }
            }           
        }

        public static int ExecuteNonQuery(string dbConnectionStringName,
                                                           CommandType cmdType, 
                                                            string cmdText,
                                                 params SqlParameter[] parameters)
        {
            string dbConnStr = ConfigurationManager.ConnectionStrings[dbConnectionStringName].ConnectionString;
            logger.Debug("Connect String :" + dbConnStr);
            logger.Debug("SQL Statement :" + cmdText);

            int timeout = ConfigurationManager.AppSettings["SQLCmdTimeOut"] == null ? 30 : int.Parse(ConfigurationManager.AppSettings["SQLCmdTimeOut"]);

            using (SqlConnection conn = new SqlConnection(dbConnStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandTimeout = timeout;
                    cmd.CommandText = cmdText;
                    cmd.CommandType = cmdType;
                    cmd.Parameters.AddRange(parameters);

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<T> ExecuteReader<T>(string connectionString,
                                                                 CommandType cmdType,
                                                                 string cmdText,
                                                                 params SqlParameter[] cmdParms) where T : new()
        {
            //Vincent add for read uncommitted data
            logger.Debug("Connect String :" + connectionString);
            //if (cmdType == CommandType.Text)
            //    cmdText = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; \n" + cmdText;

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
                        logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + (parm.Value == null ? "" : parm.Value.ToString()) + " [Direction]:" + parm.Direction.ToString());
                    }
                }

                List<T> res = new List<T>();
                using (SqlDataReader rs = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {

                    while (rs.Read())
                    {
                        T t = new T();
                        int count = rs.FieldCount;
                        for (int i = 0; i < count; ++i)
                        {
                            PropertyInfo pInfo = t.GetType().GetProperty(rs.GetName(i));
                            if (pInfo != null)
                                if (rs.IsDBNull(i))
                                    pInfo.SetValue(t, null, null);
                                else
                                    pInfo.SetValue(t, rs[i], null);

                        }
                        res.Add(t);
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

        #endregion
        #region Create SQL parameter
        static public SqlParameter CreateSqlParameter(string name,
                                                                                       string value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.VarChar);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
            return param;
        }

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
                                                                                       DataTable value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Structured);
            param.Direction = ParameterDirection.Input;
            param.Value = value;

            return param;

        }

        static public SqlParameter CreateSqlParameter(string name,
                                                                                       string typeName,
                                                                                       DataTable value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Structured);
            param.Direction = ParameterDirection.Input;
            param.TypeName = typeName;
            param.Value = value;

            return param;

        }
        #endregion

       


    }

}
