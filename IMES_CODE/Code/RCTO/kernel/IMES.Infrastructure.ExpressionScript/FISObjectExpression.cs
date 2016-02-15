using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace IMES.Infrastructure.ExpressionScript
{
    /// <summary>
    /// 
    /// </summary>
    public static class FISObjectExpression
    {
        internal static class ConstName
        {
            public const string Product = "Product";
            public const string IProduct = "IProduct";
            public const string ProductInfo = "ProductInfo";
            public const string ProductAttr = "ProductAttr";

            public const string MB = "MB";
            public const string IMB = "IMB";
            public const string PCBInfo = "PCBInfo";
            public const string PCBAttr = "PCBAttr";

            public const string Family = "Family";
            public const string FamilyInfo = "FamilyInfo";

            public const string Model = "Model";
            public const string ModelInfo = "Modelnfo";

            public const string Part = "Part";
            public const string IPart = "IPart";
            public const string PartInfo = "PartInfo";

            public const string Delivery = "Delivery";
            public const string DeliveryInfo = "DeliveryInfo";

            //for HTC add ImgOSVer
            public const string IMGOSVER = "IMGOSVER";

            public const string SESSION = "SESSION";

            public const string DATETIME = "DATETIME";
            public const string CONSTANT = "CONSTANT";

            public const string TestLog = "TestLog";

            public const string BOM = "[Bb][Oo][Mm]@(?<Level>.*)~(?<Method>.*)";

            //for get Method name
            public const string GetExtendedProperty = "GetExtendedProperty";
            public const string GetAttributeValue = "GetAttributeValue";
            public const string GetAttribute = "GetAttribute";
        }

        readonly static IDictionary<string, string> MapMethodNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {ConstName.Product, ConstName.GetAttribute},
             {ConstName.IProduct, ConstName.GetAttribute},
            //{ConstName.PRODUCTINFO, ConstName.GetExtendedProperty},
            //{ConstName.PRODUCTATTR, ConstName.GetAttributeValue},
            {ConstName.MB, ConstName.GetAttribute},
             {ConstName.IMB, ConstName.GetAttribute},
             //{ConstName.PCBINFO, ConstName.GetExtendedProperty},
            //{ConstName.PCBATTR, ConstName.GetAttributeValue},
             {ConstName.Model, ConstName.GetAttribute},
             //{ConstName.MODELINFO, ConstName.GetAttribute},
             {ConstName.Family, ConstName.GetAttribute},
            //{ConstName.FAMILYINFO, ConstName.GetAttribute},
            {ConstName.Delivery, ConstName.GetExtendedProperty},
            //{ConstName.DELIVERYINFO, ConstName.GetExtendedProperty},
            {ConstName.Part, ConstName.GetAttribute},
             {ConstName.IPart, ConstName.GetAttribute}
            //{ConstName.PARTINFO, ConstName.GetAttribute}
        };

        readonly static object syncObject = new object();

        readonly static IDictionary<string, PropertyInfo[]> PropertyInfoList = new Dictionary<string, PropertyInfo[]>();

        readonly static IDictionary<string, ScriptInfo> ParserInfoList = new Dictionary<string, ScriptInfo>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// iMES special property example:ProductInfo.XXXX, ModelInfo.XXXX....
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <param name="methodName"></param>
        /// <param name="methodArgs"></param>
        /// <returns></returns>
        public static bool FISProperty2Method(Type type, string propertyName, 
                                                                 out string methodName, out Expression[] methodArgs)
        {
            methodName = null;
            methodArgs = null;
            if (type == null) return false;
            string name = type.Name;
                
            if (MapMethodNames.ContainsKey(name))
            {
                if (GetProperty(type).Any(x => x.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }
                else
                {
                    ConstantExpression expr = Expression.Constant(propertyName);  //input property Name method agrs
                    methodArgs = new Expression[] { expr };
                    methodName = MapMethodNames[name];
                    return true;
                }
            }
         
            return false;
        }

       /// <summary>
        /// Compile ok store into cache,
        /// Arguement sequence:IProduct,IMB, Delivery,Part,Model,Family,TestLog,BOM
       /// </summary>
       /// <param name="scriptType"></param>
       /// <param name="name"></param>
       /// <param name="code"></param>
       /// <param name="parameters"></param>
       /// <returns></returns>
        public static Delegate CompileWithCache(ScriptTypeEnum scriptType, 
                                                                        string name, 
                                                                        string code, 
                                                                         params ParameterExpression[] parameters)
        {
            lock (syncObject)
            {
                if (ParserInfoList.ContainsKey(name))
                {
                    ScriptInfo info = ParserInfoList[name];
                    if (info.ScriptType == scriptType && info.Code.Equals(code))
                    {
                        return info.CompileFunction;
                    }
                }
                var lambda = Compile(scriptType, code, parameters);
                ParserInfoList[name] = new ScriptInfo
                {
                    ScriptType = ScriptTypeEnum.Condition,
                    Code = code,
                    CompileFunction = lambda,
                };
                return lambda;
            }
        }
     /// <summary>
     ///  arguement sequence:Product,PCB,Delivery,Part,Model,Family,TestLog,BOM
     /// </summary>
     /// <param name="scriptType"></param>
     /// <param name="code"></param>
     /// <param name="parameters"></param>
     /// <returns></returns>
        public static Delegate Compile(ScriptTypeEnum scriptType, string code, params ParameterExpression[] parameters)
        {
            Type retType = null;

            if (scriptType == ScriptTypeEnum.Condition)
            {
                retType = typeof(bool);
            }
            else
            {
                retType = null; //typeof(string);
            }
            var lambdaExpr = DynamicExpression.ParseLambda(parameters, retType, code);
            var lambda = lambdaExpr.Compile();

            return lambda;
        }

        static PropertyInfo[] GetProperty(Type type)
        {
            lock (syncObject)
            {
                string typeName = type.FullName;
                if (PropertyInfoList.ContainsKey(typeName))
                {
                    return PropertyInfoList[typeName];
                }
                else
                {
                    PropertyInfo[] infos = type.GetProperties();
                    PropertyInfoList.Add(typeName, infos);
                    return infos;
                }
            }
        }

        /// <summary>
        ///  return type
        /// </summary>
        public enum ScriptTypeEnum
        {
            /// <summary>
            /// return true/false
            /// </summary>
            Condition = 1, 
            /// <summary>
            /// return string
            /// </summary>
            GetValue      
        }
        internal class ScriptInfo
        {
            public ScriptTypeEnum ScriptType;
            public string Code;
            public Delegate CompileFunction;
        }
    }
}
