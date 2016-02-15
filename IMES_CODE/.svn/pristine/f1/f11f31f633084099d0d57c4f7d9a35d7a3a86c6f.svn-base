using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;

namespace IMES.Infrastructure.Repository._Metas
{
    public static class ToolsNew
    {
        private static readonly string fn_ = "fn_";

        #region . Tools .

        internal static string TransField(Type type, string prop)
        {
            FieldInfo fi = type.GetField(fn_ + prop);
            if (fi != null)
            {
                return (string)fi.GetValue(null);
            }
            return string.Empty;
        }

        internal static string TransField(FieldInfo[] fieldInfos, string prop)
        {
            //FieldInfo fi = type.GetField("fn_" + prop);
            FieldInfo fi = fieldInfos.Where(x => x.Name == fn_ + prop).FirstOrDefault();
            if (fi != null)
            {
                return (string)fi.GetValue(null);
            }
            return string.Empty;
        }

        public static string GetTableName(Type type)
        {
            string ret = string.Empty;

            object[] objs = type.GetCustomAttributes(typeof(TableAttribute), false);
            TableAttribute tbla = (TableAttribute)objs[0];
            ret = tbla.tableName;

            return tableNameChange(ret);
        }

        internal static string ClearRectBrace(string str)
        {
            return str.Replace("[", string.Empty).Replace("]", string.Empty).Replace(" ", "_").Replace("/","DIV");
        }

        #endregion

        #region .Algorithm for Schema Triangle .

        public static string caseChange(string orig)
        {
            string ret = orig = filterKeyword(orig);

            char[] cs = orig.ToArray<char>();
            if (cs != null && cs.Length > 0)
            {
                if (char.IsUpper(cs[0]))
                {
                    if (cs.Length > 1 && char.IsUpper(cs[1]))
                    {
                        for (int i = 0; i < cs.Length; i++)
                        {
                            cs[i] = char.ToLower(cs[i]);
                        }
                    }
                    else
                    {
                        cs[0] = char.ToLower(cs[0]);
                    }
                }
                ret = JoinToString(cs); // string.Join("", cs);
            }
            return filterKeyword(ret);
        }

        public static string caseChange2(string orig)
        {
            string ret = orig;

            char[] cs = orig.ToArray<char>();
            if (cs != null && cs.Length > 0)
            {
                if (char.IsLower(cs[0]))
                {
                    cs[0] = char.ToUpper(cs[0]);
                }

                if (cs.Length > 1 && char.IsUpper(cs[1]))
                {
                    for (int i = 1; i < cs.Length; i++)
                    {
                        if (cs[i - 1].Equals('_'))
                        {
                            cs[i] = char.ToUpper(cs[i]);
                        }
                        else
                        {
                            cs[i] = char.ToLower(cs[i]);
                        }
                    }
                }

                ret = JoinToString(cs); //string.Join("", cs);
            }
            return filterKeyword(ret);
        }

        public static string tableNameChange(string orig)
        {
            string ret = orig;
            if (orig.IndexOf(".") > -1)
                ret = string.Format("[{0}]", ret);
            return ret;
        }

        private static string JoinToString(char[] orig)
        {
            string ret = string.Empty;
            if (orig != null && orig.Length > 0)
            {
                foreach (char item in orig)
                    ret = ret + item.ToString();
            }
            return ret;
        }

        public static string filterKeyword(string orig)
        {
            return orig.Replace(" ", "_").Replace("#", "_Sharp").Replace("lock", "_Lock").Replace("[", "_").Replace("]", "_").Replace("class", "_Class").Replace("&", "And").Replace("/", "DIV").Replace(".", "Dot").Replace("operator", "_Operator");
        }

        public static string filterKeyword2(string orig)
        {
            if (orig.IndexOf(" ") > -1)
                return string.Format("[{0}]", orig);
            else if ((new List<string>() { "BEGIN", "END" }).Contains(orig.ToUpper()))
                return string.Format("[{0}]", orig);
            else if (orig.IndexOf("/") > -1)
                return string.Format("[{0}]", orig);
            else if (char.IsDigit(orig, 0))
                return string.Format("[{0}]", orig);
            else
                return orig;
        }

        private static DBFieldAttribute GetDBFieldInfo<T>(string columnName)
        {
            DBFieldAttribute ret = null;

            Type type = typeof(T);

            if (type != null && !string.IsNullOrEmpty(columnName))
            {
                string fName = caseChange(columnName);
                FieldInfo fi = type.GetField(fName, BindingFlags.Instance | BindingFlags.Public);
                if (fi != null)
                {
                    object[] objs = fi.GetCustomAttributes(typeof(DBFieldAttribute), false);
                    ret = (DBFieldAttribute)objs[0];
                }
            }
            return ret;
        }

        public static SqlDbType GetDBFieldType<T>(string columnName)
        {
            SqlDbType ret = SqlDbType.VarChar;
            DBFieldAttribute dbfa = GetDBFieldInfo<T>(columnName);
            if (dbfa != null)
            {
                ret = dbfa.columnType;
            }
            return ret;
        }

        #endregion
    }

    public class GetValueClass
    {
        public object GetValue(SqlDataReader sqlDataReader, int iCol, SqlDbType columnType)
        {
            switch (columnType)
            {
                case SqlDbType.Bit: return GetValue_Bit(sqlDataReader, iCol);
                case SqlDbType.VarChar:
                case SqlDbType.Char:
                case SqlDbType.NVarChar:
                case SqlDbType.NChar:
                                        return GetValue_Str(sqlDataReader, iCol);
                case SqlDbType.TinyInt: return GetValue_Byte(sqlDataReader, iCol);
                case SqlDbType.SmallInt: return GetValue_Int16(sqlDataReader, iCol);
                case SqlDbType.Int: return GetValue_Int32(sqlDataReader, iCol);
                case SqlDbType.BigInt: return GetValue_Int64(sqlDataReader, iCol);
                case SqlDbType.DateTime: return GetValue_DateTime(sqlDataReader, iCol);
                case SqlDbType.Float: return GetValue_Double(sqlDataReader, iCol);
                case SqlDbType.Decimal: return GetValue_Decimal(sqlDataReader, iCol);
                //return GetValue_Char(sqlDataReader, iCol);
            }
            return GetValue_Default(sqlDataReader, iCol);
        }

        public object GetNonDefaultFieldValue(SqlDbType columnType)
        {
            switch (columnType)
            {
                case SqlDbType.Bit: return true;
                case SqlDbType.VarChar:
                case SqlDbType.Char:
                case SqlDbType.NVarChar:
                case SqlDbType.NChar:
                    return string.Empty;
                case SqlDbType.TinyInt: return 1;
                case SqlDbType.SmallInt: return 1;
                case SqlDbType.Int: return 1;
                case SqlDbType.BigInt: return 1L;
                case SqlDbType.DateTime: return DateTime.Now;
                case SqlDbType.Float: return 1f;
                case SqlDbType.Decimal: return 1m;
                //return GetValue_Char(sqlDataReader, iCol);
            }
            return string.Empty;
        }

        #region . GetValues .

        public bool IsNull(SqlDataReader sqlDataReader, int iCol)
        {
            return sqlDataReader.IsDBNull(iCol);
        }

        public bool GetValue_Bit(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetBoolean(iCol);
            else
                return false;
        }

        /// <summary>
        /// 判断空值并赋值(string)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        public string GetValue_Str(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetString(iCol).TrimEnd();
            else
                return string.Empty;
        }

        public string GetValue_StrForNullable(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetString(iCol).TrimEnd();
            else
                return null;
        }

        /// <summary>
        /// 判断空值并赋值(Int32)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        public int GetValue_Int32(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetInt32(iCol);
            else
                return 0;
        }

        /// <summary>
        /// 判断空值并赋值(Int64)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        public long GetValue_Int64(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetInt64(iCol);
            else
                return 0L;
        }

        /// <summary>
        /// 判断空值并赋值(Byte)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        public Byte GetValue_Byte(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetByte(iCol);
            else
                return 0;
        }

        /// <summary>
        /// 判断空值并赋值(Int16)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        public short GetValue_Int16(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetInt16(iCol);
            else
                return 0;
        }

        /// <summary>
        /// 判断空值并赋值(DateTime)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        public DateTime GetValue_DateTime(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetDateTime(iCol);
            else
                return DateTime.MinValue;
        }

        public Guid GetValue_Guid(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetGuid(iCol);
            else
                return Guid.Empty;
        }

        public TimeSpan GetValue_TimeSpan(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetTimeSpan(iCol);
            else
                return TimeSpan.MinValue;
        }

        public DateTimeOffset GetValue_DateTimeOffset(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetDateTimeOffset(iCol);
            else
                return DateTimeOffset.MinValue;
        }

        /// <summary>
        /// 判断空值并赋值(float)
        /// 数据库里的 Float 类型，其实相当于double, DataReader.GetFloat()會報錯
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        public double GetValue_Double(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetDouble(iCol);// 2010-04-09 Liu Dong(eB1-4)         Modify ITC-1122-0241
            else
                return 0.0D;
        }

        ///// <summary>
        ///// 判断空值并赋值(double) 
        ///// 数据库里的 Float 类型，其实相当于double, DataReader.GetFloat()會報錯
        ///// </summary>
        ///// <param name="sqlDataReader"></param>
        ///// <param name="iCol"></param>
        //public double GetValue_Double(SqlDataReader sqlDataReader, int iCol)
        //{
        //    if (! sqlDataReader.IsDBNull(iCol) )
        //        return sqlDataReader.GetDouble(iCol);
        //    else
        //        return 0.0;
        //}

        /// <summary>
        /// 判断空值并赋值(decimal)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        public decimal GetValue_Decimal(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetDecimal(iCol);
            else
                return 0.0M;
        }

        /// <summary>
        /// 判断空值并赋值(decimal), 用-1代表NULL
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        /// <returns></returns>
        public decimal GetValue_DecimalWithMinusForNull(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetDecimal(iCol);
            else
                return -1M;
        }

        /// <summary>
        /// 判断空值并赋值(char)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        /// <returns></returns>
        public char GetValue_Char(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return Convert.ToChar(sqlDataReader.GetValue(iCol));
            else
                return Char.MinValue;
        }

        public object GetValue_Default(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetValue(iCol);
            else
                return null;
        }

        public string GetValue_Str(DataRow dr, string key)
        {
            object obj = dr[key];
            if (obj != null && obj != DBNull.Value)
                return obj.ToString();
            else
                return string.Empty;
        }

        #endregion

        public string DecBeg(string str)
        {
            return str + "_beg";
        }

        public string DecEnd(string str)
        {
            return str + "_end";
        }

        public string DecSV(string str)
        {
            return str + "_sv";
        }

        public string DecG(string str)
        {
            return str + "_G";
        }

        public string DecS(string str)
        {
            return str + "_S";
        }

        public string DecGE(string str)
        {
            return str + "_GE";
        }

        public string DecSE(string str)
        {
            return str + "_SE";
        }

        public string DecAny(string str)
        {
            return str + "_Any";
        }

        public string DecSLPM(string str)
        {
            return str + "_SLPM";
        }

        public string DecInc(string str)
        {
            return str + "_Inc";
        }

        public string DecDec(string str)
        {
            return str + "_Dec";
        }

        public string DecInSet(string str)
        {
            return "INSET[" + str + "]";
        }

        public string DecAlias(string alias, string str)
        {
            return alias + "_" + str;
        }

        public string DecAliasInner(string alias, string str)
        {
            return alias + "." + str;
        }

        public string ConvertInSet(IList<string> set)
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

        public string ConvertInSet(IList<int> set)
        {
            IList<string> setstr = new List<string>();
            foreach (int str in set)
            {
                setstr.Add(str.ToString());
            }
            return string.Join(",", setstr.ToArray());
        }

        public string ConvertInSet(IList<long> set)
        {
            IList<string> setstr = new List<string>();
            foreach (long str in set)
            {
                setstr.Add(str.ToString());
            }
            return string.Join(",", setstr.ToArray());
        }

        public string GetIteratedString(IList<string> set, string template, string replacableStr,string spliter)
        {
            string ret = string.Empty;
            IList<string> cops = new List<string>();
            if (set != null && set.Count > 0)
            {
                foreach(string val in set)
                {
                    cops.Add(template.Replace(replacableStr, val));
                }
                ret = string.Format("({0})",string.Join(spliter, cops.ToArray()));
            }
            return ret;
        }

        public string GetCertainConditionSegment(string sentence, string keyStr)
        {
            string[] conds = sentence.Split(new string[] { " AND " }, StringSplitOptions.None);
            foreach (string cond in conds)
            {
                if (cond.IndexOf(keyStr) > -1)
                {
                    return cond;
                }
            }
            return string.Empty;
        }
    }
}
