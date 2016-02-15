using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using log4net;

namespace Inventec.HPEDITS.XmlCreator.Database
{
    public static class SQLHelper
    {
        static SQLHelper()
        {
            if (null != ConfigurationManager.AppSettings["Database"])
                OnLineConnectionString = ConfigurationManager.AppSettings["Database"].ToString();
            if (null != ConfigurationManager.AppSettings["HistoryDBServer"])
                HistoryConnectionString = ConfigurationManager.ConnectionStrings["HistoryDBServer"].ToString().Trim();
        }

        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum DBConnectionCategory
        {
            Online,
            History
        }
        // Get Configure DB connection string
        public static readonly string OnLineConnectionString;
        public static readonly string HistoryConnectionString;

        #region get DB name

        public static readonly string DB_CFG; // =  ConfigurationManager.AppSettings["DB_CFG"].ToString().Trim();

        public static readonly string[] DB_CFG_LIST; // = ConfigurationManager.AppSettings["DB_CFG"].ToString().Trim().Split(new char[] { ',', ';' });
        
        //public static readonly string DB_SA = ConfigurationManager.AppSettings["DB_PCA"].ToString().Trim();
       
        //public static readonly string DB_FA = ConfigurationManager.AppSettings["DB_FA"].ToString().Trim();
        
        //public static readonly string DB_PAK = ConfigurationManager.AppSettings["DB_PAK"].ToString().Trim();

        public static readonly string [] DB_HISTROY; // = ConfigurationManager.AppSettings["DB_HISTORY"].ToString().Trim().Split(new char[] {',' , ';'});
        
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
        #endregion
      

        public static DataTable ExecuteDataFill(string connectionString, 
                                                                        CommandType cmdType, 
                                                                        string cmdText,
                                                                        params SqlParameter[] cmdParms)
        {
//            logger.Debug("SQL Statement :" + cmdText);

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
//                        logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + (parm.Value == null ? "" : parm.Value.ToString()) + " [Direction]:" + parm.Direction.ToString());
                    }
                }
                sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);                
                return dt;
            }
            catch(Exception e)
            {
                StringBuilder sb = new StringBuilder(256);
                sb.Append(e.Message).Append(Environment.NewLine).Append("cmdText= ").Append(cmdText).Append(Environment.NewLine).Append("cmdParms= ").Append(cmdParms.ToString());
                logger.Error(sb.ToString());
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
//            logger.Debug("SQL Statement :" + cmdText);
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
//                        logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + (parm.Value == null ? "" : parm.Value.ToString()) + " [Direction]:" + parm.Direction.ToString());
                    }
                }               
                return cmd.ExecuteNonQuery();              
            }
            catch(Exception e) 
            {
                StringBuilder sb = new StringBuilder(256);
                sb.Append(e.Message).Append(Environment.NewLine).Append("cmdText= ").Append(cmdText).Append(Environment.NewLine).Append("cmdParms= ").Append(cmdParms.ToString());
                logger.Error(sb.ToString());
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
    }
}
