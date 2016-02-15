using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace IMES.Infrastructure.UnitOfWork
{
    public class SqlTransactionManager
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
            if (_embeded > -1)
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
            if (_embeded > 0)
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

        public static SqlTransactionManagerInfo GetCurrentSqlTransactionManagerInfo()
        {
            return new SqlTransactionManagerInfo(_inScopeTag, _conn, _trans, _dbCataLog, _embeded, _errOccured);
        }

        public static void SetCurrentSqlTransactionManagerInfo(SqlTransactionManagerInfo info)
        {
            _inScopeTag = info.InScopeTag;
            _conn = info.Conn;
            _trans = info.Trans;
            _dbCataLog = info.DbCataLog;
            _embeded = info.Embeded;
            _errOccured = info.ErrOccured;
        }

        public static bool IsStillSomeConn()
        {
            return _conn != null;
        }

        public static bool IsStillSomeTrans()
        {
            return _trans != null;
        }

        public static void CommitCompulsorily()
        {
            if (_trans != null)
            {
                _trans.Commit();
            }
        }

        public static void RollbackCompulsorily()
        {
            if ( _trans != null)
            {
                _trans.Rollback();
            }
        }

        public static void DisposeCompulsorily()
        {
            if (_trans != null)
            {
                _trans.Dispose();
            }
        }

        public static void CloseCompulsorily()
        {
            if (_conn != null)
            {
                _conn.Close();
            }
        }

        public static void ResetCompulsorily()
        {
            _inScopeTag = false;
            _conn = null;
            _trans = null;
            _dbCataLog = null;
            _embeded = 0;
            _errOccured = false;
        }
    }

    public class SqlTransactionManagerInfo
    {
        private bool _inScopeTag = false;

        private SqlConnection _conn = null;

        private SqlTransaction _trans = null;

        private string _dbCataLog = null;

        private int _embeded = 0;

        private bool _errOccured = false;

        public bool InScopeTag
        {
            get { return _inScopeTag; }
        }

        public SqlConnection Conn
        {
            get { return _conn; }
        }

        public SqlTransaction Trans
        {
            get { return _trans; }
        }

        public string DbCataLog
        {
            get { return _dbCataLog; }
        }

        public int Embeded
        {
            get { return _embeded; }
        }

        public bool ErrOccured
        {
            get { return _errOccured; }
        }

        public SqlTransactionManagerInfo(bool inScopeTag, SqlConnection conn, SqlTransaction trans, string dbCataLog, int embeded, bool errOccured)
        {
            _inScopeTag = inScopeTag;

            _conn = conn;

            _trans = trans;

            _dbCataLog = dbCataLog;

            _embeded = embeded;

            _errOccured = errOccured;
        }
    }
}
