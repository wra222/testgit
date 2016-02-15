using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using UTL.SQL;
using UTL;
using log4net;
using System.Reflection;
using System.Collections;

namespace UTL.SQL
{
    public class SQLStatement
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //private static string TimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        static public DataTable executeSPWithReturn(string connectionString,
                                                     string SPName,
                                                    params SqlParameter[] cmdParms)
        {
            DataTable Ret = new DataTable();
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                return SQLHelper.ExecuteDataFill(connectionString, CommandType.StoredProcedure, SPName, cmdParms);

            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                return Ret;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }

        }


        static public object executeSP(string connectionString,
                                                       string SPName,
                                                       params SqlParameter[] cmdParms)
        {
            DataTable Ret = new DataTable();
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                SQLHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, SPName, cmdParms);
                if (cmdParms != null)
                {
                    foreach (SqlParameter item in cmdParms)
                    {
                        if (item.Direction == ParameterDirection.Output)
                        {
                            return item.Value;
                        }
                    }                    
                }

                return null;
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }


        static public void BulkInsertCTOBOM(string strConnect,
                                                                      DataTable dt,
                                                                       IList<string> modelList)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                using (SqlConnection conn = new SqlConnection(strConnect))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        string strSQL = @"delete CTOBom 
                                                        from CTOBom a
                                                        inner join @tb b on a.MPno =b.data";

                        SqlParameter para = new SqlParameter("@tb", SqlDbType.Structured);
                        para.TypeName = "TbStringList";
                        para.Direction = ParameterDirection.Input;
                        para.Value = SQLHelper.ToDataTable(modelList);

                        SQLHelper.ExecuteNonQuery(transaction, CommandType.Text, strSQL, para);
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction))
                        {
                            bulkCopy.BatchSize = 5000;
                            bulkCopy.BulkCopyTimeout = 0;
                            bulkCopy.DestinationTableName = "dbo.CTOBom";

                            bulkCopy.WriteToServer(dt);

                        }
                        transaction.Commit();
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }


    }

}
