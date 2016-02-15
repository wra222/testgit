// 2010-03-01 Liu Dong(eB1-4)         Modify ITC-1122-0080 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Collections;
using IMES.Infrastructure.Utility;
using IMES.Infrastructure.Repository._Metas;
using System.Reflection.Emit;

namespace IMES.Infrastructure.Repository._Schema
{
    internal static class Constants
    {
        public const long DateTimeMinVal = 552877920000000000L;//Ticks  //new DateTime(1753,1,1)
        public const long DateTimeMaxVal = 3155378112000000000L;//Ticks  //new DateTime(9999, 12, 31)
        public const long CurrencyMinVal = -999999999999999999L;//-79228162514264337593543950335M;           //decimal.MinValue
        public const long CurrencyMaxVal = 999999999999999999L;//79228162514264337593543950335M;            //decimal.MaxValue
    }

    internal class SQLContextCollection
    {
        private IDictionary<int, SQLContext> _content = new Dictionary<int, SQLContext>();

        public void AddOne(int index, SQLContext item)
        {
            lock (_content)
            {
                if (!_content.ContainsKey(index))
                    _content.Add(index, item);
            }
        }

        public SQLContext MergeToOneNonQuery()
        {
            lock (_content)
            {
                SQLContext ret = new SQLContext();
                foreach (int key in _content.Keys)
                {
                    SQLContext item = _content[key];
                    string sentence = item.Sentence;

                    if (item.Params != null && item.Params.Count > 0)
                    {
                        foreach (string paramKey in item.Params.Keys)
                        {
                            SqlParameter paramItem = item.Params[paramKey];
                            SqlParameter newItem = new SqlParameter("@" + key.ToString() + "_" + paramItem.ParameterName.Substring(1), paramItem.SqlDbType);
                            newItem.Value = paramItem.Value;
                            ret.Params.Add(key.ToString() + "_" + paramKey, newItem);
                            sentence = sentence.Replace(paramItem.ParameterName, newItem.ParameterName);
                        }
                    }
                    ret.Sentence = ret.Sentence + sentence + ";";
                }
                return ret;
            }
        }

        public SQLContext MergeToOneAndQuery()
        {
            return MergeToOneQuery("AND");
        }

        public SQLContext MergeToOneOrQuery()
        {
            return MergeToOneQuery("OR");
        }

        public SQLContext MergeToOneQuery(string andOrOr)
        {
            lock (_content)
            {
                SQLContext ret = new SQLContext();
                if (_content.Keys != null && _content.Keys.Count > 0)
                {
                    int i = 0;
                    foreach (int key in _content.Keys)
                    {
                        SQLContext item = _content[key];
                        if (i == 0)
                        {
                            ret.Indexes = item.Indexes;

                            if (_content.Keys.Count == 1)
                            {
                                ret.Sentence = item.Sentence;
                            }
                            else
                            {
                                string[] fieldAndCond = item.Sentence.Split(new string[] { "WHERE" }, StringSplitOptions.None);
                                if (fieldAndCond != null)
                                {
                                    if (fieldAndCond.Length > 1)
                                        ret.Sentence = fieldAndCond[0] + " WHERE (" + fieldAndCond[1] + ") ";
                                    else
                                        ret.Sentence = fieldAndCond[0] + " WHERE (1=1) ";
                                }
                            }
                        }
                        else
                        {
                            string[] fieldAndCond = item.Sentence.Split(new string[] { "WHERE" }, StringSplitOptions.None);
                            if (fieldAndCond != null && fieldAndCond.Length > 1)
                            {
                                ret.Sentence = ret.Sentence + " " + andOrOr + " (" + fieldAndCond[1] + ") ";
                            }
                        }
                        if (item.Params != null && item.Params.Count > 0)
                        {
                            foreach (string key_i in item.Params.Keys)
                            {
                                if (!ret.Params.ContainsKey(key_i))
                                    ret.Params.Add(key_i, item.Params[key_i]);
                            }
                        }
                        i++;
                    }
                }
                return ret;
            }
        }
    }

    internal class SQLContext
    {
        public SQLContext()
        {

        }

        public SQLContext(SQLContext orig)
        {
            this.Sentence = orig.Sentence;

            foreach (KeyValuePair<string, SqlParameter> item in orig.Params)
            {
                this.Params.Add(item.Key, (SqlParameter)((ICloneable)item.Value).Clone());
            }
            foreach (KeyValuePair<string, int> item in orig.Indexes)
            {
                this.Indexes.Add(item.Key, item.Value);
            }
        }

        public string Sentence = string.Empty;
        public IDictionary<string, SqlParameter> Params = new Dictionary<string, SqlParameter>();
        public IDictionary<string, int> Indexes = new Dictionary<string, int>();

        public SQLContextNew ToNew()
        {
            SQLContextNew ret = new SQLContextNew(new SQLContextNew(this.Sentence, this.Params, this.Indexes));
            return ret;
        }

        public static SQLContext ToOld(SQLContextNew item)
        {
            SQLContext ret = new SQLContext();
            ret.Sentence = item.Sentence;
            item.FillIndexes(ret.Indexes);
            item.FillParams(ret.Params);
            return ret;
        }
    }

    internal class TableAndFields
    {
        public TableAndFields()
        {
        }
        public TableAndFields(TableAndFields orig)
        {
            this.Table = orig.Table;
            if (orig.ToGetFieldNames == null)
            {
                this.ToGetFieldNames = null;
            }
            else
            {
                foreach (string str in orig.ToGetFieldNames)
                {
                    this.ToGetFieldNames.Add(str);
                }
            }
            this.equalcond = orig.equalcond;
            this.betweencond = orig.betweencond;

            this.likecond = orig.likecond;
            this.greatercond = orig.greatercond;
            this.smallercond = orig.smallercond;
            this.greaterOrEqualcond = orig.greaterOrEqualcond;
            this.smallerOrEqualcond = orig.smallerOrEqualcond;
            this.inSetcond = orig.inSetcond;

            this.notNullcond = orig.notNullcond;

            this.notEqualcond = orig.notEqualcond;
            this.notLikecond = orig.notLikecond;
            this.notInSetcond = orig.notInSetcond;

            this.nullOrEqual = orig.nullOrEqual;

            this.alias = orig.alias;
            this.subDBCalalog = orig.subDBCalalog;
        }

        public Type Table = null;
        public IList<string> ToGetFieldNames = new List<string>();
        public object equalcond = null;
        public object betweencond = null;

        public object likecond = null;
        public object greatercond = null;
        public object smallercond = null;
        public object greaterOrEqualcond = null;
        public object smallerOrEqualcond = null;
        public object inSetcond = null;

        public object notNullcond = null;
        public object nullcond = null;

        public object notEqualcond = null;
        public object notLikecond = null;
        public object notInSetcond = null;

        public object nullOrEqual = null;

        public string alias = "";
        public string subDBCalalog = null;

        public override string ToString()
        {
            if (subDBCalalog != null)
                return subDBCalalog + ".." + Table.Name + " " + alias;
            else
                return Table.Name + " " + alias;
        }
    }

    internal class TableConnectionItem
    {
        public string alias1 = null;
        public Type Table1 = null;
        public TableAndFields Tb1 = null;
        public string FieldNameFromTable1 = null;
        public string alias2 = null;
        public Type Table2 = null;
        public TableAndFields Tb2 = null;
        public string FieldNameFromTable2 = null;

        public string BiOperator = "{0}={1}";

        public bool tbl1_Alias = false;     //Table1的别名是否已赋
        public bool tbl2_Alias = false;     //Table2的别名是否已赋

        public TableConnectionItem(TableAndFields Tb1, string FieldNameFromTable1, TableAndFields Tb2, string FieldNameFromTable2)
            : this(Tb1.Table, FieldNameFromTable1, Tb2.Table, FieldNameFromTable2)
        {
            this.Tb1 = Tb1;
            this.Tb2 = Tb2;
        }

        public TableConnectionItem(TableAndFields Tb1, string FieldNameFromTable1, TableAndFields Tb2, string FieldNameFromTable2, string BiOperator)
            : this(Tb1, FieldNameFromTable1, Tb2, FieldNameFromTable2)
        {
            this.BiOperator = BiOperator;
        }

        private TableConnectionItem(Type Table1, string FieldNameFromTable1, Type Table2, string FieldNameFromTable2)
        {
            this.Table1 = Table1;
            this.FieldNameFromTable1 = FieldNameFromTable1;
            this.Table2 = Table2;
            this.FieldNameFromTable2 = FieldNameFromTable2;
        }

        private TableConnectionItem(Type Table1, string FieldNameFromTable1, Type Table2, string FieldNameFromTable2, string BiOperator)
            : this(Table1, FieldNameFromTable1, Table2, FieldNameFromTable2)
        {
            this.BiOperator = BiOperator;
        }

        public override bool Equals(object obj)
        {
            if (obj is TableConnectionItem)
            {
                TableConnectionItem another = (TableConnectionItem)obj;
                return
                    (this.Tb1 == another.Tb1 && this.Tb2 == another.Tb2 && this.FieldNameFromTable1 == another.FieldNameFromTable1 && this.FieldNameFromTable2 == another.FieldNameFromTable2)
                    ||
                    (this.Tb1 == another.Tb2 && this.Tb2 == another.Tb1 && this.FieldNameFromTable1 == another.FieldNameFromTable2 && this.FieldNameFromTable2 == another.FieldNameFromTable1);
            }
            else
            {
                return false;
            }
            //return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(BiOperator, string.Format("{0}.{1}", alias1, FieldNameFromTable1), string.Format("{0}.{1}", alias2, FieldNameFromTable2));
        }

        public bool AndOrOr = true;
    }

    //internal class TableConnection
    //{
    //    public TableConnection(Type Table1, string[] FieldNamesFromTable1, Type Table2, string[] FieldNamesFromTable2)
    //    {
    //        this.Table1 = Table1;
    //        this.FieldNamesFromTable1 = FieldNamesFromTable1;
    //        this.Table2 = Table2;
    //        this.FieldNamesFromTable2 = FieldNamesFromTable2;
    //    }
    //    public Type Table1 = null;
    //    public string[] FieldNamesFromTable1 = null;
    //    public Type Table2 = null;
    //    public string[] FieldNamesFromTable2 = null;
    //}

    internal class TableConnectionCollection
    {
        private IList<TableConnectionItem> content = new List<TableConnectionItem>();
        private IDictionary<string, IDictionary<string, TableConnectionItem>> regist = new Dictionary<string, IDictionary<string, TableConnectionItem>>();
        internal IDictionary<string, IDictionary<string, TableConnectionItem>> Regist
        {
            get { return this.regist; }
        }

        ////Do not recommed.
        //private TableConnectionCollection(TableConnection[] values)
        //{
        //    if (values != null && values.Length > 0)
        //    {
        //        foreach (TableConnection item in values)
        //        {
        //            if (item.Table1 != null && item.Table2 != null && item.FieldNamesFromTable1 != null && item.FieldNamesFromTable2 != null && item.FieldNamesFromTable2.Length == item.FieldNamesFromTable1.Length)
        //            {
        //                for (int i = 0; i < item.FieldNamesFromTable1.Length; i++ )
        //                {
        //                    TableConnectionItem entry = new TableConnectionItem(item.Table1, item.FieldNamesFromTable1[i], item.Table2, item.FieldNamesFromTable2[i]);
        //                    if(!content.Contains(entry))
        //                        content.Add(entry);

        //                    string key1 = Func.DecAlias(entry.Table1.Name, entry.FieldNameFromTable1);
        //                    string key2 = Func.DecAlias(entry.Table2.Name, entry.FieldNameFromTable2);
        //                    RegistInner(key1, key2, entry);
        //                    RegistInner(key2, key1, entry);
        //                }
        //            }
        //        }
        //    }
        //}

        //Recommed.
        public TableConnectionCollection(TableConnectionItem[] values)
        {
            if (values != null && values.Length > 0)
            {
                foreach (TableConnectionItem item in values)
                {
                    if (!content.Contains(item))
                        content.Add(item);

                    //string key1 = Func.DecAlias(item.Table1.Name, item.FieldNameFromTable1);
                    //string key2 = Func.DecAlias(item.Table2.Name, item.FieldNameFromTable2);
                    string key1 = Func.DecAlias(item.Tb1.GetHashCode().ToString(), item.FieldNameFromTable1);
                    string key2 = Func.DecAlias(item.Tb2.GetHashCode().ToString(), item.FieldNameFromTable2);

                    RegistInner(key1, key2, item);
                    RegistInner(key2, key1, item);
                }
            }
        }

        private void RegistInner(string key1, string key2, TableConnectionItem item)
        {
            IDictionary<string, TableConnectionItem> nowPoint = null;
            if (regist.ContainsKey(key1))
            {
                nowPoint = regist[key1];
            }
            else
            {
                nowPoint = new Dictionary<string, TableConnectionItem>();
                regist.Add(key1, nowPoint);
            }
            if (!nowPoint.ContainsKey(key2))
            {
                nowPoint.Add(key2, item);
            }
        }

        internal string[] BatchToString()
        {
            List<string> ret = new List<string>();
            foreach (TableConnectionItem tci in content)
            {
                if (tci.tbl1_Alias && tci.tbl2_Alias)
                    ret.Add(tci.ToString());
            }
            return ret.ToArray();
        }
    }

    internal class TableBiJoinedLogic
    {
        private Queue _queue = new Queue(); //TableConnectionItem or String
        public TableBiJoinedLogic() { }
        public void Add(string joinOperator)
        {
            _queue.Enqueue(joinOperator);
        }
        public void Add(TableConnectionItem joinCondtion)
        {
            _queue.Enqueue(joinCondtion);
        }
        public void Add(TableAndFields joinTblFlds)
        {
            _queue.Enqueue(joinTblFlds);
        }
        public IEnumerator Enum
        {
            get { return _queue.GetEnumerator(); }
        }
    }

    //internal class AggregationField
    //{
    //    public string funcName;
    //    public IList<string> fieldNames;
    //    public string asName;

    //    public AggregationField(AggregationField orig)
    //    {
    //        this.funcName = orig.funcName;
    //        List<string> fns = new List<string>();
    //        fns.AddRange(orig.fieldNames);
    //        this.fieldNames = fns;
    //        this.asName = orig.asName;
    //    }
    //}

    internal static class Func
    {
        internal static string JOIN = " JOIN ";
        internal static string LEFTJOIN = " LEFT JOIN ";
        internal static string RIGHTJOIN = " RIGHT JOIN ";

        private static string _onStr = " ON ";
        private static string _andStr = " AND ";
        private static string _orStr = " OR ";

        private static readonly string fn_ = "fn_";

        #region SQL Cache

        private static IDictionary<int, SQLContext> _cache = new Dictionary<int, SQLContext>();
        private static object _cacheSyncObj = new object();

        private static IDictionary<int, TableAndFields[]> _cacheForTaF = new Dictionary<int, TableAndFields[]>();
        private static object _cacheForTaFSyncObj = new object();

        internal static bool PeerTheSQL(int methodId, out SQLContext sqlCntxt)
        {
            TableAndFields[] tmp = null;
            return PeerTheSQL_Inner(methodId, out sqlCntxt, false, out tmp);
        }
        internal static bool PeerTheSQL(int methodId, out SQLContext sqlCntxt, out TableAndFields[] tblAndFldses)
        {
            return PeerTheSQL_Inner(methodId, out sqlCntxt, true, out tblAndFldses);
        }

        private static bool PeerTheSQL_Inner(int methodId, out SQLContext sqlCntxt, bool isJoinSQL, out TableAndFields[] tblAndFldses)
        {
            sqlCntxt = null;
            tblAndFldses = null;
            bool res1 = false;
            lock (_cacheSyncObj)
            {
                if (_cache.ContainsKey(methodId))
                {
                    sqlCntxt = new SQLContext(_cache[methodId]);// 2010-03-01 Liu Dong(eB1-4)         Modify ITC-1122-0080 
                    res1 = true;
                }
                else
                {
                    res1 = false;
                }
            }
            if (isJoinSQL)
            {
                bool res2 = false;
                lock (_cacheForTaFSyncObj)
                {
                    if (_cacheForTaF.ContainsKey(methodId))
                    {
                        IList<TableAndFields> lst = new List<TableAndFields>();
                        foreach (TableAndFields item in _cacheForTaF[methodId])
                        {
                            lst.Add(new TableAndFields(item));
                        }
                        tblAndFldses = lst.ToArray();
                        res2 = true;
                    }
                    else
                    {
                        res2 = false;
                    }
                }
                return res1 && res2;
            }
            else
                return res1;
        }

        private static bool GetFromCache(int methodId, ref SQLContext sqlCtx)
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
                    sqlCtx = new SQLContext();
                    _cache.Add(methodId, sqlCtx);
                    return false;
                }
            }
        }

        internal static bool InsertIntoCache(int methodId, SQLContext sqlCtx)
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

        private static bool GetFromCacheTaF(int methodId, ref TableAndFields[] tables)
        {
            lock (_cacheForTaFSyncObj)
            {
                if (_cacheForTaF.ContainsKey(methodId))
                {
                    tables = _cacheForTaF[methodId];
                    //for (int i = 0; i < _cacheForTaF[methodId].Length; i++ )
                    //{
                    //    &tables[i] = &_cacheForTaF[methodId][i];
                    //}
                    return true;
                }
                else
                {
                    _cacheForTaF.Add(methodId, tables);
                    return false;
                }
            }
        }

        #endregion

        #region get cache field Info and field attribute info and const field info
        private static readonly IDictionary<string, FieldInfo[]> _fieldCache = new Dictionary<string, FieldInfo[]>();
        private static readonly IDictionary<string, IDictionary<string, DBFieldAttribute>> _fieldAttrCache = new Dictionary<string, IDictionary<string, DBFieldAttribute>>();
        private static readonly IDictionary<string, FieldInfo[]> _constFieldCache = new Dictionary<string, FieldInfo[]>();

        private static object _fieldcacheSyncObj = new object();
        private static FieldInfo[] getCacheFieldInfos(Type type, BindingFlags bindingFlags,
                                                                        out IDictionary<string, DBFieldAttribute> fieldAttrList,
                                                                        out FieldInfo[] constFieldInfos)
        {
            string name = type.FullName;
            lock (_fieldcacheSyncObj)
            {
                if (_fieldCache.ContainsKey(name))
                {
                    fieldAttrList = _fieldAttrCache[name];
                    constFieldInfos = _constFieldCache[name];
                    return _fieldCache[name];
                }
                else
                {
                    //FieldInfo[] infos = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    FieldInfo[] infos = type.GetFields(bindingFlags);
                    _fieldCache.Add(name, infos);

                    fieldAttrList = new Dictionary<string, DBFieldAttribute>();
                    foreach (FieldInfo fi in infos)
                    {
                        object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                        if (objs != null && objs.Length > 0)
                        {
                            fieldAttrList.Add(fi.Name, (DBFieldAttribute)objs[0]);
                        }
                        else
                        {
                            throw new Exception(name + "." + fi.Name + " no defined DBFieldAttribute");
                        }
                    }
                    _fieldAttrCache.Add(name, fieldAttrList);

                    constFieldInfos = type.GetFields(BindingFlags.Static | BindingFlags.Public).Where(x => x.IsLiteral).ToArray();
                    _constFieldCache.Add(name, constFieldInfos);
                    return infos;
                }
            }
        }

        #endregion

        #region Create Object
        private static readonly string CN_CreateInstance = "CreateInstance";
        public static ObjectActivator CreateCtor(Type type)
        {
            if (type == null)
            {
                throw new Exception("type");
            }
            ConstructorInfo emptyConstructor = type.GetConstructor(Type.EmptyTypes);
            var dynamicMethod = new DynamicMethod(CN_CreateInstance, type, Type.EmptyTypes, true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            ilGenerator.Emit(OpCodes.Ret);
            return (ObjectActivator)dynamicMethod.CreateDelegate(typeof(ObjectActivator));
        }

        public delegate object ObjectActivator();

        private static readonly IDictionary<string, ObjectActivator> _fisObjCache = new Dictionary<string, ObjectActivator>();
        private static object _fisObjCacheSyncObj = new object();

        public static ObjectActivator CreateInstance(Type type)
        {
            //Type type = typeof(C);
            string name = type.FullName;
            ObjectActivator activator = null;
            lock (_fisObjCacheSyncObj)
            {
                if (_fisObjCache.ContainsKey(name))
                {
                    activator = _fisObjCache[name];
                }
                else
                {
                    activator = CreateCtor(type);
                    _fisObjCache.Add(name, activator);
                }
            }

            return activator;
        }
        #endregion

        #region Get SQL Methods

        public static SQLContext GetCommonSelect(int methodId, Type type)
        {
            SQLContext ret = null;
            if (GetFromCache(methodId, ref ret))
                return ret;

            ret.Sentence = "SELECT {0} FROM " + type.Name;
            List<string> fields = new List<string>();
            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            int i = 0;
            foreach (FieldInfo fi in fis)
            {
                //string fieldName = TransField(type,fi.Name);
                string fieldName = TransField(cfis, fi.Name);
                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute),false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                //SqlParameter sptr = new SqlParameter("@" + fieldName, dbfa.fieldType);
                //ret.Params.Add(fieldName,sptr);
                ret.Indexes.Add(fieldName, i);
                fields.Add(fieldName);
                i++;
            }
            ret.Sentence = string.Format(ret.Sentence, string.Join(",", fields.ToArray()));
            return ret;
        }

        public static SQLContext GetCommonInsert(int methodId, Type type)
        {
            SQLContext ret = null;
            if (GetFromCache(methodId, ref ret))
                return ret;

            ret.Sentence = "INSERT INTO " + type.Name + " ({0}) VALUES ({1}) ";
            List<string> fields = new List<string>();
            List<string> paramts = new List<string>();
            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            foreach (FieldInfo fi in fis)
            {
                //string fieldName = TransField(type, fi.Name);
                string fieldName = TransField(cfis, fi.Name);
                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];
                SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                ret.Params.Add(fieldName, sptr);
                fields.Add(fieldName);
                paramts.Add("@" + ClearRectBrace(fieldName));
            }
            ret.Sentence = string.Format(ret.Sentence, string.Join(",", fields.ToArray()), string.Join(",", paramts.ToArray()));
            return ret;
        }

        public static SQLContext GetAquireIdInsert(int methodId, Type type)
        {
            SQLContext ret = null;
            if (GetFromCache(methodId, ref ret))
                return ret;

            ret.Sentence = string.Format("{0};{1};", "INSERT INTO " + type.Name + " ({0}) VALUES ({1}) ", "SELECT @@IDENTITY");
            List<string> fields = new List<string>();
            List<string> paramts = new List<string>();
            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            foreach (FieldInfo fi in fis)
            {
                //string fieldName = TransField(type, fi.Name);
                string fieldName = TransField(cfis, fi.Name);
                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];
                SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                if (!dbfa.isPK)
                {
                    ret.Params.Add(fieldName, sptr);
                    fields.Add(fieldName);
                    paramts.Add("@" + ClearRectBrace(fieldName));
                }
            }
            ret.Sentence = string.Format(ret.Sentence, string.Join(",", fields.ToArray()), string.Join(",", paramts.ToArray()));
            return ret;
        }

        public static SQLContext GetCommonUpdate(int methodId, Type type)
        {
            SQLContext ret = null;
            if (GetFromCache(methodId, ref ret))
                return ret;

            ret.Sentence = "UPDATE " + type.Name + " SET {0} WHERE {1} ";
            List<string> fieldparamts = new List<string>();
            string cond = string.Empty;
            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            foreach (FieldInfo fi in fis)
            {
                //string fieldName = TransField(type, fi.Name);
                string fieldName = TransField(cfis, fi.Name);
                if (fieldName == "Cdt")
                    continue;
                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];
                SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                ret.Params.Add(fieldName, sptr);
                if (cond == string.Empty && dbfa.isPK)
                {
                    cond = fieldName + "=@" + ClearRectBrace(fieldName);
                }
                else
                {
                    fieldparamts.Add(fieldName + "=@" + ClearRectBrace(fieldName));
                }
            }
            ret.Sentence = string.Format(ret.Sentence, string.Join(",", fieldparamts.ToArray()), cond);
            return ret;
        }

        public static SQLContext GetConditionedUpdate(int methodId, Type type, IList<string> fieldNames, IList<string> ExcpetFieldNames, IList<string> fieldNamesForInc, IList<string> fieldNamesForDec, object equalcond, object likecond, object betweencond, object greatercond, object smallercond, object greaterOrEqualcond, object smallerOrEqualcond, object inSetcond)
        {
            return GetConditionedUpdate_Inner(methodId, type, fieldNames, ExcpetFieldNames, fieldNamesForInc, fieldNamesForDec, equalcond, likecond, betweencond, greatercond, smallercond, greaterOrEqualcond, smallerOrEqualcond, inSetcond, null, null, null);
        }

        public static SQLContext GetConditionedUpdateWith3NotConds(int methodId, Type type, IList<string> fieldNames, IList<string> ExcpetFieldNames, IList<string> fieldNamesForInc, IList<string> fieldNamesForDec, object equalcond, object likecond, object betweencond, object greatercond, object smallercond, object greaterOrEqualcond, object smallerOrEqualcond, object inSetcond, object notEqualcond, object notLikecond, object notInSetcond)
        {
            return GetConditionedUpdate_Inner(methodId, type, fieldNames, ExcpetFieldNames, fieldNamesForInc, fieldNamesForDec, equalcond, likecond, betweencond, greatercond, smallercond, greaterOrEqualcond, smallerOrEqualcond, inSetcond, notEqualcond, notLikecond, notInSetcond);
        }

        private static SQLContext GetConditionedUpdate_Inner(int methodId, Type type, IList<string> fieldNames, IList<string> ExcpetFieldNames, IList<string> fieldNamesForInc, IList<string> fieldNamesForDec, object equalcond, object likecond, object betweencond, object greatercond, object smallercond, object greaterOrEqualcond, object smallerOrEqualcond, object inSetcond, object notEqualcond, object notLikecond, object notInSetcond)
        {
            SQLContext ret = null;
            if (GetFromCache(methodId, ref ret))
                return ret;

            ret.Sentence = "UPDATE " + type.Name + " SET {0} WHERE {1} ";
            List<string> fieldparamts = new List<string>();
            List<string> conds = new List<string>();
            //object default_obj = Activator.CreateInstance(type);
            object default_obj = CreateCtor(type)();
            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);

            foreach (FieldInfo fi in fis)
            {
                //string fieldName = TransField(type, fi.Name);
                string fieldName = TransField(cfis, fi.Name);

                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];

                object default_val = fi.GetValue(default_obj);

                #region Conditions

                if (equalcond != null)
                {
                    object equalval = fi.GetValue(equalcond);
                    if (equalval != null && !equalval.Equals(default_val))
                    {
                        conds.Add(fieldName + "=@" + ClearRectBrace(fieldName));
                        SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                        ret.Params.Add(fieldName, sptr);
                    }
                }

                if (likecond != null)
                {
                    object likeval = fi.GetValue(likecond);
                    if (likeval != null && !likeval.Equals(default_val))
                    {
                        conds.Add(fieldName + " LIKE @" + ClearRectBrace(fieldName));
                        SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                        ret.Params.Add(fieldName, sptr);
                    }
                }

                if (betweencond != null)
                {
                    object betweenval = fi.GetValue(betweencond);
                    if (betweenval != null && !betweenval.Equals(default_val))
                    {
                        conds.Add(fieldName + " BETWEEN @" + DecBeg(ClearRectBrace(fieldName)) + " AND @" + DecEnd(ClearRectBrace(fieldName)));
                        SqlParameter sptr_beg = new SqlParameter("@" + DecBeg(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecBeg(fieldName), sptr_beg);
                        SqlParameter sptr_end = new SqlParameter("@" + DecEnd(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecEnd(fieldName), sptr_end);
                    }
                }

                if (greatercond != null)
                {
                    object greaterval = fi.GetValue(greatercond);
                    if (greaterval != null && !greaterval.Equals(default_val))
                    {
                        conds.Add(fieldName + ">@" + DecG(ClearRectBrace(fieldName)));
                        SqlParameter sptr = new SqlParameter("@" + DecG(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecG(fieldName), sptr);
                    }
                }

                if (smallercond != null)
                {
                    object smallerval = fi.GetValue(smallercond);
                    if (smallerval != null && !smallerval.Equals(default_val))
                    {
                        conds.Add(fieldName + "<@" + DecS(ClearRectBrace(fieldName)));
                        SqlParameter sptr = new SqlParameter("@" + DecS(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecS(fieldName), sptr);
                    }
                }

                if (greaterOrEqualcond != null)
                {
                    object greaterOrEqualval = fi.GetValue(greaterOrEqualcond);
                    if (greaterOrEqualval != null && !greaterOrEqualval.Equals(default_val))
                    {
                        conds.Add(fieldName + ">=@" + DecGE(ClearRectBrace(fieldName)));
                        SqlParameter sptr = new SqlParameter("@" + DecGE(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecGE(fieldName), sptr);
                    }
                }

                if (smallerOrEqualcond != null)
                {
                    object smallerOrEqualval = fi.GetValue(smallerOrEqualcond);
                    if (smallerOrEqualval != null && !smallerOrEqualval.Equals(default_val))
                    {
                        conds.Add(fieldName + "<=@" + DecSE(ClearRectBrace(fieldName)));
                        SqlParameter sptr = new SqlParameter("@" + DecSE(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecSE(fieldName), sptr);
                    }
                }

                if (inSetcond != null)
                {
                    object inSetval = fi.GetValue(inSetcond);
                    if (inSetval != null && !inSetval.Equals(default_val))
                    {
                        conds.Add(fieldName + " IN (" + DecInSet(fieldName) + ")");
                        //SqlParameter sptr = new SqlParameter("@" + DecSE(fieldName), dbfa.fieldType);
                        //ret.Params.Add(DecSE(fieldName), sptr);
                    }
                }

                if (notEqualcond != null)
                {
                    object notEqualval = fi.GetValue(notEqualcond);
                    if (notEqualval != null && !notEqualval.Equals(default_val))
                    {
                        conds.Add(fieldName + "!=@" + ClearRectBrace(fieldName));
                        SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                        ret.Params.Add(fieldName, sptr);
                    }
                }

                if (notLikecond != null)
                {
                    object notLikeval = fi.GetValue(notLikecond);
                    if (notLikeval != null && !notLikeval.Equals(default_val))
                    {
                        conds.Add(fieldName + " NOT LIKE @" + ClearRectBrace(fieldName));
                        SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                        ret.Params.Add(fieldName, sptr);
                    }
                }

                if (notInSetcond != null)
                {
                    object notInSetval = fi.GetValue(notInSetcond);
                    if (notInSetval != null && !notInSetval.Equals(default_val))
                    {
                        conds.Add(fieldName + " NOT IN (" + DecInSet(fieldName) + ")");
                    }
                }

                #endregion

                //if (fieldName == "Cdt")
                //    continue;
                if (fieldName == "Udt" && !(ExcpetFieldNames != null && ExcpetFieldNames.Contains(fieldName)))
                {
                    fieldparamts.Add(fieldName + "=@" + DecSV(ClearRectBrace(fieldName)));
                    SqlParameter sptr = new SqlParameter("@" + DecSV(ClearRectBrace(fieldName)), dbfa.fieldType);
                    ret.Params.Add(DecSV(fieldName), sptr);
                    continue;
                }

                bool defaultAll = (fieldNames == null && fieldNamesForInc == null && fieldNamesForDec == null);
                if (defaultAll)
                {
                    if (fieldName == "Cdt")
                        continue;
                    if (!(ExcpetFieldNames != null && ExcpetFieldNames.Contains(fieldName)))
                    {
                        fieldparamts.Add(fieldName + "=@" + DecSV(ClearRectBrace(fieldName)));
                        SqlParameter sptr = new SqlParameter("@" + DecSV(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecSV(fieldName), sptr);
                    }
                }
                else
                {
                    if (fieldNames != null && fieldNames.Contains(fieldName))
                    {
                        fieldparamts.Add(fieldName + "=@" + DecSV(ClearRectBrace(fieldName)));
                        SqlParameter sptr = new SqlParameter("@" + DecSV(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecSV(fieldName), sptr);
                    }
                    else if (fieldNamesForInc != null && fieldNamesForInc.Contains(fieldName))
                    {
                        fieldparamts.Add(fieldName + "=" + fieldName + "+@" + DecInc(ClearRectBrace(fieldName)));
                        SqlParameter sptr = new SqlParameter("@" + DecInc(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecInc(fieldName), sptr);
                    }
                    else if (fieldNamesForDec != null && fieldNamesForDec.Contains(fieldName))
                    {
                        fieldparamts.Add(fieldName + "=" + fieldName + "-@" + DecDec(ClearRectBrace(fieldName)));
                        SqlParameter sptr = new SqlParameter("@" + DecDec(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecDec(fieldName), sptr);
                    }
                }
            }

            ret.Sentence = string.Format(ret.Sentence, string.Join(",", fieldparamts.ToArray()), (conds.Count > 0 ? string.Join(" AND ", conds.ToArray()) : " 1=1 "));
            return ret;
        }

        public static SQLContext GetCommonDelete(int methodId, Type type)
        {
            SQLContext ret = null;
            if (GetFromCache(methodId, ref ret))
                return ret;

            ret.Sentence = "DELETE FROM " + type.Name + " WHERE {0} ";
            string cond = string.Empty;
            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            foreach (FieldInfo fi in fis)
            {
                //string fieldName = TransField(type, fi.Name);
                string fieldName = TransField(cfis, fi.Name);
                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];
                //SqlParameter sptr = new SqlParameter("@" + fieldName, dbfa.fieldType);
                //ret.Params.Add(fieldName, sptr);
                if (cond == string.Empty && dbfa.isPK)
                {
                    cond = fieldName + "=@" + ClearRectBrace(fieldName);
                    SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                    ret.Params.Add(fieldName, sptr);
                    break;
                }
            }
            ret.Sentence = string.Format(ret.Sentence, cond);
            return ret;
        }

        public static SQLContext GetCommonTruncate(int methodId, Type type)
        {
            SQLContext ret = null;
            if (GetFromCache(methodId, ref ret))
                return ret;

            ret.Sentence = "TRUNCATE TABLE " + type.Name;
            return ret;
        }

        public static SQLContext GetConditionedSelect(int methodId, Type type, object equalcond, object betweencond, object inSetcond)
        {
            return GetConditionedSelect_Inner(methodId, type, equalcond, betweencond, inSetcond, null, true);
        }

        private static SQLContext GetConditionedSelect_Inner(int methodId, Type type, object equalcond, object betweencond, object inSetcond, object nullOrEqualCond, bool andOrOr)
        {
            SQLContext ret = null;
            if (GetFromCache(methodId, ref ret))
                return ret;

            ret.Sentence = "SELECT {0} FROM " + type.Name + " WHERE {1} ";
            List<string> fields = new List<string>();
            List<string> conds = new List<string>();
            //object default_obj = Activator.CreateInstance(type);
            object default_obj = CreateCtor(type)();
            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            int i = 0;
            foreach (FieldInfo fi in fis)
            {
                //string fieldName = TransField(type, fi.Name);
                string fieldName = TransField(cfis, fi.Name);
                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];

                object default_val = fi.GetValue(default_obj);

                if (equalcond != null)
                {
                    object equalval = fi.GetValue(equalcond);
                    if (equalval != null && !equalval.Equals(default_val))
                    {
                        conds.Add(fieldName + "=@" + ClearRectBrace(fieldName));
                        SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                        ret.Params.Add(fieldName, sptr);
                    }
                }

                if (betweencond != null)
                {
                    object betweenval = fi.GetValue(betweencond);
                    if (betweenval != null && !betweenval.Equals(default_val))
                    {
                        conds.Add(fieldName + " BETWEEN @" + DecBeg(ClearRectBrace(fieldName)) + " AND @" + DecEnd(ClearRectBrace(fieldName)));
                        SqlParameter sptr_beg = new SqlParameter("@" + DecBeg(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecBeg(fieldName), sptr_beg);
                        SqlParameter sptr_end = new SqlParameter("@" + DecEnd(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecEnd(fieldName), sptr_end);
                    }
                }

                if (inSetcond != null)
                {
                    object inSetval = fi.GetValue(inSetcond);
                    if (inSetval != null && !inSetval.Equals(default_val))
                    {
                        conds.Add(fieldName + " IN (" + DecInSet(fieldName) + ")");
                    }
                }

                if (nullOrEqualCond != null)
                {
                    object nullOrEqVal = fi.GetValue(nullOrEqualCond);
                    if (nullOrEqVal != null && !nullOrEqVal.Equals(default_val))
                    {
                        conds.Add(string.Format("({0} IS NULL OR {0}={1})", fieldName, "@" + ClearRectBrace(fieldName)));
                        SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                        ret.Params.Add(fieldName, sptr);
                    }
                }

                ret.Indexes.Add(fieldName, i);
                fields.Add(fieldName);
                i++;
            }
            ret.Sentence = string.Format(ret.Sentence, string.Join(",", fields.ToArray()), (conds.Count > 0 ? string.Join(andOrOr ? " AND " : " OR ", conds.ToArray()) : " 1=1 "));
            return ret;
        }

        public static SQLContext GetConditionedSelectExt(int methodId, Type type, object equalcond, object betweencond, object inSetcond, object nullOrEqualCond)
        {
            return GetConditionedSelect_Inner(methodId, type, equalcond, betweencond, inSetcond, nullOrEqualCond, true);
        }

        public static SQLContext GetConditionedSelectExtOr(int methodId, Type type, object equalcond, object betweencond, object inSetcond, object nullOrEqualCond)
        {
            return GetConditionedSelect_Inner(methodId, type, equalcond, betweencond, inSetcond, nullOrEqualCond, false);
        }

        public static SQLContext GetConditionedFuncSelect(int methodId, Type type, string funcName, IList<string> fieldNames, object equalcond, object likecond, object betweencond, object greatercond, object smallercond, object greaterOrEqualcond, object smallerOrEqualcond, object inSetcond)
        {
            return GetConditionedFuncSelect_Inner(methodId, type, funcName, fieldNames,
                equalcond, likecond, betweencond, greatercond, smallercond,
                greaterOrEqualcond, smallerOrEqualcond, inSetcond, null, null, null, true, null, null);
        }

        public static SQLContext GetConditionedFuncSelectExt(int methodId, Type type, string funcName, IList<string> fieldNames, object equalcond, object likecond, object betweencond, object greatercond, object smallercond, object greaterOrEqualcond, object smallerOrEqualcond, object inSetcond, object nullOrEqualCond)
        {
            return GetConditionedFuncSelect_Inner(methodId, type, funcName, fieldNames,
                equalcond, likecond, betweencond, greatercond, smallercond,
                greaterOrEqualcond, smallerOrEqualcond, inSetcond, null, null, null, true, nullOrEqualCond, null);
        }

        public static SQLContext GetConditionedFuncSelectExtRvs(int methodId, Type type, string funcName, IList<string> fieldNames, object equalcond, object likecond, object betweencond, object greatercond, object smallercond, object greaterOrEqualcond, object smallerOrEqualcond, object inSetcond, object nullOrEqualCond, object reverseLikecond)
        {
            return GetConditionedFuncSelect_Inner(methodId, type, funcName, fieldNames,
                equalcond, likecond, betweencond, greatercond, smallercond,
                greaterOrEqualcond, smallerOrEqualcond, inSetcond, null, null, null, true, nullOrEqualCond, reverseLikecond);
        }

        public static SQLContext GetConditionedFuncSelectUsingOrConds(int methodId, Type type, string funcName, IList<string> fieldNames, object equalcond, object likecond, object betweencond, object greatercond, object smallercond, object greaterOrEqualcond, object smallerOrEqualcond, object inSetcond)
        {
            return GetConditionedFuncSelect_Inner(methodId, type, funcName, fieldNames,
                equalcond, likecond, betweencond, greatercond, smallercond,
                greaterOrEqualcond, smallerOrEqualcond, inSetcond, null, null, null, false, null, null);
        }

        private static SQLContext GetConditionedFuncSelect_Inner(int methodId, Type type, string funcName, IList<string> fieldNames, object equalcond, object likecond, object betweencond, object greatercond, object smallercond, object greaterOrEqualcond, object smallerOrEqualcond, object inSetcond, object notEqualcond, object notLikecond, object notInSetcond, bool AndOrOr, object nullOrEqualCond, object reverseLikecond)
        {
            SQLContext ret = null;
            if (GetFromCache(methodId, ref ret))
                return ret;

            ret.Sentence = "SELECT {0} FROM " + type.Name + " WHERE {1} ";
            List<string> fields = new List<string>();
            List<string> conds = new List<string>();
            //object default_obj = Activator.CreateInstance(type);
            object default_obj = CreateCtor(type)();
            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            int i = 0;
            foreach (FieldInfo fi in fis)
            {
                //string fieldName = TransField(type, fi.Name);
                string fieldName = TransField(cfis, fi.Name);
                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];

                object default_val = fi.GetValue(default_obj);

                if (equalcond != null)
                {
                    object equalval = fi.GetValue(equalcond);
                    if (equalval != null && !equalval.Equals(default_val))
                    {
                        conds.Add(fieldName + "=@" + ClearRectBrace(fieldName));
                        SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                        ret.Params.Add(fieldName, sptr);
                    }
                }

                if (likecond != null)
                {
                    object likeval = fi.GetValue(likecond);
                    if (likeval != null && !likeval.Equals(default_val))
                    {
                        conds.Add(fieldName + " LIKE @" + ClearRectBrace(fieldName));
                        SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                        ret.Params.Add(fieldName, sptr);
                    }
                }

                if (reverseLikecond != null)
                {
                    object revlikeval = fi.GetValue(reverseLikecond);
                    if (revlikeval != null && !revlikeval.Equals(default_val))
                    {
                        conds.Add("@" + ClearRectBrace(fieldName) + " LIKE " + fieldName);
                        SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                        ret.Params.Add(fieldName, sptr);
                    }
                }

                if (betweencond != null)
                {
                    object betweenval = fi.GetValue(betweencond);
                    if (betweenval != null && !betweenval.Equals(default_val))
                    {
                        conds.Add(fieldName + " BETWEEN @" + DecBeg(ClearRectBrace(fieldName)) + " AND @" + DecEnd(ClearRectBrace(fieldName)));
                        SqlParameter sptr_beg = new SqlParameter("@" + ClearRectBrace(DecBeg(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecBeg(fieldName), sptr_beg);
                        SqlParameter sptr_end = new SqlParameter("@" + ClearRectBrace(DecEnd(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecEnd(fieldName), sptr_end);
                    }
                }

                if (greatercond != null)
                {
                    object greaterval = fi.GetValue(greatercond);
                    if (greaterval != null && !greaterval.Equals(default_val))
                    {
                        conds.Add(fieldName + ">@" + DecG(ClearRectBrace(fieldName)));
                        SqlParameter sptr = new SqlParameter("@" + DecG(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecG(fieldName), sptr);
                    }
                }

                if (smallercond != null)
                {
                    object smallerval = fi.GetValue(smallercond);
                    if (smallerval != null && !smallerval.Equals(default_val))
                    {
                        conds.Add(fieldName + "<@" + DecS(ClearRectBrace(fieldName)));
                        SqlParameter sptr = new SqlParameter("@" + DecS(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecS(fieldName), sptr);
                    }
                }

                if (greaterOrEqualcond != null)
                {
                    object greaterOrEqualval = fi.GetValue(greaterOrEqualcond);
                    if (greaterOrEqualval != null && !greaterOrEqualval.Equals(default_val))
                    {
                        conds.Add(fieldName + ">=@" + DecGE(ClearRectBrace(fieldName)));
                        SqlParameter sptr = new SqlParameter("@" + DecGE(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecGE(fieldName), sptr);
                    }
                }

                if (smallerOrEqualcond != null)
                {
                    object smallerOrEqualval = fi.GetValue(smallerOrEqualcond);
                    if (smallerOrEqualval != null && !smallerOrEqualval.Equals(default_val))
                    {
                        conds.Add(fieldName + "<=@" + DecSE(ClearRectBrace(fieldName)));
                        SqlParameter sptr = new SqlParameter("@" + DecSE(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecSE(fieldName), sptr);
                    }
                }

                if (inSetcond != null)
                {
                    object inSetval = fi.GetValue(inSetcond);
                    if (inSetval != null && !inSetval.Equals(default_val))
                    {
                        conds.Add(fieldName + " IN (" + DecInSet(fieldName) + ")");
                    }
                }

                if (notEqualcond != null)
                {
                    object notEqualval = fi.GetValue(notEqualcond);
                    if (notEqualval != null && !notEqualval.Equals(default_val))
                    {
                        conds.Add(fieldName + "!=@" + ClearRectBrace(fieldName));
                        SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                        ret.Params.Add(fieldName, sptr);
                    }
                }

                if (notLikecond != null)
                {
                    object notLikeval = fi.GetValue(notLikecond);
                    if (notLikeval != null && !notLikeval.Equals(default_val))
                    {
                        conds.Add(fieldName + " NOT LIKE @" + ClearRectBrace(fieldName));
                        SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                        ret.Params.Add(fieldName, sptr);
                    }
                }

                if (notInSetcond != null)
                {
                    object notInSetval = fi.GetValue(notInSetcond);
                    if (notInSetval != null && !notInSetval.Equals(default_val))
                    {
                        conds.Add(fieldName + " NOT IN (" + DecInSet(fieldName) + ")");
                    }
                }

                if (nullOrEqualCond != null)
                {
                    object nullOrEqVal = fi.GetValue(nullOrEqualCond);
                    if (nullOrEqVal != null && !nullOrEqVal.Equals(default_val))
                    {
                        conds.Add(string.Format("({0} IS NULL OR {0}={1})", fieldName, "@" + ClearRectBrace(fieldName)));
                        SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                        ret.Params.Add(fieldName, sptr);
                    }
                }

                if (fieldNames == null || fieldNames.Contains(fieldName))
                {
                    ret.Indexes.Add(fieldName, i);
                    fields.Add(fieldName);
                    i++;
                }
            }

            string fStr = string.Empty;
            fStr = string.Join(",", fields.ToArray());
            if (funcName != null)
            {
                if (funcName.ToUpper().Equals("DISTINCT") || funcName.ToUpper().StartsWith("TOP "))
                {
                    fStr = string.Format("{0} {1} ", funcName, fStr);
                }
                else
                {
                    fStr = string.Format("{0}({1})", funcName, fStr);
                    ret.Indexes.Clear();
                    ret.Indexes.Add(funcName, 0);
                }
            }

            ret.Sentence = string.Format(ret.Sentence, fStr, (conds.Count > 0 ? string.Join(AndOrOr ? " AND " : " OR ", conds.ToArray()) : " 1=1 "));
            return ret;
        }

        //public static SQLContext GetConditionedGroupBySelect(int methodId, Type type, IList<string> groupByFieldNames, IList<AggregationField> aggregationFields, object equalcond, object likecond, object betweencond, object greatercond, object smallercond, object greaterOrEqualcond, object smallerOrEqualcond, object inSetcond, object notEqualcond, object notLikecond, object notInSetcond, object nullOrEqualCond, object reverseLikecond)
        //{
        //    SQLContext ret = null;
        //    if (GetFromCache(methodId, ref ret))
        //        return ret;

        //    ret.Sentence = "SELECT {0} FROM " + type.Name + " WHERE {1} GROUP BY {2} ";
        //    List<string> fields = new List<string>();
        //    List<string> conds = new List<string>();
        //    List<string> grpBys = new List<string>();
        //    object default_obj = Activator.CreateInstance(type);
        //    FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
        //    //int i = 0;
        //    foreach (FieldInfo fi in fis)
        //    {
        //        string fieldName = TransField(type, fi.Name);
        //        object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
        //        DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];

        //        object default_val = fi.GetValue(default_obj);

        //        #region Conditions

        //        if (equalcond != null)
        //        {
        //            object equalval = fi.GetValue(equalcond);
        //            if (equalval != null && !equalval.Equals(default_val))
        //            {
        //                conds.Add(fieldName + "=@" + ClearRectBrace(fieldName));
        //                SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
        //                ret.Params.Add(fieldName, sptr);
        //            }
        //        }

        //        if (likecond != null)
        //        {
        //            object likeval = fi.GetValue(likecond);
        //            if (likeval != null && !likeval.Equals(default_val))
        //            {
        //                conds.Add(fieldName + " LIKE @" + ClearRectBrace(fieldName));
        //                SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
        //                ret.Params.Add(fieldName, sptr);
        //            }
        //        }

        //        if (reverseLikecond != null)
        //        {
        //            object revlikeval = fi.GetValue(reverseLikecond);
        //            if (revlikeval != null && !revlikeval.Equals(default_val))
        //            {
        //                conds.Add("@" + ClearRectBrace(fieldName) + " LIKE " + fieldName);
        //                SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
        //                ret.Params.Add(fieldName, sptr);
        //            }
        //        }

        //        if (betweencond != null)
        //        {
        //            object betweenval = fi.GetValue(betweencond);
        //            if (betweenval != null && !betweenval.Equals(default_val))
        //            {
        //                conds.Add(fieldName + " BETWEEN @" + DecBeg(ClearRectBrace(fieldName)) + " AND @" + DecEnd(ClearRectBrace(fieldName)));
        //                SqlParameter sptr_beg = new SqlParameter("@" + ClearRectBrace(DecBeg(fieldName)), dbfa.fieldType);
        //                ret.Params.Add(DecBeg(fieldName), sptr_beg);
        //                SqlParameter sptr_end = new SqlParameter("@" + ClearRectBrace(DecEnd(fieldName)), dbfa.fieldType);
        //                ret.Params.Add(DecEnd(fieldName), sptr_end);
        //            }
        //        }

        //        if (greatercond != null)
        //        {
        //            object greaterval = fi.GetValue(greatercond);
        //            if (greaterval != null && !greaterval.Equals(default_val))
        //            {
        //                conds.Add(fieldName + ">@" + DecG(ClearRectBrace(fieldName)));
        //                SqlParameter sptr = new SqlParameter("@" + DecG(ClearRectBrace(fieldName)), dbfa.fieldType);
        //                ret.Params.Add(DecG(fieldName), sptr);
        //            }
        //        }

        //        if (smallercond != null)
        //        {
        //            object smallerval = fi.GetValue(smallercond);
        //            if (smallerval != null && !smallerval.Equals(default_val))
        //            {
        //                conds.Add(fieldName + "<@" + DecS(ClearRectBrace(fieldName)));
        //                SqlParameter sptr = new SqlParameter("@" + DecS(ClearRectBrace(fieldName)), dbfa.fieldType);
        //                ret.Params.Add(DecS(fieldName), sptr);
        //            }
        //        }

        //        if (greaterOrEqualcond != null)
        //        {
        //            object greaterOrEqualval = fi.GetValue(greaterOrEqualcond);
        //            if (greaterOrEqualval != null && !greaterOrEqualval.Equals(default_val))
        //            {
        //                conds.Add(fieldName + ">=@" + DecGE(ClearRectBrace(fieldName)));
        //                SqlParameter sptr = new SqlParameter("@" + DecGE(ClearRectBrace(fieldName)), dbfa.fieldType);
        //                ret.Params.Add(DecGE(fieldName), sptr);
        //            }
        //        }

        //        if (smallerOrEqualcond != null)
        //        {
        //            object smallerOrEqualval = fi.GetValue(smallerOrEqualcond);
        //            if (smallerOrEqualval != null && !smallerOrEqualval.Equals(default_val))
        //            {
        //                conds.Add(fieldName + "<=@" + DecSE(ClearRectBrace(fieldName)));
        //                SqlParameter sptr = new SqlParameter("@" + DecSE(ClearRectBrace(fieldName)), dbfa.fieldType);
        //                ret.Params.Add(DecSE(fieldName), sptr);
        //            }
        //        }

        //        if (inSetcond != null)
        //        {
        //            object inSetval = fi.GetValue(inSetcond);
        //            if (inSetval != null && !inSetval.Equals(default_val))
        //            {
        //                conds.Add(fieldName + " IN (" + DecInSet(fieldName) + ")");
        //            }
        //        }

        //        if (notEqualcond != null)
        //        {
        //            object notEqualval = fi.GetValue(notEqualcond);
        //            if (notEqualval != null && !notEqualval.Equals(default_val))
        //            {
        //                conds.Add(fieldName + "!=@" + ClearRectBrace(fieldName));
        //                SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
        //                ret.Params.Add(fieldName, sptr);
        //            }
        //        }

        //        if (notLikecond != null)
        //        {
        //            object notLikeval = fi.GetValue(notLikecond);
        //            if (notLikeval != null && !notLikeval.Equals(default_val))
        //            {
        //                conds.Add(fieldName + " NOT LIKE @" + ClearRectBrace(fieldName));
        //                SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
        //                ret.Params.Add(fieldName, sptr);
        //            }
        //        }

        //        if (notInSetcond != null)
        //        {
        //            object notInSetval = fi.GetValue(notInSetcond);
        //            if (notInSetval != null && !notInSetval.Equals(default_val))
        //            {
        //                conds.Add(fieldName + " NOT IN (" + DecInSet(fieldName) + ")");
        //            }
        //        }

        //        if (nullOrEqualCond != null)
        //        {
        //            object nullOrEqVal = fi.GetValue(nullOrEqualCond);
        //            if (nullOrEqVal != null && !nullOrEqVal.Equals(default_val))
        //            {
        //                conds.Add(string.Format("({0} IS NULL OR {0}={1})", fieldName, "@" + ClearRectBrace(fieldName)));
        //                SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
        //                ret.Params.Add(fieldName, sptr);
        //            }
        //        }

        //        #endregion

        //        //if (fieldNames == null || fieldNames.Contains(fieldName))
        //        //{
        //        //    ret.Indexes.Add(fieldName, i);
        //        //    fields.Add(fieldName);
        //        //    i++;
        //        //}
        //    }

        //    int i = 0;
        //    foreach(AggregationField aggregationField in aggregationFields)
        //    {
        //        AggregationField curr = new AggregationField(aggregationField);

        //        foreach (FieldInfo fi in fis)
        //        {
        //            string fieldName = TransField(type, fi.Name);
        //            if (curr.fieldNames.Contains(fieldName))
        //            {
        //            }
        //            else
        //            {
        //                curr.fieldNames.Remove(fieldName);
        //            }
        //        }
        //        if (curr.fieldNames.Count > 0)
        //        {
        //            if (curr.asName.Equals(string.Empty) || curr.funcName.Equals(string.Empty))
        //            {
        //                foreach(string fld in curr.fieldNames)
        //                {
        //                    ret.Indexes.Add(fld, i);
        //                    fields.Add(fld);
        //                    i++;
        //                }
        //            }
        //            else
        //            {
        //                ret.Indexes.Add(curr.asName, i);
        //                fields.Add(string.Format("{0}({1}) AS {2}", curr.funcName, string.Join(",", curr.fieldNames.ToArray()), curr.asName));
        //                i++;
        //            }
        //        }
        //    }

        //    string fStr = string.Empty;
        //    fStr = string.Join(",", fields.ToArray());
        //    //if (funcName != null)
        //    //{
        //    //    if (funcName.ToUpper().Equals("DISTINCT") || funcName.ToUpper().StartsWith("TOP "))
        //    //    {
        //    //        fStr = string.Format("{0} {1} ", funcName, fStr);
        //    //    }
        //    //    else
        //    //    {
        //    //        fStr = string.Format("{0}({1})", funcName, fStr);
        //    //        ret.Indexes.Clear();
        //    //        ret.Indexes.Add(funcName, 0);
        //    //    }
        //    //}

        //    ret.Sentence = string.Format(ret.Sentence, fStr, (conds.Count > 0 ? string.Join(" AND ", conds.ToArray()) : " 1=1 "), );
        //    return ret;
        //}

        public static SQLContext GetConditionedFuncSelectWith3NotConds(int methodId, Type type, string funcName, IList<string> fieldNames, object equalcond, object likecond, object betweencond, object greatercond, object smallercond, object greaterOrEqualcond, object smallerOrEqualcond, object inSetcond, object notEqualcond, object notLikecond, object notInSetcond)
        {
            return GetConditionedFuncSelect_Inner(methodId, type, funcName, fieldNames,
                equalcond, likecond, betweencond, greatercond, smallercond,
                greaterOrEqualcond, smallerOrEqualcond, inSetcond, notEqualcond, notLikecond, notInSetcond, true, null, null);
        }

        public static SQLContext GetConditionedFuncSelectWith3NotCondsExt(int methodId, Type type, string funcName, IList<string> fieldNames, object equalcond, object likecond, object betweencond, object greatercond, object smallercond, object greaterOrEqualcond, object smallerOrEqualcond, object inSetcond, object notEqualcond, object notLikecond, object notInSetcond, object nullOrEqualCond)
        {
            return GetConditionedFuncSelect_Inner(methodId, type, funcName, fieldNames,
                equalcond, likecond, betweencond, greatercond, smallercond,
                greaterOrEqualcond, smallerOrEqualcond, inSetcond, notEqualcond, notLikecond, notInSetcond, true, nullOrEqualCond, null);
        }

        public static SQLContext GetConditionedDelete(int methodId, Type type, object equalcond, object betweencond, object inSetcond)
        {
            SQLContext ret = null;
            if (GetFromCache(methodId, ref ret))
                return ret;

            ret.Sentence = "DELETE FROM " + type.Name + " WHERE {0} ";
            List<string> conds = new List<string>();
            //object default_obj = Activator.CreateInstance(type);
            object default_obj = CreateCtor(type)();
            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            foreach (FieldInfo fi in fis)
            {
                //string fieldName = TransField(type, fi.Name);
                string fieldName = TransField(cfis, fi.Name);
                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];

                object default_val = fi.GetValue(default_obj);

                if (equalcond != null)
                {
                    object equalval = fi.GetValue(equalcond);
                    if (equalval != null && !equalval.Equals(default_val))
                    {
                        conds.Add(fieldName + "=@" + ClearRectBrace(fieldName));
                        SqlParameter sptr = new SqlParameter("@" + ClearRectBrace(fieldName), dbfa.fieldType);
                        ret.Params.Add(fieldName, sptr);
                    }
                }

                if (betweencond != null)
                {
                    object betweenval = fi.GetValue(betweencond);
                    if (betweenval != null && !betweenval.Equals(default_val))
                    {
                        conds.Add(fieldName + " BETWEEN @" + DecBeg(ClearRectBrace(fieldName)) + " AND @" + DecEnd(ClearRectBrace(fieldName)));
                        SqlParameter sptr_beg = new SqlParameter("@" + DecBeg(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecBeg(fieldName), sptr_beg);
                        SqlParameter sptr_end = new SqlParameter("@" + DecEnd(ClearRectBrace(fieldName)), dbfa.fieldType);
                        ret.Params.Add(DecEnd(fieldName), sptr_end);
                    }
                }

                if (inSetcond != null)
                {
                    object inSetval = fi.GetValue(inSetcond);
                    if (inSetval != null && !inSetval.Equals(default_val))
                    {
                        conds.Add(fieldName + " IN (" + DecInSet(fieldName) + ")");
                    }
                }
            }

            ret.Sentence = string.Format(ret.Sentence, (conds.Count > 0 ? string.Join(" AND ", conds.ToArray()) : " 1=1 "));
            return ret;
        }

        public static SQLContext GetConditionedJoinedSelect(int methodId, string funcName, ref TableAndFields[] tblAndFldses, TableConnectionCollection tblCnnts)
        {
            SQLContext ret = null;
            bool res1 = GetFromCache(methodId, ref ret);
            bool res2 = GetFromCacheTaF(methodId, ref tblAndFldses);
            if (res1 && res2)
                return ret;

            ret.Sentence = "SELECT {0} FROM {1} WHERE {2} AND {3} ";
            List<string> tables = new List<string>();
            List<string> fields = new List<string>();
            List<string> conds = new List<string>();
            //List<string> cnnts = new List<string>();

            int i = 0;
            int j = 1;
            foreach (TableAndFields tblAndFlds in tblAndFldses)
            {
                string alias = "t" + j.ToString();
                tblAndFlds.alias = alias;

                if (tblAndFlds.subDBCalalog != null)
                    tables.Add(tblAndFlds.subDBCalalog + ".." + tblAndFlds.Table.Name + " " + alias); //Sub Tables
                else
                    tables.Add(tblAndFlds.Table.Name + " " + alias);

                //object default_obj = Activator.CreateInstance(tblAndFlds.Table);
                object default_obj = CreateCtor(tblAndFlds.Table)();
                //FieldInfo[] fis = tblAndFlds.Table.GetFields(BindingFlags.Instance | BindingFlags.Public);
                IDictionary<string, DBFieldAttribute> dbAttrList;
                FieldInfo[] cfis;
                FieldInfo[] fis = getCacheFieldInfos(tblAndFlds.Table, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
                foreach (FieldInfo fi in fis)
                {
                    //string fieldName = TransField(tblAndFlds.Table, fi.Name);
                    string fieldName = TransField(cfis, fi.Name);
                    //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                    //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                    DBFieldAttribute dbfa = dbAttrList[fi.Name];

                    object default_val = fi.GetValue(default_obj);

                    if (tblAndFlds.equalcond != null)
                    {
                        object equalval = fi.GetValue(tblAndFlds.equalcond);
                        if (equalval != null && !equalval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + "=@" + DecAlias(alias, ClearRectBrace(fieldName)));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, ClearRectBrace(fieldName)), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, fieldName), sptr);
                        }
                    }

                    if (tblAndFlds.betweencond != null)
                    {
                        object betweenval = fi.GetValue(tblAndFlds.betweencond);
                        if (betweenval != null && !betweenval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + " BETWEEN @" + DecAlias(alias, DecBeg(ClearRectBrace(fieldName))) + " AND @" + DecAlias(alias, DecEnd(ClearRectBrace(fieldName))));
                            SqlParameter sptr_beg = new SqlParameter("@" + DecAlias(alias, DecBeg(ClearRectBrace(fieldName))), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, DecBeg(fieldName)), sptr_beg);
                            SqlParameter sptr_end = new SqlParameter("@" + DecAlias(alias, DecEnd(ClearRectBrace(fieldName))), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, DecEnd(fieldName)), sptr_end);
                        }
                    }

                    if (tblAndFlds.likecond != null)
                    {
                        object likeval = fi.GetValue(tblAndFlds.likecond);
                        if (likeval != null && !likeval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + " LIKE @" + DecAlias(alias, ClearRectBrace(fieldName)));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, ClearRectBrace(fieldName)), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, fieldName), sptr);
                        }
                    }

                    if (tblAndFlds.greatercond != null)
                    {
                        object greaterval = fi.GetValue(tblAndFlds.greatercond);
                        if (greaterval != null && !greaterval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + ">@" + DecAlias(alias, DecG(ClearRectBrace(fieldName))));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, DecG(ClearRectBrace(fieldName))), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, DecG(fieldName)), sptr);
                        }
                    }

                    if (tblAndFlds.smallercond != null)
                    {
                        object smallerval = fi.GetValue(tblAndFlds.smallercond);
                        if (smallerval != null && !smallerval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + "<@" + DecAlias(alias, DecS(ClearRectBrace(fieldName))));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, DecS(ClearRectBrace(fieldName))), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, DecS(fieldName)), sptr);
                        }
                    }

                    if (tblAndFlds.greaterOrEqualcond != null)
                    {
                        object greaterOrEqualval = fi.GetValue(tblAndFlds.greaterOrEqualcond);
                        if (greaterOrEqualval != null && !greaterOrEqualval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + ">=@" + DecAlias(alias, DecGE(ClearRectBrace(fieldName))));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, DecGE(ClearRectBrace(fieldName))), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, DecGE(fieldName)), sptr);
                        }
                    }

                    if (tblAndFlds.smallerOrEqualcond != null)
                    {
                        object smallerOrEqualval = fi.GetValue(tblAndFlds.smallerOrEqualcond);
                        if (smallerOrEqualval != null && !smallerOrEqualval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + "<=@" + DecAlias(alias, DecSE(ClearRectBrace(fieldName))));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, DecSE(ClearRectBrace(fieldName))), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, DecSE(fieldName)), sptr);
                        }
                    }

                    if (tblAndFlds.inSetcond != null)
                    {
                        object inSetval = fi.GetValue(tblAndFlds.inSetcond);
                        if (inSetval != null && !inSetval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + " IN (" + DecAlias(alias, DecInSet(fieldName)) + ")");
                            //SqlParameter sptr = new SqlParameter("@" + DecSE(fieldName), dbfa.fieldType);
                            //ret.Params.Add(DecSE(fieldName), sptr);
                        }
                    }

                    if (tblAndFlds.notNullcond != null)
                    {
                        object notNullval = fi.GetValue(tblAndFlds.notNullcond);
                        if (notNullval != null && !notNullval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + " IS NOT NULL ");
                        }
                    }

                    if (tblAndFlds.nullcond != null)
                    {
                        object nullval = fi.GetValue(tblAndFlds.nullcond);
                        if (nullval != null && !nullval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + " IS NULL ");
                        }
                    }

                    if (tblAndFlds.notEqualcond != null)
                    {
                        object notEqualval = fi.GetValue(tblAndFlds.notEqualcond);
                        if (notEqualval != null && !notEqualval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + "!=@" + DecAlias(alias, ClearRectBrace(fieldName)));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, ClearRectBrace(fieldName)), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, fieldName), sptr);
                        }
                    }

                    if (tblAndFlds.notLikecond != null)
                    {
                        object notLikeval = fi.GetValue(tblAndFlds.notLikecond);
                        if (notLikeval != null && !notLikeval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + " NOT LIKE @" + DecAlias(alias, ClearRectBrace(fieldName)));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, ClearRectBrace(fieldName)), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, fieldName), sptr);
                        }
                    }

                    if (tblAndFlds.notInSetcond != null)
                    {
                        object notInSetval = fi.GetValue(tblAndFlds.notInSetcond);
                        if (notInSetval != null && !notInSetval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + " NOT IN (" + DecAlias(alias, DecInSet(fieldName)) + ")");
                        }
                    }

                    if (tblAndFlds.nullOrEqual != null)
                    {
                        object nullOrEqVal = fi.GetValue(tblAndFlds.nullOrEqual);
                        if (nullOrEqVal != null && !nullOrEqVal.Equals(default_val))
                        {
                            conds.Add(string.Format("({0} IS NULL OR {0}={1})", DecAliasInner(alias, fieldName), "@" + DecAlias(alias, ClearRectBrace(fieldName))));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, ClearRectBrace(fieldName)), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, fieldName), sptr);
                        }
                    }

                    //string key = DecAlias(tblAndFlds.Table.Name, fieldName);
                    string key = DecAlias(tblAndFlds.GetHashCode().ToString(), fieldName);
                    if (tblCnnts.Regist.ContainsKey(key))
                    {
                        IDictionary<string, TableConnectionItem> point = tblCnnts.Regist[key];
                        foreach (TableConnectionItem tcti in point.Values)
                        {
                            if (!tcti.tbl1_Alias && tblAndFlds == tcti.Tb1)
                            {
                                tcti.alias1 = alias;
                                tcti.tbl1_Alias = true;
                            }
                            if (!tcti.tbl2_Alias && tblAndFlds == tcti.Tb2)
                            {
                                tcti.alias2 = alias;
                                tcti.tbl2_Alias = true;
                            }
                        }
                    }

                    if (/*tblAndFlds.ToGetFieldNames == null ||*/ tblAndFlds.ToGetFieldNames != null && (tblAndFlds.ToGetFieldNames.Count < 1 || tblAndFlds.ToGetFieldNames.Contains(fieldName)))
                    {
                        ret.Indexes.Add(DecAlias(alias, fieldName), i);
                        fields.Add(DecAliasInner(alias, fieldName));
                        i++;
                    }
                }
                j++;
            }

            string fStr = string.Join(",", fields.ToArray());
            if (funcName != null)
            {
                if (funcName.ToUpper().Equals("DISTINCT") || funcName.ToUpper().StartsWith("TOP "))
                {
                    fStr = string.Format("{0} {1} ", funcName, fStr);
                }
                else
                {
                    fStr = string.Format("{0}({1})", funcName, fStr);
                    ret.Indexes.Clear();
                    ret.Indexes.Add(funcName, 0);
                }
            }
            string tStr = string.Join(",", tables.ToArray());
            string oStr = string.Join(" AND ", tblCnnts.BatchToString());
            string cStr = string.Empty;
            if (conds != null && conds.Count > 0)
                cStr = string.Join(" AND ", conds.ToArray());
            else
                cStr = " 1=1 ";

            ret.Sentence = string.Format(ret.Sentence, fStr, tStr, oStr, cStr);
            return ret;
        }

        public static SQLContext GetConditionedComprehensiveJoinedSelect(int methodId, string funcName, ref TableAndFields[] tblAndFldses, TableConnectionCollection tblCnnts, TableBiJoinedLogic tblBiJndLgc)
        {
            return GetConditionedComprehensiveJoinedSelect_Inner(methodId, funcName, ref tblAndFldses, tblCnnts, tblBiJndLgc, true);
        }

        public static SQLContext GetConditionedComprehensiveJoinedSelectExtOr(int methodId, string funcName, ref TableAndFields[] tblAndFldses, TableConnectionCollection tblCnnts, TableBiJoinedLogic tblBiJndLgc)
        {
            return GetConditionedComprehensiveJoinedSelect_Inner(methodId, funcName, ref tblAndFldses, tblCnnts, tblBiJndLgc, false);
        }

        private static SQLContext GetConditionedComprehensiveJoinedSelect_Inner(int methodId, string funcName, ref TableAndFields[] tblAndFldses, TableConnectionCollection tblCnnts, TableBiJoinedLogic tblBiJndLgc, bool andOrOr)
        {
            SQLContext ret = null;
            bool res1 = GetFromCache(methodId, ref ret);
            bool res2 = GetFromCacheTaF(methodId, ref tblAndFldses);
            if (res1 && res2)
                return ret;

            ret.Sentence = "SELECT {0} FROM {1} WHERE {2} ";
            //List<string> tables = new List<string>();
            List<string> fields = new List<string>();
            List<string> conds = new List<string>();
            //List<string> cnnts = new List<string>();

            int i = 0;
            int j = 1;
            foreach (TableAndFields tblAndFlds in tblAndFldses)
            {
                string alias = "t" + j.ToString();
                tblAndFlds.alias = alias;

                //if (tblAndFlds.subDBCalalog != null)
                //    tables.Add(tblAndFlds.subDBCalalog + ".." + tblAndFlds.Table.Name + " " + alias); //Sub Tables
                //else
                //    tables.Add(tblAndFlds.Table.Name + " " + alias);

                //object default_obj = Activator.CreateInstance(tblAndFlds.Table);
                object default_obj = CreateCtor(tblAndFlds.Table)();
                //FieldInfo[] fis = tblAndFlds.Table.GetFields(BindingFlags.Instance | BindingFlags.Public);
                IDictionary<string, DBFieldAttribute> dbAttrList;
                FieldInfo[] cfis;
                FieldInfo[] fis = getCacheFieldInfos(tblAndFlds.Table, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
                foreach (FieldInfo fi in fis)
                {
                    //string fieldName = TransField(tblAndFlds.Table, fi.Name);
                    string fieldName = TransField(cfis, fi.Name);
                    //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                    //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                    DBFieldAttribute dbfa = dbAttrList[fi.Name];

                    object default_val = fi.GetValue(default_obj);

                    #region . Conditions .

                    if (tblAndFlds.equalcond != null)
                    {
                        object equalval = fi.GetValue(tblAndFlds.equalcond);
                        if (equalval != null && !equalval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + "=@" + DecAlias(alias, ClearRectBrace(fieldName)));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, ClearRectBrace(fieldName)), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, fieldName), sptr);
                        }
                    }

                    if (tblAndFlds.betweencond != null)
                    {
                        object betweenval = fi.GetValue(tblAndFlds.betweencond);
                        if (betweenval != null && !betweenval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + " BETWEEN @" + DecAlias(alias, DecBeg(ClearRectBrace(fieldName))) + " AND @" + DecAlias(alias, DecEnd(ClearRectBrace(fieldName))));
                            SqlParameter sptr_beg = new SqlParameter("@" + DecAlias(alias, DecBeg(ClearRectBrace(fieldName))), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, DecBeg(fieldName)), sptr_beg);
                            SqlParameter sptr_end = new SqlParameter("@" + DecAlias(alias, DecEnd(ClearRectBrace(fieldName))), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, DecEnd(fieldName)), sptr_end);
                        }
                    }

                    if (tblAndFlds.likecond != null)
                    {
                        object likeval = fi.GetValue(tblAndFlds.likecond);
                        if (likeval != null && !likeval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + " LIKE @" + DecAlias(alias, ClearRectBrace(fieldName)));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, ClearRectBrace(fieldName)), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, fieldName), sptr);
                        }
                    }

                    if (tblAndFlds.greatercond != null)
                    {
                        object greaterval = fi.GetValue(tblAndFlds.greatercond);
                        if (greaterval != null && !greaterval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + ">@" + DecAlias(alias, DecG(ClearRectBrace(fieldName))));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, DecG(ClearRectBrace(fieldName))), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, DecG(fieldName)), sptr);
                        }
                    }

                    if (tblAndFlds.smallercond != null)
                    {
                        object smallerval = fi.GetValue(tblAndFlds.smallercond);
                        if (smallerval != null && !smallerval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + "<@" + DecAlias(alias, DecS(ClearRectBrace(fieldName))));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, DecS(ClearRectBrace(fieldName))), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, DecS(fieldName)), sptr);
                        }
                    }

                    if (tblAndFlds.greaterOrEqualcond != null)
                    {
                        object greaterOrEqualval = fi.GetValue(tblAndFlds.greaterOrEqualcond);
                        if (greaterOrEqualval != null && !greaterOrEqualval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + ">=@" + DecAlias(alias, DecGE(ClearRectBrace(fieldName))));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, DecGE(ClearRectBrace(fieldName))), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, DecGE(fieldName)), sptr);
                        }
                    }

                    if (tblAndFlds.smallerOrEqualcond != null)
                    {
                        object smallerOrEqualval = fi.GetValue(tblAndFlds.smallerOrEqualcond);
                        if (smallerOrEqualval != null && !smallerOrEqualval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + "<=@" + DecAlias(alias, DecSE(ClearRectBrace(fieldName))));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, DecSE(ClearRectBrace(fieldName))), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, DecSE(fieldName)), sptr);
                        }
                    }

                    if (tblAndFlds.inSetcond != null)
                    {
                        object inSetval = fi.GetValue(tblAndFlds.inSetcond);
                        if (inSetval != null && !inSetval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + " IN (" + DecAlias(alias, DecInSet(fieldName)) + ")");
                            //SqlParameter sptr = new SqlParameter("@" + DecSE(fieldName), dbfa.fieldType);
                            //ret.Params.Add(DecSE(fieldName), sptr);
                        }
                    }

                    if (tblAndFlds.notNullcond != null)
                    {
                        object notNullVal = fi.GetValue(tblAndFlds.notNullcond);
                        if (notNullVal != null && !notNullVal.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + " IS NOT NULL ");
                        }
                    }

                    if (tblAndFlds.nullcond != null)
                    {
                        object nullval = fi.GetValue(tblAndFlds.nullcond);
                        if (nullval != null && !nullval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + " IS NULL ");
                        }
                    }

                    if (tblAndFlds.notEqualcond != null)
                    {
                        object notEqualval = fi.GetValue(tblAndFlds.notEqualcond);
                        if (notEqualval != null && !notEqualval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + "!=@" + DecAlias(alias, ClearRectBrace(fieldName)));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, ClearRectBrace(fieldName)), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, fieldName), sptr);
                        }
                    }

                    if (tblAndFlds.notLikecond != null)
                    {
                        object notLikeval = fi.GetValue(tblAndFlds.notLikecond);
                        if (notLikeval != null && !notLikeval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + " NOT LIKE @" + DecAlias(alias, ClearRectBrace(fieldName)));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, ClearRectBrace(fieldName)), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, fieldName), sptr);
                        }
                    }

                    if (tblAndFlds.notInSetcond != null)
                    {
                        object notInSetval = fi.GetValue(tblAndFlds.notInSetcond);
                        if (notInSetval != null && !notInSetval.Equals(default_val))
                        {
                            conds.Add(DecAliasInner(alias, fieldName) + " NOT IN (" + DecAlias(alias, DecInSet(fieldName)) + ")");
                        }
                    }

                    if (tblAndFlds.nullOrEqual != null)
                    {
                        object nullOrEqVal = fi.GetValue(tblAndFlds.nullOrEqual);
                        if (nullOrEqVal != null && !nullOrEqVal.Equals(default_val))
                        {
                            conds.Add(string.Format("({0} IS NULL OR {0}={1})", DecAliasInner(alias, fieldName), "@" + DecAlias(alias, ClearRectBrace(fieldName))));
                            SqlParameter sptr = new SqlParameter("@" + DecAlias(alias, ClearRectBrace(fieldName)), dbfa.fieldType);
                            ret.Params.Add(DecAlias(alias, fieldName), sptr);
                        }
                    }

                    #endregion

                    //string key = DecAlias(tblAndFlds.Table.Name, fieldName);
                    string key = DecAlias(tblAndFlds.GetHashCode().ToString(), fieldName);
                    if (tblCnnts.Regist.ContainsKey(key))
                    {
                        IDictionary<string, TableConnectionItem> point = tblCnnts.Regist[key];
                        foreach (TableConnectionItem tcti in point.Values)
                        {
                            if (!tcti.tbl1_Alias && tblAndFlds == tcti.Tb1)
                            {
                                tcti.alias1 = alias;
                                tcti.tbl1_Alias = true;
                            }
                            if (!tcti.tbl2_Alias && tblAndFlds == tcti.Tb2)
                            {
                                tcti.alias2 = alias;
                                tcti.tbl2_Alias = true;
                            }
                        }
                    }

                    if (/*tblAndFlds.ToGetFieldNames == null ||*/ tblAndFlds.ToGetFieldNames != null && (tblAndFlds.ToGetFieldNames.Count < 1 || tblAndFlds.ToGetFieldNames.Contains(fieldName)))
                    {
                        ret.Indexes.Add(DecAlias(alias, fieldName), i);
                        fields.Add(DecAliasInner(alias, fieldName));
                        i++;
                    }
                }
                j++;
            }

            string fStr = string.Join(",", fields.ToArray());
            if (funcName != null)
            {
                if (funcName.ToUpper().Equals("DISTINCT") || funcName.ToUpper().StartsWith("TOP "))
                {
                    fStr = string.Format("{0} {1} ", funcName, fStr);
                }
                else
                {
                    fStr = string.Format("{0}({1})", funcName, fStr);
                    ret.Indexes.Clear();
                    ret.Indexes.Add(funcName, 0);
                }
            }

            object lastObj = null;
            string toStr = string.Empty;
            IEnumerator enmrt = tblBiJndLgc.Enum;
            while (enmrt.MoveNext())
            {
                object obj = enmrt.Current;
                if (obj is TableAndFields)
                {
                    toStr += obj.ToString();
                }
                else if (obj is TableConnectionItem)
                {
                    if (lastObj is TableConnectionItem)
                    {
                        if (((TableConnectionItem)obj).AndOrOr)
                            toStr += _andStr;
                        else
                            toStr += _orStr;
                    }
                    else if (lastObj is TableAndFields)
                    {
                        toStr += _onStr;
                    }

                    toStr += obj.ToString();
                }
                else if (obj is string)
                {
                    toStr += obj.ToString();
                }
                lastObj = obj;
            }
            string cStr = (conds.Count > 0 ? string.Join(andOrOr ? " AND " : " OR ", conds.ToArray()) : " 1=1 ");

            ret.Sentence = string.Format(ret.Sentence, fStr, toStr, cStr);
            return ret;
        }

        #endregion

        #region Decoration Methods

        /// <summary>
        /// Set Value For Update SQL
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static string DecSV(string str)
        {
            return str + "_sv";
        }

        internal static string DecG(string str)
        {
            return str + "_G";
        }

        internal static string DecS(string str)
        {
            return str + "_S";
        }

        internal static string DecGE(string str)
        {
            return str + "_GE";
        }

        internal static string DecSE(string str)
        {
            return str + "_SE";
        }

        internal static string DecBeg(string str)
        {
            return str + "_beg";
        }

        internal static string DecEnd(string str)
        {
            return str + "_end";
        }

        internal static string DecInc(string str)
        {
            return str + "_Inc";
        }

        internal static string DecDec(string str)
        {
            return str + "_Dec";
        }

        internal static string DecAlias(string alias, string str)
        {
            return alias + "_" + str;
        }

        internal static string DecAliasInner(string alias, string str)
        {
            return alias + "." + str;
        }

        internal static string DecInSet(string str)
        {
            return "INSET[" + str + "]";
        }

        #endregion

        #region Others

        internal static string ConvertInSet(IList<string> set)
        {
            string ret = string.Empty;
            string[] strs = set.ToArray();
            for (int i = 0; i < strs.Length; i++)
            {
                strs[i] = strs[i].Replace("'", "''");
            }
            ret = string.Format("'{0}'", string.Join("','", strs));
            return ret;
        }

        internal static string ConvertInSet(IList<int> set)
        {
            IList<string> setstr = new List<string>();
            foreach (int str in set)
            {
                setstr.Add(str.ToString());
            }
            return string.Join(",", setstr.ToArray());
        }

        private static string TransField(Type type, string prop)
        {
            FieldInfo fi = type.GetField(fn_ + prop);
            if (fi != null)
            {
                return (string)fi.GetValue(null);
            }
            return string.Empty;
        }

        private static string TransField(FieldInfo[] fieldInfos, string prop)
        {
            //FieldInfo fi = type.GetField("fn_" + prop);
            FieldInfo fi = fieldInfos.Where(x => x.Name == fn_ + prop).FirstOrDefault();
            if (fi != null)
            {
                return (string)fi.GetValue(null);
            }
            return string.Empty;
        }

        internal static string OrderBy
        {
            get { return " ORDER BY {0} "; }
        }

        internal static string OrderByDesc
        {
            get { return " ORDER BY {0} DESC "; }
        }

        internal static string ClearRectBrace(string str)
        {
            return str.Replace("[", string.Empty).Replace("]", string.Empty);
        }

        internal static string MakeKeyForIdxPre(params string[] strKeyNames)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strKeyNames.Length; i++)
            {
                sb.AppendFormat("{0}:", strKeyNames[i]);
                sb.Append("{");
                sb.AppendFormat("{0}", i.ToString());
                sb.Append("};");
            }
            return sb.ToString();
        }

        internal static string MakeKeyForIdx(string preStr, params string[] strKeyValues)
        {
            for (int i = 0; i < strKeyValues.Length; i++)
            {
                if (strKeyValues[i] == null)
                    strKeyValues[i] = "<NULL>";
                else
                    strKeyValues[i] = strKeyValues[i].Trim();
            }
            return string.Format(preStr, strKeyValues);
        }

        internal static DataTable SortColumns(DataTable orig, int[] orderWanted)
        {
            DataTable ret = new DataTable();
            for (int i = 0; i < orderWanted.Length; i++)
            {
                ret.Columns.Add(new DataColumn(orig.Columns[orderWanted[i]].ColumnName, orig.Columns[orderWanted[i]].DataType, orig.Columns[orderWanted[i]].Expression));
            }
            for (int j = 0; j < orig.Rows.Count; j++)
            {
                object[] newRow = new object[orderWanted.Length];
                for (int k = 0; k < orderWanted.Length; k++)
                {
                    newRow[k] = orig.Rows[j][orderWanted[k]];
                }
                ret.Rows.Add(newRow);
            }
            orig = ret;
            return ret;
        }

        #endregion
    }
}
   
