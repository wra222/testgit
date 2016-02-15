using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace IMES.Infrastructure.Utility.Generates.cust
{
    public static class DynamicGenerator
    {
        /// <summary>
        /// 獲得一個繼承了IComposer接口的類的實例
        /// </summary>
        /// <param name="assemblyFileName">程序集名稱</param>
        /// <param name="classNameWithNS">類的名稱(帶命名空間)</param>
        /// <param name="ctorParams">構造函數所用參數</param>
        /// <returns>類的實例</returns>
        /// <remarks></remarks>
        public static IConverter GetAComposer(string assemblyFileName, string classNameWithNS, object[] ctorParams)
        {
            try
            {
                Assembly ass = Assembly.Load(assemblyFileName);
                object obj = ass.CreateInstance(classNameWithNS, false, BindingFlags.CreateInstance, null, ctorParams, null, null);
                return (IConverter)obj;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            return null;
        }
    }
}
