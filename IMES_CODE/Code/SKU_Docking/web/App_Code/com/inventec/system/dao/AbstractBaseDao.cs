/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: base class of other dao.
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2008-11-26   itc98047     Create 
 
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
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.SqlClient;
using System.Transactions;
using com.inventec.system.exception;
using com.inventec.system;
using log4net; 
/// <summary>
/// Summary description for AbstractBaseDao
/// </summary>
/// 
namespace com.inventec.system.dao
{

    public abstract class AbstractBaseDao
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AbstractBaseDao));
        private Database db;

        public AbstractBaseDao()
        {
            db = DatabaseFactory.CreateDatabase();
            //
            // TODO: Add constructor logic here
            //
        }

        protected Database getDbObject()
        { 
            return db;

        }

        protected Database getDbObject(String serverName, String databaseName, String userId, String pwd)
        {
             
            //string myConnectionString = "server=" + serverName + ";database=" + databaseName + ";uid=" + userId + ";pwd=" + pwd;
             
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder["server"] = serverName;
            builder["database"] = databaseName;
            builder["uid"] = userId;
            builder["pwd"] = pwd;

            log.Debug("last connectionString=" + builder.ConnectionString);
            Database db = new SqlDatabase(builder.ConnectionString);
            
            return db;

        }

        protected Database getDbObject(String serverName, String userId, String pwd)
        {
            string myConnectionString = "server=" + serverName + ";uid=" + userId + ";pwd=" + pwd;
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder["server"] = serverName;
            builder["uid"] = userId;
            builder["pwd"] = pwd;
            log.Debug("last connectionString=" + builder.ConnectionString);
            Database db = new SqlDatabase(builder.ConnectionString);

            return db;

        }

        /* 
         * The Interface1's description:
         *      execute db.ExecuteReader.
         * Parameters: 
         *      sql:  
         * Return Value 
         *      DataReader object
         * Remark: 
         *      NULL
         * Output 
         *      NULL 
         */
        protected IDataReader ExecuteReader(String sql)
        {
            Database db = getDbObject();
            DbCommand dbCommand = db.GetSqlStringCommand(sql);

            return db.ExecuteReader(dbCommand);

        }

        /* 
         * The Interface1's description:
         *      execute db.ExecuteReader.
         * Parameters: 
         *      sql:  
         *      db: database object
         * Return Value 
         *      DataReader object
         * Remark: 
         *      NULL
         * Output 
         *      NULL 
         */
        protected IDataReader ExecuteReader(String sql, Database db)
        {
            if (db == null)
                ExceptionManager.Throw(ExceptionMsg.DB_OBJECT_NULL);

            DbCommand dbCommand = db.GetSqlStringCommand(sql);

            return db.ExecuteReader(dbCommand);

        }

        /* 
         * The Interface1's description:
         *      execute update/insert etc.
         * Parameters: 
         *      sql:  
         * Return Value 
         *      the affected records count
         * Remark: 
         *      NULL
         * Output 
         *      NULL 
         */
        protected int ExecuteNonQuery(String sql)
        {
            Database db = getDbObject();
            DbCommand dbCommand = db.GetSqlStringCommand(sql);

            return db.ExecuteNonQuery(dbCommand);

        }


        /* 
         * The Interface1's description:
         *      execute update/insert etc.
         * Parameters: 
         *      sql:  
         *      db: database object
         * Return Value 
         *      the affected records count
         * Remark: 
         *      NULL
         * Output 
         *      NULL 
         */
        protected int ExecuteNonQuery(String sql, Database db)
        {
            if (db == null)
                ExceptionManager.Throw(ExceptionMsg.DB_OBJECT_NULL);

            DbCommand dbCommand = db.GetSqlStringCommand(sql);

            return db.ExecuteNonQuery(dbCommand);

        }

        /* 
     * The Interface1's description:
     *      execute db.ExecuteScalar.
     * Parameters: 
     *      sql:  
     * Return Value 
     *      the first field of the first row.
     * Remark: 
     *      NULL
     * Output 
     *      NULL 
     */
        protected Object ExecuteScalar(String sql)
        {
            Database db = getDbObject();
            DbCommand dbCommand = db.GetSqlStringCommand(sql);

            return db.ExecuteScalar(dbCommand);

        }


        /* 
         * The Interface1's description:
         *      execute db.ExecuteScalar.
         * Parameters: 
         *      sql:  
         *      db: database object
         * Return Value 
         *      the first field of the first row.
         * Remark: 
         *      NULL
         * Output 
         *      NULL 
         */
        protected Object ExecuteScalar(String sql, Database db)
        {
            if (db == null)
                ExceptionManager.Throw(ExceptionMsg.DB_OBJECT_NULL);

            DbCommand dbCommand = db.GetSqlStringCommand(sql);

            return db.ExecuteScalar(dbCommand);

        }


        /* 
        * The Interface1's description:
        *      execute db.ExecuteScalar.
        * Parameters: 
        *      sql:  
        * Return Value 
        *      DataSet.
        * Remark: 
        *      NULL
        * Output 
        *      NULL 
        */
        protected DataSet ExecuteDataSet(String sql)
        {
            Database db = getDbObject();
            //DbCommand dbCommand = db.GetSqlStringCommand(sql);

            return db.ExecuteDataSet(System.Data.CommandType.Text, sql);

        }


        /* 
         * The Interface1's description:
         *      execute db.ExecuteScalar.
         * Parameters: 
         *      sql:  
         *      db: database object
         * Return Value 
         *      DataSet.
         * Remark: 
         *      NULL
         * Output 
         *      NULL 
         */
        protected DataSet ExecuteDataSet(String sql, Database db)
        {
            if (db == null)
                ExceptionManager.Throw(ExceptionMsg.DB_OBJECT_NULL);

            //DbCommand dbCommand = db.GetSqlStringCommand(sql);
            
            return db.ExecuteDataSet(CommandType.Text, sql);

        }

        /* 
         * The Interface1's description:
         *      call StoredProc
         * Parameters: 
         *      sql:  
         *      db: database object
         * Return Value 
         *      DataSet.
         * Remark: 
         *      NULL
         * Output 
         *      NULL 
         */
        protected int ExecuteStoredProc(String procName)
        {
            if (db == null)
                ExceptionManager.Throw(ExceptionMsg.DB_OBJECT_NULL);

            DbCommand dbCommand = db.GetStoredProcCommand(procName);
            //db.AddInParameter(dbCommand, "ProductName", DbType.String, "ProductName", DataRowVersion.Current);
            //db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, "CategoryID", DataRowVersion.Current);
            //db.AddInParameter(dbCommand, "UnitPrice", DbType.Currency, "UnitPrice", DataRowVersion.Current);

            return db.ExecuteNonQuery(dbCommand);

        }


        /// <summary>
        /// Get uuid(32b).
        /// </summary>
        /// <returns></returns>
        protected string getUUID() {
            Guid guid = System.Guid.NewGuid();
            return guid.ToString("N");
        }

    }
}
