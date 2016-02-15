using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data.Common;
using System.Reflection;
using IMES.Infrastructure.UnitOfWork;
using log4net;

namespace IMES.Infrastructure.Repository._Schema
{
    /// <summary>
    /// Bulk Copy Helper
    /// </summary>
    public static class BulkCopyHelper
    {
        private readonly static ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal class DataTableCreator<T>
        {
            internal class Constant
            {
                public const string Value = "Value";
                public const string System = "System";
                public const string Search = "Search";
                public const string System_Linq = "System.Linq";
                public const string System_Data = "System.Data";
            } 
      
            public static DataTable CreateDataTable(IEnumerable<T> source, DataTable table, LoadOption? options)
            {
                if (typeof(T).IsPrimitive)
                {
                    return CreateDatatablePrimitive(source, table, options);
                }


                if (table == null)
                {
                    table = new DataTable(typeof(T).Name);
                }

                // now see if need to extend datatable base on the type T + build ordinal map
                IDictionary<string, int> ordinalMap = null;
                table = ExtendTable(table, out ordinalMap);

                table.BeginLoadData();
                using (IEnumerator<T> e = source.GetEnumerator())
                {
                    while (e.MoveNext())
                    {
                        if (options != null)
                        {
                            table.LoadDataRow(CreateDataRow(table, e.Current,ordinalMap ), (LoadOption)options);
                        }
                        else
                        {
                            table.LoadDataRow(CreateDataRow(table, e.Current,ordinalMap ), true);
                        }
                    }
                }
                table.EndLoadData();
                return table;
            }

            private static DataTable CreateDatatablePrimitive(IEnumerable<T> source, DataTable table, LoadOption? options)
            {
                if (table == null)
                {
                    table = new DataTable(typeof(T).Name);
                }

                if (!table.Columns.Contains(Constant.Value))
                {
                    table.Columns.Add(Constant.Value, typeof(T));
                }

                table.BeginLoadData();
                using (IEnumerator<T> e = source.GetEnumerator())
                {
                    Object[] values = new object[table.Columns.Count];
                    while (e.MoveNext())
                    {
                        values[table.Columns[Constant.Value].Ordinal] = e.Current;

                        if (options != null)
                        {
                            table.LoadDataRow(values, (LoadOption)options);
                        }
                        else
                        {
                            table.LoadDataRow(values, true);
                        }
                    }
                }
                table.EndLoadData();
                return table;
            }

            private static DataTable ExtendTable(DataTable table, out IDictionary<string, int> ordinalMap)
            {
                ordinalMap = new Dictionary<string, int>();
                // value is type derived from T, may need to extend table.
                FieldInfo[] fi = SQLDataCache.GetFieldInfos<T>(BindingFlags.Instance | BindingFlags.Public);
                foreach (FieldInfo f in fi) //type.GetFields())
                {
                    if (!ordinalMap.ContainsKey(f.Name))
                    {
                        DataColumn dc = table.Columns.Contains(f.Name) ? table.Columns[f.Name]
                            : table.Columns.Add(f.Name, f.FieldType);
                        ordinalMap.Add(f.Name, dc.Ordinal);
                    }
                }

                PropertyInfo[] pi = SQLDataCache.GetPropertyInfos<T>();
                foreach (PropertyInfo p in  pi) //type.GetProperties())
                {
                    if (!ordinalMap.ContainsKey(p.Name))
                    {
                        Type propType = p.PropertyType;
                        if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            propType = Nullable.GetUnderlyingType(propType);
                        }
                        //if (propType.Namespace == "System")
                        if (p.PropertyType.FullName.ToString().StartsWith(Constant.System)
                                    && !p.PropertyType.FullName.ToString().StartsWith(Constant.System_Linq)
                                    && !p.PropertyType.FullName.ToString().StartsWith(Constant.System_Data)
                                    && !p.Name.ToString().StartsWith(Constant.Search))
                        {
                            DataColumn dc = table.Columns.Contains(p.Name) ? table.Columns[p.Name]
                                : table.Columns.Add(p.Name, propType);
                            ordinalMap.Add(p.Name, dc.Ordinal);
                        }
                    }
                }
                return table;
            }

            private static object[] CreateDataRow(DataTable table, T instance, IDictionary<string, int> ordinalMap)
            {

                FieldInfo[] fi = SQLDataCache.GetFieldInfos<T>(BindingFlags.Instance |BindingFlags.Public); //_fi;
                PropertyInfo[] pi = SQLDataCache.GetPropertyInfos<T>(); //_pi;

                //if (instance.GetType() != typeof(T))
                //{
                //    ExtendTable(table, instance.GetType());
                //    fi = instance.GetType().GetFields();
                //    pi = instance.GetType().GetProperties();
                //}

                Object[] values = new object[table.Columns.Count];
                foreach (FieldInfo f in fi)
                {
                    values[ordinalMap[f.Name]] = f.GetValue(instance);
                }

                foreach (PropertyInfo p in pi)
                {
                    if (p.PropertyType.FullName.ToString().StartsWith(Constant.System)
                                && !p.PropertyType.FullName.ToString().StartsWith(Constant.System_Linq)
                                && !p.PropertyType.FullName.ToString().StartsWith(Constant.System_Data)
                                && !p.Name.ToString().StartsWith(Constant.Search))
                    {
                        values[ordinalMap[p.Name]] = p.GetValue(instance, null);
                    }
                }
                return values;
            }
        }

        internal class Constant
        {
             public const string OpenBracket = "[";
            public const string CloseBracket = "]";
            public const string sp_columns = "sp_columns";
            public const string column_name = "column_name";
            public const string table_name = "@table_name";

        }
         /// <summary>
        /// Generate Data Table from object 
         /// </summary>
         /// <typeparam name="T"></typeparam>
         /// <param name="source"></param>
         /// <returns></returns>
        public static DataTable CopyToDataTable<T>(this IEnumerable<T> source)
        {
            return DataTableCreator<T>.CreateDataTable(source, null, null);
        }
        /// <summary>
        /// Generate Data Table from object 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="table"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static DataTable CopyToDataTable<T>(this IEnumerable<T> source, DataTable table, LoadOption? options)
        {
            return DataTableCreator<T>.CreateDataTable(source, table, options);
        }

        
       /// <summary>
        /// bulk copy from data table
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="source"></param>
       /// <param name="connectionString"></param>
       /// <param name="tableName"></param>
        public static void BulkCopyToDatabase<T>(this IEnumerable<T> source, 
                                                                        string connectionString,
                                                                        string tableName) where T : class
        {
            SqlConnection dbConnect = null;
            SqlTransaction trans = null;
            try
            {                
                if (Logger.IsDebugEnabled)
                {
                    Logger.DebugFormat("Bulk Copy Table : {0} !", tableName);
                }
                dbConnect = SqlTransactionManager.GetSqlConnection(connectionString);
                if (dbConnect.State != ConnectionState.Open)
                    dbConnect.Open();
                
                if (SqlTransactionManager.inScopeTag)
                {
                    trans = SqlTransactionManager.GetTransaction();
                    SqlTransactionManager.ChangeCataLog(SqlHelper.GetCataLog(connectionString));
                }                

                using (var dataTable = BulkCopyHelper.CopyToDataTable(source))
                {
                       using (var bulkCopy = new SqlBulkCopy(dbConnect,
                        SqlBulkCopyOptions.KeepNulls, trans))
                    {
                        IList<String> destColumn = GetSchema(dbConnect, trans, tableName);
                        foreach (DataColumn dc in dataTable.Columns)
                        {
                            if (destColumn.Contains(dc.ColumnName))
                            {
                                bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(dc.ColumnName, dc.ColumnName));
                            }
                        }
                        //  We could use "dataTable.TableName" in the following line, but this does sometimes have problems, as 
                        //  LINQ-to-SQL will drop trailing "s" off table names, so try to insert into [Product], rather than [Products]
                        bulkCopy.DestinationTableName = Constant.OpenBracket + tableName + Constant.CloseBracket;
                        bulkCopy.WriteToServer(dataTable);
                    }
                }
            }
            catch(Exception e)
            {               
                 Logger.Error(e.Message,e);
                 throw;
            }
            finally
            {
                SqlTransactionManager.CloseSqlConnection(dbConnect);
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
                command.CommandText = Constant.sp_columns;
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(Constant.table_name, SqlDbType.NVarChar, 384).Value = tableName;
               
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.Add( (string)reader[Constant.column_name]);
                    }
                }
            }
            return ret;
        }
       
    }
}
