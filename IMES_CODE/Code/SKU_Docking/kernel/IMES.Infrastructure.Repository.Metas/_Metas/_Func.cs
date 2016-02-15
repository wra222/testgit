using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using IMES.Infrastructure;
using System.Reflection.Emit;

namespace IMES.Infrastructure.Repository._Metas
{
    public static class FuncNew
    {
        public const string DescendOrder = " DESC";

        #region get cache field Info and field attribute info and const field info    
        private static readonly IDictionary<string, FieldInfo[]> _fieldCache = new Dictionary<string, FieldInfo[]>();
        private static readonly IDictionary<string, IDictionary<string, DBFieldAttribute>> _fieldAttrCache = new Dictionary<string, IDictionary<string, DBFieldAttribute>>();
        private static readonly IDictionary<string, FieldInfo[]> _constFieldCache = new Dictionary<string, FieldInfo[]>();

        private static object _fieldcacheSyncObj = new object();
        private static FieldInfo[] getCacheFieldInfos(Type type, BindingFlags bindingFlags, 
                                                                        out IDictionary<string,DBFieldAttribute> fieldAttrList,
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
                            throw new Exception(name +"." + fi.Name +" no defined DBFieldAttribute");
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

        #region Get SQLs

        public static SQLContextNew GetCommonSelect<T>(int methodId, params string[] orderbyField)
        {
            SQLContextNew ret = null;
            if (SQLCache.GetFromCache(methodId, ref ret))
                return ret;

            Type type = typeof(T);

            ret.Sentence = "SELECT {0} FROM " + ToolsNew.GetTableName(type);
            List<string> fields = new List<string>();
            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            
            int i = 0;
            foreach (FieldInfo fi in fis)
            {
                //string columnName = ToolsNew.TransField(type, fi.Name);
                string columnName = ToolsNew.TransField(cfis, fi.Name);
                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];

                ret.AddIndex(columnName, i);
                fields.Add(columnName);
                i++;
            }
            ret.Sentence = string.Format(ret.Sentence, string.Join(",", fields.ToArray()));
            if (orderbyField != null && orderbyField.Length > 0)
                ret.Sentence = ret.Sentence + " ORDER BY " + string.Join(",", orderbyField);

            return ret;
        }

        public static SQLContextNew GetCommonInsert<T>(int methodId)
        {
            SQLContextNew ret = null;
            if (SQLCache.GetFromCache(methodId, ref ret))
                return ret;

            Type type = typeof(T);

            ret.Sentence = "INSERT INTO " + ToolsNew.GetTableName(type) + " ({0}) VALUES ({1}) ";
            List<string> fields = new List<string>();
            List<string> paramts = new List<string>();
            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            foreach (FieldInfo fi in fis)
            {
                //string columnName = ToolsNew.TransField(type, fi.Name);
                string columnName = ToolsNew.TransField(cfis, fi.Name);
                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa =dbAttrList[fi.Name];
                SqlParameter sptr = new SqlParameter("@" + ToolsNew.ClearRectBrace(columnName), dbfa.columnType);
                ret.AddParam(columnName, sptr);
                fields.Add(columnName);
                paramts.Add("@" + ToolsNew.ClearRectBrace(columnName));
            }
            ret.Sentence = string.Format(ret.Sentence, string.Join(",", fields.ToArray()), string.Join(",", paramts.ToArray()));
            return ret;
        }

        public static SQLContextNew GetCommonInsert<T>()
        {
            SQLContextNew ret = new SQLContextNew();

            Type type = typeof(T);

            ret.Sentence = "INSERT INTO " + ToolsNew.GetTableName(type) + " ({0}) VALUES ({1}) ";
            List<string> fields = new List<string>();
            List<string> paramts = new List<string>();
            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            foreach (FieldInfo fi in fis)
            {
                //string columnName = ToolsNew.TransField(type, fi.Name);
                string columnName = ToolsNew.TransField(cfis, fi.Name);
                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];
                SqlParameter sptr = new SqlParameter("@" + ToolsNew.ClearRectBrace(columnName), dbfa.columnType);
                ret.AddParam(columnName, sptr);
                fields.Add(columnName);
                paramts.Add("@" + ToolsNew.ClearRectBrace(columnName));
            }
            ret.Sentence = string.Format(ret.Sentence, string.Join(",", fields.ToArray()), string.Join(",", paramts.ToArray()));
            return ret;
        }

        public static SQLContextNew GetAquireIdInsert<T>(int methodId)
        {
            SQLContextNew ret = null;
            if (SQLCache.GetFromCache(methodId, ref ret))
                return ret;

            Type type = typeof(T);

            ret.Sentence = string.Format("{0};{1};", "INSERT INTO " + ToolsNew.GetTableName(type) + " ({0}) VALUES ({1}) ", "SELECT @@IDENTITY");
            List<string> fields = new List<string>();
            List<string> paramts = new List<string>();
            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            foreach (FieldInfo fi in fis)
            {
                //string columnName = ToolsNew.TransField(type, fi.Name);
                string columnName = ToolsNew.TransField(cfis, fi.Name);
                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];
                SqlParameter sptr = new SqlParameter("@" + ToolsNew.ClearRectBrace(columnName), dbfa.columnType);
                if (!dbfa.isPK)
                {
                    ret.AddParam(columnName, sptr);
                    fields.Add(columnName);
                    paramts.Add("@" + ToolsNew.ClearRectBrace(columnName));
                }
            }
            ret.Sentence = string.Format(ret.Sentence, string.Join(",", fields.ToArray()), string.Join(",", paramts.ToArray()));
            return ret;
        }

        public static SQLContextNew GetAquireIdInsert<T>()
        {
            SQLContextNew ret = new SQLContextNew();

            Type type = typeof(T);

            ret.Sentence = string.Format("{0};{1};", "INSERT INTO " + ToolsNew.GetTableName(type) + " ({0}) VALUES ({1}) ", "SELECT @@IDENTITY");
            List<string> fields = new List<string>();
            List<string> paramts = new List<string>();
            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            foreach (FieldInfo fi in fis)
            {
                //string columnName = ToolsNew.TransField(type, fi.Name);
                string columnName = ToolsNew.TransField(cfis, fi.Name);
                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];
                SqlParameter sptr = new SqlParameter("@" + ToolsNew.ClearRectBrace(columnName), dbfa.columnType);
                if (!dbfa.isPK)
                {
                    ret.AddParam(columnName, sptr);
                    fields.Add(columnName);
                    paramts.Add("@" + ToolsNew.ClearRectBrace(columnName));
                }
            }
            ret.Sentence = string.Format(ret.Sentence, string.Join(",", fields.ToArray()), string.Join(",", paramts.ToArray()));
            return ret;
        }

        public static SQLContextNew GetCommonUpdate<T>(int methodId)
        {
            SQLContextNew ret = null;
            if (SQLCache.GetFromCache(methodId, ref ret))
                return ret;

            Type type = typeof(T);

            ret.Sentence = "UPDATE " + ToolsNew.GetTableName(type) + " SET {0} WHERE {1} ";
            List<string> fieldparamts = new List<string>();
            string cond = string.Empty;
            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            foreach (FieldInfo fi in fis)
            {
                //string columnName = ToolsNew.TransField(type, fi.Name);
                string columnName = ToolsNew.TransField(cfis, fi.Name);
                if (columnName == "Cdt")
                    continue;
                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];
                string paramFormat = "@{0}";
                string paramName = string.Format(paramFormat, ToolsNew.ClearRectBrace(columnName));
                SqlParameter sptr = new SqlParameter(paramName, dbfa.columnType);
                ret.AddParam(columnName, sptr);
                if (cond == string.Empty && dbfa.isPK)
                {
                    cond = columnName + "=" + paramName;
                }
                else
                {
                    fieldparamts.Add(columnName + "=" + paramName);
                }
            }
            ret.Sentence = string.Format(ret.Sentence, string.Join(",", fieldparamts.ToArray()), cond);
            return ret;
        }

        public static SQLContextNew GetConditionedUpdate<T>(int methodId, SetValueCollection<T> setValues, ConditionCollection<T> conditions)
        {
            SQLContextNew ret = null;
            if (SQLCache.GetFromCache(methodId, ref ret))
                return ret;
            return GetConditionedUpdate_Inner<T>(ret, setValues, conditions);
        }

        public static SQLContextNew GetConditionedUpdate<T>(SetValueCollection<T> setValues, ConditionCollection<T> conditions)
        {
            SQLContextNew ret = new SQLContextNew();
            return GetConditionedUpdate_Inner<T>(ret, setValues, conditions);
        }

        private static SQLContextNew GetConditionedUpdate_Inner<T>(SQLContextNew ret, SetValueCollection<T> setValues, ConditionCollection<T> conditions)
        {
            Type type = typeof(T);

            ret.Sentence = "UPDATE " + ToolsNew.GetTableName(type) + " SET {0} WHERE {1} ";
            List<string> fieldparamts = new List<string>();
            List<string> conds = new List<string>();

            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);

            foreach (FieldInfo fi in fis)
            {
                //string columnName = ToolsNew.TransField(type, fi.Name);
                string columnName = ToolsNew.TransField(cfis, fi.Name);

                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];
                foreach (ICondition condition in conditions.Items)
                {
                    string condSegment = string.Empty;
                    IDictionary<string, SqlParameter> parames = null;

                    if (condition.TryToAsACondition(fi, columnName, dbfa.columnType, out condSegment, out parames, null))
                    {
                        conds.Add(condSegment);
                        foreach (string key in parames.Keys)
                            ret.AddParam(key, parames[key]);
                    }
                }

                foreach (SetValue<T> setVal in setValues.Items)
                {
                    string setValSegment = string.Empty;
                    IDictionary<string, SqlParameter> parames = null;

                    if (setVal.TryToAsASetValue(fi, columnName, dbfa.columnType, out setValSegment, out parames))
                    {
                        fieldparamts.Add(setValSegment);
                        foreach (string key in parames.Keys)
                            ret.AddParam(key, parames[key]);
                    }
                }
            }

            ret.Sentence = string.Format(ret.Sentence, string.Join(",", fieldparamts.ToArray()), (conds.Count > 0 ? string.Join(conditions.AndOrOr ? " AND " : " OR ", conds.ToArray()) : " 1=1 "));
            return ret;
        }

        public static SQLContextNew GetCommonDelete<T>(int methodId)
        {
            SQLContextNew ret = null;
            if (SQLCache.GetFromCache(methodId, ref ret))
                return ret;

            Type type = typeof(T);

            ret.Sentence = "DELETE FROM " + ToolsNew.GetTableName(type) + " WHERE {0} ";
            string cond = string.Empty;
            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            foreach (FieldInfo fi in fis)
            {
                //string columnName = ToolsNew.TransField(type, fi.Name);
                string columnName = ToolsNew.TransField(cfis, fi.Name);
                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];
                string paramFormat = "@{0}";
                string paramName = string.Format(paramFormat, ToolsNew.ClearRectBrace(columnName));
                if (cond == string.Empty && dbfa.isPK)
                {
                    SqlParameter sptr = new SqlParameter(paramName, dbfa.columnType);
                    ret.AddParam(columnName, sptr);
                    cond = columnName + "=" + paramName;
                    break;
                }
            }
            ret.Sentence = string.Format(ret.Sentence, cond);
            return ret;
        }

        public static SQLContextNew GetConditionedDelete<T>(ConditionCollection<T> conditions)
        {
            SQLContextNew ret = new SQLContextNew();
            return GetConditionedDelete_Inner<T>(ret, conditions);
        }

        public static SQLContextNew GetConditionedDelete<T>(int methodId, ConditionCollection<T> conditions)
        {
            SQLContextNew ret = null;
            if (SQLCache.GetFromCache(methodId, ref ret))
                return ret;
            return GetConditionedDelete_Inner<T>(ret, conditions);
        }

        private static SQLContextNew GetConditionedDelete_Inner<T>(SQLContextNew ret, ConditionCollection<T> conditions)
        {
            Type type = typeof(T);

            ret.Sentence = "DELETE FROM " + ToolsNew.GetTableName(type) + " WHERE {0} ";
            List<string> conds = new List<string>();

           // FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            foreach (FieldInfo fi in fis)
            {
                //string columnName = ToolsNew.TransField(type, fi.Name);
                string columnName = ToolsNew.TransField(cfis, fi.Name);

                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];
            
                foreach (ICondition condition in conditions.Items)
                {
                    string condSegment = string.Empty;
                    IDictionary<string, SqlParameter> parames = null;

                    if (condition.TryToAsACondition(fi, columnName, dbfa.columnType, out condSegment, out parames, null))
                    {
                        conds.Add(condSegment);
                        foreach (string key in parames.Keys)
                            ret.AddParam(key, parames[key]);
                    }
                }
            }
            ret.Sentence = string.Format(ret.Sentence, (conds.Count > 0 ? string.Join(conditions.AndOrOr ? " AND " : " OR ", conds.ToArray()) : " 1=1 "));
            return ret;
        }

        public static SQLContextNew GetCommonTruncate<T>(int methodId)
        {
            SQLContextNew ret = null;
            if (SQLCache.GetFromCache(methodId, ref ret))
                return ret;

            Type type = typeof(T);

            ret.Sentence = "TRUNCATE TABLE " + ToolsNew.GetTableName(type);
            return ret;
        }

        public static SQLContextNew GetConditionedSelect<T>(int methodId, string funcName, string[] fieldNames, ConditionCollection<T> conditions, params string[] orderbyField)
        {
            SQLContextNew ret = null;
            if (SQLCache.GetFromCache(methodId, ref ret))
                return ret;
            return GetConditionedSelect_Inner<T>(ret, funcName, fieldNames, conditions, orderbyField);
        }

        public static SQLContextNew GetConditionedSelect<T>(string funcName, string[] fieldNames, ConditionCollection<T> conditions, params string[] orderbyField)
        {
            SQLContextNew ret = new SQLContextNew();
            return GetConditionedSelect_Inner<T>(ret, funcName, fieldNames, conditions, orderbyField);
        }

        private static SQLContextNew GetConditionedSelect_Inner<T>(SQLContextNew ret, string funcName, string[] fieldNames, ConditionCollection<T> conditions, params string[] orderbyField)
        {
            Type type = typeof(T);

            ret.Sentence = "SELECT {0} FROM " + ToolsNew.GetTableName(type) + " WHERE {1} ";
            List<string> fields = new List<string>();
            List<string> conds = new List<string>();

            IDictionary<int, string> indexesBeforeSort = new Dictionary<int, string>();
            IDictionary<int, string> fieldsBeforeSort = new Dictionary<int, string>();

            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            int i = 0;

            foreach (FieldInfo fi in fis)
            {
                //string columnName = ToolsNew.TransField(type, fi.Name);
                string columnName = ToolsNew.TransField(cfis, fi.Name);

                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];

                foreach (ICondition condition in conditions.Items)
                {
                    string condSegment = string.Empty;
                    IDictionary<string, SqlParameter> parames = null;

                    if (condition.TryToAsACondition(fi, columnName, dbfa.columnType, out condSegment, out parames, null))
                    {
                        foreach (string key in parames.Keys)
                        {
                            string oldName = parames[key].ParameterName;
                            string modifiedName = ret.AddParam(key, parames[key]);
                            if (modifiedName != null)
                            {
                                condSegment = condSegment.Replace(oldName, modifiedName);
                            }
                        }
                        conds.Add(condSegment);
                    }
                }

                if (fieldNames == null) // || fieldNames.Contains(columnName))
                {
                    ret.AddIndex(columnName, i);
                    fields.Add(columnName);
                    i++;
                }
                else
                {
                    int k = 1;
                    for (int j = 0; j < fieldNames.Length; j++)
                    {
                        if (fieldNames[j].Equals(columnName))
                        {
                            string postFix = k > 1 ? "_" + k.ToString() : "";

                            indexesBeforeSort.Add(j, columnName + postFix);
                            fieldsBeforeSort.Add(j, columnName);

                            k++;
                        }
                    }
                }
            }

            if (indexesBeforeSort.Count > 0 || fieldsBeforeSort.Count > 0)
            {
                i = 0;
                for (int j = 0; j < fieldNames.Length; j++)
                {
                    if (indexesBeforeSort.ContainsKey(j) && fieldsBeforeSort.ContainsKey(j))
                    {
                        ret.AddIndex(indexesBeforeSort[j], i++);
                        fields.Add(fieldsBeforeSort[j]);
                    }
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
                    ret.ClearIndexes();
                    ret.AddIndex(funcName, 0);
                }
            }

            ret.Sentence = string.Format(ret.Sentence, fStr, (conds.Count > 0 ? string.Join(conditions.AndOrOr ? " AND " : " OR ", conds.ToArray()) : " 1=1 "));
            if (orderbyField != null && orderbyField.Length > 0)
                ret.Sentence = ret.Sentence + " ORDER BY " + string.Join(",", orderbyField);

            return ret;
        }

        public static SQLContextNew GetConditionedSelectForFuncedField<T>(int methodId, string funcName, string[][] funcedFieldNames, ConditionCollection<T> conditions, params string[] orderbyField)
        {
            SQLContextNew ret = null;
            if (SQLCache.GetFromCache(methodId, ref ret))
                return ret;
            return GetConditionedSelectForFuncedField_Inner<T>(ret, funcName, funcedFieldNames, conditions, orderbyField);
        }

        public static SQLContextNew GetConditionedSelectForFuncedField<T>(string funcName, string[][] funcedFieldNames, ConditionCollection<T> conditions, params string[] orderbyField)
        {
            SQLContextNew ret = new SQLContextNew();
            return GetConditionedSelectForFuncedField_Inner<T>(ret, funcName, funcedFieldNames, conditions, orderbyField);
        }

        private static SQLContextNew GetConditionedSelectForFuncedField_Inner<T>(SQLContextNew ret, string funcName, string[][] funcedFieldNames, ConditionCollection<T> conditions, params string[] orderbyField)
        {
            //SQLContextNew ret = null;
            //if (SQLCache.GetFromCache(methodId, ref ret))
            //    return ret;

            Type type = typeof(T);

            bool isGroup = false;
            List<string> GroupByFields = new List<string>();

            ret.Sentence = "SELECT {0} FROM " + ToolsNew.GetTableName(type) + " WHERE {1} ";
            List<string> fields = new List<string>();
            List<string> conds = new List<string>();

            IDictionary<int, string> indexesBeforeSort = new Dictionary<int, string>();
            IDictionary<int, string> fieldsBeforeSort = new Dictionary<int, string>();

            //FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fis = getCacheFieldInfos(type, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            int i = 0;

            foreach (FieldInfo fi in fis)
            {
                //string columnName = ToolsNew.TransField(type, fi.Name);
                string columnName = ToolsNew.TransField(cfis, fi.Name);
                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];
                foreach (ICondition condition in conditions.Items)
                {
                    string condSegment = string.Empty;
                    IDictionary<string, SqlParameter> parames = null;

                    if (condition.TryToAsACondition(fi, columnName, dbfa.columnType, out condSegment, out parames, null))
                    {
                        conds.Add(condSegment);
                        foreach (string key in parames.Keys)
                            ret.AddParam(key, parames[key]);
                    }
                }

                if (funcedFieldNames == null)// || . .Contains(columnName))
                {
                    ret.AddIndex(columnName, i);
                    fields.Add(columnName);
                    i++;
                }
                else
                {
                    int k = 1;
                    for (int j = 0; j < funcedFieldNames.GetLength(0); j++)
                    {
                        if (funcedFieldNames[j][0].Equals(columnName))
                        {
                            string postFix = k > 1 ? "_" + k.ToString() : "";
                            //ret.AddIndex(columnName + postFix, i);
                            indexesBeforeSort.Add(j, columnName + postFix);

                            if (funcedFieldNames[j].Length == 2)
                            {
                                //fields.Add(funcedFieldNames[j][1]);
                                fieldsBeforeSort.Add(j, funcedFieldNames[j][1]);
                            }
                            else
                            {
                                //fields.Add(funcedFieldNames[j][0]);
                                fieldsBeforeSort.Add(j, funcedFieldNames[j][0]);
                            }

                            k++;
                            //i++;
                        }
                    }
                }
            }

            if (indexesBeforeSort.Count > 0 || fieldsBeforeSort.Count > 0)
            {
                i = 0;
                for (int j = 0; j < funcedFieldNames.GetLength(0); j++)
                {
                    if (indexesBeforeSort.ContainsKey(j) && fieldsBeforeSort.ContainsKey(j))
                    {
                        ret.AddIndex(indexesBeforeSort[j], i++);
                        fields.Add(fieldsBeforeSort[j]);
                        if (  
                            (
                            fieldsBeforeSort[j].IndexOf("COUNT") > -1 
                            ||
                            fieldsBeforeSort[j].IndexOf("SUM") > -1 && fieldsBeforeSort[j].IndexOf("(SELECT ISNULL(SUM") < 0 
                            )  && funcedFieldNames.Length > 1)
                            isGroup = true;
                        else
                            GroupByFields.Add(fieldsBeforeSort[j]);
                    }
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
                    ret.ClearIndexes();
                    ret.AddIndex(funcName, 0);
                }
            }

            ret.Sentence = string.Format(ret.Sentence, fStr, (conds.Count > 0 ? string.Join(conditions.AndOrOr ? " AND " : " OR ", conds.ToArray()) : " 1=1 "));
            if (isGroup)
                ret.Sentence = ret.Sentence + " GROUP BY " + string.Join(",", GroupByFields.ToArray());

            if (orderbyField != null && orderbyField.Length > 0)
                ret.Sentence = ret.Sentence + " ORDER BY " + string.Join(",", orderbyField);

            return ret;
        }

        public static SQLContextNew GetConditionedJoinedSelect(int methodId, string funcName, ITableAndFields[] tblFlds, TableConnectionCollection tblCnnts, params string[] orderbyField)
        {
            SQLContextNew ret = null;
            if (SQLCache.GetFromCache(methodId, ref ret))
                return ret;
            return GetConditionedJoinedSelect_Inner(ret, funcName, tblFlds, tblCnnts, orderbyField);
        }

        public static SQLContextNew GetConditionedJoinedSelect(string funcName, ITableAndFields[] tblFlds, TableConnectionCollection tblCnnts, params string[] orderbyField)
        {
            SQLContextNew ret = new SQLContextNew();
            return GetConditionedJoinedSelect_Inner(ret, funcName, tblFlds, tblCnnts, orderbyField);
        }

        private static SQLContextNew GetConditionedJoinedSelect_Inner(SQLContextNew ret, string funcName, ITableAndFields[] tblFlds, TableConnectionCollection tblCnnts, params string[] orderbyField)
        {
            //SQLContextNew ret = null;
            //bool res1 = GetFromCache(methodId, ref ret);
            //bool res2 = GetFromCacheTaF(methodId, ref tblAndFldses);
            //if (res1 && res2)
            //    return ret;
            //if (SQLCache.GetFromCache(methodId, ref ret))
            //    return ret;

            bool isGroup = false;
            List<string> GroupByFields = new List<string>();

            ret.Sentence = "SELECT {0} FROM {1} WHERE {2} AND {3} ";
            List<string> tables = new List<string>();
            List<string> fields = new List<string>();
            List<string> conds = new List<string>();

            int i = 0;
            int j = 1;
            foreach (ITableAndFields tblFld in tblFlds)
            {
                string alias = "t" + j.ToString();
                tblFld.Alias = alias;

                if (tblFld.SubDBCalalog != null)
                    tables.Add(tblFld.SubDBCalalog + ".." + ToolsNew.GetTableName(tblFld.Table) + " " + alias); //Sub Tables
                else
                    tables.Add(ToolsNew.GetTableName(tblFld.Table) + " " + alias);

               // FieldInfo[] fis = tblFld.Table.GetFields(BindingFlags.Instance | BindingFlags.Public);
                IDictionary<string, DBFieldAttribute> dbAttrList;
                FieldInfo[] cfis;
                FieldInfo[] fis = getCacheFieldInfos(tblFld.Table, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);

                foreach (FieldInfo fi in fis)
                {
                    //string columnName = ToolsNew.TransField(tblFld.Table, fi.Name);
                    string columnName = ToolsNew.TransField(cfis, fi.Name);

                    //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                    //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                    DBFieldAttribute dbfa = dbAttrList[fi.Name];
                    foreach (ICondition condition in tblFld.Conditions.Items)
                    {
                        string condSegment = string.Empty;
                        IDictionary<string, SqlParameter> parames = null;

                        if (condition.TryToAsACondition(fi, columnName, dbfa.columnType, out condSegment, out parames, tblFld))
                        {
                            conds.Add(condSegment);
                            foreach (string key in parames.Keys)
                                ret.AddParam(key, parames[key]);
                        }
                    }

                    string key4Reg = TableConnectionCollection.DecAlias(tblFld.GetHashCode().ToString(), columnName);
                    if (tblCnnts.Regist.ContainsKey(key4Reg))
                    {
                        IDictionary<string, ITableConnectionItem> point = tblCnnts.Regist[key4Reg];
                        foreach (ITableConnectionItem tcti in point.Values)
                        {
                            if (!tcti.IsAliasSet_tbl1 && tblFld == tcti.Tb1)
                            {
                                tcti.Tb1.Alias = alias;
                                tcti.IsAliasSet_tbl1 = true;
                            }
                            if (!tcti.IsAliasSet_tbl2 && tblFld == tcti.Tb2)
                            {
                                tcti.Tb2.Alias = alias;
                                tcti.IsAliasSet_tbl2 = true;
                            }
                        }
                    }

                    if (/*tblFld.ToGetFieldNames == null ||*/ tblFld.ToGetFieldNames != null && (tblFld.ToGetFieldNames.Count < 1 || tblFld.ToGetFieldNames.Contains(columnName)))
                    {
                        ret.AddIndex(TableConnectionCollection.DecAlias(alias, columnName), i);
                        fields.Add(TableConnectionCollection.DecAliasInner(alias, columnName));
                        i++;
                    }

                    if (tblFld.ToGetFuncedFieldNames != null && tblFld.ToGetFuncedFieldNames.ContainsKey(columnName))
                    {
                        ret.AddIndex(TableConnectionCollection.DecAlias(alias, columnName), i);
                        string fld = string.Format(tblFld.ToGetFuncedFieldNames[columnName], TableConnectionCollection.DecAliasInner(alias, columnName));
                        fields.Add(fld);

                        if ((fld.IndexOf("COUNT") > -1 || fld.IndexOf("SUM") > -1) && !fld.StartsWith("(SELECT"))
                            isGroup = true;
                        else
                            GroupByFields.Add(fld);
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
                    if (funcName.IndexOf("{") > -1 && funcName.IndexOf("}") > -1)
                    {
                        fStr = string.Format(funcName, fStr);
                        ret.ClearIndexes();
                        ret.AddIndex(funcName, 0);
                    }
                    else
                    {
                        fStr = string.Format("{0}({1})", funcName, fStr);
                        ret.ClearIndexes();
                        ret.AddIndex(funcName, 0);
                    }
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
            if (isGroup && GroupByFields.Count > 0)
                ret.Sentence = ret.Sentence + " GROUP BY " + string.Join(",", GroupByFields.ToArray());

            if (orderbyField != null && orderbyField.Length > 0)
                ret.Sentence = ret.Sentence + " ORDER BY " + string.Join(",", orderbyField);

            ret.TableFields = tblFlds;

            return ret;
        }

        public static SQLContextNew GetConditionedJoinedSelectWithCertainResultSetSequence(int methodId, string funcName, ITableAndFields[] tblFlds, TableConnectionCollection tblCnnts, params string[] orderbyField)
        {
            SQLContextNew ret = null;
            //bool res1 = GetFromCache(methodId, ref ret);
            //bool res2 = GetFromCacheTaF(methodId, ref tblAndFldses);
            //if (res1 && res2)
            //    return ret;
            if (SQLCache.GetFromCache(methodId, ref ret))
                return ret;

            bool isGroup = false;
            List<string> GroupByFields = new List<string>();

            ret.Sentence = "SELECT {0} FROM {1} WHERE {2} AND {3} ";
            List<string> tables = new List<string>();
            List<string> fields = new List<string>();
            List<string> conds = new List<string>();

            IDictionary<IntPair, string> indexesBeforeSort = new Dictionary<IntPair, string>();
            IDictionary<IntPair, string> fieldsBeforeSort = new Dictionary<IntPair, string>();

            int i = 0;
            int j = 1;
            foreach (ITableAndFields tblFld in tblFlds)
            {
                string alias = "t" + j.ToString();
                tblFld.Alias = alias;

                if (tblFld.SubDBCalalog != null)
                    tables.Add(tblFld.SubDBCalalog + ".." + ToolsNew.GetTableName(tblFld.Table) + " " + alias); //Sub Tables
                else
                    tables.Add(ToolsNew.GetTableName(tblFld.Table) + " " + alias);

                //FieldInfo[] fis = tblFld.Table.GetFields(BindingFlags.Instance | BindingFlags.Public);
                IDictionary<string, DBFieldAttribute> dbAttrList;
                FieldInfo[] cfis;
                FieldInfo[] fis = getCacheFieldInfos(tblFld.Table, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);

                int f = 0;
                foreach (FieldInfo fi in fis)
                {
                    //string columnName = ToolsNew.TransField(tblFld.Table, fi.Name);
                    string columnName = ToolsNew.TransField(cfis, fi.Name);

                    //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                    //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                    DBFieldAttribute dbfa = dbAttrList[fi.Name];

                    foreach (ICondition condition in tblFld.Conditions.Items)
                    {
                        string condSegment = string.Empty;
                        IDictionary<string, SqlParameter> parames = null;

                        if (condition.TryToAsACondition(fi, columnName, dbfa.columnType, out condSegment, out parames, tblFld))
                        {
                            conds.Add(condSegment);
                            foreach (string key in parames.Keys)
                                ret.AddParam(key, parames[key]);
                        }
                    }

                    string key4Reg = TableConnectionCollection.DecAlias(tblFld.GetHashCode().ToString(), columnName);
                    if (tblCnnts.Regist.ContainsKey(key4Reg))
                    {
                        IDictionary<string, ITableConnectionItem> point = tblCnnts.Regist[key4Reg];
                        foreach (ITableConnectionItem tcti in point.Values)
                        {
                            if (!tcti.IsAliasSet_tbl1 && tblFld == tcti.Tb1)
                            {
                                tcti.Tb1.Alias = alias;
                                tcti.IsAliasSet_tbl1 = true;
                            }
                            if (!tcti.IsAliasSet_tbl2 && tblFld == tcti.Tb2)
                            {
                                tcti.Tb2.Alias = alias;
                                tcti.IsAliasSet_tbl2 = true;
                            }
                        }
                    }

                    if (/*tblFld.ToGetFieldNames == null ||*/ tblFld.ToGetFieldNames != null && (tblFld.ToGetFieldNames.Count < 1 || tblFld.ToGetFieldNames.Contains(columnName)))
                    {
                        ////ret.AddIndex(TableConnectionCollection.DecAlias(alias, columnName), i);
                        ////fields.Add(TableConnectionCollection.DecAliasInner(alias, columnName));

                        if (tblFld.ToGetFieldNames.Count < 1)
                        {
                            indexesBeforeSort.Add(new IntPair( j, f ), TableConnectionCollection.DecAlias(alias, columnName));
                            fieldsBeforeSort.Add(new IntPair( j, f ), TableConnectionCollection.DecAliasInner(alias, columnName));
                        }
                        else
                        {
                            int k = 1;
                            for (int b = 0; b < tblFld.ToGetFieldNames.Count; b++)
                            {
                                if (tblFld.ToGetFieldNames[b].Equals(columnName))
                                {
                                    string postFix = k > 1 ? "_" + k.ToString() : "";

                                    indexesBeforeSort.Add(new IntPair( j, b ), TableConnectionCollection.DecAlias(alias, columnName) + postFix);
                                    fieldsBeforeSort.Add(new IntPair( j, b ), TableConnectionCollection.DecAliasInner(alias, columnName));

                                    k++;
                                }
                            }
                        }

                        ////i++;
                    }

                    if (tblFld.ToGetFuncedFieldNames != null && tblFld.ToGetFuncedFieldNames.ContainsKey(columnName))
                    {
                        ////ret.AddIndex(TableConnectionCollection.DecAlias(alias, columnName), i);
                        string fld = string.Format(tblFld.ToGetFuncedFieldNames[columnName], TableConnectionCollection.DecAliasInner(alias, columnName));
                        ////fields.Add(fld);

                        if (fld.IndexOf("COUNT") > -1 || fld.IndexOf("SUM") > -1)
                            isGroup = true;
                        else
                            GroupByFields.Add(fld);

                        int k = 1;
                        int b = 0;
                        foreach (KeyValuePair<string, string> kvp in tblFld.ToGetFuncedFieldNames)
                        {
                            if (kvp.Key.Equals(columnName) || (kvp.Key.StartsWith(columnName) && kvp.Key.EndsWith("<DupFunc>")))
                            {
                                string postFix = k > 1 ? "_" + k.ToString() : "";

                                indexesBeforeSort.Add(new IntPair( j, b ), TableConnectionCollection.DecAlias(alias, columnName) + postFix);
                                fieldsBeforeSort.Add(new IntPair( j, b ), fld);

                                k++;
                            }
                            b++;
                        }
                        
                        ////i++;
                    }
                }
                f++;
                j++;
            }

            if (indexesBeforeSort.Count > 0 || fieldsBeforeSort.Count > 0)
            {
                j = 1;
                foreach (ITableAndFields tblFld in tblFlds)
                {
                    if (tblFld.ToGetFieldNames != null)
                    {
                        if (tblFld.ToGetFieldNames.Count < 1)
                        {
                            FieldInfo[] fis = tblFld.Table.GetFields(BindingFlags.Instance | BindingFlags.Public);

                            int f = 0;
                            foreach (FieldInfo fi in fis)
                            {
                                IntPair ip = new IntPair(j, f);
                                if (indexesBeforeSort.ContainsKey(ip) && fieldsBeforeSort.ContainsKey(ip))
                                {
                                    ret.AddIndex(indexesBeforeSort[ip], i++);
                                    fields.Add(fieldsBeforeSort[ip]);
                                }
                                f++;
                            }
                        }
                        else
                        {
                            for (int b = 0; b < tblFld.ToGetFieldNames.Count; b++)
                            {
                                IntPair ip = new IntPair(j, b);
                                if (indexesBeforeSort.ContainsKey(ip) && fieldsBeforeSort.ContainsKey(ip))
                                {
                                    ret.AddIndex(indexesBeforeSort[ip], i++);
                                    fields.Add(fieldsBeforeSort[ip]);
                                }
                            }
                        }
                    }
                    if (tblFld.ToGetFuncedFieldNames != null && tblFld.ToGetFuncedFieldNames.Count > 0)
                    {
                        for (int b = 0; b < tblFld.ToGetFuncedFieldNames.Count; b++)
                        {
                            IntPair ip = new IntPair(j, b);
                            if (indexesBeforeSort.ContainsKey(ip) && fieldsBeforeSort.ContainsKey(ip))
                            {
                                ret.AddIndex(indexesBeforeSort[ip], i++);
                                fields.Add(fieldsBeforeSort[ip]);
                            }
                        }
                    }
                    j++;
                }
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
                    if (funcName.IndexOf("{") > -1 && funcName.IndexOf("}") > -1)
                    {
                        fStr = string.Format(funcName, fStr);
                        ret.ClearIndexes();
                        ret.AddIndex(funcName, 0);
                    }
                    else
                    {
                        fStr = string.Format("{0}({1})", funcName, fStr);
                        ret.ClearIndexes();
                        ret.AddIndex(funcName, 0);
                    }
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
            if (isGroup && GroupByFields.Count > 0)
                ret.Sentence = ret.Sentence + " GROUP BY " + string.Join(",", GroupByFields.ToArray());

            if (orderbyField != null && orderbyField.Length > 0)
                ret.Sentence = ret.Sentence + " ORDER BY " + string.Join(",", orderbyField);

            ret.TableFields = tblFlds;

            return ret;
        }

        public static SQLContextNew GetConditionedComprehensiveJoinedSelect(int methodId, string funcName, ITableAndFields[] tblFlds, TableConnectionCollection tblCnnts, TableBiJoinedLogic tblBiJndLgc, bool andOrOr)
        {
            SQLContextNew ret = null;
            //bool res1 = GetFromCache(methodId, ref ret);
            //bool res2 = GetFromCacheTaF(methodId, ref tblAndFldses);
            //if (res1 && res2)
            //    return ret;
            if (SQLCache.GetFromCache(methodId, ref ret))
                return ret;

            ret.Sentence = "SELECT {0} FROM {1} WHERE {2} ";
            //List<string> tables = new List<string>();
            List<string> fields = new List<string>();
            List<string> conds = new List<string>();

            int j = 1;
            foreach (ITableAndFields tblFld in tblFlds)
            {
                string alias = "t" + j.ToString();
                tblFld.Alias = alias;

                //if (tblAndFlds.subDBCalalog != null)
                //    tables.Add(tblAndFlds.subDBCalalog + ".." + Tools.GetTableName(tblAndFlds.Table) + " " + alias); //Sub Tables
                //else
                //    tables.Add(Tools.GetTableName(tblAndFlds.Table) + " " + alias);

                //object default_obj = Activator.CreateInstance(tblAndFlds.Table);
                //FieldInfo[] fis = tblFld.Table.GetFields(BindingFlags.Instance | BindingFlags.Public);
                IDictionary<string, DBFieldAttribute> dbAttrList;
                FieldInfo[] cfis;
                FieldInfo[] fis = getCacheFieldInfos(tblFld.Table, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
                int i = 0;

                foreach (FieldInfo fi in fis)
                {
                    //string columnName = ToolsNew.TransField(tblFld.Table, fi.Name);
                    string columnName = ToolsNew.TransField(cfis, fi.Name);

                    //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                    //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                    DBFieldAttribute dbfa = dbAttrList[fi.Name];

                    foreach (ICondition condition in tblFld.Conditions.Items)
                    {
                        string condSegment = string.Empty;
                        IDictionary<string, SqlParameter> parames = null;

                        if (condition.TryToAsACondition(fi, columnName, dbfa.columnType, out condSegment, out parames, tblFld))
                        {
                            conds.Add(condSegment);
                            foreach (string key in parames.Keys)
                                ret.AddParam(key, parames[key]);
                        }
                    }

                    string key4Reg = TableConnectionCollection.DecAlias(tblFld.GetHashCode().ToString(), columnName);
                    if (tblCnnts.Regist.ContainsKey(key4Reg))
                    {
                        IDictionary<string, ITableConnectionItem> point = tblCnnts.Regist[key4Reg];
                        foreach (ITableConnectionItem tcti in point.Values)
                        {
                            if (!tcti.IsAliasSet_tbl1 && tblFld == tcti.Tb1)
                            {
                                tcti.Tb1.Alias = alias;
                                tcti.IsAliasSet_tbl1 = true;
                            }
                            if (!tcti.IsAliasSet_tbl2 && tblFld == tcti.Tb2)
                            {
                                tcti.Tb2.Alias = alias;
                                tcti.IsAliasSet_tbl2 = true;
                            }
                        }
                    }

                    if (/*tblAndFlds.ToGetFieldNames == null ||*/ tblFld.ToGetFieldNames != null && (tblFld.ToGetFieldNames.Count < 1 || tblFld.ToGetFieldNames.Contains(columnName)))
                    {
                        ret.AddIndex(TableConnectionCollection.DecAlias(alias, columnName), i);
                        fields.Add(TableConnectionCollection.DecAliasInner(alias, columnName));
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
                    ret.ClearIndexes();
                    ret.AddIndex(funcName, 0);
                }
            }

            object lastObj = null;
            string toStr = string.Empty;
            IEnumerator enmrt = tblBiJndLgc.Enum;
            while (enmrt.MoveNext())
            {
                object obj = enmrt.Current;
                if (obj is ITableAndFields)
                {
                    toStr += obj.ToString();
                }
                else if (obj is ITableConnectionItem)
                {
                    if (lastObj is ITableConnectionItem)
                    {
                        if (((ITableConnectionItem)obj).AndOrOr)
                            toStr += TableBiJoinedLogic.c_andStr;
                        else
                            toStr += TableBiJoinedLogic.c_orStr;
                    }
                    else if (lastObj is ITableAndFields)
                    {
                        toStr += TableBiJoinedLogic.c_onStr;
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

            ret.TableFields = tblFlds;

            return ret;
        }

        public static SQLContextNew GetConditionedForBackupInsert<TSrc, TDest>(string[][] fieldsSourceTargetMapping, ConditionCollection<TSrc> conditions)
        {
            SQLContextNew ret = new SQLContextNew();
            return GetConditionedForBackupInsert_Inner<TSrc, TDest>(ret, fieldsSourceTargetMapping, conditions, null);
        }

        public static SQLContextNew GetConditionedForBackupInsert<TSrc, TDest>(string[][] fieldsSourceTargetMapping, ConditionCollection<TSrc> conditions, string srcDb)
        {
            SQLContextNew ret = new SQLContextNew();
            return GetConditionedForBackupInsert_Inner<TSrc, TDest>(ret, fieldsSourceTargetMapping, conditions, srcDb);
        }

        public static SQLContextNew GetConditionedForBackupInsert<TSrc, TDest>(int methodId, string[][] fieldsSourceTargetMapping, ConditionCollection<TSrc> conditions)
        {
            SQLContextNew ret = null;
            if (SQLCache.GetFromCache(methodId, ref ret))
                return ret;
            return GetConditionedForBackupInsert_Inner<TSrc, TDest>(ret, fieldsSourceTargetMapping, conditions, null);
        }

        public static SQLContextNew GetConditionedForBackupInsert<TSrc, TDest>(int methodId, string[][] fieldsSourceTargetMapping, ConditionCollection<TSrc> conditions, string srcDb)
        {
            SQLContextNew ret = null;
            if (SQLCache.GetFromCache(methodId, ref ret))
                return ret;
            return GetConditionedForBackupInsert_Inner<TSrc, TDest>(ret, fieldsSourceTargetMapping, conditions, srcDb);
        }

        private static SQLContextNew GetConditionedForBackupInsert_Inner<TSrc, TDest>(SQLContextNew ret, string[][] fieldsSourceTargetMapping, ConditionCollection<TSrc> conditions, string srcDb)
        {
            Type typeSrc = typeof(TSrc);
            Type typeDest = typeof(TDest);

            ret.Sentence = "INSERT INTO " + ToolsNew.GetTableName(typeDest) + " ({0}) SELECT {1} FROM " + (srcDb == null ? "" : srcDb + "..") + ToolsNew.GetTableName(typeSrc) + " WHERE {2}";

            List<string> fieldsDest = new List<string>();
            List<string> fieldsSrc = new List<string>();

            //FieldInfo[] fisDest = typeDest.GetFields(BindingFlags.Instance | BindingFlags.Public);
            //foreach (FieldInfo fi in fisDest)
            //{
            //    string columnName = ToolsNew.TransField(typeDest, fi.Name);
            //    //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
            //    //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];

            //    fieldsDest.Add(columnName);
            //}

            List<string> fields_src = new List<string>();
            List<string> conds = new List<string>();

            //FieldInfo[] fisSrc= typeSrc.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, DBFieldAttribute> dbAttrList;
            FieldInfo[] cfis;
            FieldInfo[] fisSrc = getCacheFieldInfos(typeSrc, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
            //int i = 0;
            foreach (FieldInfo fi in fisSrc)
            {
                //string columnName = ToolsNew.TransField(typeSrc, fi.Name);
                string columnName = ToolsNew.TransField(cfis, fi.Name);

                //fieldsSrc.Add(columnName);

                //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                DBFieldAttribute dbfa = dbAttrList[fi.Name];

                foreach (ICondition condition in conditions.Items)
                {
                    string condSegment = string.Empty;
                    IDictionary<string, SqlParameter> parames = null;

                    if (condition.TryToAsACondition(fi, columnName, dbfa.columnType, out condSegment, out parames, null))
                    {
                        conds.Add(condSegment);
                        foreach (string key in parames.Keys)
                            ret.AddParam(key, parames[key]);
                    }
                }
            }

            if (fieldsSourceTargetMapping != null)
            {
                foreach (string[] pair in fieldsSourceTargetMapping)
                {
                    fieldsDest.Add(pair[1]);
                    fieldsSrc.Add(pair[0]);
                }
            }

            ret.Sentence = string.Format(ret.Sentence, string.Join(",", fieldsDest.ToArray()), string.Join(",", fieldsSrc.ToArray()), (conds.Count > 0 ? string.Join(conditions.AndOrOr ? " AND " : " OR ", conds.ToArray()) : " 1=1 "));
            return ret;
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

        public static ObjectActivator CreateInstance<C>()
        {
            Type type = typeof(C);
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
                    activator =CreateCtor(type);
                    _fisObjCache.Add(name, activator);
                }
            }

            return activator;
        }
        #endregion

        #region ORMapping Copes

        public static C SetFieldFromColumnWithoutReadReader<T, C>(C fisObj, SqlDataReader sqlR, SQLContextNew sqlCtx)
        {
            if (fisObj == null)
            {
                //fisObj = Activator.CreateInstance<C>();
                fisObj =(C)(CreateInstance<C>())();
                if (fisObj != null && fisObj is FisObjectBase.FisObjectBase)
                {
                    object obj = fisObj;
                    ((FisObjectBase.FisObjectBase)obj).Tracker.Clear();
                }
            }

            Type clsType = typeof(C);
            Type tblType = typeof(T);

            IDictionary<string, FieldInfo> enm = GetColumnToField(clsType, tblType);
            //IDictionary<string, FieldInfo> enm_FldToCol = GetFieldToColumn(clsType);
            IDictionary<string, SqlDbType> enm_FldToColType = GetFieldToColumnType(clsType);
            if (enm != null)
            {
                GetValueClass g = new GetValueClass();

                foreach (KeyValuePair<string, FieldInfo> pair in enm)
                {
                    SqlDbType columnType = enm_FldToColType[pair.Value.Name];
                    object val = g.GetValue(sqlR, sqlCtx.Indexes(pair.Key), columnType);
                    pair.Value.SetValue(fisObj, val);
                }
            }
            return fisObj;
        }

        //public static T SetColumnFromColumn<T>(SqlDataReader sqlR, SQLContextNew sqlCtx)
        //{
        //    T ret = default(T);
        //    if (sqlR != null && sqlR.Read())
        //    {
        //        ret = Activator.CreateInstance<T>();

        //        Type tblType = typeof(T);

        //        //IDictionary<string, FieldInfo> enm = //GetColumnToField(tblType);


        //        FieldInfo[] flds = tblType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        //        if (flds != null && flds.Length > 0)
        //        {
        //            foreach (FieldInfo fld in flds)
        //            {
        //                object[] attrObjs = fld.GetCustomAttributes(typeof(DBFieldAttribute), false);
        //                if (attrObjs != null && attrObjs.Length > 0)
        //                {

        //                }
        //            }
        //        }


        //        if (fldis != null)
        //        {
        //            foreach (KeyValuePair<string, FieldInfo> pair in esqlCtxnm)
        //            {
        //                //if (exceptColumns != null && exceptColumns.Contains(pair.Key))
        //                //    continue;

        //                object val = pair.Value.GetValue(fisObj);
        //                FieldInfo fiCol = enm_FldToCol[pair.Value.Name];
        //                if (fiCol != null)
        //                {
        //                    fiCol.SetValue(ret, val);
        //                }
        //            }
        //        }
        //    }
        //    return ret;
        //}

        public static C SetFieldFromColumn<T, C>(C fisObj, SqlDataReader sqlR, SQLContextNew sqlCtx)
        {
            if (sqlR != null && sqlR.Read())
            {
                fisObj = SetFieldFromColumnWithoutReadReader<T, C>(fisObj, sqlR, sqlCtx);
            }
            return fisObj;
        }

        public static C SetFieldFromColumn<T, C>(C fisObj, SqlDataReader sqlR, SQLContextNew sqlCtx, string alias)
        {
            if (sqlR != null)
            {
                if (fisObj == null)
                {
                    //fisObj = Activator.CreateInstance<C>();
                    fisObj = (C)(CreateInstance<C>())();
                    if (fisObj != null && fisObj is FisObjectBase.FisObjectBase)
                    {
                        object obj = fisObj;
                        ((FisObjectBase.FisObjectBase)obj).Tracker.Clear();
                    }
                }

                Type clsType = typeof(C);
                Type tblType = typeof(T);

                IDictionary<string, FieldInfo> enm = GetColumnToField(clsType, tblType);
                //IDictionary<string, FieldInfo> enm_FldToCol = GetFieldToColumn(clsType);
                IDictionary<string, SqlDbType> enm_FldToColType = GetFieldToColumnType(clsType);
                if (enm != null)
                {
                    GetValueClass g = new GetValueClass();

                    foreach (KeyValuePair<string, FieldInfo> pair in enm)
                    {
                        SqlDbType columnType = enm_FldToColType[pair.Value.Name];
                        object val = g.GetValue(sqlR, sqlCtx.Indexes(TableConnectionCollection.DecAlias(alias, pair.Key)), columnType);
                        pair.Value.SetValue(fisObj, val);
                    }
                }
            }
            return fisObj;
        }

        public static IList<I> SetFieldFromColumn<T, C, I>(IList<I> fisObjLst, SqlDataReader sqlR, SQLContextNew sqlCtx)
        {
            return SetFieldFromColumn_Inner<T, C, I>(fisObjLst, sqlR, sqlCtx, null);
        }

        public static IList<I> SetFieldFromColumn<T, C, I>(IList<I> fisObjLst, SqlDataReader sqlR, SQLContextNew sqlCtx, string alias)
        {
            return SetFieldFromColumn_Inner<T, C, I>(fisObjLst, sqlR, sqlCtx, alias);
        }

        private static IList<I> SetFieldFromColumn_Inner<T, C, I>(IList<I> fisObjLst, SqlDataReader sqlR, SQLContextNew sqlCtx, string alias)
        {
            if (sqlR != null)
            {
                if (fisObjLst == null)
                    fisObjLst = new List<I>();

                Type clsType = typeof(C);
                Type tblType = typeof(T);

                IDictionary<string, FieldInfo> enm = GetColumnToField(clsType, tblType);
                //IDictionary<string, FieldInfo> enm_FldToCol = GetFieldToColumn(clsType);
                IDictionary<string, SqlDbType> enm_FldToColType = GetFieldToColumnType(clsType);
                if (enm != null)
                {
                    GetValueClass g = new GetValueClass();

                    IDictionary<string, SqlDbType> clmnTypes = new Dictionary<string, SqlDbType>();
                    ObjectActivator activator= CreateInstance<C>();
                    while (sqlR.Read())
                    {
                        //object fisObj = Activator.CreateInstance<C>();
                        object fisObj = activator();
                        if (fisObj != null && fisObj is FisObjectBase.FisObjectBase)
                            ((FisObjectBase.FisObjectBase)fisObj).Tracker.Clear();
                        foreach (KeyValuePair<string, FieldInfo> pair in enm)
                        {
                            SqlDbType columnType;
                            if (clmnTypes.ContainsKey(pair.Value.Name))
                            {
                                columnType = clmnTypes[pair.Value.Name];
                            }
                            else
                            {
                                columnType = enm_FldToColType[pair.Value.Name];
                                clmnTypes.Add(pair.Value.Name, columnType);
                            }
                            object val = null;
                            if (alias == null)
                                val = g.GetValue(sqlR, sqlCtx.Indexes(pair.Key), columnType);
                            else
                                val = g.GetValue(sqlR, sqlCtx.Indexes(TableConnectionCollection.DecAlias(alias, pair.Key)), columnType);

                            pair.Value.SetValue(fisObj, val);
                        }
                        fisObjLst.Add((I)fisObj);
                    }
                }
            }
            return fisObjLst;
        }

        public static SQLContextNew SetColumnFromField<T, C>(SQLContextNew sqlCtx, C fisObj, params string[] exceptColumns)
        {
            return SetColumnFromField_Inner<T, C>(sqlCtx, fisObj, false, exceptColumns);
        }

        public static SQLContextNew SetColumnFromField<T, C>(SQLContextNew sqlCtx, C fisObj, bool isSetVal, params string[] exceptColumns)
        {
            return SetColumnFromField_Inner<T, C>(sqlCtx, fisObj, isSetVal, exceptColumns);
        }

        private static SQLContextNew SetColumnFromField_Inner<T, C>(SQLContextNew sqlCtx, C fisObj, bool isSetVal, params string[] exceptColumns)
        {
            if (fisObj != null && sqlCtx != null)
            {
                Type clsType = typeof(C);
                Type tblType = typeof(T);

                IDictionary<string, FieldInfo> enm = GetColumnToField(clsType, tblType);

                if (enm != null)
                {
                    foreach (KeyValuePair<string, FieldInfo> pair in enm)
                    {
                        if (exceptColumns != null && exceptColumns.Contains(pair.Key))
                            continue;

                        SqlParameter spmt = isSetVal ? sqlCtx.Param(CommonSetValue<T>.Dec(pair.Key)) : sqlCtx.Param(pair.Key);
                        if (spmt != null)   //以排除那些诸如生成ID的不需要全字段赋值的SQL.
                        {
                            object val = pair.Value.GetValue(fisObj);
                            spmt.Value = val;
                        }
                    }
                }
            }
            return sqlCtx;
        }

        public static T SetColumnFromField<T, C>(C fisObj, params string[] exceptColumns)
        {
            T ret = default(T);
            if (fisObj != null)
            {
                //ret = Activator.CreateInstance<T>();
                ret = (T)(CreateInstance<T>())();

                Type clsType = typeof(C);
                Type tblType = typeof(T);

                IDictionary<string, FieldInfo> enm = GetColumnToField(clsType, tblType);
                IDictionary<string, FieldInfo> enm_FldToCol = GetFieldToColumn(clsType);

                if (enm != null)
                {
                    foreach (KeyValuePair<string, FieldInfo> pair in enm)
                    {
                        if (exceptColumns != null && exceptColumns.Contains(pair.Key))
                            continue;

                        object val = pair.Value.GetValue(fisObj);
                        FieldInfo fiCol = enm_FldToCol[pair.Value.Name];
                        if (fiCol != null)
                        {
                            fiCol.SetValue(ret, val);
                        }
                    }
                }
            }
            return ret;
        }

        public static IList<string[]> SetFieldFromColumnInKeyPair<T>(SqlDataReader sqlR, SQLContextNew sqlCtx)
        {
            IList<string[]> ret = new List<string[]>();

            if (sqlR != null)
            {
                Type tblType = typeof(T);

                GetValueClass g = new GetValueClass();

                //FieldInfo[] fis = tblType.GetFields(BindingFlags.Instance | BindingFlags.Public);
                IDictionary<string, DBFieldAttribute> dbAttrList;
                FieldInfo[] cfis;
                FieldInfo[] fis = getCacheFieldInfos(tblType, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
                foreach (FieldInfo fi in fis)
                {
                    //string columnName = ToolsNew.TransField(tblType, fi.Name);
                    string columnName = ToolsNew.TransField(cfis, fi.Name);
                    //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                    //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                    DBFieldAttribute dbfa = dbAttrList[fi.Name];
                    SqlDbType columnType = dbfa.columnType;

                    object val = g.GetValue(sqlR, sqlCtx.Indexes(columnName), columnType);

                    ret.Add(new string[] { columnName, val.ToString() });
                }
            }
            return ret.ToArray();
        }

        public static T GetAAllNonDefaultObject<T>(string[] exceptColumns,params SqlDbType[] columnTypes)
        {
            T ret = default(T);
            if (true)
            {
                //ret = Activator.CreateInstance<T>();
                ret = (T)(CreateInstance<T>())();

                Type tblType = typeof(T);

                GetValueClass g = new GetValueClass();

                //FieldInfo[] fis = tblType.GetFields(BindingFlags.Instance | BindingFlags.Public);
                IDictionary<string, DBFieldAttribute> dbAttrList;
                FieldInfo[] cfis;
                FieldInfo[] fis = getCacheFieldInfos(tblType, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
                foreach (FieldInfo fi in fis)
                {
                    //string columnName = ToolsNew.TransField(tblType, fi.Name);
                    string columnName = ToolsNew.TransField(cfis, fi.Name);
                    //object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                    //DBFieldAttribute dbfa = (DBFieldAttribute)objs[0];
                    DBFieldAttribute dbfa = dbAttrList[fi.Name];
                    SqlDbType columnType = dbfa.columnType;

                    if (exceptColumns != null && exceptColumns.Contains(columnName))
                        continue;

                    if (fi != null && (columnTypes == null || columnTypes.Count() < 1 || columnTypes.Contains(columnType)))
                    {
                        object val = g.GetNonDefaultFieldValue(columnType);
                        fi.SetValue(ret, val);
                    }
                }
            }
            return ret;
        }

        public static SQLContextNew SetColumnFromField<T>(SQLContextNew sqlCtx, T scmObj, object commonValue, SqlDbType commonValueType)
        {
            return SetColumnFromField<T>(sqlCtx, scmObj, commonValue, "{0}", commonValueType);
        }

        public static SQLContextNew SetColumnFromField<T>(SQLContextNew sqlCtx, T scmObj, object commonValue, string paramDecStr, SqlDbType commonValueType)
        {
            return SetColumnFromField_Inner<T>(sqlCtx, scmObj, commonValue, paramDecStr, commonValueType);
        }

        public static SQLContextNew SetColumnFromField<T>(SQLContextNew sqlCtx, T scmObj)
        {
            return SetColumnFromField_Inner<T>(sqlCtx, scmObj, null, "{0}", SqlDbType.NVarChar);
        }

        private static SQLContextNew SetColumnFromField_Inner<T>(SQLContextNew sqlCtx, T scmObj, object commonValue, string paramDecStr, SqlDbType commonValueType)
        {
            if (sqlCtx != null)
            {
                Type tblType = typeof(T);

                //FieldInfo[] fis = tblType.GetFields(BindingFlags.Instance | BindingFlags.Public);
                IDictionary<string, DBFieldAttribute> dbAttrList;
                FieldInfo[] cfis;
                FieldInfo[] fis = getCacheFieldInfos(tblType, BindingFlags.Instance | BindingFlags.Public, out dbAttrList, out cfis);
                foreach (FieldInfo fi in fis)
                {
                    //if (exceptColumns != null && exceptColumns.Contains(pair.Key))
                    //    continue;
                    //string columnName = ToolsNew.TransField(tblType, fi.Name);
                    string columnName = ToolsNew.TransField(cfis, fi.Name);

                    SqlParameter spmt = sqlCtx.Param(string.Format(paramDecStr,columnName));
                    if (spmt != null)   //以排除那些诸如生成ID的不需要全字段赋值的SQL.
                    {
                        object val = null;
                        if (commonValue == null && scmObj != null)
                        {
                            val = fi.GetValue(scmObj);
                        }
                        else
                        {
                            val = commonValue;
                            spmt.SqlDbType = commonValueType;
                        }
                        spmt.Value = val;
                    }
                }
            }
            return sqlCtx;
        }

        #region . Read Cache .

        private static IDictionary<string, FieldInfo> GetColumnToField(Type clsType)
        {
            IDictionary<string, FieldInfo> ret = null;

            lock (clsType) //(SQLCache._cacheSyncObj_orm)
            {
                if (IsInORMCache(clsType))
                {
                    Type tblType = SQLCache.GetForClassToType(clsType.Name);
                    //2015-06-11 Vincent change get key is TableName.ClassName
                    //IDictionary<string, FieldInfo> pairs = SQLCache.GetForColumnToField(tblType.Name);
                    IDictionary<string, FieldInfo> pairs = SQLCache.GetForColumnToField(tblType.Name, clsType.Name);
                    if (pairs != null && pairs.Count > 0)
                    {
                        ret = pairs;//不用考虑Copy,因 ORM Cache 无加入后的更改.
                    }
                }
            }
            return ret;
        }

        private static IDictionary<string, FieldInfo> GetColumnToField(Type clsType, Type tblType)
        {
            IDictionary<string, FieldInfo> ret = null;

            lock (clsType) //(SQLCache._cacheSyncObj_orm)
            {
                if (IsInORMCache(clsType))
                {
                    //2015-06-11 Vincent change get data key is TableName.ClassName
                    //IDictionary<string, FieldInfo> pairs = SQLCache.GetForColumnToField(tblType.Name);
                    IDictionary<string, FieldInfo> pairs = SQLCache.GetForColumnToField(tblType.Name, clsType.Name);
                    if (pairs != null && pairs.Count > 0)
                    {
                        ret = pairs;//不用考虑Copy,因 ORM Cache 无加入后的更改.
                    }
                }
            }
            return ret;
        }

        private static IDictionary<string, FieldInfo> GetFieldToColumn(Type clsType)
        {
            IDictionary<string, FieldInfo> ret = null;

            lock (clsType) //(SQLCache._cacheSyncObj_orm)
            {
                if (IsInORMCache(clsType))
                {
                    IDictionary<string, FieldInfo> pairs = SQLCache.GetForFieldToColumn(clsType.Name);
                    if (pairs != null && pairs.Count > 0)
                    {
                        ret = pairs;//不用考虑Copy,因 ORM Cache 无加入后的更改.
                    }
                }
            }
            return ret;
        }

        private static IDictionary<string, SqlDbType> GetFieldToColumnType(Type clsType)
        {
            IDictionary<string, SqlDbType> ret = null;

            lock (clsType) //(SQLCache._cacheSyncObj_orm)
            {
                if (IsInORMCache(clsType))
                {
                    IDictionary<string, SqlDbType> pairs = SQLCache.GetForFieldToColumnType(clsType.Name);
                    if (pairs != null && pairs.Count > 0)
                    {
                        ret = pairs;//不用考虑Copy,因 ORM Cache 无加入后的更改.
                    }
                }
            }
            return ret;
        }

        private static Type GetTableType(Type clsType)
        {
            Type ret = null;

            lock (clsType) //(SQLCache._cacheSyncObj_orm)
            {
                if (IsInORMCache(clsType))
                {
                    ret = SQLCache.GetForClassToType(clsType.Name);
                }
            }
            return ret;
        }

        #endregion

        #region . Write Cache .

        private static bool IsInORMCache(Type clsType)
        {
            bool ret = false;

            if (!SQLCache.PeerTheORM_Table(clsType.Name))
            {
                object[] attrObjs_cls = clsType.GetCustomAttributes(typeof(ORMappingAttribute), false);
                if (attrObjs_cls != null && attrObjs_cls.Length > 0)
                {
                    ORMappingAttribute ormpa_cls = (ORMappingAttribute)attrObjs_cls[0];
                    Type tblType = ormpa_cls.table;

                    SQLCache.AddForClassToType(clsType.Name, tblType);
                    ret = true;

                    FieldInfo[] flds = clsType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (flds != null && flds.Length > 0)
                    {
                        foreach (FieldInfo fld in flds)
                        {
                            object[] attrObjs = fld.GetCustomAttributes(typeof(ORMappingAttribute), false);
                            if (attrObjs != null && attrObjs.Length > 0)
                            {
                                ORMappingAttribute ormpa = (ORMappingAttribute)attrObjs[0];
                                FieldInfo clmn = tblType.GetField(ToolsNew.caseChange(ormpa.column));
                                if (clmn != null)
                                {
                                    //2015-06-11 Vincent Change key from tableName to tableName+'.' + className
                                    //SQLCache.AddForColumnToField(tblType.Name, ormpa.column, fld);
                                    SQLCache.AddForColumnToField(tblType.Name, clsType.Name, ormpa.column, fld);
                                    SQLCache.AddForFieldToColumn(clsType.Name, fld.Name, clmn);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        #endregion

        #endregion
    }
}
