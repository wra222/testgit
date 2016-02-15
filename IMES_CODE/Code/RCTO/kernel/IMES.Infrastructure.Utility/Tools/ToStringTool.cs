//2010-04-19 Liu Dong(eB1-4)         Modify ITC-1122-0249

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace IMES.Infrastructure.Utility
{
    /// <summary>
    /// 对象字符串化类
    /// </summary>
    public static class ToStringTool
    {
        /// <summary>
        /// 将Object转换成字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToString(object obj)
        {
            if (null == obj)
                return "<NULL>";

            if (obj is ICollection)
            {
                if (((ICollection)obj).Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("{");
                    foreach (object obj_item in (ICollection)obj)
                    {
                        sb.Append(ToString(obj_item));
                        sb.Append(", ");
                    }
                    if (sb.ToString().EndsWith(", "))
                        sb.Remove(sb.Length - 2, 2);
                    sb.Append("}");
                    return sb.ToString();
                }
                else
                    return "<EMPTY COLLECTION>";
            }
            else if (obj.GetType().IsEnum)
                return Enum.GetName(obj.GetType(), obj);
            else if (obj is string || obj is DateTime)
                return "'" + obj.ToString() + "'";
            else if (obj.GetType().GetMethod("ToString",new Type[]{}) != null)
                return obj.ToString();
            else
                return ObjFieldsToString(obj);

            //return obj.ToString();
        }

        /// <summary>
        /// 将实体的属性字段拼成串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static string ObjFieldsToString(object obj)
        {
            PropertyInfo[] props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (props != null && props.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                foreach (PropertyInfo prop in props)
                {
                    sb.Append(prop.Name);
                    sb.Append("=");
                    sb.Append(ToString(prop.GetValue(obj, null)));
                    sb.Append(", ");
                }
                if (sb.ToString().EndsWith(", "))
                    sb.Remove(sb.Length - 2, 2);
                sb.Append("]");
                return sb.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 字符串是否符合SQL通配符表达式(LIKE)
        /// </summary>
        /// <param name="sqlWildcardStr">SQL通配符表达式</param>
        /// <param name="strToCmp">要检查的字符串</param>
        /// <returns>LIKE与否</returns>
        public static bool IsLike(string sqlWildcardStr, string strToCmp)
        {
            Regex regex = new Regex(string.Format("^{0}$", sqlWildcardStr.Replace("%", ".*").Replace("_", ".")));//2010-04-19 Liu Dong(eB1-4)         Modify ITC-1122-0249
            return regex.IsMatch(strToCmp);
        }
    }
}
