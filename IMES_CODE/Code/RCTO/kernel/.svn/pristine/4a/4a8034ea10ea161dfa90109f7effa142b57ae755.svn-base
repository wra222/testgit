using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace IMES.Infrastructure.ExpressionScript
{
    /// <summary>
    /// Get Type Extension Refection     
    /// </summary>
    public static class TypeExtension
    {
        private static readonly List<MethodInfo> extensionMethodList = GetAllExtensionMethods();
        
        private static List<MethodInfo> GetAllExtensionMethods()
        {
            List<Type> AssTypes = new List<Type>();

            foreach (Assembly item in AppDomain.CurrentDomain.GetAssemblies())
            {
                AssTypes.AddRange(item.GetTypes());
            }

            var query = from type in AssTypes
                        where type.IsSealed && !type.IsGenericType && !type.IsNested
                        from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                        where method.IsDefined(typeof(ExtensionAttribute), false)                      
                        select method;
            return query.ToList();
        }

       
       /// <summary>
       /// get all entension method info
       /// </summary>
       /// <param name="t"></param>
       /// <returns></returns>
        public static MethodInfo[] GetExtensionMethods(this Type t)
        {
            return extensionMethodList.Where(x => x.GetParameters()[0].ParameterType == t).ToArray < MethodInfo>();
            #region disable
            //List<Type> AssTypes = new List<Type>();

            //foreach (Assembly item in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    AssTypes.AddRange(item.GetTypes());
            //}

            //var query = from type in AssTypes
            //            where type.IsSealed && !type.IsGenericType && !type.IsNested
            //            from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            //            where method.IsDefined(typeof(ExtensionAttribute), false)
            //            where method.GetParameters()[0].ParameterType == t
            //            select method;
            //return query.ToArray<MethodInfo>();
            #endregion
        }
  
        /// <summary>
        /// Extends the System.Type-type to search for a given extended MethodeName
        /// </summary>
        /// <param name="t"></param>
        /// <param name="MethodeName">Name of the Methode</param>
        /// <returns>the found Methode or null</returns>
        public static MethodInfo GetExtensionMethod(this Type t, string MethodeName)
        {
            var mi = from methode in t.GetExtensionMethods()
                     where methode.Name == MethodeName
                     select methode;
            if (mi.Count<MethodInfo>() <= 0)
                return null;
            else
                return mi.First<MethodInfo>();
        }
    }
}
