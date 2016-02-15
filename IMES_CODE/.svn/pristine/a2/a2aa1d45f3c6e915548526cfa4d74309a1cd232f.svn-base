using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace UTL.Reflection
{
    public static class ReflectObj
    {
        public static void CopyObjectByField(Object OriginalObj, Object NewObj)
        {
            object value;
            string name;
            foreach (FieldInfo f in OriginalObj.GetType().GetFields())
            {
                value = f.GetValue(OriginalObj);
                name = f.Name;
                NewObj.GetType().GetField(name).SetValue(NewObj, (object)value);

            }

        }
        private static string GetValueByType(string type, object dataObj)
        {

            FieldInfo fi = dataObj.GetType().GetField(type);
            if (fi == null)
                return "";
            else
                if (fi.GetValue(dataObj) == null)
                { return ""; }
                else
                { return fi.GetValue(dataObj).ToString(); }

        }
        public static void CheckNullData(List<string> checkIem, object dataObj)
        {
            string className = dataObj.GetType().BaseType.Name;
            if (className == "Object")
            { 
                className = dataObj.GetType().Name;
            }
            string title = "These columns of " + className + " are null or no data : ";
            string error = "";
            foreach (string item in checkIem)
            {
                if (string.IsNullOrEmpty(GetValueByType(item, dataObj).Trim()))
                { 
                    error = error + item + ","; 
                }
            }
            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(title + error);
            }
        }
        public static string Object2String<T>(T dataObj) where T:class
        {
            object value;
            string name;
            string result = "";
            foreach (FieldInfo f in dataObj.GetType().GetFields())
            {
                name = f.Name;
                value = GetValueByType(name, dataObj);

                result = result + name + ":" + value + "\r\n";

            }
            return result;
        }
        public static string GenTxnId()
        {
            return string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);
        }

        public static DateTime String2Date(string strDate)
        {
            return DateTime.ParseExact(strDate, "yyyyMMdd", null);
        }

        public static string ObjectProperty2String<T>(T dataObj) where T:class
        {
            object value;
            string name;
            string result = "";
            foreach (PropertyInfo property in dataObj.GetType().GetProperties())
            {
                name = property.Name;
                value = GetPropertyValueByType(property, dataObj);
                result = result + name + ":" + value + "\r\n";

            }
            return result;
        }

        private static string GetPropertyValueByType(PropertyInfo property, object dataObj)
        {

            return property.GetValue(dataObj, null) == null ? "" : property.GetValue(dataObj, null).ToString();

        }
    }
    
}
