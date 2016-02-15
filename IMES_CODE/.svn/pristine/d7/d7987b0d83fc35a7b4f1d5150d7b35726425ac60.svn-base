using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


namespace IMES.Infrastructure.Utility
{
    /// <summary>
    /// 对象拷贝工具类
    /// </summary>
    public static class CloneTool
    {
        /// <summary>
        /// 深拷贝一个对象实例
        /// 局限性: 至少 IDictionary 的不能被深拷贝.
        /// </summary>
        /// <param name="obj">被拷贝的对象</param>
        /// <returns>拷贝出来的对象</returns>
        public static object DoDeepCopyObj(object obj)
        {
            object ret = null;

            if (obj != null)
                ret = DoDeepCopyObj_Inner(obj, obj.GetType());

            return ret;

            #region .OLD.
            //Object DeepCopyObj;

            //if (obj == null || obj.GetType().IsValueType)//值类型
            //{
            //    DeepCopyObj = obj;
            //}
            //else if (obj.GetType() == typeof(string))//字符串类型
            //{
            //    DeepCopyObj = string.Empty;
            //    DeepCopyObj = string.Copy((string)obj);
            //}
            //else//引用类型
            //{
            //    DeepCopyObj = System.Activator.CreateInstance(obj.GetType(), null);//创建引用对象

            //    System.Reflection.FieldInfo[] fieldCollection = obj.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            //    foreach (System.Reflection.FieldInfo field in fieldCollection)
            //    {
            //        if (field.FieldType.GetInterface(typeof(IDictionary).Name) != null)
            //            continue;
            //        Object fieldValue = field.GetValue(obj);
            //        if (field.FieldType.IsArray)
            //        {
            //            Array vals = (Array)fieldValue;
            //            ArrayList arrfld = new ArrayList(vals.Length);
            //            Type itemType = null; 
            //            string arrName = field.FieldType.AssemblyQualifiedName.Replace("[]", "");
            //            itemType = Type.GetType(arrName);
            //            for (int i = 0; i < vals.Length; i++)
            //            {
            //                object oo = DoDeepCopyObj(vals.GetValue(i));
            //                arrfld.Add(oo);
            //            }
            //            if (itemType != null)
            //                field.SetValue(DeepCopyObj, arrfld.ToArray(itemType));
            //            else
            //                field.SetValue(DeepCopyObj, arrfld.ToArray());
            //        }
            //        else
            //        {
            //            field.SetValue(DeepCopyObj, DoDeepCopyObj(fieldValue));
            //        }
            //    }
            //}
            //return DeepCopyObj;
            #endregion
        }

        private static object DoDeepCopyObj_Inner(object obj, Type type)
        {
            Object DeepCopyObj;

            if (obj == null || type.IsValueType)//值类型
            {
                DeepCopyObj = obj;
            }
            else if (type == typeof(string))//字符串类型
            {
                DeepCopyObj = string.Empty;
                DeepCopyObj = string.Copy((string)obj);
            }
            //else if (type.IsArray)
            //{
            //    Array vals = (Array)obj;
            //    ArrayList arrfld = new ArrayList(vals.Length);
            //    Type itemType = null;
            //    string arrName = type.AssemblyQualifiedName.Replace("[]", "");
            //    itemType = Type.GetType(arrName);
            //    for (int i = 0; i < vals.Length; i++)
            //    {
            //        object oo = DoDeepCopyObj(vals.GetValue(i));
            //        arrfld.Add(oo);
            //    }
            //    if (itemType != null)
            //        DeepCopyObj = arrfld.ToArray(itemType);
            //    else
            //        DeepCopyObj = arrfld.ToArray();
            //}
            else//引用类型
            {
                DeepCopyObj = System.Activator.CreateInstance(type, null);//创建引用对象

                System.Reflection.FieldInfo[] fieldCollection = GetAllFields(type);
                
                foreach (System.Reflection.FieldInfo field in fieldCollection)
                {
                    if (field.FieldType.GetInterface(typeof(IDictionary).Name) != null)
                        continue;

                    Object fieldValue = field.GetValue(obj);

                    if (field.FieldType.IsArray)
                    {
                        Array vals = (Array)fieldValue;
                        ArrayList arrfld = new ArrayList(vals.Length);
                        Type itemType = null; 
                        string arrName = field.FieldType.AssemblyQualifiedName.Replace("[]", "");
                        itemType = Type.GetType(arrName);
                        for (int i = 0; i < vals.Length; i++)
                        {
                            object oo = DoDeepCopyObj(vals.GetValue(i));
                            arrfld.Add(oo);
                        }
                        if (itemType != null)
                            field.SetValue(DeepCopyObj, arrfld.ToArray(itemType));
                        else
                            field.SetValue(DeepCopyObj, arrfld.ToArray());
                    }
                    else
                    {
                        if (fieldValue != null)
                            field.SetValue(DeepCopyObj, DoDeepCopyObj_Inner(fieldValue, fieldValue.GetType()));
                        else
                            field.SetValue(DeepCopyObj, DoDeepCopyObj_Inner(fieldValue, field.FieldType));
                    }
                }
            }
            return DeepCopyObj;
        }

        private static System.Reflection.FieldInfo[] GetAllFields(Type type)
        {
            IList<System.Reflection.FieldInfo> ret = new List<System.Reflection.FieldInfo>();
            Type curr = type;
            do
            {
                System.Reflection.FieldInfo[] fields = curr.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                if (fields != null && fields.Length > 0)
                {
                    foreach (System.Reflection.FieldInfo item in fields)
                    {
                        ret.Add(item);
                    }
                }
                curr = curr.BaseType;
            }
            while (curr != null);
            return ret.ToArray();
        }
    }
}
