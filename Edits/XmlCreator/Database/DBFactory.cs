using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Inventec.HPEDITS.XmlCreator.Database
{
    public class DBFactory
    {
        static string ConnectionString = ConfigurationManager.AppSettings["Database"].ToString();//×¥webconfigÅäÖÃ×Ö¶Î
        public static void PopulateTable(DataTable dataTable, 
            string dbTableName, 
            string keyFieldName, 
            string idValue,
            bool isChild,
            string keyColumnName,
            Int32 keyColumnValue,
            string selectColumnName,Dictionary<string,string> fieldsMap)
        {
            string whereClause = string.Empty;            
            string selectClause = "SELECT "+selectColumnName+" FROM " + dbTableName;
            
            //ensure key is not empty and is not a flag to indicate that parent and child is in the same table
            if (keyFieldName != string.Empty && keyFieldName!=TableLookupResult.SAMEASPARENTKEY)
            {
                whereClause = "WHERE " + keyFieldName + "='" + idValue + "'";
            }

            string query = selectClause + " " + whereClause;
            DataTable temporaryTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    adapter.Fill(temporaryTable);                    
                    int key = 0;
                    //need to add a column to dataTable
                    if (temporaryTable.Columns.Contains(TableLookupResult.IDFIELDNAME)&&!dataTable.Columns.Contains(TableLookupResult.IDFIELDNAME))
                    {
                        dataTable.Columns.Add(TableLookupResult.IDFIELDNAME);
                    }
                    foreach (DataRow readRow in temporaryTable.Rows)
                    {                        
                        DataRow newRow = dataTable.NewRow();
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            string dbFieldName = string.Empty;
                            if (fieldsMap != null)
                            {
                                dbFieldName = fieldsMap[column.ColumnName];
                            }
                            else
                                dbFieldName = column.ColumnName;
                            if (temporaryTable.Columns.Contains(dbFieldName))
                            {
                                //if contains crap
                                newRow[column.ColumnName] = readRow[dbFieldName];
                            }
                            else if (dataTable.PrimaryKey.Length>0&&dataTable.PrimaryKey[0].ColumnName!=column.ColumnName)
                            {
                                //if not, fill with crap
                                if (column.DataType == typeof(string))
                                    newRow[column.ColumnName] = string.Empty;
                                else
                                    newRow[column.ColumnName] = 0;
                            }
                            else
                            {
                                newRow[column.ColumnName] = key;
                            }
                            if (isChild)
                            {
                                newRow[keyColumnName] = keyColumnValue;
                            }
                        }

                        key++;
                        dataTable.Rows.Add(newRow);
                    }
                }
            }
        }
        public static void PopulateTable(DataTable dataTable,
            string dbTableName,
            string keyFieldName,
            string idValue,
            bool isChild,
            string keyColumnName,
            Int32 keyColumnValue,
            string selectColumnName)
        {
            PopulateTable(dataTable, dbTableName, keyFieldName, idValue, isChild, keyColumnName, keyColumnValue, selectColumnName, null);
        }
        public static void PopulateTable(DataTable dataTable,string dbTableName)
        {
            PopulateTable(dataTable, dbTableName, string.Empty, string.Empty,false,null,0,"*",null);
        }
        public static void BuildChildRelationship(DataTable childTable, DataTable parentTable, TableLookupResult result)
        {
            foreach (DataRow parentRow in parentTable.Rows)
            {
                //get the parent key column
                DataColumn parentKeyColumn = parentTable.PrimaryKey[0];
                
                //get the query value                                
                string queryValue = string.Empty;
                string queryKey = string.Empty;
                string selectColumn = "*";
                if (result.ForeignKeyFieldName != TableLookupResult.SAMEASPARENTKEY)
                {
                    queryKey = result.ForeignKeyFieldName;
                    queryValue = parentRow[result.ForeignKeyFieldName].ToString();
                }
                if (!result.IsResultDBTable)
                {
                    selectColumn = result.FieldName;
                }
                PopulateTable(childTable, result.TableName, queryKey, queryValue, true, parentKeyColumn.ColumnName, Convert.ToInt32(parentRow[parentKeyColumn]), selectColumn);                
            }
        }

        public static DataTable PopulateTempTable(            
            string dbTableName, 
            string whereClause, 
            List<string> fields)
        {
            string groubystr = "group by PALLET_ID,MAWB,MASTER_WAYBILL_NUMBER,[REGION],[PALLET_ACT_WEIGHT],[PALLET_BOX_QTY],[PALLET_UNIT_QTY]";
           
            //first, build the selects from the fields
            string fieldsClause = BuildFields(fields);
            DataTable tempTable = new DataTable();
            //then build the query
            string query = "SELECT " + fieldsClause + " FROM " + dbTableName + " " + whereClause;
            if (dbTableName == "[v_Pos_PALLET]")
            {
                query += " " + groubystr;
            }
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    conn.Open();
                    adapter.Fill(tempTable);
                }
            }
            return tempTable;
        }

        public static DataTable PopulateTempTable_BySp(
        string spname,
        string whereClause)
        {
            DataTable tempTable = new DataTable();
            //then build the query
            //string query = "SELECT " + fieldsClause + " FROM " + dbTableName + " " + whereClause;
            string query = "exec " + spname + " " + whereClause;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    conn.Open();
                    adapter.Fill(tempTable);
                }
            }
            return tempTable;
        }

        static string BuildFields(List<string> fields)
        {
            StringBuilder fieldsBuilder = new StringBuilder();
            for (int i = 0; i < fields.Count; i++)
            {
                if (i > 0)
                    fieldsBuilder.Append("," + fields[i]);
                else
                    fieldsBuilder.Append(fields[i]);
            }
            return fieldsBuilder.ToString();
        }
    }
}
