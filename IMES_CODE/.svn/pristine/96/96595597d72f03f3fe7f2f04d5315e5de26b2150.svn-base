using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Transactions;
using System.Xml;
using System.Data;
using System.IO;

namespace ArchiveDB
{
    public static class SQLTool
    {


        public static DataSet GetDataByDataSet(SqlConnection con, string cmdText)
        {
            SqlDataAdapter daData =
                new SqlDataAdapter(cmdText, con);
            DataSet dsData = new DataSet();
            daData.Fill(dsData);
            return dsData;

        }
        public static void ExcuteSQL(SqlConnection con, string cmdText)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand(cmdText, con);
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " SQL : " + cmdText);
            }
            finally
            {
                con.Close();
            }
        }
        public static DataTable GetDataByDataTable(SqlConnection con, string cmdText)
        {
            try
            {
                
                SqlDataAdapter daData =
                  new SqlDataAdapter(cmdText, con);
                daData.SelectCommand.CommandTimeout = 0;
                DataSet dsData = new DataSet();
                daData.Fill(dsData);
                return dsData.Tables[0];

            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " SQL : " + cmdText);
            }
            finally
            {
                con.Close();
            }
        }
        public static object ExecuteScalar(SqlConnection con, string cmdText)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand(cmdText, con);
               return sqlCmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " SQL : " + cmdText);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
