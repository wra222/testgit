using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Infrastructure.Repository
{
    public class TableLockManager
    {
        #region Cache
        private static IDictionary<string, SqlTransactionManagerInfo> _cache_real = null;
        private static IDictionary<string, SqlTransactionManagerInfo> _cache
        {
            get
            {
                if (_cache_real == null)
                    _cache_real = new Dictionary<string, SqlTransactionManagerInfo>();
                return _cache_real;
            }
        }
        private static object _syncObj_cache = new object();//Is there a bottleneck of this lock?
        #endregion

        public static void lockTheTable(string type, object key, IUnitOfWork uow)
        {
            lock (_syncObj_cache)
            {
                string key4Trans = getKey(type, key);
                if (!_cache.ContainsKey(key4Trans))
                {
                    SqlTransactionManager.Begin();
                    uow.Commit();
                    _cache.Add(key4Trans, SqlTransactionManager.GetCurrentSqlTransactionManagerInfo());
                }
            }
        }

        public static void releaseTheTable(string type, object key, IUnitOfWork uow)
        {
            lock (_syncObj_cache)
            {
                string key4Trans = getKey(type, key);
                if (_cache.ContainsKey(key4Trans))
                {
                    var info = (SqlTransactionManagerInfo)_cache[key4Trans];
                    if (info != null)
                    {
                        SqlTransactionManager.SetCurrentSqlTransactionManagerInfo(info);
                        try
                        {
                            uow.Commit();
                            SqlTransactionManager.Commit();
                        }
                        catch (Exception e)
                        {
                            SqlTransactionManager.Rollback();
                            throw e;
                        }
                        finally
                        {
                            SqlTransactionManager.Dispose();
                            SqlTransactionManager.End();
                        }
                    }
                }
            }
        }

        private static string getKey(string type, object key)
        {
            return type + "||" + key.ToString();
        }
    }
}
