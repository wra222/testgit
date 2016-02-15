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

namespace IMES.LD
{
    public  static class SqlHelper
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string connStr = ConfigurationManager.ConnectionStrings["OnlineDBServer"].ConnectionString;

        public static DataTable ExecuteDataTable(CommandType cmdType, string cmdText,
                                                                         params SqlParameter[] parameters)
        {

            logger.Debug("Connect String :" + connStr);
            logger.Debug("SQL Statement :" + cmdText);

            int timeout = ConfigurationManager.AppSettings["SQLCmdTimeOut"] == null ? 30 : int.Parse(ConfigurationManager.AppSettings["SQLCmdTimeOut"]);

            using (SqlConnection conn = new SqlConnection(connStr))
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

        public static int ExecuteNonQuery(CommandType cmdType, string cmdText,
                                                 params SqlParameter[] parameters)
        {
            logger.Debug("Connect String :" + connStr);
            logger.Debug("SQL Statement :" + cmdText);

            int timeout = ConfigurationManager.AppSettings["SQLCmdTimeOut"] == null ? 30 : int.Parse(ConfigurationManager.AppSettings["SQLCmdTimeOut"]);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandTimeout = timeout;
                    cmd.CommandText = cmdText;
                    cmd.CommandType = cmdType;
                    cmd.Parameters.AddRange(parameters);

                    int str = cmd.ExecuteNonQuery();
                    return str;
                }
            }
        }


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


    }

}
