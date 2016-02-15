using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data.SqlClient;

namespace IMES.Query.DB
{
    public class SQLTxn
    {
        //Timeout unit is minute
        public static TransactionScope CreateDbTxnScope( int dbTimeOut)
        {
         
            TransactionOptions TransOpt = new TransactionOptions();

            TransOpt.IsolationLevel = IsolationLevel.ReadCommitted;
            // Set the timeout to be 2 minutes           
            TimeSpan txnTimeOut = new TimeSpan(0, dbTimeOut, 0);
            TransOpt.Timeout = txnTimeOut;
            return new TransactionScope(TransactionScopeOption.RequiresNew, TransOpt);
        }

    }

    public class SQLTxnManager
    {
        [ThreadStatic]
        private static bool _inScopeTag = false;
        [ThreadStatic]
        private static SqlConnection _conn = null;
        [ThreadStatic]
        private static SqlTransaction _trans = null;
        [ThreadStatic]
        private static string _dbCataLog = null;
        [ThreadStatic]
        private static int _embeded = 0;
        [ThreadStatic]
        private static bool _errOccured = false;

        public static void Begin()
        {
            _embeded++;
            _inScopeTag = true;
            _errOccured = false;
        }

        public static bool inScopeTag
        {
            get { return _inScopeTag; }
        }

        public static SqlConnection GetSqlConnection(string connectionString)
        {
            if (!_inScopeTag)
            {
                return new SqlConnection(connectionString);
            }
            else
            {
                if (_conn == null)
                    _conn = new SqlConnection(connectionString);
                return _conn;
            }
        }

        public static void ChangeCataLog(string dbCataLog)
        {
            if (_inScopeTag)
            {
                if (_dbCataLog == null)
                {
                    _dbCataLog = dbCataLog;
                }
                else
                {
                    if (_dbCataLog != dbCataLog)
                    {
                        if (_conn != null)
                            _conn.ChangeDatabase(dbCataLog);
                        _dbCataLog = dbCataLog;
                    }
                }
            }
        }

        public static SqlTransaction GetTransaction()
        {
            if (_inScopeTag && _conn != null)
            {
                if (_trans == null)
                {
                    _trans = _conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                }
                return _trans;
            }
            else
                return null;
        }

        public static void CloseSqlConnection(SqlConnection conn)
        {
            if (!_inScopeTag)
            {
                if (conn != null)
                {
                    conn.Close();
                    conn = null;
                }
            }
        }

        public static void SetDBCataLog(string dbCataLog)
        {
            _dbCataLog = dbCataLog;
        }

        public static void End()
        {
            if (IsFirstLevel())
            {
                _inScopeTag = false;
            }
            _embeded--;
        }

        public static void Commit()
        {
            if (_inScopeTag && IsFirstLevel() && _trans != null)
            {
                _trans.Commit();
            }
        }

        public static void Rollback()
        {
            _errOccured = true;
            if (_inScopeTag && _trans != null)
            {
                _trans.Rollback();
                _trans = null;
            }
        }

        public static void Dispose()
        {
            if (_inScopeTag && (IsFirstLevel() || _errOccured))
            {
                if (_trans != null)
                {
                    _trans.Dispose();
                    _trans = null;
                }
                if (_conn != null)
                {
                    _conn.Close();
                    _conn = null;
                }
            }
        }

        private static bool IsFirstLevel()
        {
            return _embeded == 1;
        }

        public static void Suppress()
        {
            if (_embeded > 0)
                _inScopeTag = false;
        }

        public static void Recover()
        {
            if (_embeded > 0)
                _inScopeTag = true;
        }
    }
}
