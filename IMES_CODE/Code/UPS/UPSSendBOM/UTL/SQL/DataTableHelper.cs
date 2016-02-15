using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data.Common;

namespace UPS.UTL.LINQhelper
{
    public static class DataTableHelper
    {
        // Helper function for ADO.Net Bulkcopy to transfer a IEnumerable list to a datatable
        // Adapted from: http://msdn.microsoft.com/en-us/library/bb396189.aspx
        public static DataTable CopyToDataTable<T>(this IEnumerable<T> source)
        {
            return new DataTableCreator<T>().CreateDataTable(source, null, null);
        }

        public static DataTable CopyToDataTable<T>(this IEnumerable<T> source, DataTable table, LoadOption? options)
        {
            return new DataTableCreator<T>().CreateDataTable(source, table, options);
        }

        public static void BulkCopyToDatabase<T>(this IEnumerable<T> source, string tableName, System.Data.Linq.DataContext databaseContext) where T : class
        {
            using (var dataTable = DataTableHelper.CopyToDataTable(source))
            {
                using (var bulkCopy = new SqlBulkCopy(databaseContext.Connection.ConnectionString, SqlBulkCopyOptions.KeepIdentity & SqlBulkCopyOptions.KeepNulls))
                {
                    SqlConnection connect = (SqlConnection)databaseContext.Connection;
                    IList<String> destColumn = GetSchema(connect,null, tableName);
                    foreach (DataColumn dc in dataTable.Columns)
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(dc.ColumnName, dc.ColumnName));

                    //  We could use "dataTable.TableName" in the following line, but this does sometimes have problems, as 
                    //  LINQ-to-SQL will drop trailing "s" off table names, so try to insert into [Product], rather than [Products]
                    bulkCopy.DestinationTableName = "[" + tableName + "]";
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }

        public static void BulkCopyToDatabase<T>(this IEnumerable<T> source, string tableName, SqlConnection dbConnect, SqlTransaction trans ) where T : class
        {
            using (var dataTable = DataTableHelper.CopyToDataTable(source))
            {
                using (var bulkCopy = new SqlBulkCopy(dbConnect,
                    SqlBulkCopyOptions.KeepIdentity & SqlBulkCopyOptions.KeepNulls, trans  ))
                {                    
                    IList<String> destColumn= GetSchema(dbConnect, trans, tableName);
                    foreach (DataColumn dc in dataTable.Columns)
                    {
                        if (destColumn.Contains(dc.ColumnName))
                        {
                            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(dc.ColumnName, dc.ColumnName));
                        }
                    }
                    //  We could use "dataTable.TableName" in the following line, but this does sometimes have problems, as 
                    //  LINQ-to-SQL will drop trailing "s" off table names, so try to insert into [Product], rather than [Products]
                    bulkCopy.DestinationTableName = "[" + tableName + "]";
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }       

        private static IList<string> GetSchema(SqlConnection dbConnect,SqlTransaction trans, string tableName)
        {
            IList<string> ret = new List<string>();
            using (SqlCommand command = dbConnect.CreateCommand())
            {
                if (trans != null)
                {
                    command.Transaction = trans;
                }
                command.CommandText = "sp_columns";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@table_name", SqlDbType.NVarChar, 384).Value = tableName;
               
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.Add( (string)reader["column_name"]);
                    }
                }
            }
            return ret;
        }
       
    }
}
