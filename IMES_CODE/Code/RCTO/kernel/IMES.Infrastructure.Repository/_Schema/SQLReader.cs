using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.Collections;
using META=IMES.Infrastructure.Repository._Metas;

namespace IMES.Infrastructure.Repository._Schema
{
    /// <summary>
    /// Cache PropertyInfo and FiledInfo reflection 
    /// </summary>
    public sealed class SQLDataCache
    {
        private static readonly IDictionary<string, PropertyInfo[]> _propertyCache = new Dictionary<string, PropertyInfo[]>();
        private static readonly IDictionary<string, FieldInfo[]> _fieldCache = new Dictionary<string, FieldInfo[]>();

        private static object _propertycacheSyncObj = new object();
        private static object _fieldcacheSyncObj = new object();
        private const string MaxInt32Digital="D10";
        /// <summary>
        /// get propertyInfo in cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertyInfos<T>()
        {
            string name = typeof(T).FullName;
            lock (_propertycacheSyncObj)
            {
                if (_propertyCache.ContainsKey(name))
                {
                    return _propertyCache[name];
                }
                else
                {
                    PropertyInfo[] infos = typeof(T).GetProperties();
                    _propertyCache.Add(name, infos);
                    return infos;
                }
            }
        }

         /// <summary>
        /// get FielInfo in Cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static FieldInfo[] GetFieldInfos<T>(BindingFlags bindingFlags)
        {
            string name = typeof(T).FullName + ((int)bindingFlags).ToString(MaxInt32Digital);
            lock (_fieldcacheSyncObj)
            {
                if (_fieldCache.ContainsKey(name))
                {
                    return _fieldCache[name];
                }
                else
                {
                    //FieldInfo[] infos = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    FieldInfo[] infos = typeof(T).GetFields(bindingFlags);
                    _fieldCache.Add(name, infos);
                    return infos;
                }
            }
        }
    }
    
    /// <summary>
    /// SQLData to object utility
    /// </summary>
    public static class SQLData
    {
        /// <summary>
        /// SqlDataReader to defined class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static T ToObject<T>(this SqlDataReader rs) where T : new()
        {
           PropertyInfo[] pInfos= SQLDataCache.GetPropertyInfos<T>();

            T t = new T();
            int count = rs.FieldCount;
            for (int i = 0; i < count; ++i)
            {
                //PropertyInfo pInfo = t.GetType().GetProperty(rs.GetName(i));
                PropertyInfo pInfo = pInfos.Where(x => x.Name == rs.GetName(i)).FirstOrDefault();
                if (pInfo != null)
                {
                    if (rs.IsDBNull(i))
                    {
                        pInfo.SetValue(t, null, null);
                    }
                    else
                    {
                        pInfo.SetValue(t, rs[i], null);
                        stringTrim(t, pInfo);
                    }
                }
            }
            return t;
        }

       
        /// <summary>
        /// convert type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static T ToObjectWithAttribute<T>(this SqlDataReader rs) where T : new()
        {
            PropertyInfo[] pInfos = SQLDataCache.GetPropertyInfos<T>();

            T t = new T();
            //PropertyDescriptorCollection descrInfos = TypeDescriptor.GetProperties(t);
            int count = rs.FieldCount;
            for (int i = 0; i < count; ++i)
            {
                string fieldName = rs.GetName(i);
                //PropertyInfo pInfo = t.GetType().GetProperty(fieldName);
                PropertyInfo pInfo = pInfos.Where(x => x.Name == fieldName).FirstOrDefault();
                if (pInfo != null)
                {
                    if (rs.IsDBNull(i))
                    {
                        pInfo.SetValue(t, null, null);
                    }
                    else
                    {

                        //var descr = descrInfos[fieldName];
                        //TypeConverter converter = descr.Converter;                       
                        TypeConverter converter = TypeDescriptor.GetConverter(pInfo.PropertyType);
                        if (converter.CanConvertFrom(rs[i].GetType()))
                        {
                            pInfo.SetValue(t, converter.ConvertFrom(rs[i]), null);
                            stringTrim(t, pInfo);
                        }
                        else
                        {
                            pInfo.SetValue(t, rs[i], null);
                            stringTrim(t, pInfo);
                        }
                    }
                }
            }
            return t;
        }

        /// <summary>
        ///  access private field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static T ToReadOnlyObject<T>(this SqlDataReader rs) where T : new()
        {
            FieldInfo[] pInfos = SQLDataCache.GetFieldInfos<T>(BindingFlags.Instance | BindingFlags.NonPublic);

            T t = new T();
            int count = rs.FieldCount;
            string fieldName = null;
            for (int i = 0; i < count; ++i)
            {
                fieldName = "_" + rs.GetName(i);
               // FieldInfo pInfo = t.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
                FieldInfo pInfo = pInfos.Where(x => x.Name == fieldName).FirstOrDefault();
                if (pInfo != null)
                {
                    if (rs.IsDBNull(i))
                    {
                        pInfo.SetValue(t, null);
                    }
                    else
                    {
                        pInfo.SetValue(t, rs[i]);
                        stringTrim(t, pInfo);
                    }
                }
            }
            return t;
        }

        /// <summary>
        /// access private field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rs"></param>
        /// <param name="t"></param>
        public static void ToReadOnlyObject<T>(this SqlDataReader rs, ref T  t)  where T:class
        {
            FieldInfo[] pInfos = SQLDataCache.GetFieldInfos<T>(BindingFlags.Instance | BindingFlags.NonPublic);
            int count = rs.FieldCount;
            string fieldName = null;
            for (int i = 0; i < count; ++i)
            {
                fieldName = "_" + rs.GetName(i);
               //FieldInfo pInfo = t.GetType().GetField("_"+rs.GetName(i), BindingFlags.Instance | BindingFlags.NonPublic);
                FieldInfo pInfo = pInfos.Where(x => x.Name == fieldName).FirstOrDefault();
                if (pInfo != null)
                {
                    if (rs.IsDBNull(i))
                    {
                        pInfo.SetValue(t, null);
                    }
                    else
                    {
                        pInfo.SetValue(t, rs[i]);
                        stringTrim(t, pInfo);
                    }
                }
            }
           
        }

        /// <summary>
        /// Defined class to DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            
            DataTable table = new DataTable();
           //special handling for value types and string
            if (typeof(T).IsValueType || typeof(T).Equals(typeof(string)))
            {

                DataColumn dc = new DataColumn("data");

                table.Columns.Add(dc);

                foreach (T item in data)
                {
                    DataRow dr = table.NewRow();
                    dr[0] = item;

                    table.Rows.Add(dr);
                }
            }
            else
            {
                PropertyInfo[] pInfos = SQLDataCache.GetPropertyInfos<T>();
                //無法確定MetadataToken 順序, 確認可以使用
                //IList<PropertyInfo> properties = typeof(T).GetProperties().OrderBy(x => x.MetadataToken).ToList();
                IList<PropertyInfo> properties = pInfos.OrderBy(x => x.MetadataToken).ToList();
                //IList<PropertyInfo> properties = typeof(T).GetProperties().OrderBy(p => ((META.OrderAttribute)p.GetCustomAttributes(typeof(META.OrderAttribute), false)[0]).Order).ToList();

                foreach (PropertyInfo prop in properties)
                {
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }

                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyInfo prop in properties)
                    {
                        try
                        {
                            row[prop.Name] = prop.GetValue(item,null) ?? DBNull.Value;
                        }
                        catch (Exception)
                        {
                            row[prop.Name] = DBNull.Value;
                        }

                    }
                    table.Rows.Add(row);
                }
            }
            return table;            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static T ToObjectByField<T>(this SqlDataReader rs) where T : new()
        {
            FieldInfo[] pInfos = SQLDataCache.GetFieldInfos<T>(BindingFlags.Instance | BindingFlags.Public);
            T t = new T();
            int count = rs.FieldCount;
            //IList<FieldInfo> fields = t.GetType().GetFields();
            //IList<FieldInfo> fields = pInfos.ToList();
            for (int i = 0; i < count; ++i)
            {
                //FieldInfo fInfo = t.GetType().GetField(rs.GetName(i), BindingFlags.Public);
                FieldInfo rsField = pInfos.Where(p => p.Name.ToUpper() == rs.GetName(i).ToUpper()).FirstOrDefault();
                if (rsField != null)
                {
                    if (rs.IsDBNull(i))
                    {
                        rsField.SetValue(t, null);
                    }
                    else
                    {
                        rsField.SetValue(t, rs[i]);
                        stringTrim(t, rsField);
                    }
                }

            }
            return t;
        }

        /// <summary>
        /// Trim all String properties of the given object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>       
        public static void TrimStringProperties<T>(this T input)
        {
            PropertyInfo[] pInfos = SQLDataCache.GetPropertyInfos<T>();
            var stringProperties = pInfos.Where(p => p.PropertyType == typeof(string));
            //var stringProperties = input.GetType().GetProperties()
            //    .Where(p => p.PropertyType == typeof(string));

            foreach (var stringProperty in stringProperties)
            {
                string currentValue = (string)stringProperty.GetValue(input, null);
                if (currentValue != null)
                    stringProperty.SetValue(input, currentValue.Trim(), null);
            }
            //return input;
        }
        /// <summary>
        ///  Field trim data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        public static void TrimStringFields<T>(this T input)
        {
            FieldInfo[] pInfos = SQLDataCache.GetFieldInfos<T>(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var stringFields = pInfos.Where(p => p.FieldType == typeof(string));
            //var stringFields = input.GetType().GetFields()
            //    .Where(p => p.FieldType == typeof(string));

            foreach (var stringField in stringFields)
            {
                string currentValue = (string)stringField.GetValue(input);
                if (currentValue != null)
                    stringField.SetValue(input, currentValue.Trim());
            }
            //return input;
        }

        #region private function
        private static void stringTrim<T>(T t, PropertyInfo pInfo)
        {
            if (pInfo.PropertyType == typeof(string))
            {
                string currentValue = (string)pInfo.GetValue(t, null);
                if (currentValue != null)
                    pInfo.SetValue(t, currentValue.Trim(), null);
            }
        }

        private static void stringTrim<T>(T t, FieldInfo fInfo)
        {
            if (fInfo.FieldType == typeof(string))
            {
                string currentValue = (string)fInfo.GetValue(t);
                if (currentValue != null)
                    fInfo.SetValue(t, currentValue.Trim());
            }
        }
        #endregion

    }


    public static class BuildDBTableType
    {
        #region create DB Table Type for TVP
        public static DataTable CreateProductPlanTb()
            {
                System.Data.DataTable productPlan = new System.Data.DataTable("TbProductPlan");
                productPlan.Columns.Add("PdLine", typeof(string));
                productPlan.Columns.Add("ShipData", typeof(DateTime));
                productPlan.Columns.Add("Family", typeof(string));
                productPlan.Columns.Add("Model", typeof(string));
                productPlan.Columns.Add("PlanQty", typeof(int));
                productPlan.Columns.Add("AddPrintQty", typeof(int));
                productPlan.Columns.Add("PrePrintQty", typeof(int));
                productPlan.Columns.Add("Editor", typeof(string));
                productPlan.Columns.Add("Udt", typeof(DateTime));
                productPlan.Columns.Add("Udt", typeof(DateTime));
                productPlan.Columns.Add("Pass", typeof(string));
                return productPlan;
            }

            public static DataTable CreateStringListTb()
            {
                System.Data.DataTable list = new System.Data.DataTable("TbStringList");
                list.Columns.Add("data", typeof(string));

                return list;
            }

            public static DataTable CreateIntListTb()
            {
                System.Data.DataTable list = new System.Data.DataTable("TbIntList");
                list.Columns.Add("data", typeof(int));

                return list;
            }

            public static DataTable CreateProductStatusTb()
            {
                System.Data.DataTable status = new System.Data.DataTable("TbProductStatus");
                status.Columns.Add("ProductID", typeof(string));
                status.Columns.Add("Station", typeof(string));
                status.Columns.Add("Status", typeof(int));
                status.Columns.Add("ReworkCode", typeof(string));
                status.Columns.Add("Line", typeof(string));
                status.Columns.Add("TestFailCount", typeof(int));
                status.Columns.Add("Editor", typeof(string));
                status.Columns.Add("Udt", typeof(DateTime));
                return status;
            }
        #endregion

       
    }

    //public class DeclarationOrderComparator : IComparer
    //{
    //    int IComparer.Compare(Object x, Object y)
    //    {
    //        PropertyInfo first = x as PropertyInfo;
    //        PropertyInfo second = y as PropertyInfo;
    //        if (first.MetadataToken < second.MetadataToken)
    //            return -1;
    //        else if (first.MetadataToken > second.MetadataToken)
    //            return 1;

    //        return 0;
    //    }
    //}

   
}
