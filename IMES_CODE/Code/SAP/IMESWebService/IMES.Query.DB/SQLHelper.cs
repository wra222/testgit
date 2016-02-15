using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using log4net;
using System.Reflection;
using System.Text.RegularExpressions;

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

        public static readonly ConnectionStringSettingsCollection DBConnectionStrings = ConfigurationManager.ConnectionStrings;
        #region get DB name

        public static readonly string DB_CFG = ConfigurationManager.AppSettings["DB_CFG"].ToString().Trim();
        
        public static readonly string DB_SA = ConfigurationManager.AppSettings["DB_SA"].ToString().Trim();
       
        public static readonly string DB_FA = ConfigurationManager.AppSettings["DB_FA"].ToString().Trim();
        
        public static readonly string DB_PAK = ConfigurationManager.AppSettings["DB_PAK"].ToString().Trim();

        public static readonly string [] DB_HISTROY = ConfigurationManager.AppSettings["DB_HISTORY"].ToString().Trim().Split(new char[] {',' , ';'});
        
        #endregion

        public static string ConnectionString_CFG()
        {
            return GetDBConnectionString(DBConnectionCategory.Online, DB_CFG);
        }

        public static string ConnectionString_SA()
        {
            return GetDBConnectionString(DBConnectionCategory.Online, DB_SA);
        }
        public static string ConnectionString_FA()
        {
            return GetDBConnectionString(DBConnectionCategory.Online, DB_FA);
        }
        public static string ConnectionString_PAK()
        {
            return GetDBConnectionString(DBConnectionCategory.Online, DB_PAK);
        }
        public static string ConnectionString_HISTORY(int indx)
        {            
            return GetDBConnectionString(DBConnectionCategory.History, DB_HISTROY[indx]);
        }

        public static string GetCataLog(string connectionStringX)
        {
            Regex re = new Regex(@"(Initial Catalog=|Database=){1}([\w-]+)[;]?");
            return re.Match(connectionStringX).Groups[2].Value;
        }

        private static string GetDBConnectionString(DBConnectionCategory category, string dbName)
        {
            if (category == DBConnectionCategory.Online)
            {
                return string.Format(OnLineConnectionString, dbName);

            }
            return string.Format(HistoryConnectionString, dbName);
        }

        public static string GetDBConnectionString(string connectionName,
                                                                               int dbIndx)
        {
            if (DBConnectionStrings[connectionName] != null)
            {

                if (ConfigurationManager.AppSettings[connectionName] == null)
                    throw new Exception(string.Format("No DB Connection Name : {0} in [appSettings] section of configure file", connectionName));

                string dbName = ConfigurationManager.AppSettings[connectionName].ToString().Trim().Split(new char[] { ',', ';' })[dbIndx];
                return string.Format(DBConnectionStrings[connectionName].ToString().Trim(), dbName);       
            }

            throw new Exception(string.Format("No DB Connection Name : {0} in [connectionStrings] section of configure file",connectionName));
            
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

        static public SqlParameter CreateSqlParameter(string name,
                                                      string tableName, 
                                                     DataTable value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Structured);
            param.Direction = ParameterDirection.Input;
            param.TypeName = tableName;
            param.Value = value;
            return param;

        }
        #endregion
      

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
                        logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + (parm.Value==null ? "": parm.Value.ToString()) + " [Direction]:" + parm.Direction.ToString());
                    }
                }               
                return cmd.ExecuteNonQuery();              
            }
            catch(Exception e) 
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
                        logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + (parm.Value == null ? "" : parm.Value.ToString()) + " [Direction]:" + parm.Direction.ToString());
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
                        logger.Debug("SQL Paraneter [Name]:" + parm.ParameterName + "  [Value]:" + (parm.Value == null ? "" : parm.Value.ToString()) + " [Direction]:" + parm.Direction.ToString());
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
    }
}
