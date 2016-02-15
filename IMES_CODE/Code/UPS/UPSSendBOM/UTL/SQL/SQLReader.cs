using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.Collections;


namespace UTL.SQL
{
    public static class SQLReader
    {
        public static T ToObject<T>(this SqlDataReader rs) where T : new()
        {
            T t = new T();
            int count = rs.FieldCount;
            for (int i = 0; i < count; ++i)
            {
                PropertyInfo pInfo = t.GetType().GetProperty(rs.GetName(i));
                if (pInfo != null)
                    if (rs.IsDBNull(i))
                    {
                        pInfo.SetValue(t, null, null);
                    }
                    else
                    {
                        pInfo.SetValue(t, rs[i], null);
                        stringTrim(t,pInfo);
                    }

            }
            return t;
        }

       

        public static T ToObjectWithAttribute<T>(this SqlDataReader rs) where T : new()
        {
            T t = new T();
            //PropertyDescriptorCollection descrInfos = TypeDescriptor.GetProperties(t);
            int count = rs.FieldCount;
            for (int i = 0; i < count; ++i)
            {
                string fieldName = rs.GetName(i);
                PropertyInfo pInfo = t.GetType().GetProperty(fieldName);
                if (pInfo != null)
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
                            stringTrim(t,pInfo);
                        }
                        else
                        {
                            pInfo.SetValue(t, rs[i], null);
                            stringTrim(t,pInfo);
                        }                      
                    }

            }
            return t;
        }

        public static T ToReadOnlyObject<T>(this SqlDataReader rs) where T : new()
        {
            T t = new T();
            int count = rs.FieldCount;
            for (int i = 0; i < count; ++i)
            {
                FieldInfo pInfo = t.GetType().GetField("_" + rs.GetName(i), BindingFlags.Instance | BindingFlags.NonPublic);
                if (pInfo != null)
                    if (rs.IsDBNull(i))
                    {
                        pInfo.SetValue(t, null);
                    }
                    else
                    {
                        pInfo.SetValue(t, rs[i]);
                        stringTrim(t,pInfo);
                    }
            }
            return t;
        }

        public static void ToReadOnlyObject<T>(this SqlDataReader rs, ref T  t)  where T:class
        {            
            int count = rs.FieldCount;
            for (int i = 0; i < count; ++i)
            {
               FieldInfo pInfo = t.GetType().GetField("_"+rs.GetName(i), BindingFlags.Instance | BindingFlags.NonPublic);
                if (pInfo != null)
                    if (rs.IsDBNull(i))
                    {
                        pInfo.SetValue(t, null);
                    }
                    else
                    {
                        pInfo.SetValue(t, rs[i]);
                        stringTrim(t,pInfo);
                    }
            }
           
        }

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
                //無法確定MetadataToken 順序, 確認可以使用
                IList<PropertyInfo> properties = typeof(T).GetProperties().OrderBy(x => x.MetadataToken).ToList();
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
                        catch (Exception ex)
                        {
                            row[prop.Name] = DBNull.Value;
                        }

                    }
                    table.Rows.Add(row);
                }
            }
            return table;            
        }

        public static T ToObjectByField<T>(this SqlDataReader rs) where T : new() 
        {
            T t = new T();
            int count = rs.FieldCount;
            IList<FieldInfo> fields = t.GetType().GetFields();
            for (int i = 0; i < count; ++i)
            {

                //FieldInfo fInfo = t.GetType().GetField(rs.GetName(i), BindingFlags.Public);
                IList<FieldInfo> rsFields = fields.Where(p => p.Name.ToUpper() == rs.GetName(i).ToUpper()).ToList();
                if (rsFields != null && rsFields.Count() > 0)
                    if (rs.IsDBNull(i))
                    {
                        rsFields[0].SetValue(t, null);
                    }
                    else
                    {
                        rsFields[0].SetValue(t, rs[i]);
                        stringTrim(t, rsFields[0]);
                    }

            }
            return t;
        }
        /// <summary>Trim all String properties of the given object</summary>
        public static void TrimStringProperties<T>(this T input)
        {
            var stringProperties = input.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string));

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
            var stringFields = input.GetType().GetFields()
                .Where(p => p.FieldType == typeof(string));

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
   
}
