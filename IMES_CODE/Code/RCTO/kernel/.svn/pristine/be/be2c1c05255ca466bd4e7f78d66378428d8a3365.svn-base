using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;
using System.Collections;

namespace IMES.Infrastructure.Repository._Metas
{
    public class IntPair
    {
        private int _first = int.MinValue;
        private int _second = int.MinValue;

        public IntPair(int first, int second)
        {
            _first = first;
            _second = second;
        }

        public override int GetHashCode()
        {
            return (_first.ToString() + "," + _second.ToString()).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IntPair))
                return false;
            return _first == ((IntPair)obj)._first && _second == ((IntPair)obj)._second;
        }
    }

    public class SQLContextNew
    {
        public SQLContextNew()
        {
        }

        public SQLContextNew(string sentence, IDictionary<string, SqlParameter> parames, IDictionary<string, int> indexes)
        {
            this._sentence = sentence;
            this._parames = parames;
            this._indexes = indexes;
        }

        public SQLContextNew(SQLContextNew orig)
        {
            this._sentence = orig._sentence;

            foreach (KeyValuePair<string, SqlParameter> item in orig._parames)
            {
                this._parames.Add(item.Key, (SqlParameter)((ICloneable)item.Value).Clone());
            }
            foreach (KeyValuePair<string, int> item in orig._indexes)
            {
                this._indexes.Add(item.Key, item.Value);
            }
            this._tblFlds = orig._tblFlds;
        }

        public void FillIndexes(IDictionary<string, int> carrier)
        {
            if (carrier == null)
                return;
            foreach (KeyValuePair<string, int> item in this._indexes)
            {
                carrier.Add(item.Key, item.Value);
            }
        }

        public void FillParams(IDictionary<string, SqlParameter> carrier)
        {
            if (carrier == null)
                return;
            foreach (KeyValuePair<string, SqlParameter> item in this._parames)
            {
                carrier.Add(item.Key, (SqlParameter)((ICloneable)item.Value).Clone());
            }
        }

        public static void CopyParams(SQLContextNew dest, SQLContextNew orig)
        {
            foreach (KeyValuePair<string, SqlParameter> item in orig._parames)
            {
                dest._parames.Add(item.Key, (SqlParameter)((ICloneable)item.Value).Clone());
            }
        }

        private string _sentence = string.Empty;
        private IDictionary<string, SqlParameter> _parames = new Dictionary<string, SqlParameter>();
        private IDictionary<string, int> _indexes = new Dictionary<string, int>();

        private ITableAndFields[] _tblFlds = null;
        public ITableAndFields[] TableFields
        {
            get { return this._tblFlds; }
            set { this._tblFlds = value; }
        }

        public string Sentence
        {
            get { return this._sentence; }
            set { this._sentence = value; }
        }

        public SqlParameter Param(string name)
        {
            if (_parames.ContainsKey(name))
                return this._parames[name];
            else
                return null;
        }

        public SqlParameter[] Params
        {
            get
            {
                if (_parames != null && _parames.Count > 0)
                    return this._parames.Values.ToArray();
                else
                    return null;
            }
        }

        public bool ContainsParamKey(string paramKey)
        {
            return _parames.ContainsKey(paramKey);
        }

        public string[] ParamKeys
        {
            get
            {
                if (_parames != null && _parames.Count > 0)
                    return this._parames.Keys.ToArray();
                else
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Return modified parameter name.</returns>
        public string AddParam(string key, SqlParameter value)
        {
            string ret = null;
            if (this._parames.ContainsKey(key))
            {
                string newKey = key;
                string newParameterName = value.ParameterName;
                int i = 1;
                do
                {
                    int postIdx = newKey.IndexOf("$");
                    if (postIdx > -1)
                    {
                        newKey = newKey.Substring(0, postIdx + 1) + (int.Parse(newKey.Substring(postIdx + 1)) + 1).ToString();

                        int postParamIdx = newParameterName.IndexOf("$");
                        newParameterName = newParameterName.Substring(0, postParamIdx + 1) + (int.Parse(newParameterName.Substring(postParamIdx + 1)) + 1).ToString();
                    }
                    else
                    {
                        newKey = newKey + "$" + i.ToString();
                        newParameterName = newParameterName + "$" + i.ToString();
                    }
                    i++;
                }
                while (this._parames.ContainsKey(newKey));

                this._parames.Add(newKey, value);
                ret = value.ParameterName = newParameterName;
            }
            else
            {
                this._parames.Add(key, value);
            }
            return ret;
        }

        public void OverrideParams(SQLContextNew overrider)
        {
            if (overrider != null && overrider._parames != null)
            {
                foreach (var item in overrider._parames)
                {
                    if (this._parames.ContainsKey(item.Key))
                    {
                        this._parames[item.Key] = item.Value;
                    }
                }
            }
        }

        public int IndexCount
        {
            get { return _indexes.Count; }
        }

        public int Indexes(string name)
        {
            return this._indexes[name];
        }

        public static void SetIndexes(SQLContextNew source, SQLContextNew target)
        {
            target._indexes = source._indexes;
        }

        public void AddIndex(string key, int value)
        {
            this._indexes.Add(key, value);
        }

        public void ClearIndexes()
        {
            this._indexes.Clear();
        }
    }

    public class SQLContextCollectionNew
    {
        private IDictionary<int, SQLContextNew> _content = new Dictionary<int, SQLContextNew>();

        public void AddOne(int index, SQLContextNew item)
        {
            lock (_content)
            {
                if (!_content.ContainsKey(index))
                    _content.Add(index, item);
            }
        }

        public SQLContextNew MergeToOneNonQuery()
        {
            lock (_content)
            {
                SQLContextNew ret = new SQLContextNew();
                foreach (int key in _content.Keys)
                {
                    SQLContextNew item = _content[key];
                    string sentence = item.Sentence;

                    if (item.Params != null && item.Params.Length > 0)
                    {
                        foreach (string paramKey in item.ParamKeys)
                        {
                            SqlParameter paramItem = item.Param(paramKey);
                            SqlParameter newItem = new SqlParameter("@" + key.ToString() + "_" + paramItem.ParameterName.Substring(1), paramItem.SqlDbType);
                            newItem.Value = paramItem.Value;
                            ret.AddParam(key.ToString() + "_" + paramKey, newItem);
                            sentence = sentence.Replace(paramItem.ParameterName, newItem.ParameterName);
                        }
                    }
                    ret.Sentence = ret.Sentence + sentence + ";";
                }
                return ret;
            }
        }

        public SQLContextNew MergeToOneAndQuery()
        {
            return MergeToOneQuery("AND");
        }

        public SQLContextNew MergeToOneOrQuery()
        {
            return MergeToOneQuery("OR");
        }

        public SQLContextNew MergeToOneQuery(string andOrOr)
        {
            lock (_content)
            {
                SQLContextNew ret = new SQLContextNew();
                if (_content.Keys != null && _content.Keys.Count > 0)
                {
                    int i = 0;
                    foreach (int key in _content.Keys)
                    {
                        SQLContextNew item = _content[key];
                        if (i == 0)
                        {
                            SQLContextNew.SetIndexes(item, ret);//ret.Indexes = item.Indexes;

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
                                    {
                                        ret.Sentence = string.Join("WHERE", fieldAndCond, 0, fieldAndCond.Length - 1) + " WHERE (" + fieldAndCond[fieldAndCond.Length - 1] + ") ";
                                    }
                                    else
                                        ret.Sentence = fieldAndCond[0] + andOrOr == "AND" ? " WHERE (1=1) " : " WHERE (1=0) ";
                                }
                            }
                        }
                        else
                        {
                            string[] fieldAndCond = item.Sentence.Split(new string[] { "WHERE" }, StringSplitOptions.None);
                            if (fieldAndCond != null && fieldAndCond.Length > 1)
                            {
                                ret.Sentence = ret.Sentence + " " + andOrOr + " (" + fieldAndCond[fieldAndCond.Length - 1] + ") ";
                            }
                        }
                        if (item.Params != null && item.Params.Length > 0)
                        {
                            foreach (string key_i in item.ParamKeys)
                            {
                                if (!ret.ContainsParamKey(key_i))
                                    ret.AddParam(key_i, item.Param(key_i));
                            }
                        }
                        i++;
                    }
                }
                return ret;
            }
        }

        public SQLContextNew MergeToOneUnionQuery()
        {
            lock (_content)
            {
                SQLContextNew ret = new SQLContextNew();
                if (_content.Keys != null && _content.Keys.Count > 0)
                {
                    int i = 0;
                    foreach (int key in _content.Keys)
                    {
                        SQLContextNew item = _content[key];
                        if (i == 0)
                        {
                            SQLContextNew.SetIndexes(item, ret);
                            ret.Sentence = item.Sentence;
                            ret.TableFields = item.TableFields;
                        }
                        else
                        {
                            ret.Sentence = ret.Sentence + " UNION " + item.Sentence;
                        }
                        if (item.Params != null && item.Params.Length > 0)
                        {
                            foreach (string key_i in item.ParamKeys)
                            {
                                if (!ret.ContainsParamKey(key_i))
                                    ret.AddParam(key_i, item.Param(key_i));
                            }
                        }
                        i++;
                    }
                }
                return ret;
            }
        }
    }

    #region " JOIN 相关 "

    public interface ITableAndFields
    {
        string Alias { get; set; }
        string SubDBCalalog { get; set; }
        string TableName { get; }
        Type Table { get; }
        IConditionCollection Conditions { get; }
        IList<string> ToGetFieldNames { get; }
        IDictionary<string, string> ToGetFuncedFieldNames { get; }
        void AddRangeToGetFieldNames(params string[] strs);
        void AddRangeToGetFuncedFieldNames(params string[][] strs);
        void ClearToGetFieldNames();
    }

    public class TableAndFields<T> : ITableAndFields
    {
        public TableAndFields()
        {

        }

        //public TableAndFields<T>(TableAndFields<T> orig)
        //{
        //    //this.Table = orig.Table;
        //    if (orig.ToGetFieldNames == null)
        //    {
        //        this.ToGetFieldNames = null;
        //    }
        //    else
        //    {
        //        foreach (string str in orig.ToGetFieldNames)
        //        {
        //            this.ToGetFieldNames.Add(str);
        //        }
        //    }

        //    this.alias = orig.alias;
        //    this.subDBCalalog = orig.subDBCalalog;
        //}

        public Type Table
        {
            get { return typeof(T); }
        }

        private List<string> _toGetFieldNames = new List<string>();
        public IList<string> ToGetFieldNames
        {
            get { return this._toGetFieldNames; }
        }

        private Dictionary<string, string> _toGetFuncedFieldNames = new Dictionary<string, string>();
        public IDictionary<string, string> ToGetFuncedFieldNames
        {
            get { return this._toGetFuncedFieldNames; }
        }

        private ConditionCollection<T> conditions = new ConditionCollection<T>();
        public IConditionCollection Conditions
        {
            get { return conditions; }
        }

        private string _alias = "";
        public string Alias
        {
            get { return this._alias; }
            set { this._alias = value; }
        }

        private string _subDBCalalog = null;
        public string SubDBCalalog
        {
            get { return this._subDBCalalog; }
            set { this._subDBCalalog = value; }
        }

        public string TableName 
        {
            get { return Table.Name; }
        }

        public override string ToString()
        {
            if (_subDBCalalog != null)
                return _subDBCalalog + ".." + ToolsNew.GetTableName(typeof(T)) + " " + _alias;
            else
                return ToolsNew.GetTableName(typeof(T)) + " " + _alias;
        }

        public void AddRangeToGetFieldNames(params string[] strs)
        {
            this._toGetFuncedFieldNames = null;//Remove Another

            this._toGetFieldNames.AddRange(strs);
        }

        public void AddRangeToGetFuncedFieldNames(params string[][] strs)
        {
            this._toGetFieldNames = null;//Remove Another

            if (strs != null && strs.Length > 0)
            {
                foreach(string[] pair in strs)
                {
                    this._toGetFuncedFieldNames.Add(pair[0], pair[1]);
                }
            }
        }

        public void ClearToGetFieldNames()
        {
            this._toGetFuncedFieldNames = null;//Remove Another

            this._toGetFieldNames = null;//.Clear();
        }

        private void ClearToGetFuncedFieldNames()
        {
            this._toGetFuncedFieldNames = null;//.Clear();
        }
    }

    public interface ITableConnectionItem
    {
        ITableAndFields Tb1 { get; }
        ITableAndFields Tb2 { get; }
        string FieldNameFromTable1 { get; }
        string FieldNameFromTable2 { get; }
        bool IsAliasSet_tbl1 { get; set; }
        bool IsAliasSet_tbl2 { get; set; }
        bool AndOrOr { get; set; }
    }

    public class TableConnectionItem<T1, T2> : ITableConnectionItem
    {
        //public string alias1 = null;
        //public Type Table1 = null;
        private ITableAndFields _tb1 = null;
        private string _fieldNameFromTable1 = null;

        //public string alias2 = null;
        //public Type Table2 = null;
        private ITableAndFields _tb2 = null;
        private string _fieldNameFromTable2 = null;

        private string _biOperator = "{0}={1}";

        private bool _tbl1_Alias = false;     //Table1的别名是否已赋
        public bool IsAliasSet_tbl1
        {
            get { return this._tbl1_Alias; }
            set { this._tbl1_Alias = value; }
        }
        private bool _tbl2_Alias = false;     //Table2的别名是否已赋
        public bool IsAliasSet_tbl2
        {
            get { return this._tbl2_Alias; }
            set { this._tbl2_Alias = value; }
        }

        public ITableAndFields Tb1
        {
            get { return this._tb1; }
        }
        public ITableAndFields Tb2
        {
            get { return this._tb2; }
        }
        public string FieldNameFromTable1
        {
            get { return this._fieldNameFromTable1; }
        }
        public string FieldNameFromTable2
        {
            get { return this._fieldNameFromTable2; }
        }

        public TableConnectionItem(ITableAndFields tb1, string fieldNameFromTable1, ITableAndFields tb2, string fieldNameFromTable2, string biOperator)
            : this(/*TTb1.Table,*/ tb1, fieldNameFromTable1, /*TTb2.Table,*/ tb2, fieldNameFromTable2)
        {
            this._biOperator = biOperator;
        }

        public TableConnectionItem(ITableAndFields tb1, string fieldNameFromTable1, ITableAndFields tb2, string fieldNameFromTable2)
            : this(/*TTb1.Table,*/ fieldNameFromTable1, /*TTb2.Table,*/ fieldNameFromTable2)
        {
            this._tb1 = tb1;
            this._tb2 = tb2;
        }

        public TableConnectionItem(/*TableAndFields Tb1,*/ string fieldNameFromTable1, /*TableAndFields Tb2,*/ string fieldNameFromTable2, string biOperator)
            : this(/*Tb1,*/ fieldNameFromTable1, /*Tb2,*/ fieldNameFromTable2)
        {
            this._biOperator = biOperator;
        }

        private TableConnectionItem(/*Type Table1,*/ string fieldNameFromTable1, /*Type Table2,*/ string fieldNameFromTable2)
        {
            //this.Table1 = Table1;
            this._fieldNameFromTable1 = fieldNameFromTable1;
            //this.Table2 = Table2;
            this._fieldNameFromTable2 = fieldNameFromTable2;
        }

        //private TableConnectionItem(/*Type Table1,*/ string FieldNameFromTable1, /*Type Table2,*/ string FieldNameFromTable2, string BiOperator)
        //    : this(/*Table1,*/ FieldNameFromTable1, /*Table2,*/ FieldNameFromTable2)
        //{
        //    this.BiOperator = BiOperator;
        //}

        public override bool Equals(object obj)
        {
            if (obj is TableConnectionItem<T1, T2>)
            {
                TableConnectionItem<T1, T2> another = (TableConnectionItem<T1, T2>)obj;
                return
                    (this._tb1 == another._tb1 && this._tb2 == another._tb2 && this._fieldNameFromTable1 == another._fieldNameFromTable1 && this._fieldNameFromTable2 == another._fieldNameFromTable2);
            }
            else if (obj is TableConnectionItem<T2, T1>)
            {
                TableConnectionItem<T2, T1> another = (TableConnectionItem<T2, T1>)obj;
                return
                    (this._tb1 == another._tb2 && this._tb2 == another._tb1 && this._fieldNameFromTable1 == another._fieldNameFromTable2 && this._fieldNameFromTable2 == another._fieldNameFromTable1);
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
            return string.Format(_biOperator, string.Format("{0}.{1}", _tb1.Alias, _fieldNameFromTable1), string.Format("{0}.{1}", _tb2.Alias, _fieldNameFromTable2));
        }

        private bool _andOrOr = true;
        public bool AndOrOr
        {
            get { return this._andOrOr; }
            set { this._andOrOr = value; }
        }
    }

    public class TableConnectionCollection
    {
        private IList<ITableConnectionItem> content = new List<ITableConnectionItem>();

        private IDictionary<string, IDictionary<string, ITableConnectionItem>> regist = new Dictionary<string, IDictionary<string, ITableConnectionItem>>();
        internal IDictionary<string, IDictionary<string, ITableConnectionItem>> Regist
        {
            get { return this.regist; }
        }

        internal static string DecAlias(string alias, string str)
        {
            return alias + "_" + str;
        }

        internal static string DecAliasInner(string alias, string str)
        {
            return alias + "." + str;
        }

        //Recommed.
        public TableConnectionCollection(params ITableConnectionItem[] values)
        {
            if (values != null && values.Length > 0)
            {
                foreach (ITableConnectionItem item in values)
                {
                    if (!content.Contains(item))
                        content.Add(item);

                    string key1 = DecAlias(item.Tb1.GetHashCode().ToString(), item.FieldNameFromTable1);
                    string key2 = DecAlias(item.Tb2.GetHashCode().ToString(), item.FieldNameFromTable2);

                    RegistInner(key1, key2, item);
                    RegistInner(key2, key1, item);
                }
            }
        }

        private void RegistInner(string key1, string key2, ITableConnectionItem item)
        {
            IDictionary<string, ITableConnectionItem> nowPoint = null;
            if (regist.ContainsKey(key1))
            {
                nowPoint = regist[key1];
            }
            else
            {
                nowPoint = new Dictionary<string, ITableConnectionItem>();
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
            foreach (ITableConnectionItem tci in content)
            {
                if (tci.IsAliasSet_tbl1 && tci.IsAliasSet_tbl2)
                    ret.Add(tci.ToString());
            }
            return ret.ToArray();
        }
    }

    public class TableBiJoinedLogic
    {
        public const string c_onStr = " ON ";
        public const string c_andStr = " AND ";
        public const string c_orStr = " OR ";

        private Queue _queue = new Queue(); //TableConnectionItem or String

        public TableBiJoinedLogic() { }
        public void Add(string joinOperator)
        {
            _queue.Enqueue(joinOperator);
        }
        public void Add(ITableConnectionItem joinCondtion)
        {
            _queue.Enqueue(joinCondtion);
        }
        public void Add(ITableAndFields joinTblFlds)
        {
            _queue.Enqueue(joinTblFlds);
        }
        public IEnumerator Enum
        {
            get { return _queue.GetEnumerator(); }
        }
    }

    #endregion

    public abstract class Root<T>
    {
        protected string paramFormat = "@{0}";

        protected T conditionOrSetValInstance = default(T);

        protected string funcedForParam = null;

        protected string funcedForField = null;

        //Record the default value.
        protected static IDictionary<Guid, object> default_objs = new Dictionary<Guid, object>();

        public Root(T instance)
        {
            this.conditionOrSetValInstance = instance;
            Type type = typeof(T);
            Guid key = type.GUID;

            lock (default_objs)
            {
                if (!default_objs.ContainsKey(key))
                {
                    object default_obj = Activator.CreateInstance(type);
                    default_objs.Add(key, default_obj);
                }
            }
        }

        public Root(T instance, string funcedForField) : this(instance)
        {
            this.funcedForField = funcedForField;
        }

        public Root(T instance, string funcedForField, string funcedForParam) : this(instance, funcedForField)
        {
            this.funcedForParam = funcedForParam;
        }

        protected bool TryToAsAConditionOrSetVal_Inner(FieldInfo fi, out string condOrSetValSegment, out IDictionary<string, SqlParameter> parames)
        {
            bool ret = false;

            condOrSetValSegment = string.Empty;
            parames = new Dictionary<string, SqlParameter>();

            if (this.conditionOrSetValInstance != null)
            {
                Type type = typeof(T);
                Guid key = type.GUID;

                object condVal = fi.GetValue(conditionOrSetValInstance);
                object dfutVal = fi.GetValue(default_objs[key]);
                if (condVal != null && !condVal.Equals(dfutVal))
                {
                    ret = true;
                }
            }
            return ret;
        }

        protected static string DecAlias(string str, ITableAndFields tblFlds)
        {
            if (tblFlds != null && !string.IsNullOrEmpty(tblFlds.Alias))
            {
                return TableConnectionCollection.DecAlias(tblFlds.Alias, str);
            }
            else
            {
                return str;
            }
        }

        protected static string DecAliasInner(string str, ITableAndFields tblFlds)
        {
            if (tblFlds != null && !string.IsNullOrEmpty(tblFlds.Alias))
            {
                return TableConnectionCollection.DecAliasInner(tblFlds.Alias, str);
            }
            else
            {
                return str;
            }
        }

        protected string DecFuncedForParam(string str)
        {
            if (!string.IsNullOrEmpty(funcedForParam))
            {
                return string.Format(funcedForParam, str);
            }
            else
            {
                return str;
            }
        }

        protected string DecFuncedForField(string str)
        {
            if (!string.IsNullOrEmpty(funcedForField))
            {
                return string.Format(funcedForField, str);
            }
            else
            {
                return str;
            }
        }
    }

    #region . Conditions .

    public interface IConditionCollection
    {
        void Add(ICondition item);
        IList<ICondition> Items { get; }
    }

    public class ConditionCollection<T> : IConditionCollection
    {
        private IList<ICondition> content = new List<ICondition>();

        private bool _andOrOr = true;
        internal bool AndOrOr
        {
            get { return this._andOrOr; }
        }

        public ConditionCollection(bool andOrOr)
        {
            _andOrOr = andOrOr;
        }

        public ConditionCollection(params ICondition[] items)
        {
            foreach (ICondition item in items)
            {
                content.Add(item);
            }
        }

        public void Add(ICondition item)
        {
            content.Add(item);
        }

        public void AddRange(params ICondition[] items)
        {
            foreach (ICondition item in items)
            {
                content.Add(item);
            }
        }

        public IList<ICondition> Items
        {
            get { return content; }
        }
    }

    public interface ICondition
    {
        bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds);
        string FuncedForField { get; }
        string FuncedForParam { get; }
    }

    public abstract class Condition<T> : Root<T>, ICondition
    {
        public Condition(T instance) : base(instance){}
        public Condition(T instance, string funcedForField) : base(instance, funcedForField){}
        public Condition(T instance, string funcedForField, string funcedForParam) : base(instance, funcedForField, funcedForParam){}

        public virtual bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            condSegment = string.Empty;
            parames = new Dictionary<string, SqlParameter>();
            return false;
        }

        public string FuncedForField 
        {
            get { return this.funcedForField; } 
        }
        public string FuncedForParam 
        {
            get { return this.funcedForParam; } 
        }
    }

    public class EqualCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0}={1}";

        public EqualCondition(T instance) : base(instance){}
        public EqualCondition(T instance, string funcedForField) : base(instance, funcedForField){}
        public EqualCondition(T instance, string funcedForField, string funcedForParam) : base(instance, funcedForField, funcedForParam) { }

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                string paramName = string.Format(paramFormat, DecAlias(ToolsNew.ClearRectBrace(columnName), tblFlds));
                string fieldName = DecAliasInner(columnName, tblFlds);
                string paramNameKey = DecAlias(columnName, tblFlds);
                condSegment = string.Format(conditionFormat, DecFuncedForField(fieldName), DecFuncedForParam(paramName));

                SqlParameter sptr = new SqlParameter(paramName, sqlType);
                parames.Add(paramNameKey, sptr);
            }
            return ret;
        }
    }

    public class BetweenCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0} BETWEEN {1} AND {2}";

        public BetweenCondition(T instance) : base(instance){}
        public BetweenCondition(T instance, string funcedForField) : base(instance, funcedForField) { }

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                string paramName_beg = string.Format(paramFormat, DecAlias(DecBeg(ToolsNew.ClearRectBrace(columnName)), tblFlds));
                string paramName_end = string.Format(paramFormat, DecAlias(DecEnd(ToolsNew.ClearRectBrace(columnName)), tblFlds));
                string fieldName = DecAliasInner(columnName, tblFlds);
                string paramNameKey_beg = DecAlias(DecBeg(columnName), tblFlds);
                string paramNameKey_end = DecAlias(DecEnd(columnName), tblFlds);

                condSegment = string.Format(conditionFormat, DecFuncedForField(fieldName), DecFuncedForParam(paramName_beg), DecFuncedForParam(paramName_end));

                SqlParameter sptr_beg = new SqlParameter(paramName_beg, sqlType);
                parames.Add(paramNameKey_beg, sptr_beg);

                SqlParameter sptr_end = new SqlParameter(paramName_end, sqlType);
                parames.Add(paramNameKey_end, sptr_end);
            }
            return ret;
        }

        internal static string DecBeg(string str)
        {
            return str + "_beg";
        }

        internal static string DecEnd(string str)
        {
            return str + "_end";
        }
    }

    public class LikeCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0} LIKE {1}";

        public LikeCondition(T instance) : base(instance){}
        public LikeCondition(T instance, string funcedForField) : base(instance, funcedForField) { }
        public LikeCondition(T instance, string funcedForField, string funcedForParam) : base(instance, funcedForField, funcedForParam) { }

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                string paramName = string.Format(paramFormat, DecAlias(ToolsNew.ClearRectBrace(columnName), tblFlds));
                string fieldName = DecAliasInner(columnName, tblFlds);
                string paramNameKey = DecAlias(columnName, tblFlds);
                condSegment = string.Format(conditionFormat, DecFuncedForField(fieldName), DecFuncedForParam(paramName));
                
                SqlParameter sptr = new SqlParameter(paramName, sqlType);
                parames.Add(paramNameKey, sptr);
            }
            return ret;
        }
    }

    public class GreaterCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0}>{1}";

        public GreaterCondition(T instance) : base(instance){}
        public GreaterCondition(T instance, string funcedForField) : base(instance, funcedForField) { }
        public GreaterCondition(T instance, string funcedForField, string funcedForParam) : base(instance, funcedForField, funcedForParam) { }

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                string paramName = string.Format(paramFormat, DecAlias(ToolsNew.ClearRectBrace(columnName), tblFlds));
                string fieldName = DecAliasInner(columnName, tblFlds);
                string paramNameKey = DecAlias(Dec(columnName), tblFlds);
                condSegment = string.Format(conditionFormat, DecFuncedForField(fieldName), DecFuncedForParam(Dec(paramName)));

                SqlParameter sptr = new SqlParameter(Dec(paramName), sqlType);
                parames.Add(paramNameKey, sptr);
            }
            return ret;
        }

        internal static string Dec(string str)
        {
            return str + "_G";
        }
    }

    public class SmallerCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0}<{1}";

        public SmallerCondition(T instance) : base(instance){}
        public SmallerCondition(T instance, string funcedForField) : base(instance, funcedForField) { }

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                string paramName = string.Format(paramFormat, DecAlias(ToolsNew.ClearRectBrace(columnName), tblFlds));
                string fieldName = DecAliasInner(columnName, tblFlds);
                string paramNameKey = DecAlias(Dec(columnName), tblFlds);
                condSegment = string.Format(conditionFormat, DecFuncedForField(fieldName), DecFuncedForParam(Dec(paramName)));

                SqlParameter sptr = new SqlParameter(Dec(paramName), sqlType);
                parames.Add(paramNameKey, sptr);
            }
            return ret;
        }

        internal static string Dec(string str)
        {
            return str + "_S";
        }
    }

    public class GreaterOrEqualCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0}>={1}";

        public GreaterOrEqualCondition(T instance) : base(instance){}
        public GreaterOrEqualCondition(T instance, string funcedForField) : base(instance, funcedForField){}
        public GreaterOrEqualCondition(T instance, string funcedForField, string funcedForParam) : base(instance, funcedForField, funcedForParam){}

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                string paramName = string.Format(paramFormat, DecAlias(ToolsNew.ClearRectBrace(columnName), tblFlds));
                string fieldName = DecAliasInner(columnName, tblFlds);
                string paramNameKey = DecAlias(Dec(columnName), tblFlds);
                condSegment = string.Format(conditionFormat, DecFuncedForField(fieldName), DecFuncedForParam(Dec(paramName)));

                SqlParameter sptr = new SqlParameter(Dec(paramName), sqlType);
                parames.Add(paramNameKey, sptr);
            }
            return ret;
        }

        internal static string Dec(string str)
        {
            return str + "_GE";
        }
    }

    public class SmallerOrEqualCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0}<={1}";

        public SmallerOrEqualCondition(T instance) : base(instance){}
        public SmallerOrEqualCondition(T instance, string funcedForField) : base(instance, funcedForField){}
        public SmallerOrEqualCondition(T instance, string funcedForField, string funcedForParam) : base(instance, funcedForField, funcedForParam) { }

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                string paramName = string.Format(paramFormat, DecAlias(ToolsNew.ClearRectBrace(columnName), tblFlds));
                string fieldName = DecAliasInner(columnName, tblFlds);
                string paramNameKey = DecAlias(Dec(columnName), tblFlds);
                condSegment = string.Format(conditionFormat, DecFuncedForField(fieldName), DecFuncedForParam(Dec(paramName)));

                SqlParameter sptr = new SqlParameter(Dec(paramName), sqlType);
                parames.Add(paramNameKey, sptr);
            }
            return ret;
        }

        internal static string Dec(string str)
        {
            return str + "_SE";
        }
    }

    public class InSetCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0} IN ({1})";

        public InSetCondition(T instance) : base(instance){}
        public InSetCondition(T instance, string funcedForField) : base(instance, funcedForField) { }

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                //string paramName = string.Format(paramFormat, DecAlias(Tools.ClearRectBrace(columnName), tblFlds));
                string fieldName = DecAliasInner(columnName, tblFlds);
                string paramNameKey = DecAlias(Dec(columnName), tblFlds);
                condSegment = string.Format(conditionFormat, DecFuncedForField(fieldName), paramNameKey);

                //SqlParameter sptr = new SqlParameter(paramName, sqlType);
                //parames.Add(columnName, sptr);
            }
            return ret;
        }

        internal static string Dec(string str)
        {
            return "INSET[" + str + "]";
        } 
    }

    public class NotNullCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0} IS NOT NULL";

        public NotNullCondition(T instance) : base(instance){}

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                string fieldName = DecAliasInner(columnName, tblFlds);
                condSegment = string.Format(conditionFormat, DecFuncedForField(fieldName));
            }
            return ret;
        }
    }

    public class NullCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0} IS NULL";

        public NullCondition(T instance) : base(instance){}

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                string fieldName = DecAliasInner(columnName, tblFlds);
                condSegment = string.Format(conditionFormat, DecFuncedForField(fieldName));
            }
            return ret;
        }
    }

    public class NotEqualCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0}!={1}";

        public NotEqualCondition(T instance) : base(instance){}
        public NotEqualCondition(T instance, string funcedForField) : base(instance, funcedForField){}
        public NotEqualCondition(T instance, string funcedForField, string funcedForParam) : base(instance, funcedForField, funcedForParam) { }

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                string paramName = string.Format(paramFormat, DecAlias(ToolsNew.ClearRectBrace(columnName), tblFlds));
                string fieldName = DecAliasInner(columnName, tblFlds);
                string paramNameKey = DecAlias(columnName, tblFlds);
                condSegment = string.Format(conditionFormat, DecFuncedForField(fieldName), DecFuncedForParam(paramName));

                SqlParameter sptr = new SqlParameter(paramName, sqlType);
                parames.Add(paramNameKey, sptr);
            }
            return ret;
        }
    }

    public class NotLikeCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0} NOT LIKE {1}";

        public NotLikeCondition(T instance) : base(instance){}

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                string paramName = string.Format(paramFormat, DecAlias(ToolsNew.ClearRectBrace(columnName), tblFlds));
                string fieldName = DecAliasInner(columnName, tblFlds);
                string paramNameKey = DecAlias(columnName, tblFlds);
                condSegment = string.Format(conditionFormat, DecFuncedForField(fieldName), DecFuncedForParam(paramName));

                SqlParameter sptr = new SqlParameter(paramName, sqlType);
                parames.Add(columnName, sptr);
            }
            return ret;
        }
    }

    public class NotInSetCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0} NOT IN ({1})";

        public NotInSetCondition(T instance) : base(instance){}

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                //string paramName = string.Format(paramFormat, DecAlias(Tools.ClearRectBrace(columnName), tblFlds));
                string fieldName = DecAliasInner(columnName, tblFlds);
                string paramNameKey = DecAlias(Dec(columnName), tblFlds);
                condSegment = string.Format(conditionFormat, DecFuncedForField(fieldName), paramNameKey);

                //SqlParameter sptr = new SqlParameter(paramName, sqlType);
                //parames.Add(columnName, sptr);
            }
            return ret;
        }

        internal static string Dec(string str)
        {
            return "INSET[" + str + "]";
        } 
    }

    public class NullOrEqualCondition<T> : Condition<T>
    {
        private string conditionFormat = "({0} IS NULL OR {0}={1})";

        public NullOrEqualCondition(T instance) : base(instance){}

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                string paramName = string.Format(paramFormat, DecAlias(ToolsNew.ClearRectBrace(columnName), tblFlds));
                string fieldName = DecAliasInner(columnName, tblFlds);
                string paramNameKey = DecAlias(columnName, tblFlds);
                condSegment = string.Format(conditionFormat, DecFuncedForField(fieldName), DecFuncedForParam(paramName));

                SqlParameter sptr = new SqlParameter(paramName, sqlType);
                parames.Add(paramNameKey, sptr);
            }
            return ret;
        }
    }

    public class ReverseLikeCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0} LIKE {1}";

        public ReverseLikeCondition(T instance) : base(instance){}

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                string paramName = string.Format(paramFormat, DecAlias(ToolsNew.ClearRectBrace(columnName), tblFlds));
                string fieldName = DecAliasInner(columnName, tblFlds);
                string paramNameKey = DecAlias(columnName, tblFlds);
                condSegment = string.Format(conditionFormat, DecFuncedForParam(paramName), DecFuncedForField(fieldName));
                
                SqlParameter sptr = new SqlParameter(paramName, sqlType);
                parames.Add(paramNameKey, sptr);
            }
            return ret;
        }
    }

    public class AnyCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0}={1}";

        public AnyCondition(T instance) : base(instance) { }
        public AnyCondition(T instance, string conditionFormat) : base(instance) { this.conditionFormat = conditionFormat; }

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                string paramName = string.Format(paramFormat, DecAlias(ToolsNew.ClearRectBrace(columnName), tblFlds));
                string fieldName = DecAliasInner(columnName, tblFlds);
                string paramNameKey = DecAlias(Dec(columnName), tblFlds);
                condSegment = string.Format(conditionFormat, fieldName, Dec(paramName));

                SqlParameter sptr = new SqlParameter(Dec(paramName), sqlType);
                parames.Add(paramNameKey, sptr);
            }
            return ret;
        }

        internal static string Dec(string str)
        {
            return str + "_Any";
        }
    }

    public class AnySoloCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0} IS NOT NULL";

        public AnySoloCondition(T instance) : base(instance) { }
        public AnySoloCondition(T instance, string conditionFormat) : base(instance) { this.conditionFormat = conditionFormat; }

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                string fieldName = DecAliasInner(columnName, tblFlds);
                condSegment = string.Format(conditionFormat, fieldName);
            }
            return ret;
        }
    }

    public class AnySoloParamCondition<T> : Condition<T>
    {
        private string conditionFormat = "{0} IS NOT NULL";

        public AnySoloParamCondition(T instance) : base(instance) { }
        public AnySoloParamCondition(T instance, string conditionFormat) : base(instance) { this.conditionFormat = conditionFormat; }

        public override bool TryToAsACondition(FieldInfo fi, string columnName, SqlDbType sqlType, out string condSegment, out IDictionary<string, SqlParameter> parames, ITableAndFields tblFlds)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out condSegment, out parames);
            if (ret)
            {
                string paramName = string.Format(paramFormat, DecAlias(ToolsNew.ClearRectBrace(columnName), tblFlds));
                
                string paramNameKey = DecAlias(Dec(columnName), tblFlds);
                condSegment = string.Format(conditionFormat, Dec(paramName));

                SqlParameter sptr = new SqlParameter(Dec(paramName), sqlType);
                parames.Add(paramNameKey, sptr);
            }
            return ret;
        }

        internal static string Dec(string str)
        {
            return str + "_SLPM";
        }
    }

    #endregion

    #region . SetValues .

    public class SetValueCollection<T>
    {
        private IList<SetValue<T>> content = new List<SetValue<T>>();

        public SetValueCollection()
        {

        }

        public SetValueCollection(params SetValue<T>[] items)
        {
            foreach (SetValue<T> item in items)
            {
                content.Add(item);
            }
        }

        public void Add(SetValue<T> item)
        {
            content.Add(item);
        }
        public IList<SetValue<T>> Items
        {
            get { return content; }
        }
    }

    public abstract class SetValue<T> : Root<T>
    {
        //protected string setValFormat = "SET {0}={1}";

        public SetValue(T instance) : base(instance) { }

        public virtual bool TryToAsASetValue(FieldInfo fi, string columnName, SqlDbType sqlType, out string setValSegment, out IDictionary<string, SqlParameter> parames)
        {
            setValSegment = string.Empty;
            parames = new Dictionary<string, SqlParameter>();
            return false;
        }
    }

    public class CommonSetValue<T> : SetValue<T>
    {
        protected string setValFormat = "{0}={1}";

        public CommonSetValue(T instance) : base(instance) { }

        public override bool TryToAsASetValue(FieldInfo fi, string columnName, SqlDbType sqlType, out string setValSegment, out IDictionary<string, SqlParameter> parames)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out setValSegment, out parames);
            if (ret)
            {
                string paramName = string.Format(paramFormat, Dec(ToolsNew.ClearRectBrace(columnName)));
                setValSegment = string.Format(setValFormat, DecFuncedForField(columnName), DecFuncedForParam(paramName));

                SqlParameter sptr = new SqlParameter(paramName, sqlType);
                parames.Add(Dec(columnName), sptr);
            }
            return ret;
        }

        internal static string Dec(string str)
        {
            return str + "_sv";
        }
    }

    public class ForIncSetValue<T> : SetValue<T>
    {
        protected string setValFormat = "{0}={0}+{1}";

        public ForIncSetValue(T instance) : base(instance) { }
        public ForIncSetValue(T instance, string setValFormat) : base(instance) { this.setValFormat = setValFormat; }

        public override bool TryToAsASetValue(FieldInfo fi, string columnName, SqlDbType sqlType, out string setValSegment, out IDictionary<string, SqlParameter> parames)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out setValSegment, out parames);
            if (ret)
            {
                string paramName = string.Format(paramFormat, Dec(ToolsNew.ClearRectBrace(columnName)));
                setValSegment = string.Format(setValFormat, DecFuncedForField(columnName), DecFuncedForParam(paramName));

                SqlParameter sptr = new SqlParameter(paramName, sqlType);
                parames.Add(Dec(columnName), sptr);
            }
            return ret;
        }

        internal static string Dec(string str)
        {
            return str + "_Inc";
        }
    }

    public class ForIncSetValueSelf<T> : SetValue<T>
    {
        protected string setValFormat = "{0}={0}+1";

        public ForIncSetValueSelf(T instance) : base(instance) { }
        public ForIncSetValueSelf(T instance, string setValFormat) : base(instance) { this.setValFormat = setValFormat; }

        public override bool TryToAsASetValue(FieldInfo fi, string columnName, SqlDbType sqlType, out string setValSegment, out IDictionary<string, SqlParameter> parames)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out setValSegment, out parames);
            if (ret)
            {
                //string paramName = string.Format(paramFormat, Dec(ToolsNew.ClearRectBrace(columnName)));
                setValSegment = string.Format(setValFormat, DecFuncedForField(columnName));

                //SqlParameter sptr = new SqlParameter(paramName, sqlType);
                //parames.Add(Dec(columnName), sptr);
            }
            return ret;
        }

        //internal static string Dec(string str)
        //{
        //    return str + "_Inc";
        //}
    }

    public class ForDecSetValue<T> : SetValue<T>
    {
        protected string setValFormat = "{0}={0}-{1}";

        public ForDecSetValue(T instance) : base(instance) { }

        public override bool TryToAsASetValue(FieldInfo fi, string columnName, SqlDbType sqlType, out string setValSegment, out IDictionary<string, SqlParameter> parames)
        {
            bool ret = TryToAsAConditionOrSetVal_Inner(fi, out setValSegment, out parames);
            if (ret)
            {
                string paramName = string.Format(paramFormat, Dec(ToolsNew.ClearRectBrace(columnName)));
                setValSegment = string.Format(setValFormat, DecFuncedForField(columnName), DecFuncedForParam(paramName));

                SqlParameter sptr = new SqlParameter(paramName, sqlType);
                parames.Add(Dec(columnName), sptr);
            }
            return ret;
        }

        internal static string Dec(string str)
        {
            return str + "_Dec";
        }
    }

    #endregion
}
