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
   
    class SQLHelper
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #region create input SQL parameter
        static public  void createInputSqlParameter(SqlCommand cmd,
                                                                                string name,
                                                                                 int size,
                                                                                    string value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.VarChar, size);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
        }
        static public void createInputSqlParameter(SqlCommand cmd,
                                                              string name,                                                             
                                                              DateTime value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.DateTime);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
        }

        static public void createInputSqlParameter(SqlCommand cmd,
                                                              string name,
                                                              int size,
                                                              int value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.Int);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
        }
        static public void createInputSqlParameter(SqlCommand cmd,
                                                              string name,
                                                               int value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.Int);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
        }
        static public  void createInputSqlParameter(SqlCommand cmd,
                                                                    string name,
                                                                     int size,
                                                                     float value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.Float);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
        }



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


        public static DataTable ExecuteDataFill(SqlConnection conn,
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
                //conn.Close();
            }
        }
        #endregion

        
    }
}
