/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: utility function for database             
 * 
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2009-5-10   ZhangXueMin     Create 
 * Known issues:Any restrictions about this file 
 *              1 xxxxxxxx
 *              2 xxxxxxxx
 */

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Collections;
using log4net;
using com.inventec.system;
using com.inventec.system.exception;
using System.Data.SqlClient;
using com.inventec.system.dao;

using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Transactions;


namespace com.inventec.portal.databaseutil
{
    [Serializable]
    public class ConditionValueSet
    {
        private String paramName;

        public String ParamName
        {
            get { return paramName; }
            set { paramName = value; }
        }
        private String dataType;

        public String DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }
        private String paramValue;

        public String ParamValue
        {
            get { return paramValue; }
            set { paramValue = value; }
        }
    }
    
    public class DatabaseUtil
    {
        log4net.ILog log = log4net.LogManager.GetLogger("SqlUtility");

        protected const Boolean isDebug = true;

        //取得连接数据库的字符串
        //by xmzhang
        public static string GetConnectionString()
        {
            return GeneralUtil.Null2String(ConfigurationManager.ConnectionStrings["ConStr"]);
        }
             

        //取得带参数的sql的执行SQL返回的Datatable,内部可以以事务方式执行此段SQL
        private static DataTable DealGetResultBySQLWidthParam(String _sqlString, String _connectionString, List<ConditionValueSet> _params)
        {

            DataTable datatable = new DataTable();
            //DebugInfo("Sql String:" + _sqlString);

            using (SqlConnection connection =
                new SqlConnection(_connectionString))
            {

                SqlDataAdapter adapter = new SqlDataAdapter();
                connection.Open();
                SqlCommand command;
                SqlTransaction transaction;
                SqlCommandDealWidthParams(_sqlString, _params, connection, out command, out transaction);
                adapter.SelectCommand = command;
                adapter.Fill(datatable);
                transaction.Commit();
                connection.Close();
            }
            return datatable;

        }

        private static void SqlCommandDealWidthParams(String _sqlString, List<ConditionValueSet> _params, SqlConnection connection, out SqlCommand command, out SqlTransaction transaction)
        {
            command = connection.CreateCommand();

            // Start a local transaction.
            transaction = connection.BeginTransaction("SampleTransaction");
            command.Connection = connection;
            command.Transaction = transaction;
            int sqlTimeOut = GeneralUtil.GetSqlTimeOut();
            if (sqlTimeOut != -1)
            {
                command.CommandTimeout = sqlTimeOut;
            }
            command.CommandText = _sqlString;
            SqlParameter[] parms;
            if (_params.Count > 0)
            {
                parms = new SqlParameter[_params.Count];
                for (int i = 0; i < _params.Count; i++)
                {
                    parms[i] = new SqlParameter();
                    parms[i].ParameterName = _params[i].ParamName;
                    parms[i].SqlDbType = TypeSwitch.GetSqlDbType(_params[i].DataType);
                    parms[i].Value = _params[i].ParamValue;
                    //log.Debug("parameter:" + parms[i].ParameterName + " " + _params[i].DataType + " " + parms[i].Value + " parameterEnd");
                    //need delete
                    //LogFile.WriteLog("parameter:" + parms[i].ParameterName + " " + _params[i].DataType + " " + parms[i].Value + " parameterEnd");
                    //对boolean类型数据判断范围变宽处理
                    if (parms[i].SqlDbType.Equals(SqlDbType.Bit))
                    {
                        if (_params[i].ParamValue.Trim().ToLower() == "true" || _params[i].ParamValue == "1")
                        {
                            parms[i].Value = "true";
                        }
                        else if (_params[i].ParamValue.Trim().ToLower() == "false" || _params[i].ParamValue == "0")
                        {
                            parms[i].Value = "false";
                        }
                        else
                        {
                            parms[i].Value = "false";
                            //String errmsg = ExceptionMsg.SQL_GENERATE_MSG_BIT_PARAM_NOT_RIGHT;
                            //ExceptionManager.Throw(errmsg);
                        }
                    }

                    if ((parms[i].SqlDbType.Equals( SqlDbType.BigInt)
                        || parms[i].SqlDbType .Equals(SqlDbType.Decimal)
                        || parms[i].SqlDbType.Equals(SqlDbType.Float)
                        || parms[i].SqlDbType .Equals( SqlDbType.Int)
                        || parms[i].SqlDbType.Equals( SqlDbType.Decimal)
                        || parms[i].SqlDbType.Equals( SqlDbType.Real)
                        || parms[i].SqlDbType.Equals( SqlDbType.SmallInt)
                        || parms[i].SqlDbType.Equals( SqlDbType.TinyInt)) && parms[i].Value.Equals(""))
                    {

                        parms[i].Value = System.DBNull.Value;
                    }

                    if (parms[i].SqlDbType.Equals(SqlDbType.DateTime))
                    {
                        parms[i].SqlDbType = SqlDbType.VarChar;
                    }

                    //DebugInfo("Sql parameter- ParameterName:" + parms[i].ParameterName + " SqlDbType:" + parms[i].SqlDbType + " Value:" + parms[i].Value);
                    command.Parameters.Add(parms[i]);
                }
            }
        }

        public static DataTable GetResultBySQL(String _sqlString, String _connectionString, List<ConditionValueSet> _params)
        {
            DataTable datatable = new DataTable();
            try
            {
                datatable = DealGetResultBySQLWidthParam(_sqlString, _connectionString, _params);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                String errmsg = ExceptionMsg.SQL_GENERATE_MSG_SQLSERVER_ERROR_PROMPT + ex.Message;
                ExceptionManager.Throw(errmsg);
            }
            catch (Exception ex)
            {
                String errmsg = ExceptionMsg.SQL_GENERATE_MSG_SQLSERVER_ERROR_PROMPT + ex.Message;
                ExceptionManager.Throw(errmsg);
            }
            return datatable;
        }

        public static void ExecSqlNonQueryWithParam(String _sqlString, String _connectionString, List<ConditionValueSet> _params)
        {
            try
            {
                DealSqlNonQueryWithParam(_sqlString, _connectionString, _params);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                String errmsg = ExceptionMsg.SQL_GENERATE_MSG_SQLSERVER_ERROR_PROMPT + ex.Message;
                ExceptionManager.Throw(errmsg);
            }
        }

        //执行带参数的sql,内部可以以事务方式执行此段SQL
        private  static void DealSqlNonQueryWithParam(String _sqlString, String _connectionString, List<ConditionValueSet> _params)
        {

            //DebugInfo("Sql String:" + _sqlString + "---sql end");
            using (SqlConnection connection =
                new SqlConnection(_connectionString))
            {
                //log.Debug("_connectionString" + _connectionString);
                connection.Open();
                SqlCommand command;
                SqlTransaction transaction;
                SqlCommandDealWidthParams(_sqlString, _params, connection, out command, out transaction);
                Int32 rowsAffected = command.ExecuteNonQuery();
                transaction.Commit();
                connection.Close();
            }
        }

        public static String GetUUID()
        {
            Guid guid = System.Guid.NewGuid();
            return guid.ToString("N");
        }

      

    }

}
