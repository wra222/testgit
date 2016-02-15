using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;

namespace IMES.Infrastructure.Repository._Metas
{
    public class SQLCache
    {
        #region Get SQLs

        private static IDictionary<int, SQLContextNew> _cache = new Dictionary<int, SQLContextNew>();

        private static object _cacheSyncObj = new object();

        public static bool PeerTheSQL(int methodId, out SQLContextNew sqlCntxt)
        {
            return PeerTheSQL_Inner(methodId, out sqlCntxt);
        }

        private static bool PeerTheSQL_Inner(int methodId, out SQLContextNew sqlCntxt)
        {
            sqlCntxt = null;

            bool res1 = false;
            lock (_cacheSyncObj)
            {
                if (_cache.ContainsKey(methodId))
                {
                    sqlCntxt = new SQLContextNew(_cache[methodId]);
                    res1 = true;
                }
                else
                {
                    res1 = false;
                }
            }
            return res1;
        }

        internal static bool GetFromCache(int methodId, ref SQLContextNew sqlCtx)
        {
            lock (_cacheSyncObj)
            {
                if (_cache.ContainsKey(methodId))
                {
                    sqlCtx = _cache[methodId];
                    return true;
                }
                else
                {
                    sqlCtx = new SQLContextNew();
                    _cache.Add(methodId, sqlCtx);
                    return false;
                }
            }
        }

        public static bool InsertIntoCache(int methodId, SQLContextNew sqlCtx)
        {
            lock (_cacheSyncObj)
            {
                if (!_cache.ContainsKey(methodId))
                {
                    _cache.Add(methodId, sqlCtx);
                    return true;
                }
                else
                    return false;
            }
        }

        #endregion

        #region ORMapping Copes

        private static IDictionary<string, IDictionary<string,FieldInfo>> _cacheForFieldToColumn = new Dictionary<string, IDictionary<string,FieldInfo>>();
        private static IDictionary<string, IDictionary<string, SqlDbType>> _cacheForFieldToColumnType = new Dictionary<string, IDictionary<string, SqlDbType>>();//一一伴随上面的集合

        private static IDictionary<string, IDictionary<string,FieldInfo>> _cacheForColumnToField = new Dictionary<string, IDictionary<string,FieldInfo>>();

        private static IDictionary<string, Type> _cacheForClassToType = new Dictionary<string, Type>();

        //internal static object _cacheSyncObj_orm = new object();

        internal static bool PeerTheORM_Table(string className)
        {
            if (_cacheForClassToType.ContainsKey(className))
                return true;
            else
                return false;
        }

        internal static void AddForClassToType(string className, Type type)
        {
            _cacheForClassToType.Add(className, type);
        }

        internal static void AddForFieldToColumn(string className, string fieldName, FieldInfo type)
        {
            IDictionary<string,FieldInfo> entry = null;
            IDictionary<string,SqlDbType> entry4clmnType = null;
            if (_cacheForFieldToColumn.ContainsKey(className))
            {
                entry = _cacheForFieldToColumn[className];
                entry4clmnType = _cacheForFieldToColumnType[className];
            }
            else
            {
                entry = new Dictionary<string,FieldInfo>();
                entry4clmnType = new Dictionary<string, SqlDbType>();
                _cacheForFieldToColumn.Add(className, entry);
                _cacheForFieldToColumnType.Add(className, entry4clmnType);
            }
            entry.Add(fieldName, type);
            entry4clmnType.Add(fieldName, ((DBFieldAttribute)type.GetCustomAttributes(typeof(DBFieldAttribute), false)[0]).columnType);
        }
        internal static void AddForColumnToField(string tableName, string columnName, FieldInfo type)
        {
            IDictionary<string, FieldInfo> entry = null;
            if (_cacheForColumnToField.ContainsKey(tableName))
            {
                entry = _cacheForColumnToField[tableName];
            }
            else
            {
                entry = new Dictionary<string, FieldInfo>();
                _cacheForColumnToField.Add(tableName, entry);
            }
            //2015-06-11 Vincent check exists or not
            if (!_cacheForColumnToField.ContainsKey(tableName))
            {
                entry.Add(columnName, type);
            }
        }

        //2015-06-11 Vincent : add Cache key by tableName + "." + className
        internal static void AddForColumnToField(string tableName, string className,string columnName, FieldInfo type)
        {
            IDictionary<string, FieldInfo> entry = null;
            string key = tableName + "." + className;
            if (_cacheForColumnToField.ContainsKey(key))
            {
                entry = _cacheForColumnToField[key];
            }
            else
            {
                entry = new Dictionary<string, FieldInfo>();
                _cacheForColumnToField.Add(key, entry);
            }

            if (!entry.ContainsKey(columnName))
            {
                entry.Add(columnName, type);
            }
        }

        internal static Type GetForClassToType(string className)
        {
            return _cacheForClassToType[className];
        }

        internal static IDictionary<string, FieldInfo> GetForFieldToColumn(string className)
        {
            return _cacheForFieldToColumn[className];
        }

        internal static IDictionary<string, FieldInfo> GetForColumnToField(string tableName)
        {
            return _cacheForColumnToField[tableName];
        }

        internal static IDictionary<string, FieldInfo> GetForColumnToField(string tableName, string className)
        {
            return _cacheForColumnToField[tableName + "." + className];
        }

        internal static IDictionary<string, SqlDbType> GetForFieldToColumnType(string className)
        {
            return _cacheForFieldToColumnType[className];
        }

        #endregion
    }
}
