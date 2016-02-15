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
                foreach(int key in _content.Keys)
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
        public IDictionary<string,int> Indexes = new Dictionary<string,int>();

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
            return string.Format(BiOperator, string.Format("{0}.{1}",alias1, FieldNameFromTable1), string.Format("{0}.{1}",alias2, FieldNameFromTable2));
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
        private IDictionary<string, IDictionary<string,TableConnectionItem>> regist = new Dictionary<string, IDictionary<string,TableConnectionItem>>();
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
        public TableBiJoinedLogic(){}
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
            get { return _queue.GetEnumerator();}
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

    #region .  IMES_PCA  .

    internal class SMTMO
    {
        public const string fn_SmtMo = "SMTMO";
        [DBField(SqlDbType.Char, 0, 8, false, true, "")]
        public string SmtMo = null;
        public const string fn_PCBFamily = "PCBFamily";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string PCBFamily = null;
        public const string fn_IECPartNo = "IECPartNo";
        [DBField(SqlDbType.VarChar, 0, 12, false, false, "")]
        public string IECPartNo = null;
        public const string fn_Qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Qty = int.MinValue;
        public const string fn_PrintQty = "PrintQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int PrintQty = int.MinValue;
        public const string fn_Remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Remark = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string Status = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class PCB
    {
        public const string fn_PCBNo = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, true, "")]
        public string PCBNo = null;
        public const string fn_CUSTSN = "CUSTSN";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string CUSTSN = null;
        public const string fn_MAC = "MAC";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public string MAC = null;
        public const string fn_UUID = "UUID";
        [DBField(SqlDbType.Char, 0, 32, true, false, "")]
        public string UUID = null;
        public const string fn_ECR = "ECR";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public string ECR = null;
        public const string fn_IECVER = "IECVER";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public string IECVER = null;
        public const string fn_CUSTVER = "CUSTVER";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string CUSTVER = null;
        public const string fn_SMTMOID = "SMTMO";
        [DBField(SqlDbType.Char, 0, 8, true, false, "")]
        public string SMTMOID = null;
        public const string fn_PCBModelID = "PCBModelID";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public string PCBModelID = null;
        public const string fn_DateCode = "DateCode";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string DateCode = null;
        public const string fn_CVSN = "CVSN";
        [DBField(SqlDbType.Char, 0, 35, true, false, "")]
        public string CVSN = null;

        public const string fn_State = "State";
        [DBField(SqlDbType.Char, 0, 64, true, false, "")]
        public string State = null;

        public const string fn_ShipMode = "ShipMode";
        [DBField(SqlDbType.Char, 0, 64, true, false, "")]
        public string ShipMode = null;

        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;

        public const string fn_unitWeight = "UnitWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal unitWeight = decimal.MinValue;

        public const string fn_cartonWeight = "CartonWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal cartonWeight = decimal.MinValue;

        public const string fn_pizzaID = "PizzaID";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String pizzaID = null;

        public const string fn_palletNo = "PalletNo";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String palletNo = null;

        public const string fn_cartonSN = "CartonSN";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String cartonSN = null;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String deliveryNo = null;

        public const string fn_qcStatus = "QCStatus";
        [DBField(SqlDbType.VarChar, 0, 8, true, false, "")]
        public String qcStatus = null;

        public const string fn_skuModel = "SkuModel";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String skuModel = null;	
    }

    internal class PCB_Part
    {
        public const string fn_bomNodeType = "BomNodeType";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String bomNodeType = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_checkItemType = "CheckItemType";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String checkItemType = null;

        public const string fn_custmerPn = "CustmerPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String custmerPn = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_iecpn = "IECPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String iecpn = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_partSn = "PartSn";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String partSn = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partType = null;

        public const string fn_pcbno = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public String pcbno = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    internal class PCBInfo
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_PCBID = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public string PCBID = null;
        public const string fn_InfoType = "InfoType";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string InfoType = null;
        public const string fn_InfoValue = "InfoValue";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public string InfoValue = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class PCBLog
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_PCBID = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public string PCBID = null;
        public const string fn_PCBModelID = "PCBModel";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public string PCBModelID = null;
        public const string fn_StationID = "Station";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public string StationID = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Status = int.MinValue;
        public const string fn_LineID = "Line";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public string LineID = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class PCBStatus
    {
        public const string fn_PCBID = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, true, "")]
        public string PCBID = null;
        public const string fn_StationID = "Station";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public string StationID = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Status = int.MinValue;
        public const string fn_LineID = "Line";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public string LineID = null;
        public const string fn_TestFailCount = "TestFailCount";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int TestFailCount = int.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class PCBTestLog
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_ActionName = "ActionName";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public string ActionName = null;
        public const string fn_PCBID = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public string PCBID = null;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Type = null;
        public const string fn_LineID = "Line";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public string LineID = null;
        public const string fn_FixtureID = "FixtureID";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string FixtureID = null;
        public const string fn_StationID = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string StationID = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Status = int.MinValue;
        public const string fn_JoinID = "JoinID";
        [DBField(SqlDbType.VarChar, 0, 36, true, false, "")]
        public string JoinID = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_ErrorCode = "ErrorCode";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public string ErrorCode = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 1024, true, false, "")]
        public string Descr = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public String remark = null;
    }

    internal class PCBTestLog_DefectInfo
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_PCBTestLogID = "PCBTestLogID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int PCBTestLogID = int.MinValue;
        public const string fn_DefectCodeID = "DefectCodeID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string DefectCodeID = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class PCBRepair
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_PCBID = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public string PCBID = null;
        public const string fn_PCBModelID = "PCBModelID";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public string PCBModelID = null;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public string Type = null;
        public const string fn_LineID = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string LineID = null;
        public const string fn_StationID = "Station";
        [DBField(SqlDbType.Char, 0, 2, true, false, "")]
        public string StationID = null;
        //public const string fn_PCBTestLogID = "PCBTestLogID";
        //[DBField(SqlDbType.Char, long.MinValue, long.MaxValue, false, false, "")]
        //public long PCBTestLogID = long.MinValue;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Status = int.MinValue;
        //public const string fn_ReturnID = "ReturnID";
        //[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        //public int ReturnID = int.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_TestLogID = "TestLogID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int TestLogID = int.MinValue;
        public const string fn_logID = "LogID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 logID = int.MinValue;
    }

    internal class PCBRepair_DefectInfo
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_PCARepairID = "PCARepairID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int PCARepairID = int.MinValue;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Type = null;
        public const string fn_DefectCodeID = "DefectCode";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string DefectCodeID = null;
        public const string fn_Cause = "Cause";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Cause = null;
        public const string fn_Obligation = "Obligation";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Obligation = null;
        public const string fn_Component = "Component";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Component = null;
        public const string fn_Site = "Site";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Site = null;
        public const string fn_Location = "Location";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Location = null;
        public const string fn_MajorPart = "MajorPart";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string MajorPart = null;
        public const string fn_Remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public string Remark = null;
        public const string fn_VendorCT = "VendorCT";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string VendorCT = null;
        public const string fn_PartType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string PartType = null;
        public const string fn_OldPart = "OldPart";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string OldPart = null;
        public const string fn_OldPartSno = "OldPartSno";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string OldPartSno = null;
        public const string fn_NewPart = "NewPart";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string NewPart = null;
        public const string fn_NewPartSno = "NewPartSno";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string NewPartSno = null;
        public const string fn_NewPartDateCode = "NewPartDateCode";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public string NewPartDateCode = null;
        public const string fn_Manufacture = "Manufacture";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Manufacture = null;
        public const string fn_VersionA = "VersionA";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string VersionA = null;
        public const string fn_VersionB = "VersionB";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string VersionB = null;
        public const string fn_ReturnSign = "ReturnSign";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string ReturnSign = null;
        public const string fn_Mark = "Mark";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string Mark = null;
        public const string fn_Side = "Side";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Side = null;
        public const string fn_SubDefect = "SubDefect";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string SubDefect = null;
        public const string fn_PIAStation = "PIAStation";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string PIAStation = null;
        public const string fn_Distribution = "Distribution";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Distribution = null;
        public const string fn__4M_ = "[4M]";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string _4M_ = null;
        public const string fn_Responsibility = "Responsibility";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Responsibility = null;
        public const string fn_Action = "Action";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Action = null;
        public const string fn_Cover = "Cover";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Cover = null;
        public const string fn_Uncover = "Uncover";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Uncover = null;
        public const string fn_TrackingStatus = "TrackingStatus";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string TrackingStatus = null;
        public const string fn_IsManual = "IsManual";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int IsManual = int.MinValue;
        public const string fn_MTAID = "MTAID";
        [DBField(SqlDbType.VarChar, 0, 14, true, false, "")]
        public string MTAID = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class MACRange
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Code = "Code";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Code = null;
        public const string fn_BegNo = "BegNo";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public string BegNo = null;
        public const string fn_EndNo = "EndNo";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public string EndNo = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string Status = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class EcrVersion
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Family = null;
        public const string fn_MBCode = "MBCode";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public string MBCode = null;
        public const string fn_ECR = "ECR";
        [DBField(SqlDbType.Char, 0, 5, false, false, "")]
        public string ECR = null;
        public const string fn_IECVer = "IECVer";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public string IECVer = null;
        public const string fn_remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public String remark = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class MODismantleLog
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_PCBNo = "PCBNo";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string PCBNo = null;
        public const string fn_SMTMO = "SMTMO";
        [DBField(SqlDbType.Char, 0, 8, false, false, "")]
        public string SMTMO = null;
        public const string fn_Reason = "Reason";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public string Reason = null;
        public const string fn_Tp = "Tp";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Tp = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class SnoLog3D
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_PCBID = "PCBNo";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string PCBID = null;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Type = null;
        public const string fn_LineID = "Line";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public string LineID = null;
        public const string fn_FixtureID = "FixtureID";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string FixtureID = null;
        public const string fn_StationID = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string StationID = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Status = int.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class MTA_SnoRep
    {
        public const string fn_Rep_Id = "Rep_Id";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Rep_Id = int.MinValue;
        public const string fn_Signal = "Signal";
        [DBField(SqlDbType.Char, 0, 50, false, false, "")]
        public string Signal = null;
        public const string fn_Vol = "Vol";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public decimal Vol = decimal.MinValue;
        public const string fn_Diod = "Diod";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int Diod = int.MinValue;
        public const string fn_Freq = "Freq";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public decimal Freq = decimal.MinValue;
        public const string fn_id = "id";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int id = int.MinValue;
    }

    internal class MTA_Location
    {
        public const string fn_Rep_Id = "Rep_Id";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Rep_Id = int.MinValue;
        public const string fn_Location = "Location";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string Location = null;
        public const string fn_Id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Id = int.MinValue;
    }

    internal class MTA_Mark
    {
        public const string fn_Rep_Id = "Rep_Id";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Rep_Id = int.MinValue;
        public const string fn_Defect = "Defect";
        [DBField(SqlDbType.Char, 0, 8, true, false, "")]
        public string Defect = null;
        public const string fn_Version = "Version";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Version = null;
        public const string fn_Mark = "Mark";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string Mark = null;
    }

    internal class SA_FUNHDCP_ID
    {
        public const string fn_MAC = "MAC";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public string MAC = null;
        public const string fn_KSV = "KSV";
        [DBField(SqlDbType.VarChar, 0, 10, false, true, "")]
        public string KSV = null;
        public const string fn_Cnt = "Cnt";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int Cnt = int.MinValue;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class HDCPKey
    {
        public const string fn_KSV = "KSV";
        [DBField(SqlDbType.VarChar, 0, 10, false, true, "")]
        public string KSV = null;
        public const string fn_hdcpKey = "HDCPKey";
        [DBField(SqlDbType.VarBinary, 0, 472, false, false, "")]
        public byte[] hdcpKey = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public string Status = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class ReturnRepair
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_PCBRepairID = "PCBRepairID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int PCBRepairID = int.MinValue;
        public const string fn_ProductRepairID = "ProductRepairID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int ProductRepairID = int.MinValue;
        public const string fn_ProductRepairDefectID = "ProductRepairDefectID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int ProductRepairDefectID = int.MinValue;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class Change_PCB
    {
        public const string fn_OldPCBNo = "OldPCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, true, "")]
        public string OldPCBNo = null;
        public const string fn_NewPCBNo = "NewPCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, false, "")]
        public string NewPCBNo = null;
        public const string fn_Reason = "Reason";
        [DBField(SqlDbType.VarChar, 0, 100, false, false, "")]
        public string Reason = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class TestBoxDataLog
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_PCBNo = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, true, false, "")]
        public string PCBNo = null;
        public const string fn_TestCase = "TestCase";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public string TestCase = null;
        public const string fn_LineNum = "LineNum";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public string LineNum = null;
        public const string fn_WC = "WC";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public string WC = null;
        public const string fn_IsPass = "IsPass";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int IsPass = int.MinValue;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 1024, true, false, "")]
        public string Descr = null;
        public const string fn_ErrorCode = "ErrorCode";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public string ErrorCode = null;
        public const string fn_TestTime = "TestTime";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public string TestTime = null;
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public string ProductID = null;
        public const string fn_SerialNumber = "SerialNumber";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public string SerialNumber = null;
        public const string fn_CartonSn = "CartonSn";
        [DBField(SqlDbType.VarChar, 0, 12, true, false, "")]
        public string CartonSn = null;
        public const string fn_DateManufactured = "DateManufactured";
        [DBField(SqlDbType.VarChar, 0, 12, true, false, "")]
        public string DateManufactured = null;
        public const string fn_MACAddress = "MACAddress";
        [DBField(SqlDbType.VarChar, 0, 12, true, false, "")]
        public string MACAddress = null;
        public const string fn_IMEI = "IMEI";
        [DBField(SqlDbType.VarChar, 0, 15, true, false, "")]
        public string IMEI = null;
        public const string fn_IMSI = "IMSI";
        [DBField(SqlDbType.VarChar, 0, 15, true, false, "")]
        public string IMSI = null;
        public const string fn_EventType = "EventType";
        [DBField(SqlDbType.VarChar, 0, 25, true, false, "")]
        public string EventType = null;
        public const string fn_DeviceAttribute = "DeviceAttribute";
        [DBField(SqlDbType.VarChar, 0, 25, true, false, "")]
        public string DeviceAttribute = null;
        public const string fn_Platform = "Platform";
        [DBField(SqlDbType.VarChar, 0, 25, true, false, "")]
        public string Platform = null;
        public const string fn_ICCID = "ICCID";
        [DBField(SqlDbType.VarChar, 0, 25, true, false, "")]
        public string ICCID = null;
        public const string fn_EAN = "EAN";
        [DBField(SqlDbType.VarChar, 0, 13, true, false, "")]
        public string EAN = null;
        public const string fn_ModelNumber = "ModelNumber";
        [DBField(SqlDbType.VarChar, 0, 8, true, false, "")]
        public string ModelNumber = null;
        public const string fn_PalletSerialNo = "PalletSerialNo";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string PalletSerialNo = null;
        public const string fn_PublicKey = "PublicKey";
        [DBField(SqlDbType.VarChar, 0, 512, true, false, "")]
        public string PublicKey = null;
        public const string fn_PrivateKey = "PrivateKey";
        [DBField(SqlDbType.VarChar, 0, 128, true, false, "")]
        public string PrivateKey = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class PCBAttr
    {
        public const string fn_AttrName = "AttrName";
        [DBField(SqlDbType.VarChar, 0, 64, false, true, "")]
        public string AttrName = null;
        public const string fn_PCBNo = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, false, true, "")]
        public string PCBNo = null;
        public const string fn_AttrValue = "AttrValue";
        [DBField(SqlDbType.NVarChar, 0, 1024, false, false, "")]
        public string AttrValue = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class PCBAttrLog
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public string ID = null;
        public const string fn_PCBNo = "PCBNo";
        [DBField(SqlDbType.Char, 0, 11, true, false, "")]
        public string PCBNo = null;
        public const string fn_PCBModelID = "PCBModelID";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public string PCBModelID = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Station = null;
        public const string fn_AttrName = "AttrName";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public string AttrName = null;
        public const string fn_AttrOldValue = "AttrOldValue";
        [DBField(SqlDbType.NVarChar, 0, 1024, true, false, "")]
        public string AttrOldValue = null;
        public const string fn_AttrNewValue = "AttrNewValue";
        [DBField(SqlDbType.NVarChar, 0, 1024, true, false, "")]
        public string AttrNewValue = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 1024, true, false, "")]
        public string Descr = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    #endregion

    #region .  IMES_GetData  .

    internal class NumControl
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_NoType = "NoType";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string NoType = null;
        public const string fn_NoName = "NoName";
        [DBField(SqlDbType.VarChar, 0, 25, true, false, "")]
        public string NoName = null;
        public const string fn_Value = "Value";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Value = null;
        public const string fn_CustomerID = "Customer";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string CustomerID = null;
    }

    internal class PrintLog
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Name = "Name";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string Name = null;
        public const string fn_BegNo = "BegNo";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string BegNo = null;
        public const string fn_EndNo = "EndNo";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string EndNo = null;

        public const string fn_LabelTemplate = "LabelTemplate";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public string LabelTemplate = null;

        public const string fn_Station = "Station";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string Station = null;

        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public string Descr = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class DefectInfo
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Code = "Code";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Code = null;
        public const string fn_Description = "Description";
        [DBField(SqlDbType.NVarChar, 0, 80, true, false, "")]
        public string Description = null;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string Type = null;
        public const string fn_CustomerID = "CustomerID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string CustomerID = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String Editor = null;
        public const string fn_engDescr = "EngDescr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public String engDescr = null;
    }

    internal class DefectCode
    {
        public const string fn_Defect = "Defect";
        [DBField(SqlDbType.Char, 0, 10, false, true, "")]
        public string Defect = null;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Type = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 80, true, false, "")]
        public string Descr = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_engDescr = "EngDescr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public String engDescr = null;
    }

    internal class ReworkRejectStation
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Customer = "Customer";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Customer = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int Status = int.MinValue;
    }

    internal class RePrintLog
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_LabelName = "LabelName";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string LabelName = null;
        public const string fn_BegNo = "BegNo";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string BegNo = null;
        public const string fn_EndNo = "EndNo";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string EndNo = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public string Descr = null;
        public const string fn_Reason = "Reason";
        [DBField(SqlDbType.NVarChar, 0, 80, true, false, "")]
        public string Reason = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class Label
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Customer = "Customer";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Customer = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 2, true, false, "")]
        public string Station = null;
        public const string fn_LabelName = "LabelName";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string LabelName = null;
        public const string fn_SPC = "SPC";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string SPC = null;
        public const string fn_TemplateName = "TemplateName";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string TemplateName = null;
        public const string fn_Mode = "Mode";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string Mode = null;
        public const string fn_Piece = "Piece";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, true, false, "")]
        public short Piece = short.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;

    }

    internal class Warranty
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Customer = "Customer";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Customer = null;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Type = null;
        public const string fn_DateCodeType = "DateCodeType";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string DateCodeType = null;
        public const string fn_WarrantyFormat = "WarrantyFormat";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string WarrantyFormat = null;
        public const string fn_ShipTypeCode = "ShipTypeCode";
        [DBField(SqlDbType.Char, 0, 2, true, false, "")]
        public string ShipTypeCode = null;
        public const string fn_WarrantyCode = "WarrantyCode";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public string WarrantyCode = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Descr = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class CacheUpdate
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Type = null;
        public const string fn_Item = "Item";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Item = null;
        public const string fn_Updated = "Updated";
        [DBField(SqlDbType.Bit, 1, 1, false, false, "")]
        public bool Updated = default(bool);
        public const string fn_CacheServerIP = "CacheServerIP";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string CacheServerIP = null;
        public const string fn_AppName = "AppName";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string AppName = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class PartCheckSetting
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Customer = "Customer";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Customer = null;
        public const string fn_Tp = "PartType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Tp = null;
        public const string fn_ValueType = "ValueType";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string ValueType = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string Model = null;
        public const string fn_WC = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string WC = null;
        //public const string fn_CheckCollection = "CheckCollection";
        //[DBField(SqlDbType.Char, 0, 1, false, false, "")]
        //public string CheckCollection = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class CheckItem
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Customer = "Customer";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Customer = null;
        public const string fn_ItemName = "ItemName";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string ItemName = null;
        public const string fn_Mode = "Mode";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Mode = int.MinValue;
        public const string fn_ItemType = "ItemType";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int ItemType = int.MinValue;
        //public const string fn_OperationObject = "OperationObject";
        //[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        //public int OperationObject = int.MinValue;
        public const string fn_ItemDisplayName = "ItemDisplayName";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string ItemDisplayName = null;
        public const string fn_MatchRule = "MatchRule";
        [DBField(SqlDbType.VarChar, 0, 255, false, false, "")]
        public string MatchRule = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class CheckItemSetting
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Customer = "Customer";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Customer = null;
        public const string fn_CheckCondition = "CheckCondition";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string CheckCondition = null;
        public const string fn_CheckConditionType = "CheckConditionType";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public string CheckConditionType = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Station = null;
        public const string fn_checkItem = "CheckItemID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int checkItem = int.MinValue;
        public const string fn_CheckType = "CheckType";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int CheckType = int.MinValue;
        public const string fn_CheckValue = "CheckValue";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string CheckValue = null;
        //public const string fn_ItemDisplayName = "ItemDisplayName";
        //[DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        //public string ItemDisplayName = null;
        //public const string fn_MatchRule = "MatchRule";
        //[DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        //public string MatchRule = null;
        //public const string fn_CheckRule = "CheckRule";
        //[DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        //public string CheckRule = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class Model_Process
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        //public const string fn_Customer = "Customer";
        //[DBField(SqlDbType.Char, 0, 10, false, false, "")]
        //public string Customer = null;
        //public const string fn_RuleType = "RuleType";
        //[DBField(SqlDbType.Char, 0, 10, false, false, "")]
        //public string RuleType = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Model = null;
        public const string fn_Process = "Process";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Process = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Line = null;
    }

    internal class Process
    {
        public const string fn_process = "Process";
        [DBField(SqlDbType.VarChar, 0, 20, false, true, "")]
        public string process = null;
        //public const string fn_Name = "Name";
        //[DBField(SqlDbType.Char, 0, 10, true, false, "")]
        //public string Name = null;
        //public const string fn_LastStation = "LastStation";
        //[DBField(SqlDbType.Char, 0, 10, true, false, "")]
        //public string LastStation = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public string Descr = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_type = "Type";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String type = null;
    }

    internal class Rework_Process
    {
        public const string fn_ReworkCode = "ReworkCode";
        [DBField(SqlDbType.Char, 0, 8, false, true, "")]
        public string ReworkCode = null;
        public const string fn_Process = "Process";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Process = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class PalletProcess
    {
        public const string fn_Customer = "Customer";
        [DBField(SqlDbType.Char, 0, 10, false, true, "")]
        public string Customer = null;
        public const string fn_Process = "Process";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Process = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class Process_Station
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Process = "Process";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Process = null;
        //public const string fn_Type = "Type";
        //[DBField(SqlDbType.Char, 0, 10, false, false, "")]
        //public string Type = null;
        public const string fn_PreStation = "PreStation";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string PreStation = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Status = int.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class Station
    {
        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, true, "")]
        public string station = null;
        public const string fn_Name = "Name";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public string Name = null;
        public const string fn_StationType = "StationType";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string StationType = null;
        public const string fn_OperationObject = "OperationObject";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int OperationObject = int.MinValue;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public string Descr = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class Line_Station
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string Line = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Status = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class Line
    {
        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, true, "")]
        public string line = null;
        public const string fn_CustomerID = "CustomerID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string CustomerID = null;
        public const string fn_Stage = "Stage";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Stage = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public string Descr = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class CELDATA //add by yunfeng
    {
        public const string fn_platform = "Platform";
        [DBField(SqlDbType.Char, 0, 100, false, false, "")]
        public String platform = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_productSeriesName = "ProductSeriesName";
        [DBField(SqlDbType.Char, 0, 100, false, false, "")]
        public String productSeriesName = null;

        public const string fn_category = "Category";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String category = null;

        public const string fn_grade = "Grade";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int grade = int.MinValue;

        public const string fn_tec = "TEC";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String tec = null;

        public const string fn_zmod = "ZMOD";
        [DBField(SqlDbType.Char, 0, 20, false, true, "")]
        public String zmod = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 50, false, false, "")]
        public String editor = null;
    }

    internal class Stage
    {
        public const string fn_stage = "Stage";
        [DBField(SqlDbType.Char, 0, 10, false, true, "")]
        public string stage = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Descr = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class Customer
    {
        public const string fn_customer = "Customer";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public string customer = null;
        public const string fn_Code = "Code";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Code = null;
        public const string fn_Plant = "Plant";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Plant = null;
        public const string fn_Description = "Description";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Description = null;
    }

    internal class PCode_LabelType//Station_LabelType
    {
        //public const string fn_ID = "ID";
        //[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        //public int ID = int.MinValue;
        //public const string fn_Station = "Station";
        //[DBField(SqlDbType.Char, 0, 10, false, true, "")]
        //public string Station = null;
        public const string fn_PCode = "PCode";
        [DBField(SqlDbType.Char, 0, 10, false, true, "")]
        public string PCode = null;
        public const string fn_LabelType = "LabelType";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public string LabelType = null;
    }

    internal class PrintTemplate
    {
        public const string fn_TemplateName = "TemplateName";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public string TemplateName = null;
        public const string fn_LabelType = "LabelType";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public string LabelType = null;
        public const string fn_Piece = "Piece";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Piece = int.MinValue;
        public const string fn_SpName = "SpName";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string SpName = null;
        public const string fn_Description = "Description";
        [DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
        public string Description = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_layout = "Layout";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 layout = int.MinValue;
    }

    internal class LabelType
    {
        public const string fn_labelType = "LabelType";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public string labelType = null;
        //public const string fn_Mode = "Mode";
        //[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        //public int Mode = int.MinValue;
        public const string fn_PrintMode = "PrintMode";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int PrintMode = int.MinValue;
        public const string fn_RuleMode = "RuleMode";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int RuleMode = int.MinValue;
        public const string fn_Description = "Description";
        [DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
        public string Description = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class Maintain_MFGAlarm
    {
        public const string fn_Station = "Station";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Station = null;
        public const string fn_DefectTp = "[Defect Tp]";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string DefectTp = null;
        public const string fn_DefectFPF = "[Defect FPF]";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string DefectFPF = null;
        public const string fn_Family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Family = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class MFG_tmpforCount
    {
        public const string fn_SnoId = "SnoId";
        [DBField(SqlDbType.VarChar, 0, 14, false, false, "")]
        public string SnoId = null;
        public const string fn_Pno = "Pno";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public string Pno = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Model = null;
        public const string fn_WC = "WC";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string WC = null;
        public const string fn_PdLine = "PdLine";
        [DBField(SqlDbType.VarChar, 0, 12, false, false, "")]
        public string PdLine = null;
        public const string fn_Defect = "Defect";
        [DBField(SqlDbType.VarChar, 0, 500, true, false, "")]
        public string Defect = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class MFG_AlarmDefect
    {
        public const string fn_Pno = "Pno";
        [DBField(SqlDbType.VarChar, 0, 12, false, false, "")]
        public string Pno = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Model = null;
        public const string fn_PdLine = "PdLine";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string PdLine = null;
        public const string fn_WC = "WC";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string WC = null;
        public const string fn_Defect = "Defect";
        [DBField(SqlDbType.VarChar, 0, 1000, false, false, "")]
        public string Defect = null;
        public const string fn_Symptom = "Symptom";
        [DBField(SqlDbType.VarChar, 0, 8000, false, false, "")]
        public string Symptom = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Mark = "Mark";
        [DBField(SqlDbType.VarChar, 0, 1, false, false, "")]
        public string Mark = null;
    }

    internal class PartCheck
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Customer = "Customer";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Customer = null;
        public const string fn_PartType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string PartType = null;
        public const string fn_ValueType = "ValueType";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string ValueType = null;
        //public const string fn_Mode = "Mode";
        //[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        //public int Mode = int.MinValue;
        public const string fn_NeedSave = "NeedSave";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int NeedSave = int.MinValue;
        public const string fn_NeedCheck = "NeedCheck";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int NeedCheck = int.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class PartCheckMatchRule
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_PartCheckID = "PartCheckID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int PartCheckID = int.MinValue;
        //public const string fn_Customer = "Customer";
        //[DBField(SqlDbType.Char, 0, 10, false, false, "")]
        //public string Customer = null;
        //public const string fn_PartType = "PartType";
        //[DBField(SqlDbType.Char, 0, 10, false, false, "")]
        //public string PartType = null;
        public const string fn_RegExp = "RegExp";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string RegExp = null;
        public const string fn_PnExp = "PnExp";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string PnExp = null;
        public const string fn_PartPropertyExp = "PartPropertyExp";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string PartPropertyExp = null;
        public const string fn_ContainCheckBit = "ContainCheckBit";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int ContainCheckBit = int.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class ErrorMessage
    {
        public const string fn_Code = "Code";
        [DBField(SqlDbType.Char, 0, 10, false, true, "")]
        public string Code = null;
        public const string fn_LanguageCode = "LanguageCode";
        [DBField(SqlDbType.Char, 0, 3, false, true, "")]
        public string LanguageCode = null;
        public const string fn_Message = "Message";
        [DBField(SqlDbType.NVarChar, 0, 255, false, false, "")]
        public string Message = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class PartTypeDescription
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_PartType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string PartType = null;
        public const string fn_Description = "Description";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Description = null;
    }

    internal class MFG_AlarmDefect_Dump
    {
        public const string fn_Pno = "Pno";
        [DBField(SqlDbType.VarChar, 0, 12, false, false, "")]
        public string Pno = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Model = null;
        public const string fn_PdLine = "PdLine";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string PdLine = null;
        public const string fn_WC = "WC";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string WC = null;
        public const string fn_Defect = "Defect";
        [DBField(SqlDbType.VarChar, 0, 1000, false, false, "")]
        public string Defect = null;
        public const string fn_Symptom = "Symptom";
        [DBField(SqlDbType.VarChar, 0, 8000, false, false, "")]
        public string Symptom = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Mark = "Mark";
        [DBField(SqlDbType.VarChar, 0, 1, false, false, "")]
        public string Mark = null;
    }

    //internal class Label_Attribute
    //{
    //    public const string fn_LabelType = "LabelType";
    //    [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
    //    public string LabelType = null;
    //    public const string fn_AttributeName = "AttributeName";
    //    [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
    //    public string AttributeName = null;
    //    public const string fn_Mode = "Mode";
    //    [DBField(SqlDbType.Char, 0, 1, true, false, "")]
    //    public string Mode = null;
    //}

    internal class LabelRuleSet
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_RuleID = "RuleID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int RuleID = int.MinValue;
        public const string fn_AttributeName = "AttributeName";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string AttributeName = null;
        public const string fn_AttributeValue = "AttributeValue";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string AttributeValue = null;
        public const string fn_Mode = "Mode";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public string Mode = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class LabelRule
    {
        public const string fn_RuleID = "RuleID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int RuleID = int.MinValue;
        public const string fn_TemplateName = "TemplateName";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string TemplateName = null;
    }

    internal class MO_Label
    {
        public const string fn_LabelType = "LabelType";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public string LabelType = null;
        public const string fn_MO = "MO";
        [DBField(SqlDbType.VarChar, 0, 20, false, true, "")]
        public string MO = null;
        public const string fn_TemplateName = "TemplateName";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string TemplateName = null;
    }

    internal class PO_Label
    {
        public const string fn_LabelType = "LabelType";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public string LabelType = null;
        public const string fn_PO = "PO";
        [DBField(SqlDbType.VarChar, 0, 20, false, true, "")]
        public string PO = null;
        public const string fn_TemplateName = "TemplateName";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string TemplateName = null;
    }

    internal class PartProcess
    {
        public const string fn_MBFamily = "MBFamily";
        [DBField(SqlDbType.VarChar, 0, 80, false, true, "")]
        public string MBFamily = null;
        public const string fn_Process = "Process";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Process = null;
        public const string fn_PilotRun = "PilotRun";
        [DBField(SqlDbType.VarChar, 0, 1, false, false, "")]
        public string PilotRun = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    //internal class ModelProcessRule
    //{
    //    public const string fn_ID = "ID";
    //    [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
    //    public int ID = int.MinValue;
    //    public const string fn_Customer = "Customer";
    //    [DBField(SqlDbType.Char, 0, 10, true, false, "")]
    //    public string Customer = null;
    //    public const string fn_Family = "Family";
    //    [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
    //    public string Family = null;
    //    public const string fn_Region = "Region";
    //    [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
    //    public string Region = null;
    //    public const string fn_Model = "Model";
    //    [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
    //    public string Model = null;
    //    public const string fn_Process = "Process";
    //    [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
    //    public string Process = null;
    //    public const string fn_Editor = "Editor";
    //    [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
    //    public string Editor = null;
    //    public const string fn_Cdt = "Cdt";
    //    [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
    //    public DateTime Cdt = DateTime.MinValue;
    //    public const string fn_Udt = "Udt";
    //    [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
    //    public DateTime Udt = DateTime.MinValue;
    //}

    internal class ProcessRuleset
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Priority = "Priority";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Priority = int.MinValue;
        public const string fn_Condition1 = "Condition1";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Condition1 = null;
        public const string fn_Condition2 = "Condition2";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Condition2 = null;
        public const string fn_Condition3 = "Condition3";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Condition3 = null;
        public const string fn_Condition4 = "Condition4";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Condition4 = null;
        public const string fn_Condition5 = "Condition5";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Condition5 = null;
        public const string fn_Condition6 = "Condition6";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Condition6 = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class ProcessRule
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_RuleSetID = "RuleSetID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int RuleSetID = int.MinValue;
        public const string fn_Process = "Process";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Process = null;
        public const string fn_Value1 = "Value1";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Value1 = null;
        public const string fn_Value2 = "Value2";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Value2 = null;
        public const string fn_Value3 = "Value3";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Value3 = null;
        public const string fn_Value4 = "Value4";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Value4 = null;
        public const string fn_Value5 = "Value5";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Value5 = null;
        public const string fn_Value6 = "Value6";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Value6 = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    //internal class ProcessRuleItem
    //{
    //    public const string fn_ID = "ID";
    //    [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
    //    public int ID = int.MinValue;
    //    public const string fn_RuleID = "RuleID";
    //    [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
    //    public int RuleID = int.MinValue;
    //    public const string fn_Name = "Name";
    //    [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
    //    public string Name = null;
    //    public const string fn_Value = "Value";
    //    [DBField(SqlDbType.VarChar, 0, 200, false, false, "")]
    //    public string Value = null;
    //}

    internal class TPCB_Maintain
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_TPCB = "TPCB";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string TPCB = null;
        public const string fn_Tp = "Tp";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string Tp = null;
        public const string fn_Vcode = "Vcode";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string Vcode = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class ModelInfoName
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Region = "Region";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Region = null;
        public const string fn_Name = "Name";
        [DBField(SqlDbType.NVarChar, 0, 50, false, false, "")]
        public string Name = null;
        public const string fn_Description = "Description";
        [DBField(SqlDbType.NVarChar, 0, 80, true, false, "")]
        public string Description = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class CacheUpdateServer
    {
        public const string fn_ServerIP = "ServerIP";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public string ServerIP = null;
        public const string fn_AppName = "AppName";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public string AppName = null;

        public const string fn_PortNo = "PortNo";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int PortNo = int.MinValue;

        public const string fn_CustomerList = "CustomerList";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public string CustomerList = null;
    }

    internal class Rework_ReleaseType
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Process = "Process";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Process = null;
        //public const string fn_ReworkCode = "ReworkCode";
        //[DBField(SqlDbType.Char, 0, 8, false, true, "")]
        //public string ReworkCode = null;
        public const string fn_ReleaseType = "ReleaseType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string ReleaseType = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class SFGSite
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Code = "Code";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Code = null;
        public const string fn_InfoType = "InfoType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string InfoType = null;
        public const string fn_InfoValue = "InfoValue";
        [DBField(SqlDbType.VarChar, 0, 200, false, false, "")]
        public string InfoValue = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class List_NewModel
    {
        public const string fn_Family = "Family";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Family = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 12, true, false, "")]
        public string Model = null;
        public const string fn_PlanDate = "PlanDate";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public string PlanDate = null;
        public const string fn_DateFIS = "DateFIS";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string DateFIS = null;
        public const string fn_EditorFIS = "EditorFIS";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string EditorFIS = null;
        public const string fn_CdtFIS = "CdtFIS";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime CdtFIS = DateTime.MinValue;
        public const string fn_DateDMI = "DateDMI";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string DateDMI = null;
        public const string fn_EditorDMI = "EditorDMI";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string EditorDMI = null;
        public const string fn_CdtDMI = "CdtDMI";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime CdtDMI = DateTime.MinValue;
        public const string fn_DateIMG = "DateIMG";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string DateIMG = null;
        public const string fn_EditorIMG = "EditorIMG";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string EditorIMG = null;
        public const string fn_CdtIMG = "CdtIMG";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime CdtIMG = DateTime.MinValue;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public string Status = null;
        public const string fn_OpenStatus = "OpenStatus";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int OpenStatus = int.MinValue;
    }

    internal class NEW_ModelList
    {
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 12, false, false, "")]
        public string Model = null;
        public const string fn_PlanDate = "PlanDate";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public string PlanDate = null;
    }

    internal class OLD_ModelList
    {
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Model = null;
        public const string fn_PlanDate = "PlanDate";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string PlanDate = null;
    }

    internal class Dept
    {
        public const string fn_dept = "Dept";
        [DBField(SqlDbType.VarChar, 0, 4, true, false, "")]
        public string dept = null;
        public const string fn_Section = "Section";
        [DBField(SqlDbType.VarChar, 0, 1, true, false, "")]
        public string Section = null;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Line = null;
        public const string fn_FISLine = "FISLine";
        [DBField(SqlDbType.VarChar, 0, 4, true, false, "")]
        public string FISLine = null;
        public const string fn_StartTime = "StartTime";
        [DBField(SqlDbType.VarChar, 0, 11, true, false, "")]
        public string StartTime = null;
        public const string fn_EndTime = "EndTime";
        [DBField(SqlDbType.VarChar, 0, 11, true, false, "")]
        public string EndTime = null;
        public const string fn_Remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Remark = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class MBCode
    {
        public const string fn_mbCode = "MBCode";
        [DBField(SqlDbType.VarChar, 0, 3, false, true, "")]
        public string mbCode = null;
        public const string fn_Description = "Description";
        [DBField(SqlDbType.NVarChar, 0, 255, true, false, "")]
        public string Description = null;
        public const string fn_MultiQty = "MultiQty";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public short MultiQty = short.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_type = "Type";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String type = null;
    }

    #region .  Bom  .

    internal class MO
    {
        public const string fn_Mo = "MO";
        [DBField(SqlDbType.VarChar, 0, 20, false, true, "")]
        public string Mo = null;
        public const string fn_Plant = "Plant";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Plant = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Model = null;
        public const string fn_CreateDate = "CreateDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime CreateDate = DateTime.MinValue;
        public const string fn_StartDate = "StartDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime StartDate = DateTime.MinValue;
        public const string fn_Qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Qty = int.MinValue;
        public const string fn_SAPStatus = "SAPStatus";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string SAPStatus = null;
        public const string fn_SAPQty = "SAPQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int SAPQty = int.MinValue;
        public const string fn_Print_Qty = "Print_Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Print_Qty = int.MinValue;
        public const string fn_Transfer_Qty = "Transfer_Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Transfer_Qty = int.MinValue;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string Status = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_customerSN_Qty = "CustomerSN_Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 customerSN_Qty = int.MinValue;

        public const string fn_PoNo = "PoNo";
        [DBField(SqlDbType.VarChar, 0, 32, true, false, "")]
        public string PoNo = null;

    }

    internal class Model
    {
        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, true, "")]
        public string model = null;
        public const string fn_Family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Family = null;
        public const string fn_CustPN = "CustPN";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public string CustPN = null;
        public const string fn_Region = "Region";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Region = null;
        public const string fn_ShipType = "ShipType";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string ShipType = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Status = int.MinValue;
        public const string fn_OSCode = "OSCode";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string OSCode = null;
        public const string fn_OSDesc = "OSDesc";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string OSDesc = null;
        public const string fn_Price = "Price";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string Price = null;
        public const string fn_BOMApproveDate = "BOMApproveDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime BOMApproveDate = DateTime.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class ModelInfo
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.BigInt, int.MinValue, int.MaxValue, false, true, "")]
        public long ID = int.MinValue;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Model = null;
        public const string fn_Name = "Name";
        [DBField(SqlDbType.NVarChar, 0, 50, false, false, "")]
        public string Name = null;
        public const string fn_Value = "Value";
        [DBField(SqlDbType.NVarChar, 0, 200, false, false, "")]
        public string Value = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public string Descr = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class Family
    {
        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public string family = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 50, false, false, "")]
        public string Descr = null;
        public const string fn_CustomerID = "CustomerID";
        [DBField(SqlDbType.VarChar, 0, 80, false, false, "")]
        public string CustomerID = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class AssemblyCode
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_PartNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string PartNo = null;
        public const string fn_Family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Family = null;
        public const string fn_Region = "Region";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Region = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string Model = null;
        public const string fn_assemblyCode = "AssemblyCode";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public string assemblyCode = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class Part
    {
        public const string fn_PartNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, true, "")]
        public string PartNo = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public string Descr = null;
        public const string fn_PartType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string PartType = null;
        public const string fn_CustPartNo = "CustPartNo";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string CustPartNo = null;
        //public const string fn_FruNo = "FruNo";
        //[DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        //public string FruNo = null;
        //public const string fn_Vendor = "Vendor";
        //[DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        //public string Vendor = null;
        //public const string fn_IECVersion = "IECVersion";
        //[DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        //public string IECVersion = null;
        public const string fn_AutoDL = "AutoDL";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string AutoDL = null;
        public const string fn_Remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public string Remark = null;
        public const string fn_Flag = "Flag";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Flag = int.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_Descr2 = "Descr2";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public string Descr2 = null;
    }

    internal class PartInfo
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_PartNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string PartNo = null;
        public const string fn_InfoType = "InfoType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string InfoType = null;
        public const string fn_InfoValue = "InfoValue";
        [DBField(SqlDbType.VarChar, 0, 200, false, false, "")]
        public string InfoValue = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class PartTypeMapping
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_SAPType = "SAPType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string SAPType = null;
        public const string fn_FISType = "FISType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string FISType = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class PartTypeAttribute
    {
        public const string fn_PartType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public string PartType = null;
        public const string fn_Code = "Code";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public string Code = null;
        public const string fn_Description = "Description";
        [DBField(SqlDbType.VarChar, 0, 500, true, false, "")]
        public string Description = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class PartTypeEx //New name: PartType_Old
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string partType = null;
        public const string fn_PartTypeGroup = "PartTypeGroup";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string PartTypeGroup = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class MoBOM
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_MO = "MO";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string MO = null;
        public const string fn_PartNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string PartNo = null;
        public const string fn_AssemblyCode = "AssemblyCode";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string AssemblyCode = null;
        public const string fn_Qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int Qty = int.MinValue;
        public const string fn_Group = "[Group]";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int Group = int.MinValue;
        public const string fn_Deviation = "Deviation";
        [DBField(SqlDbType.Bit, 1, 1, true, false, "")]
        public bool Deviation = default(bool);
        public const string fn_Action = "Action";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Action = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class ModelBOM
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Material = "Material";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Material = null;
        public const string fn_Plant = "Plant";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Plant = null;
        public const string fn_Bom = "Bom";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Bom = null;
        public const string fn_Material_group = "Material_group";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Material_group = null;
        public const string fn_Item_text = "Item_text";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Item_text = null;
        public const string fn_Component = "Component";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Component = null;
        public const string fn_Valid_from_date = "Valid_from_date";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Valid_from_date = null;
        public const string fn_Valid_to_date = "Valid_to_date";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Valid_to_date = null;
        public const string fn_Base_qty = "Base_qty";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Base_qty = null;
        public const string fn_Quantity = "Quantity";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Quantity = null;
        public const string fn_UOM = "UOM";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string UOM = null;
        public const string fn_Change_number = "Change_number";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Change_number = null;
        public const string fn_Alternative_item_group = "Alternative_item_group";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Alternative_item_group = null;
        public const string fn_Priority = "Priority";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Priority = null;
        public const string fn_Usage_probability = "Usage_probability";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Usage_probability = null;
        public const string fn_Item_number = "Item_number";
        [DBField(SqlDbType.VarChar, 0, 255, true, false, "")]
        public string Item_number = null;
        public const string fn_Sub_items = "Sub_items";
        [DBField(SqlDbType.VarChar, 0, 8000, true, false, "")]
        public string Sub_items = null;
        public const string fn_Flag = "Flag";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Flag = int.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class ModelDefinition
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_KW = "KW";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string KW = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 250, false, false, "")]
        public string Descr = null;
    }

    internal class AssemblyCodeInfo
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_AssemblyCode = "AssemblyCode";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public string AssemblyCode = null;
        public const string fn_InfoType = "InfoType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string InfoType = null;
        public const string fn_InfoValue = "InfoValue";
        [DBField(SqlDbType.VarChar, 0, 200, false, false, "")]
        public string InfoValue = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class Region
    {
        public const string fn_region = "Region";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public string region = null;
        public const string fn_Description = "Description";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public string Description = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class ShipType
    {
        public const string fn_shipType = "ShipType";
        [DBField(SqlDbType.VarChar, 0, 30, false, true, "")]
        public string shipType = null;
        public const string fn_Description = "Description";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public string Description = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class RefreshModel
    {
        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, true, "")]
        public string model = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, true, "")]
        public string Editor = null;
    }

    #endregion

    #endregion

    #region  .  IMES_FA  .

    internal class Product
    {
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, true, "")]
        public string ProductID = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Model = null;
        public const string fn_PCBID = "PCBID";
        [DBField(SqlDbType.Char, 0, 15, true, false, "")]
        public string PCBID = null;
        public const string fn_PCBModel = "PCBModel";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public string PCBModel = null;
        public const string fn_MAC = "MAC";
        [DBField(SqlDbType.Char, 0, 15, true, false, "")]
        public string MAC = null;
        public const string fn_UUID = "UUID";
        [DBField(SqlDbType.VarChar, 0, 35, true, false, "")]
        public string UUID = null;
        public const string fn_MBECR = "MBECR";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public string MBECR = null;
        public const string fn_CVSN = "CVSN";
        [DBField(SqlDbType.Char, 0, 35, true, false, "")]
        public string CVSN = null;
        public const string fn_CUSTSN = "CUSTSN";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string CUSTSN = null;
        public const string fn_ECR = "ECR";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public string ECR = null;
        //public const string fn_BIOS = "BIOS";
        //[DBField(SqlDbType.Char, 0, 5, true, false, "")]
        //public string BIOS = null;
        //public const string fn_IMGVER = "IMGVER";
        //[DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        //public string IMGVER = null;
        //public const string fn_WMAC = "WMAC";
        //[DBField(SqlDbType.Char, 0, 12, true, false, "")]
        //public string WMAC = null;
        //public const string fn_IMEI = "IMEI";
        //[DBField(SqlDbType.Char, 0, 20, true, false, "")]
        //public string IMEI = null;
        //public const string fn_MEID = "MEID";
        //[DBField(SqlDbType.Char, 0, 20, true, false, "")]
        //public string MEID = null;
        //public const string fn_ICCID = "ICCID";
        //[DBField(SqlDbType.Char, 0, 20, true, false, "")]
        //public string ICCID = null;
        //public const string fn_COAID = "COAID";
        //[DBField(SqlDbType.Char, 0, 20, true, false, "")]
        //public string COAID = null;
        public const string fn_PizzaID = "PizzaID";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string PizzaID = null;
        public const string fn_MO = "MO";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string MO = null;
        public const string fn_UnitWeight = "UnitWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public decimal UnitWeight = decimal.MinValue;
        public const string fn_CartonSN = "CartonSN";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string CartonSN = null;
        public const string fn_CartonWeight = "CartonWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public decimal CartonWeight = decimal.MinValue;
        public const string fn_DeliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string DeliveryNo = null;
        public const string fn_PalletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string PalletNo = null;
        //public const string fn_HDVD = "HDVD";
        //[DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        //public string HDVD = null;
        //public const string fn_BLMAC = "BLMAC";
        //[DBField(SqlDbType.Char, 0, 12, true, false, "")]
        //public string BLMAC = null;
        //public const string fn_TVTuner = "TVTuner";
        //[DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        //public string TVTuner = null;
        public const string fn_ooaid = "OOAID";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String ooaid = null;
        public const string fn_prsn = "PRSN";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String prsn = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_state = "State";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public String state = null;
    }

    internal class UnpackProduct
    {
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, true, "")]
        public string ProductID = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Model = null;
        public const string fn_PCBID = "PCBID";
        [DBField(SqlDbType.Char, 0, 15, true, false, "")]
        public string PCBID = null;
        public const string fn_PCBModel = "PCBModel";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public string PCBModel = null;
        public const string fn_MAC = "MAC";
        [DBField(SqlDbType.Char, 0, 15, true, false, "")]
        public string MAC = null;
        public const string fn_UUID = "UUID";
        [DBField(SqlDbType.VarChar, 0, 35, true, false, "")]
        public string UUID = null;
        public const string fn_MBECR = "MBECR";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public string MBECR = null;
        public const string fn_CVSN = "CVSN";
        [DBField(SqlDbType.Char, 0, 35, true, false, "")]
        public string CVSN = null;
        public const string fn_CUSTSN = "CUSTSN";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string CUSTSN = null;
        public const string fn_ECR = "ECR";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public string ECR = null;
        //public const string fn_BIOS = "BIOS";
        //[DBField(SqlDbType.Char, 0, 5, true, false, "")]
        //public string BIOS = null;
        //public const string fn_IMGVER = "IMGVER";
        //[DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        //public string IMGVER = null;
        //public const string fn_WMAC = "WMAC";
        //[DBField(SqlDbType.Char, 0, 12, true, false, "")]
        //public string WMAC = null;
        //public const string fn_IMEI = "IMEI";
        //[DBField(SqlDbType.Char, 0, 20, true, false, "")]
        //public string IMEI = null;
        //public const string fn_MEID = "MEID";
        //[DBField(SqlDbType.Char, 0, 20, true, false, "")]
        //public string MEID = null;
        //public const string fn_ICCID = "ICCID";
        //[DBField(SqlDbType.Char, 0, 20, true, false, "")]
        //public string ICCID = null;
        //public const string fn_COAID = "COAID";
        //[DBField(SqlDbType.Char, 0, 20, true, false, "")]
        //public string COAID = null;
        public const string fn_PizzaID = "PizzaID";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string PizzaID = null;
        public const string fn_MO = "MO";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string MO = null;
        public const string fn_UnitWeight = "UnitWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public decimal UnitWeight = decimal.MinValue;
        public const string fn_CartonSN = "CartonSN";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string CartonSN = null;
        public const string fn_CartonWeight = "CartonWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public decimal CartonWeight = decimal.MinValue;
        public const string fn_DeliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string DeliveryNo = null;
        public const string fn_PalletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string PalletNo = null;
        //public const string fn_HDVD = "HDVD";
        //[DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        //public string HDVD = null;
        //public const string fn_BLMAC = "BLMAC";
        //[DBField(SqlDbType.Char, 0, 12, true, false, "")]
        //public string BLMAC = null;
        //public const string fn_TVTuner = "TVTuner";
        //[DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        //public string TVTuner = null;
        public const string fn_UEditor = "UEditor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string UEditor = null;
        public const string fn_UPdt = "UPdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime UPdt = DateTime.MinValue;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;
        public const string fn_ooaid = "OOAID";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String ooaid = null;
        public const string fn_prsn = "PRSN";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public String prsn = null;
        public const string fn_state = "State";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public String state = null;
        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    internal class ProductStatus
    {
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, true, "")]
        public string ProductID = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Status = int.MinValue;
        public const string fn_ReworkCode = "ReworkCode";
        [DBField(SqlDbType.Char, 0, 8, false, false, "")]
        public string ReworkCode = null;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string Line = null;
        public const string fn_TestFailCount = "TestFailCount";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int TestFailCount = int.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class UnpackProductStatus
    {
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, true, "")]
        public string ProductID = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Status = int.MinValue;
        public const string fn_ReworkCode = "ReworkCode";
        [DBField(SqlDbType.Char, 0, 8, false, false, "")]
        public string ReworkCode = null;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string Line = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_UEditor = "UEditor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string UEditor = null;
        public const string fn_UPdt = "UPdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime UPdt = DateTime.MinValue;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;
        public const string fn_testFailCount = "TestFailCount";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 testFailCount = int.MinValue;
    }

    internal class ChangeLog
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string ProductID = null;
        public const string fn_Mo = "Mo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string Mo = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class ProductLog
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string ProductID = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Model = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Status = int.MinValue;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string Line = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }
 
    internal class ProductInfo
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string ProductID = null;
        public const string fn_InfoType = "InfoType";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string InfoType = null;
        public const string fn_InfoValue = "InfoValue";
        [DBField(SqlDbType.VarChar, 0, 35, true, false, "")]
        public string InfoValue = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class ProductTestLog
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_ActionName = "ActionName";
        [DBField(SqlDbType.VarChar, 0, 64, true, false, "")]
        public string ActionName = null;
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string ProductID = null;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Type = null;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public string Line = null;
        public const string fn_FixtureID = "FixtureID";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string FixtureID = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Station = null;
        public const string fn_ErrorCode = "ErrorCode";
        [DBField(SqlDbType.VarChar, 0, 16, true, false, "")]
        public string ErrorCode = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 1024, true, false, "")]
        public string Descr = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Status = int.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class ProductTestLog_DefectInfo
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_ProductTestLogID = "ProductTestLogID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int ProductTestLogID = int.MinValue;
        public const string fn_DefectCodeID = "DefectCodeID";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string DefectCodeID = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class ProductRepair
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string ProductID = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Model = null;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Type = null;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public string Line = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Station = null;
        public const string fn_TestLogID = "TestLogID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int TestLogID = int.MinValue;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Status = int.MinValue;
        //public const string fn_ReturnID = "ReturnID";
        //[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        //public int ReturnID = int.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_logID = "LogID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public Int32 logID = int.MinValue;
    }

    internal class ProductRepair_DefectInfo
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_ProductRepairID = "ProductRepairID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int ProductRepairID = int.MinValue;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Type = null;
        public const string fn_DefectCodeID = "DefectCode";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string DefectCodeID = null;
        public const string fn_Cause = "Cause";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Cause = null;
        public const string fn_Obligation = "Obligation";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Obligation = null;
        public const string fn_Component = "Component";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Component = null;
        public const string fn_Site = "Site";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Site = null;
        public const string fn_Location = "Location";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Location = null;
        public const string fn_MajorPart = "MajorPart";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string MajorPart = null;
        public const string fn_Remark = "Remark";
        [DBField(SqlDbType.NVarChar, 0, 100, true, false, "")]
        public string Remark = null;
        public const string fn_VendorCT = "VendorCT";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string VendorCT = null;
        public const string fn_PartType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string PartType = null;
        public const string fn_OldPart = "OldPart";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string OldPart = null;
        public const string fn_OldPartSno = "OldPartSno";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string OldPartSno = null;
        public const string fn_NewPart = "NewPart";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string NewPart = null;
        public const string fn_NewPartSno = "NewPartSno";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string NewPartSno = null;
        public const string fn_Manufacture = "Manufacture";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Manufacture = null;
        public const string fn_VersionA = "VersionA";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string VersionA = null;
        public const string fn_VersionB = "VersionB";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string VersionB = null;
        public const string fn_ReturnSign = "ReturnSign";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string ReturnSign = null;
        public const string fn_Mark = "Mark";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string Mark = null;
        public const string fn_SubDefect = "SubDefect";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string SubDefect = null;
        public const string fn_PIAStation = "PIAStation";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string PIAStation = null;
        public const string fn_Distribution = "Distribution";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Distribution = null;
        public const string fn__4M = "[4M]";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string _4M = null;
        public const string fn_Responsibility = "Responsibility";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Responsibility = null;
        public const string fn_Action = "Action";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Action = null;
        public const string fn_Cover = "Cover";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Cover = null;
        public const string fn_Uncover = "Uncover";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Uncover = null;
        public const string fn_TrackingStatus = "TrackingStatus";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string TrackingStatus = null;
        public const string fn_IsManual = "IsManual";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int IsManual = int.MinValue;
        public const string fn_MTAID = "MTAID";
        [DBField(SqlDbType.VarChar, 0, 10, true, false, "")]
        public string MTAID = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_returnStn = "ReturnStn";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String returnStn = null;
    }

    public class Product_Part
    {
        public const string fn_bomNodeType = "BomNodeType";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String bomNodeType = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_checkItemType = "CheckItemType";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String checkItemType = null;

        public const string fn_custmerPn = "CustmerPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String custmerPn = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_iecpn = "IECPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String iecpn = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_partSn = "PartSn";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String partSn = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partType = null;

        public const string fn_productID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String productID = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String station = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;


    }

    internal class PartForbidden
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Family = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public string Model = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Descr = null;
        public const string fn_PartNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string PartNo = null;
        public const string fn_AssemblyCode = "AssemblyCode";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string AssemblyCode = null;
        //public const string fn_PartType = "PartType";
        //[DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        //public string PartType = null;
        //public const string fn_DateCode = "DateCode";
        //[DBField(SqlDbType.Char, 0, 10, true, false, "")]
        //public string DateCode = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Status = int.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class Rework
    {
        public const string fn_ReworkCode = "ReworkCode";
        [DBField(SqlDbType.Char, 0, 8, false, true, "")]
        public string ReworkCode = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public string Status = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class Rework_Product
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_ReworkCode = "ReworkCode";
        [DBField(SqlDbType.Char, 0, 8, false, false, "")]
        public string ReworkCode = null;
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string ProductID = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Model = null;
        public const string fn_PCBID = "PCBID";
        [DBField(SqlDbType.Char, 0, 15, true, false, "")]
        public string PCBID = null;
        public const string fn_PCBModel = "PCBModel";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public string PCBModel = null;
        public const string fn_MAC = "MAC";
        [DBField(SqlDbType.Char, 0, 15, true, false, "")]
        public string MAC = null;
        public const string fn_UUID = "UUID";
        [DBField(SqlDbType.VarChar, 0, 35, true, false, "")]
        public string UUID = null;
        public const string fn_MBECR = "MBECR";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public string MBECR = null;
        public const string fn_CVSN = "CVSN";
        [DBField(SqlDbType.Char, 0, 15, true, false, "")]
        public string CVSN = null;
        public const string fn_CUSTSN = "CUSTSN";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string CUSTSN = null;
        public const string fn_ECR = "ECR";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public string ECR = null;
        public const string fn_BIOS = "BIOS";
        [DBField(SqlDbType.Char, 0, 5, true, false, "")]
        public string BIOS = null;
        public const string fn_IMGVER = "IMGVER";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string IMGVER = null;
        public const string fn_WMAC = "WMAC";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public string WMAC = null;
        public const string fn_IMEI = "IMEI";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string IMEI = null;
        public const string fn_MEID = "MEID";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string MEID = null;
        public const string fn_ICCID = "ICCID";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string ICCID = null;
        public const string fn_COAID = "COAID";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string COAID = null;
        public const string fn_PizzaID = "PizzaID";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string PizzaID = null;
        public const string fn_MO = "MO";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string MO = null;
        public const string fn_UnitWeight = "UnitWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public decimal UnitWeight = decimal.MinValue;
        public const string fn_CartonSN = "CartonSN";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string CartonSN = null;
        public const string fn_CartonWeight = "CartonWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public decimal CartonWeight = decimal.MinValue;
        public const string fn_DeliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string DeliveryNo = null;
        public const string fn_PalletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string PalletNo = null;
        //public const string fn_Cdt = "Cdt";
        //[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        //public DateTime Cdt = DateTime.MinValue;
        public const string fn_HDVD = "HDVD";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string HDVD = null;
        public const string fn_BLMAC = "BLMAC";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public string BLMAC = null;
        public const string fn_TVTuner = "TVTuner";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string TVTuner = null;
    }

    internal class PartSN
    {
        public const string fn_IECSN = "IECSN";
        [DBField(SqlDbType.VarChar, 0, 30, false, true, "")]
        public string IECSN = null;
        public const string fn_IECPn = "IECPn";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string IECPn = null;
        public const string fn_PartType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string PartType = null;
        public const string fn_VendorSN = "VendorSN";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string VendorSN = null;
        public const string fn_DateCode = "DateCode";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string DateCode = null;
        public const string fn_VendorDCode = "VendorDCode";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string VendorDCode = null;
        public const string fn_VCode = "VCode";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string VCode = null;
        public const string fn__151PartNo = "[151PartNo]";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public string _151PartNo = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class HDDCopyInfo
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_CopyMachineID = "CopyMachineID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string CopyMachineID = null;
        public const string fn_ConnectorID = "ConnectorID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string ConnectorID = null;
        public const string fn_SourceHDD = "SourceHDD";
        [DBField(SqlDbType.Char, 0, 15, false, false, "")]
        public string SourceHDD = null;
        public const string fn_HDDNo = "HDDNo";
        [DBField(SqlDbType.Char, 0, 15, false, false, "")]
        public string HDDNo = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class QCStatus
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string ProductID = null;
        public const string fn_Tp = "Tp";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Tp = null;
        //public const string fn_PdLine = "PdLine";
        //[DBField(SqlDbType.Char, 0, 10, true, false, "")]
        //public string PdLine = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Model = null;
        //public const string fn_Date = "Date";
        //[DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        //public DateTime Date = DateTime.MinValue;
        //public const string fn_CustSN = "CustSN";
        //[DBField(SqlDbType.Char, 0, 20, true, false, "")]
        //public string CustSN = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string Status = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string Line = null;
        public const string fn_remark = "Remark";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public String remark = null;
    }

    internal class QCRatio
    {
        //public const string fn_ID = "ID";
        //[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        //public int ID = int.MinValue;
        public const string fn_Family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, true, "")]
        public string Family = null;
        public const string fn_qcRatio = "QCRatio";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int qcRatio = int.MinValue;
        public const string fn_EOQCRatio = "EOQCRatio";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int EOQCRatio = int.MinValue;
        public const string fn_PAQCRatio = "PAQCRatio";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int PAQCRatio = int.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_RPAQCRatio = "RPAQCRatio";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int RPAQCRatio = int.MinValue;
    }

    internal class Rework_ProductStatus
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_ReworkCode = "ReworkCode";
        [DBField(SqlDbType.Char, 0, 8, false, false, "")]
        public string ReworkCode = null;
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string ProductID = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Status = int.MinValue;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string Line = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class Rework_Product_Part
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_ReworkCode = "ReworkCode";
        [DBField(SqlDbType.Char, 0, 8, false, false, "")]
        public string ReworkCode = null;
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string ProductID = null;
        public const string fn_PartNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string PartNo = null;
        public const string fn_Value = "Value";
        [DBField(SqlDbType.VarChar, 0, 35, false, false, "")]
        public string Value = null;
        public const string fn_ValueType = "ValueType";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string ValueType = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class KittingBoxSN
    {
        public const string fn_SnoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 9, false, false, "")]
        public string SnoId = null;
        public const string fn_Tp = "Tp";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public string Tp = null;
        public const string fn_Sno = "Sno";
        [DBField(SqlDbType.Char, 0, 4, false, false, "")]
        public string Sno = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string Status = null;
        public const string fn_Remark = "Remark";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string Remark = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class BinData
    {
        public const string fn_SnoId = "SnoId";
        [DBField(SqlDbType.Char, 0, 9, false, false, "")]
        public string SnoId = null;
        public const string fn_Bin = "Bin";
        [DBField(SqlDbType.Char, 0, 4, false, false, "")]
        public string Bin = null;
        public const string fn_Qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Qty = int.MinValue;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class LCMBind
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_LCMSno = "LCMSno";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string LCMSno = null;
        public const string fn_MESno = "MESno";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string MESno = null;
        public const string fn_METype = "METype";
        [DBField(SqlDbType.VarChar, 0, 5, true, false, "")]
        public string METype = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class Rework_ProductInfo
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_ReworkCode = "ReworkCode";
        [DBField(SqlDbType.Char, 0, 8, false, false, "")]
        public string ReworkCode = null;
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string ProductID = null;
        public const string fn_InfoType = "InfoType";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string InfoType = null;
        public const string fn_InfoValue = "InfoValue";
        [DBField(SqlDbType.VarChar, 0, 35, true, false, "")]
        public string InfoValue = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    [Table("RunInTimeControl")]
    public class RunInTimeControl
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String code = null;

        public const string fn_controlType = "ControlType";
        [DBField(SqlDbType.Bit, 1, 1, true, false, "")]
        public Boolean controlType = default(bool);

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_hour = "Hour";
        [DBField(SqlDbType.Char, 0, 4, false, false, "")]
        public String hour = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 100, true, false, "")]
        public String remark = null;

        public const string fn_testStation = "TestStation";
        [DBField(SqlDbType.Char, 0, 2, true, false, "")]
        public String testStation = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String type = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("RunInTimeControlLog")]
    public class RunInTimeControlLog
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_code = "Code";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String code = null;

        public const string fn_controlType = "ControlType";
        [DBField(SqlDbType.Bit, 1, 1, true, false, "")]
        public Boolean controlType = default(bool);

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_hour = "Hour";
        [DBField(SqlDbType.Char, 0, 4, false, false, "")]
        public String hour = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 id = int.MinValue;

        public const string fn_remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 100, true, false, "")]
        public String remark = null;

        public const string fn_testStation = "TestStation";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String testStation = null;

        public const string fn_type = "Type";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public String type = null;
    }

    internal class BSParts
    {
        public const string fn_PartNo = "PartNo";
        [DBField(SqlDbType.Char, 0, 12, false, true, "")]
        public string PartNo = null;
        public const string fn_FURNO = "FRUNO";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string FURNO = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 100, true, false, "")]
        public string Descr = null;
        public const string fn_PartType = "PartType";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string PartType = null;
        //public const string fn_Code = "Code";
        //[DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        //public string Code = null;
    }

    internal class WLBTDescr
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Code = "Code";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string Code = null;
        public const string fn_Tp = "Tp";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Tp = null;
        public const string fn_TpDescr = "TpDescr";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string TpDescr = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 50, false, false, "")]
        public string Descr = null;
        public const string fn_Site = "Site";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Site = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class TraceStd
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Family = null;
        public const string fn_Area = "Area";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Area = null;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Type = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class TPCB
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Family = "Family";
        [DBField(SqlDbType.NVarChar, 0, 50, true, false, "")]
        public string Family = null;
        public const string fn_PdLine = "PdLine";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public string PdLine = null;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public string Type = null;
        public const string fn_PartNo = "PartNo";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public string PartNo = null;
        public const string fn_Vendor = "Vendor";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public string Vendor = null;
        public const string fn_Dcode = "Dcode";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public string Dcode = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class KittingCode
    {
        public const string fn_Code = "Code";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string Code = null;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.Char, 0, 15, false, false, "")]
        public string Type = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.Char, 0, 50, false, false, "")]
        public string Descr = null;
        public const string fn_Remark = "Remark";
        [DBField(SqlDbType.Char, 0, 255, false, false, "")]
        public string Remark = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class TPCBDet
    {
        public const string fn_Code = "Code";
        [DBField(SqlDbType.NVarChar, 0, 30, true, false, "")]
        public string Code = null;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public string Type = null;
        public const string fn_PartNo = "PartNo";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public string PartNo = null;
        public const string fn_Vendor = "Vendor";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public string Vendor = null;
        public const string fn_Dcode = "Dcode";
        [DBField(SqlDbType.NVarChar, 0, 20, true, false, "")]
        public string Dcode = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class PilotRunPrintInfo
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Family = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Model = null;
        public const string fn_Build = "Build";
        [DBField(SqlDbType.NChar, 0, 10, false, false, "")]
        public string Build = null;
        public const string fn_SKU = "SKU";
        [DBField(SqlDbType.NChar, 0, 10, false, false, "")]
        public string SKU = null;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string Type = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string Descr = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class PilotRunPrintBuild
    {
        public const string fn_Build = "Build";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string Build = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class PilotRunPrintType
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string Type = null;
    }

    internal class BorrowLog
    {
        public const string fn_acceptor = "Acceptor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String acceptor = null;

        public const string fn_bdate = "Bdate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime bdate = DateTime.MinValue;

        public const string fn_borrower = "Borrower";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String borrower = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_lender = "Lender";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String lender = null;

        public const string fn_model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String model = null;

        public const string fn_rdate = "Rdate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime rdate = DateTime.MinValue;

        public const string fn_returner = "Returner";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String returner = null;

        public const string fn_sn = "Sn";
        [DBField(SqlDbType.VarChar, 0, 14, false, false, "")]
        public String sn = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public String status = null;
    }

    //Temporary Table
    internal class TempProductID
    {
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string ProductID = null;
        public const string fn_UserKey = "UserKey";
        [DBField(SqlDbType.Char, 0, 32, false, false, "")]
        public string UserKey = null;
    }

    internal class SmallPartsUpload
    {
        public const string fn_TSBPN = "TSBPN";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string TSBPN = null;
        public const string fn_IECPN = "IECPN";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string IECPN = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
    }

    internal class FA_Station
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string Line = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;
        public const string fn_OptCode = "OptCode";
        [DBField(SqlDbType.Char, 0, 9, false, false, "")]
        public string OptCode = null;
        public const string fn_OptName = "OptName";
        [DBField(SqlDbType.Char, 0, 8, false, false, "")]
        public string OptName = null;
        public const string fn_Remark = "Remark";
        [DBField(SqlDbType.Char, 0, 50, false, false, "")]
        public string Remark = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class ProductAttr
    {
        public const string fn_AttrName = "AttrName";
        [DBField(SqlDbType.VarChar, 0, 64, false, true, "")]
        public string AttrName = null;
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, true, "")]
        public string ProductID = null;
        public const string fn_AttrValue = "AttrValue";
        [DBField(SqlDbType.VarChar, 0, 1024, false, false, "")]
        public string AttrValue = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class ProductAttrLog
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public string ID = null;
        public const string fn_ProductID = "ProductID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string ProductID = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Model = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;
        public const string fn_AttrName = "AttrName";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public string AttrName = null;
        public const string fn_AttrOldValue = "AttrOldValue";
        [DBField(SqlDbType.VarChar, 0, 1024, false, false, "")]
        public string AttrOldValue = null;
        public const string fn_AttrNewValue = "AttrNewValue";
        [DBField(SqlDbType.VarChar, 0, 1024, false, false, "")]
        public string AttrNewValue = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 1024, false, false, "")]
        public string Descr = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    #endregion

    #region .  IMES_PAK  .

    internal class BoxerBookData
    {
        public const string fn_ID = "id";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public string ID = null;
        public const string fn_MBNo = "MBNo";
        [DBField(SqlDbType.Char, 0, 11, true, false, "")]
        public string MBNo = null;
        public const string fn_TCaseNo = "TCaseNo";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string TCaseNo = null;
        public const string fn_TLineNo = "TLineNo";
        [DBField(SqlDbType.Char, 0, 3, true, false, "")]
        public string TLineNo = null;
        public const string fn_TStatNo = "TStatNo";
        [DBField(SqlDbType.Decimal, int.MinValue, int.MaxValue, true, false, "")]
        public int TStatNo = int.MinValue;
        public const string fn_IsPass = "IsPass";
        [DBField(SqlDbType.Decimal, int.MinValue, int.MaxValue, true, false, "")]
        public int IsPass = int.MinValue;
        public const string fn_Desc = "[Desc]";
        [DBField(SqlDbType.VarChar, 0, 25, true, false, "")]
        public string Desc = null;
        public const string fn_ErrorCode = "ErrorCode";
        [DBField(SqlDbType.Char, 0, 4, true, false, "")]
        public string ErrorCode = null;
        public const string fn_datetime = "datetime";
        [DBField(SqlDbType.Char, 0, 14, true, false, "")]
        public string datetime = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_PID = "PID";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string PID = null;
        public const string fn_SerialNumber = "SerialNumber";
        [DBField(SqlDbType.Char, 0, 16, true, false, "")]
        public string SerialNumber = null;
        public const string fn_CartonSN = "CartonSN";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public string CartonSN = null;
        public const string fn_DateManufactured = "DateManufactured";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public string DateManufactured = null;
        public const string fn_PublicKey = "PublicKey";
        [DBField(SqlDbType.VarChar, 0, 500, true, false, "")]
        public string PublicKey = null;
        public const string fn_MACAddress = "MACAddress";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public string MACAddress = null;
        public const string fn_IMEI = "IMEI";
        [DBField(SqlDbType.Char, 0, 15, true, false, "")]
        public string IMEI = null;
        public const string fn_IMSI = "IMSI";
        [DBField(SqlDbType.Char, 0, 15, true, false, "")]
        public string IMSI = null;
        public const string fn_PrivateKey = "PrivateKey";
        [DBField(SqlDbType.VarChar, 0, 100, true, false, "")]
        public string PrivateKey = null;
        public const string fn_EventType = "EventType";
        [DBField(SqlDbType.VarChar, 0, 25, true, false, "")]
        public string EventType = null;
        public const string fn_DeviceAttribute = "DeviceAttribute";
        [DBField(SqlDbType.VarChar, 0, 25, true, false, "")]
        public string DeviceAttribute = null;
        public const string fn_Platform = "Platform";
        [DBField(SqlDbType.VarChar, 0, 25, true, false, "")]
        public string Platform = null;
        public const string fn_ICCID = "ICCID";
        [DBField(SqlDbType.VarChar, 0, 25, true, false, "")]
        public string ICCID = null;
        public const string fn_EAN = "EAN";
        [DBField(SqlDbType.Char, 0, 13, true, false, "")]
        public string EAN = null;
        public const string fn_ModelNumber = "ModelNumber";
        [DBField(SqlDbType.Char, 0, 8, true, false, "")]
        public string ModelNumber = null;
        public const string fn_PalletSerialNo = "PalletSerialNo";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string PalletSerialNo = null;
        
    }

    internal class DeliveryInfo
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_DeliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string DeliveryNo = null;
        public const string fn_InfoType = "InfoType";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string InfoType = null;
        public const string fn_InfoValue = "InfoValue";
        [DBField(SqlDbType.NVarChar, 0, 200, true, false, "")]
        public string InfoValue = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }
    internal class V_Delivery
    {
        public const string fn_DeliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, true, "")]
        public string DeliveryNo = null;
        public const string fn_ShipmentNo = "ShipmentNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string ShipmentNo = null;
        public const string fn_PoNo = "PoNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string PoNo = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Model = null;
        public const string fn_ShipDate = "ShipDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime ShipDate = DateTime.MinValue;
        public const string fn_Qty = "Qty";
        [DBField(SqlDbType.Int, short.MinValue, short.MaxValue, false, false, "")]
        public int Qty = int.MinValue;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public string Status = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class V_DeliveryInfo
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_DeliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string DeliveryNo = null;
        public const string fn_InfoType = "InfoType";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string InfoType = null;
        public const string fn_InfoValue = "InfoValue";
        [DBField(SqlDbType.NVarChar, 0, 200, true, false, "")]
        public string InfoValue = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class V_Delivery_Pallet
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_DeliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string DeliveryNo = null;
        public const string fn_PalletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string PalletNo = null;
        public const string fn_ShipmentNo = "ShipmentNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string ShipmentNo = null;
        public const string fn_DeliveryQty = "DeliveryQty";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public short DeliveryQty = short.MinValue;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public string Status = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;

        public const string fn_deviceQty = "DeviceQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 deviceQty = int.MinValue;

        public const string fn_palletType = "PalletType";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String palletType = null;
    }
    internal class Delivery
    {
        public const string fn_DeliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, true, "")]
        public string DeliveryNo = null;
        public const string fn_ShipmentNo = "ShipmentNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string ShipmentNo = null;
        public const string fn_PoNo = "PoNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string PoNo = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Model = null;
        public const string fn_ShipDate = "ShipDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime ShipDate = DateTime.MinValue;
        public const string fn_Qty = "Qty";
        [DBField(SqlDbType.Int, short.MinValue, short.MaxValue, false, false, "")]
        public int Qty = int.MinValue;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public string Status = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class Delivery_Pallet
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_DeliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string DeliveryNo = null;
        public const string fn_PalletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string PalletNo = null;
        public const string fn_ShipmentNo = "ShipmentNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string ShipmentNo = null;
        public const string fn_DeliveryQty = "DeliveryQty";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public short DeliveryQty = short.MinValue;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public string Status = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;

        public const string fn_deviceQty = "DeviceQty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 deviceQty = int.MinValue;

        public const string fn_palletType = "PalletType";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String palletType = null;
    }

    internal class Pallet
    {
        public const string fn_PalletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, false, true, "")]
        public string PalletNo = null;
        public const string fn_UCC = "UCC";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public string UCC = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;
        public const string fn_Weight = "Weight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public decimal Weight = decimal.MinValue;
        public const string fn_PalletModel = "PalletModel";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string PalletModel = null;
        public const string fn_Length = "Length";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public decimal Length = decimal.MinValue;
        public const string fn_Width = "Width";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public decimal Width = decimal.MinValue;
        public const string fn_Height = "Height";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public decimal Height = decimal.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_weight_L = "Weight_L";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public Decimal weight_L = decimal.MinValue;
        public const string fn_Floor = "Floor";
        [DBField(SqlDbType.VarChar, 0, 8, false, false, "")]
        public string Floor = "";
    }

    internal class PalletLog
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_PalletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string PalletNo = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string Line = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class FISToSAPPLTWeight
    {
        public const string fn_Shipment = "Shipment";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string Shipment = null;
        public const string fn_PalletNo = "PalletNo";
        [DBField(SqlDbType.Char, 0, 20, false, true, "")]
        public string PalletNo = null;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Type = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string Status = null;
        public const string fn_Weight = "Weight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public decimal Weight = decimal.MinValue;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class FISToSAPWeight
    {
        public const string fn_Shipment = "Shipment";
        [DBField(SqlDbType.Char, 0, 20, false, true, "")]
        public string Shipment = null;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string Type = null;
        public const string fn_Weight = "Weight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public decimal Weight = decimal.MinValue;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string Status = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class WeightLog
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_SN = "SN";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string SN = null;
        public const string fn_Weight = "Weight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public decimal Weight = decimal.MinValue;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string Line = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class PalletWeight
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Family = null;
        public const string fn_Region = "Region";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Region = null;
        public const string fn_Qty = "Qty";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public short Qty = short.MinValue;
        public const string fn_Weight = "Weight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public decimal Weight = decimal.MinValue;
        public const string fn_Tolerance = "Tolerance";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Tolerance = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class COAReceive
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_BegSN = "BegSN";
        [DBField(SqlDbType.Char, 0, 15, false, false, "")]
        public string BegSN = null;
        public const string fn_EndSN = "EndSN";
        [DBField(SqlDbType.Char, 0, 15, false, false, "")]
        public string EndSN = null;
        public const string fn_PO = "PO";
        [DBField(SqlDbType.Char, 0, 16, true, false, "")]
        public string PO = null;
        public const string fn_CustPN = "CustPN";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string CustPN = null;
        public const string fn_IECPN = "IECPN";
        [DBField(SqlDbType.Char, 0, 15, true, false, "")]
        public string IECPN = null;
        public const string fn_MSPN = "MSPN";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string MSPN = null;
        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public string Descr = null;
        public const string fn_ShipDate = "ShipDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime ShipDate = DateTime.MinValue;
        public const string fn_Qty = "Qty";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, true, false, "")]
        public short Qty = short.MinValue;
        public const string fn_Cust = "Cust";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string Cust = null;
        public const string fn_Upload = "Upload";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Upload = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class COAStatus
    {
        public const string fn_COASN = "COASN";
        [DBField(SqlDbType.Char, 0, 15, false, true, "")]
        public string COASN = null;
        public const string fn_IECPN = "IECPN";
        [DBField(SqlDbType.Char, 0, 12, true, false, "")]
        public string IECPN = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public string Status = null;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string Line = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class COALog
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_COASN = "COASN";
        [DBField(SqlDbType.Char, 0, 15, false, false, "")]
        public string COASN = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Line = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Tp = "Tp";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public string Tp = null;
    }

    public class Pizza_Part
    {
        public const string fn_bomNodeType = "BomNodeType";
        [DBField(SqlDbType.Char, 0, 3, false, false, "")]
        public String bomNodeType = null;

        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_checkItemType = "CheckItemType";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String checkItemType = null;

        public const string fn_custmerPn = "CustmerPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String custmerPn = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_iecpn = "IECPn";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public String iecpn = null;

        public const string fn_partNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partNo = null;

        public const string fn_partSn = "PartSn";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public String partSn = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String partType = null;

        public const string fn_pizzaID = "PizzaID";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String pizzaID = null;

        public const string fn_station = "Station";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public String station = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    internal class Pizza
    {
        public const string fn_PizzaID = "PizzaID";
        [DBField(SqlDbType.Char, 0, 20, false, true, "")]
        public string PizzaID = null;
        public const string fn_MMIID = "MMIID";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string MMIID = null;

        public const string fn_Model = "Model";
        [DBField(SqlDbType.Char, 0, 24, true, false, "")]
        public string Model = null;

        public const string fn_CartonSN = "CartonSN";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string CartonSN = null;

        public const string fn_Remark = "Remark";
        [DBField(SqlDbType.Char, 0, 64, true, false, "")]
        public string Remark = null;

        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;

    }

    internal class PizzaStatus
    {
        public const string fn_PizzaID = "PizzaID";
        [DBField(SqlDbType.Char, 0, 20, false, true, "")]
        public string PizzaID = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string Line = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class PizzaLog
    {
        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_PizzaID = "PizzaID";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string PizzaID = null;

        public const string fn_Model = "Model";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public string Model = null;

        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Station = null;

        public const string fn_Line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string Line = null;

        public const string fn_Descr = "Descr";
        [DBField(SqlDbType.Char, 0, 255, true, false, "")]
        public string Descr = null;

        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;

        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
       
    }

    internal class ModelWeight
    {
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, true, "")]
        public string Model = null;
        public const string fn_UnitWeight = "UnitWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public decimal UnitWeight = decimal.MinValue;
        //public const string fn_UnitTolerance = "UnitTolerance";
        //[DBField(SqlDbType.Char, 0, 10, true, false, "")]
        //public string UnitTolerance = null;
        public const string fn_CartonWeight = "CartonWeight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        public decimal CartonWeight = decimal.MinValue;
        //public const string fn_CartonTolerance = "CartonTolerance";
        //[DBField(SqlDbType.Char, 0, 10, true, false, "")]
        //public string CartonTolerance = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;

        public const string fn_SendStatus = "SendStatus";
        [DBField(SqlDbType.VarChar, 0, 16, false, false, "")]
        public string SendStatus = null;

        public const string fn_Remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string Remark = null;
    }

    internal class ModelWeightTolerance
    {
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, true, "")]
        public string Model = null;
        //public const string fn_UnitWeight = "UnitWeight";
        //[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        //public decimal UnitWeight = decimal.MinValue;
        public const string fn_UnitTolerance = "UnitTolerance";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string UnitTolerance = null;
        //public const string fn_CartonWeight = "CartonWeight";
        //[DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, true, false, "")]
        //public decimal CartonWeight = decimal.MinValue;
        public const string fn_CartonTolerance = "CartonTolerance";
        [DBField(SqlDbType.Char, 0, 10, true, false, "")]
        public string CartonTolerance = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class SAP_VOLUME_PLT
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_ShippmentAndDn = "[Shippment&Dn]";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string ShippmentAndDn = null;
        public const string fn_SnoId = "SnoId";
        [DBField(SqlDbType.VarChar, 0, 15, false, false, "")]
        public string SnoId = null;
        public const string fn_Weight = "Weight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public decimal Weight = decimal.MinValue;
        public const string fn_KG = "KG";
        [DBField(SqlDbType.VarChar, 0, 2, false, false, "")]
        public string KG = null;
        public const string fn_a = "a";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public decimal a = decimal.MinValue;
        public const string fn_b = "b";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public decimal b = decimal.MinValue;
        public const string fn_h = "h";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public decimal h = decimal.MinValue;
        public const string fn_CM = "CM";
        [DBField(SqlDbType.VarChar, 0, 2, false, false, "")]
        public string CM = null;
    }

    internal class Dn_Cn_Volume
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_SnoId = "SnoId";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string SnoId = null;
        public const string fn_Tp = "Tp";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Tp = null;
        public const string fn_Pallet = "Pallet";
        [DBField(SqlDbType.VarChar, 0, 3, false, false, "")]
        public string Pallet = null;
        public const string fn_Dn = "Dn";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Dn = null;
        public const string fn_Shippment = "Shippment";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string Shippment = null;
        public const string fn_Qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Qty = int.MinValue;
        public const string fn_Po = "Po";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Po = null;
        public const string fn_Weight = "Weight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public decimal Weight = decimal.MinValue;
        public const string fn_a = "a";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public decimal a = decimal.MinValue;
        public const string fn_b = "b";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public decimal b = decimal.MinValue;
        public const string fn_h = "h";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public decimal h = decimal.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class FRUCarton_FRUGift
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_CartonID = "CartonID";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string CartonID = null;
        public const string fn_GiftID = "GiftID";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string GiftID = null;
        //public const string fn_Qty = "Qty";
        //[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        //public int Qty = int.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class FRUGift
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.VarChar, 0, 20, false, true, "")]
        public string ID = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Model = null;
        public const string fn_Qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Qty = int.MinValue;
    }

    internal class FRUGift_Part
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_GiftID = "GiftID";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string GiftID = null;
        public const string fn_PartNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string PartNo = null;
        public const string fn_Value = "Value";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Value = null;
        //public const string fn_Qty = "Qty";
        //[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        //public int Qty = int.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class FRUCarton
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.VarChar, 0, 20, false, true, "")]
        public string ID = null;
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Model = null;
        public const string fn_Qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int Qty = int.MinValue;
    }

    internal class FRUCarton_Part
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_CartonID = "CartonID";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string CartonID = null;
        public const string fn_PartNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string PartNo = null;
        public const string fn_Value = "Value";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Value = null;
        //public const string fn_Qty = "Qty";
        //[DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        //public int Qty = int.MinValue;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class FRUFISToSAPWeight
    {
        public const string fn_Shipment = "Shipment";
        [DBField(SqlDbType.VarChar, 0, 20, false, true, "")]
        public string Shipment = null;
        public const string fn_Type = "Type";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string Type = null;
        public const string fn_Weight = "Weight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public decimal Weight = decimal.MinValue;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string Status = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class FRUWeightLog
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_SN = "SN";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string SN = null;
        public const string fn_Weight = "Weight";
        [DBField(SqlDbType.Decimal, Constants.CurrencyMinVal, Constants.CurrencyMaxVal, false, false, "")]
        public decimal Weight = decimal.MinValue;
        public const string fn_Line = "Line";
        [DBField(SqlDbType.Char, 0, 30, false, false, "")]
        public string Line = null;
        public const string fn_Station = "Station";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string Station = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class UploadPallet
    {
        public const string fn_UploadId = "UploadId";
        [DBField(SqlDbType.Char, 0, 32, true, false, "")]
        public string UploadId = null;
        public const string fn_Delivery_Pallet_DeliveryNo = "Delivery_Pallet_DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string Delivery_Pallet_DeliveryNo = null;
        public const string fn_Delivery_Pallet_PalletNo = "Delivery_Pallet_PalletNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string Delivery_Pallet_PalletNo = null;
        public const string fn_Delivery_Pallet_DeliveryQty = "Delivery_Pallet_DeliveryQty";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, true, false, "")]
        public short Delivery_Pallet_DeliveryQty = short.MinValue;
        public const string fn_PalletType = "PalletType";
        [DBField(SqlDbType.VarChar, 0, 80, true, false, "")]
        public string PalletType = null;
        public const string fn_Pallet_UCC = "Pallet_UCC";
        [DBField(SqlDbType.Char, 0, 30, true, false, "")]
        public string Pallet_UCC = null;
    }

    internal class UploadDelivery
    {
        public const string fn_UploadId = "UploadId";
        [DBField(SqlDbType.Char, 0, 32, true, false, "")]
        public string UploadId = null;
        public const string fn_Delivery_DeliveryNo = "Delivery_DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string Delivery_DeliveryNo = null;
        public const string fn_Delivery_ShipDate = "Delivery_ShipDate";
        [DBField(SqlDbType.VarChar, 0, 8, true, false, "")]
        public string Delivery_ShipDate = null;
        public const string fn_Delivery_PoNo = "Delivery_PoNo";
        [DBField(SqlDbType.Char, 0, 20, true, false, "")]
        public string Delivery_PoNo = null;
        public const string fn_Customer = "Customer";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Customer = null;
        public const string fn_Name1 = "Name1";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Name1 = null;
        public const string fn_Name2 = "Name2";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Name2 = null;
        public const string fn_ShiptoId = "ShiptoId";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string ShiptoId = null;
        public const string fn_Adr2 = "Adr2";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Adr2 = null;
        public const string fn_Adr3 = "Adr3";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Adr3 = null;
        public const string fn_Adr4 = "Adr4";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Adr4 = null;
        public const string fn_City = "City";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string City = null;
        public const string fn_State = "State";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string State = null;
        public const string fn_Postal = "Postal";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Postal = null;
        public const string fn_Country = "Country";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Country = null;
        public const string fn_Carrier = "Carrier";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Carrier = null;
        public const string fn_SO = "SO";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string SO = null;
        public const string fn_CustPo = "CustPo";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string CustPo = null;
        public const string fn_Delivery_Model = "Delivery_Model";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Delivery_Model = null;
        public const string fn_BOL = "BOL";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string BOL = null;
        public const string fn_Invoice = "Invoice";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Invoice = null;
        public const string fn_RetCode = "RetCode";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string RetCode = null;
        public const string fn_ProdNo = "ProdNo";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string ProdNo = null;
        public const string fn_IECSo = "IECSo";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string IECSo = null;
        public const string fn_IECSoItem = "IECSoItem";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string IECSoItem = null;
        public const string fn_PoItem = "PoItem";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string PoItem = null;
        public const string fn_CustSo = "CustSo";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string CustSo = null;
        public const string fn_ShipFrom = "ShipFrom";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string ShipFrom = null;
        public const string fn_ShipingMark = "ShipingMark";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string ShipingMark = null;
        public const string fn_Flag = "Flag";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Flag = null;
        public const string fn_RegId = "RegId";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string RegId = null;
        public const string fn_BoxSort = "BoxSort";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string BoxSort = null;
        public const string fn_Duty = "Duty";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Duty = null;
        public const string fn_RegCarrier = "RegCarrier";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string RegCarrier = null;
        public const string fn_Destination = "Destination";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Destination = null;
        public const string fn_Department = "Department";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Department = null;
        public const string fn_OrdRefrence = "OrdRefrence";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string OrdRefrence = null;
        public const string fn_DeliverTo = "DeliverTo";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string DeliverTo = null;
        public const string fn_Tel = "Tel";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Tel = null;
        public const string fn_WH = "WH";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string WH = null;
        public const string fn_Delivery_ShipmentNo = "Delivery_ShipmentNo";
        [DBField(SqlDbType.Char, 0, 1, true, false, "")]
        public string Delivery_ShipmentNo = null;
        public const string fn_SKU = "SKU";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string SKU = null;
        public const string fn_Deport = "Deport";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Deport = null;
        public const string fn_Delivery_Qty = "Delivery_Qty";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Delivery_Qty = null;
        public const string fn_CQty = "CQty";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string CQty = null;
        public const string fn_EmeaCarrier = "EmeaCarrier";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string EmeaCarrier = null;
        public const string fn_Plant = "Plant";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Plant = null;
        public const string fn_ShipTp = "ShipTp";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string ShipTp = null;
        public const string fn_ConfigID = "ConfigID";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string ConfigID = null;
        public const string fn_HYML = "HYML";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string HYML = null;
        public const string fn_CustName = "CustName";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string CustName = null;
        public const string fn_ShipHold = "ShipHold";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string ShipHold = null;
        public const string fn_CTO_DS = "CTO_DS";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string CTO_DS = null;
        public const string fn_PackType = "PackType";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string PackType = null;
        public const string fn_PltType = "PltType";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string PltType = null;
        public const string fn_ShipWay = "ShipWay";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string ShipWay = null;
        public const string fn_Dept = "Dept";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Dept = null;
        public const string fn_MISC = "MISC";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string MISC = null;
        public const string fn_ShipFromNme = "ShipFromNme";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string ShipFromNme = null;
        public const string fn_ShipFromAr1 = "ShipFromAr1";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string ShipFromAr1 = null;
        public const string fn_ShipFromCty = "ShipFromCty";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string ShipFromCty = null;
        public const string fn_ShipFromTl = "ShipFromTl";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string ShipFromTl = null;
        public const string fn_DnItem = "DnItem";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string DnItem = null;
        public const string fn_Price = "Price";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Price = null;
        public const string fn_BoxReg = "BoxReg";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string BoxReg = null;
        public const string fn_DAY850 = "DAY850";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string DAY850 = null;
        public const string fn_TSBCUPO = "TSBCUPO";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string TSBCUPO = null;
        public const string fn_OrderDate = "OrderDate";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string OrderDate = null;
        public const string fn_Descr1 = "Descr1";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Descr1 = null;
        public const string fn_UPID = "UPID";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string UPID = null;
        public const string fn_Descr2 = "Descr2";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string Descr2 = null;
    }

    internal class Kitting_Location
    {
        public const string fn_TagID = "TagID";
        [DBField(SqlDbType.NChar, 0, 12, false, false, "")]
        public string TagID = null;
        public const string fn_TagTp = "TagTp";
        [DBField(SqlDbType.NChar, 0, 4, false, false, "")]
        public string TagTp = null;
        public const string fn_GateWayIP = "GateWayIP";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, true, "")]
        public short GateWayIP = short.MinValue;
        public const string fn_GateWayPort = "GateWayPort";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public int GateWayPort = int.MinValue;
        public const string fn_RackID = "RackID";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, true, "")]
        public short RackID = short.MinValue;
        public const string fn_ConfigedLEDStatus = "ConfigedLEDStatus";
        [DBField(SqlDbType.Bit, 1, 1, false, false, "")]
        public bool ConfigedLEDStatus = default(bool);
        public const string fn_ConfigedLEDBlock = "ConfigedLEDBlock";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public short ConfigedLEDBlock = short.MinValue;
        public const string fn_ConfigedDate = "ConfigedDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime ConfigedDate = DateTime.MinValue;
        public const string fn_Comm = "Comm";
        [DBField(SqlDbType.Bit, 1, 1, false, false, "")]
        public bool Comm = default(bool);
        public const string fn_RunningLEDStatus = "RunningLEDStatus";
        [DBField(SqlDbType.Bit, 1, 1, false, false, "")]
        public bool RunningLEDStatus = default(bool);
        public const string fn_RunningLEDBlock = "RunningLEDBlock";
        [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
        public short RunningLEDBlock = short.MinValue;
        public const string fn_RunningDate = "RunningDate";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime RunningDate = DateTime.MinValue;
        public const string fn_LEDValues = "LEDValues";
        [DBField(SqlDbType.NChar, 0, 10, false, false, "")]
        public string LEDValues = null;
        public const string fn_TagDescr = "TagDescr";
        [DBField(SqlDbType.VarChar, 0, 200, true, false, "")]
        public string TagDescr = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class Forwarder
    {
        public const string fn_Id = "ID";// ITC-1361-0047 ITC-1361-0048
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int Id = int.MinValue;
        public const string fn_forwarder = "Forwarder";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public string forwarder = null;
        public const string fn_Date = "Date";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public string Date = null;
        public const string fn_MAWB = "MAWB";
        [DBField(SqlDbType.VarChar, 0, 25, false, false, "")]
        public string MAWB = null;
        public const string fn_Driver = "Driver";
        [DBField(SqlDbType.NVarChar, 0, 50, false, false, "")]
        public string Driver = null;
        public const string fn_TruckID = "TruckID";
        [DBField(SqlDbType.NVarChar, 0, 50, false, false, "")]
        public string TruckID = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;

        public const string fn_ContainerId = "ContainerId";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public string ContainerId = null;
    }

    internal class PickIDCtrl 
    {
        public const string fn_PickID = "PickID";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string PickID = null;
        public const string fn_TruckID = "TruckID";
        [DBField(SqlDbType.NVarChar, 0, 50, false, false, "")]
        public string TruckID = null;
        public const string fn_Driver = "Driver";
        [DBField(SqlDbType.NVarChar, 0, 50, false, false, "")]
        public string Driver = null;
        public const string fn_Dt = "Dt";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public string Dt = null;
        public const string fn_Fwd = "Fwd";
        [DBField(SqlDbType.VarChar, 0, 10, false, false, "")]
        public string Fwd = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_InDt = "InDt";
        [DBField(SqlDbType.VarChar, 0, 25, false, false, "")]
        public string InDt = null;
        public const string fn_OutDt = "OutDt";
        [DBField(SqlDbType.VarChar, 0, 25, false, false, "")]
        public string OutDt = null;
        public const string fn_Id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int Id = int.MinValue;
    }

    internal class FwdPlt
    {
        public const string fn_PickID = "PickID";
        [DBField(SqlDbType.VarChar, 0, 15, false, true, "")]
        public string PickID = null;
        public const string fn_Plt = "Plt";
        [DBField(SqlDbType.VarChar, 0, 15, true, false, "")]
        public string Plt = null;
        public const string fn_Qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public long Qty = int.MinValue;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.VarChar, 0, 5, true, false, "")]
        public string Status = null;
        public const string fn_Date = "Date";
        [DBField(SqlDbType.VarChar, 0, 15, true, false, "")]
        public string Date = null;
        public const string fn_Operator = "Operator";
        [DBField(SqlDbType.VarChar, 0, 15, true, false, "")]
        public string Operator = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class MAWB
    {
        public const string fn_Id = "ID";// ITC-1361-0047 ITC-1361-0048
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int Id = int.MinValue;

        public const string fn_mAWB = "MAWB";
        [DBField(SqlDbType.VarChar, 0, 25, false, true, "")]
        public string mAWB = null;

        public const string fn_Delivery = "Delivery";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string Delivery = null;

        public const string fn_DeclarationId = "DeclarationId";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public string DeclarationId = null;

        public const string fn_OceanContainer = "OceanContainer";
        [DBField(SqlDbType.VarChar, 0, 64, false, false, "")]
        public string OceanContainer = null;

        public const string fn_HAWB = "HAWB";
        [DBField(SqlDbType.VarChar, 0, 32, false, false, "")]
        public string HAWB = null;
     
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class CartonSSCC
    {
        public const string fn_CartonSN = "CartonSN";
        [DBField(SqlDbType.VarChar, 0, 20, false, true, "")]
        public string CartonSN = null;
        public const string fn_SSCC = "SSCC";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string SSCC = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class PalletId
    {
        public const string fn_PalletNo = "PalletNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, true, "")]
        public string PalletNo = null;
        public const string fn_palletId = "PalletId";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string palletId = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
    }

    internal class WipBuffer
    {
        public const string fn_ID = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public int ID = int.MinValue;
        public const string fn_Code = "Code";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Code = null;
        public const string fn_PartNo = "PartNo";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public string PartNo = null;
        public const string fn_Tp = "Tp";
        [DBField(SqlDbType.VarChar, 0, 50, true, false, "")]
        public string Tp = null;
        public const string fn_LightNo = "LightNo";
        [DBField(SqlDbType.Char, 0, 4, false, false, "")]
        public string LightNo = null;
        public const string fn_Picture = "Picture";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string Picture = null;
        public const string fn_Qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int Qty = int.MinValue;
        public const string fn_Sub = "Sub";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string Sub = null;
        public const string fn_Safety_Stock = "Safety_Stock";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int Safety_Stock = int.MinValue;
        public const string fn_Max_Stock = "Max_Stock";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int Max_Stock = int.MinValue;
        public const string fn_Remark = "Remark";
        [DBField(SqlDbType.VarChar, 0, 20, true, false, "")]
        public string Remark = null;
        public const string fn_Editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public string Editor = null;
        public const string fn_Cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime Udt = DateTime.MinValue;
        public const string fn_kittingType = "KittingType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String kittingType = null;
        public const string fn_station = "Station";
        [DBField(SqlDbType.NVarChar, 0, 50, false, false, "")]
        public String station = null;
        public const string fn_line = "Line";
        [DBField(SqlDbType.Char, 0, 2, true, false, "")]
        public String line = null;
    }

    internal class PoData_EDI
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_deliveryNo = "DeliveryNo";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String deliveryNo = null;

        public const string fn_descr = "Descr";
        [DBField(SqlDbType.NVarChar, 0, 3900, false, false, "")]
        public String descr = null;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.Char, 0, 20, false, false, "")]
        public String editor = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_model = "Model";
        [DBField(SqlDbType.Char, 0, 12, false, false, "")]
        public String model = null;

        public const string fn_poNo = "PoNo";
        [DBField(SqlDbType.VarChar, 0, 40, false, false, "")]
        public String poNo = null;

        public const string fn_qty = "Qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, false, "")]
        public Int32 qty = int.MinValue;

        public const string fn_shipDate = "ShipDate";
        [DBField(SqlDbType.Char, 0, 10, false, false, "")]
        public String shipDate = null;

        public const string fn_status = "Status";
        [DBField(SqlDbType.Char, 0, 2, false, false, "")]
        public String status = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    [Table("KitLoc")]
    public class KitLoc
    {
        public const string fn_cdt = "Cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime cdt = DateTime.MinValue;

        public const string fn_editor = "Editor";
        [DBField(SqlDbType.VarChar, 0, 30, false, false, "")]
        public String editor = null;

        public const string fn_family = "Family";
        [DBField(SqlDbType.VarChar, 0, 50, false, false, "")]
        public String family = null;

        public const string fn_id = "ID";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
        public Int32 id = int.MinValue;

        public const string fn_location = "Location";
        [DBField(SqlDbType.VarChar, 0, 255, false, false, "")]
        public String location = null;

        public const string fn_partType = "PartType";
        [DBField(SqlDbType.VarChar, 0, 20, false, false, "")]
        public String partType = null;

        public const string fn_pdLine = "PdLine";
        [DBField(SqlDbType.Char, 0, 4, false, false, "")]
        public String pdLine = null;

        public const string fn_udt = "Udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
        public DateTime udt = DateTime.MinValue;
    }

    //public class COALog
    //{
    //    public const string fn_cdt = "Cdt";
    //    [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, false, false, "")]
    //    public DateTime cdt = DateTime.MinValue;

    //    public const string fn_editor = "Editor";
    //    [DBField(SqlDbType.Char, 0, 20, false, false, "")]
    //    public String editor = null;

    //    public const string fn_id = "ID";
    //    [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, false, true, "")]
    //    public Int32 id = int.MinValue;

    //    public const string fn_isPass = "IsPass";
    //    [DBField(SqlDbType.SmallInt, short.MinValue, short.MaxValue, false, false, "")]
    //    public Int16 isPass = short.MinValue;

    //    public const string fn_pdLine = "PdLine";
    //    [DBField(SqlDbType.Char, 0, 30, false, false, "")]
    //    public String pdLine = null;

    //    public const string fn_pno = "Pno";
    //    [DBField(SqlDbType.Char, 0, 12, false, false, "")]
    //    public String pno = null;

    //    public const string fn_snoId = "SnoId";
    //    [DBField(SqlDbType.Char, 0, 14, false, false, "")]
    //    public String snoId = null;

    //    public const string fn_tp = "Tp";
    //    [DBField(SqlDbType.Char, 0, 10, false, false, "")]
    //    public String tp = null;

    //    public const string fn_wc = "WC";
    //    [DBField(SqlDbType.Char, 0, 2, false, false, "")]
    //    public String wc = null;
    //}
 
    #endregion
}

namespace IMES.Infrastructure.Repository._Schema.KIT
{
    #region .  KIT  .

    internal class BinData
    {
        public const string fn_SnoId = "snoid";
        [DBField(SqlDbType.VarChar, 0, 9, true, false, "")]
        public string SnoId = null;
        public const string fn_Bin = "bin";
        [DBField(SqlDbType.VarChar, 0, 3, true, false, "")]
        public string Bin = null;
        public const string fn_Qty = "qty";
        [DBField(SqlDbType.Int, int.MinValue, int.MaxValue, true, false, "")]
        public int Qty = int.MinValue;
        public const string fn_Cdt = "cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class kittingboxsn
    {
        public const string fn_SnoId = "snoid";
        [DBField(SqlDbType.VarChar, 0, 9, false, false, "")]
        public string SnoId = null;
        public const string fn_tp = "tp";
        [DBField(SqlDbType.VarChar, 0, 3, true, false, "")]
        public string tp = null;
        public const string fn_sno = "sno";
        [DBField(SqlDbType.VarChar, 0, 4, false, false, "")]
        public string sno = null;
        public const string fn_status = "status";
        [DBField(SqlDbType.VarChar, 0, 1, true, false, "")]
        public string status = null;
        public const string fn_remark = "remark";
        [DBField(SqlDbType.VarChar, 0, 30, true, false, "")]
        public string remark = null;
        public const string fn_Cdt = "cdt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Cdt = DateTime.MinValue;
        public const string fn_Udt = "udt";
        [DBField(SqlDbType.DateTime, Constants.DateTimeMinVal, Constants.DateTimeMaxVal, true, false, "")]
        public DateTime Udt = DateTime.MinValue;
    }

    internal class Kitting_ModelConfirm
    {
        public const string fn_Model = "Model";
        [DBField(SqlDbType.VarChar, 0, 12, false, true, "")]
        public string Model = null;
        public const string fn_Status = "Status";
        [DBField(SqlDbType.Char, 0, 1, false, false, "")]
        public string Status = null;
    }

    #endregion
}
