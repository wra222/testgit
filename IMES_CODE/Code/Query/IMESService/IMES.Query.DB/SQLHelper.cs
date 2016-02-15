using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using log4net;
using System.Reflection;

namespace IMES.Query.DB
{
    public static class SQLHelper
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum DBConnectionCategory
        {
            Online,
            History
        }
        // Get Configure DB connection string
        public static readonly string OnLineConnectionString = ConfigurationManager.ConnectionStrings["OnlineDBServer"].ToString().Trim();
        public static readonly string HistoryConnectionString = ConfigurationManager.ConnectionStrings["HistoryDBServer"].ToString().Trim();

        #region get DB name

        public static readonly string DB_CFG =  ConfigurationManager.AppSettings["DB_CFG"].ToString().Trim();

        public static readonly string[] DB_CFG_LIST = ConfigurationManager.AppSettings["DB_CFG"].ToString().Trim().Split(new char[] { ',', ';' });
        
        //public static readonly string DB_SA = ConfigurationManager.AppSettings["DB_PCA"].ToString().Trim();
       
        //public static readonly string DB_FA = ConfigurationManager.AppSettings["DB_FA"].ToString().Trim();
        
        //public static readonly string DB_PAK = ConfigurationManager.AppSettings["DB_PAK"].ToString().Trim();

        public static readonly string [] DB_HISTROY = ConfigurationManager.AppSettings["DB_HISTORY"].ToString().Trim().Split(new char[] {',' , ';'});
        
        #endregion

        public static string ConnectionString_CFG()
        {
            return GetDBConnectionString(DBConnectionCategory.Online, DB_CFG);
        }

        //public static string ConnectionString_SA()
        //{
        //    return GetDBConnectionString(DBConnectionCategory.Online, DB_SA);
        //}
        //public static string ConnectionString_FA()
        //{
        //    return GetDBConnectionString(DBConnectionCategory.Online, DB_FA);
        //}
        //public static string ConnectionString_PAK()
        //{
        //    return GetDBConnectionString(DBConnectionCategory.Online, DB_PAK);
        //}

        public static string ConnectionString_ONLINE(int indx)
        {
            return GetDBConnectionString(DBConnectionCategory.Online, DB_CFG_LIST[indx]);
        }

        public static string ConnectionString_HISTORY(int indx)
        {
            return GetDBConnectionString(DBConnectionCategory.History, DB_HISTROY[indx]);
        }

        private static string GetDBConnectionString(DBConnectionCategory category, string dbName)
        {
            if (category == DBConnectionCategory.Online)
            {
                return string.Format(OnLineConnectionString, dbName);

            }
            return string.Format(HistoryConnectionString, dbName);
        }


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
        static public SqlParameter CreateSqlParameter(   string name,
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

        static public SqlParameter CreateSqlParameter( string name,                                                                            
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

        #region Create Table Schema
        public static System.Data.DataTable CreateStringListTb()
        {
            System.Data.DataTable list = new System.Data.DataTable("TbStringList");
            list.Columns.Add("data", typeof(string));

            return list;
        }

        public static System.Data.DataTable CreateIntListTb()
        {
            System.Data.DataTable list = new System.Data.DataTable("TbIntList");
            list.Columns.Add("data", typeof(int));

            return list;
        } 
        #endregion
        public static DataTable ExecuteDataFill(string connectionString, 
                                                                        CommandType cmdType, 
                                                                        string cmdText,
                                                                        params SqlParameter[] cmdParms)
        {
            //Vincent add for read uncommitted data
            logger.Debug("Connect String :" + connectionString);
            if (cmdType== CommandType.Text)
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
                        logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + (parm.Value == null ? "" : parm.Value.ToString()) + " [Direction]:" + parm.Direction.ToString());
                    }
                }
                sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);                
                return dt;
            }
            catch(Exception e)
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
                        logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + (parm.Value == null ? "" : parm.Value.ToString()) + " [Direction]:" + parm.Direction.ToString());
                    }
                }               
                return cmd.ExecuteNonQuery();              
            }
            catch(Exception e) 
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

        public static string GetSelectedDB(string DBType, int DBIndex)
        {
            string Connection = "";
            if (DBType == "HistoryDB")
            {
                Connection = SQLHelper.ConnectionString_HISTORY(DBIndex);
            }
            else
            {
                Connection = SQLHelper.ConnectionString_CFG();
            }
            return Connection;
        }

        public static DataSet ExecuteDataFillForDataSet(string connectionString,
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
                        logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + (parm.Value == null ? "" : parm.Value.ToString()) + " [Direction]:" + parm.Direction.ToString());
                    }
                }
                sda = new SqlDataAdapter(cmd);
                DataSet dsData = new DataSet();
                sda.Fill(dsData);
                return dsData;
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

        public static List<T> ExecuteReader<T>(string connectionString,
                                                                  CommandType cmdType,
                                                                  string cmdText,
                                                                  params SqlParameter[] cmdParms) where T : new()
        {
            //Vincent add for read uncommitted data
            logger.Debug("Connect String :" + connectionString);
            if (cmdType == CommandType.Text)
                cmdText = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; \n" + cmdText;

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

    }
}
