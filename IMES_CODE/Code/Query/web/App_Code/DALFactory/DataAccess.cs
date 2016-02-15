using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Configuration;
using com.inventec.imes.IDAL;
using com.inventec.imes.SQLServerDAL;

namespace com.inventec.imes.DALFactory
{
    public class DataAccess
    {
        private static readonly string path = ConfigurationManager.AppSettings["WebDAL"];

        private DataAccess() { }

        public static IModelBOM CreateModelBOM()
        {
            string className = path + ".ModelBOM";
            return (com.inventec.imes.IDAL.IModelBOM) Assembly.Load(path).CreateInstance(className);
        }
    }
}
