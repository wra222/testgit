using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace IMES.Infrastructure.ExpressionScript
{
    public static class FISObjectExpression
    {
        internal static class ConstName
        {
            public static readonly string PRODUCT = "PRODUCT";
            public static readonly string IPRODUCT = "IPRODUCT";
            public static readonly string PRODUCTINFO = "PRODUCTINFO";
            public static readonly string PRODUCTATTR = "PRODUCTATTR";

            public static readonly string PCB = "PCB";
            public static readonly string IMB = "IMB";
            public static readonly string PCBINFO = "PCBINFO";
            public static readonly string PCBATTR = "PCBATTR";

            public static readonly string FAMILY = "FAMILY";
            public static readonly string FAMILYINFO = "FAMILYINFO";

            public static readonly string MODEL = "MODEL";
            public static readonly string MODELINFO = "MODELINFO";

            public static readonly string PART = "PART";
            public static readonly string IPART = "IPART";
            public static readonly string PARTINFO = "PARTINFO";

            public static readonly string DELIVERY = "DELIVERY";
            public static readonly string DELIVERYINFO = "DELIVERYINFO";

            //for HTC add ImgOSVer
            public static readonly string IMGOSVER = "IMGOSVER";

            public static readonly string SESSION = "SESSION";

            public static readonly string DATETIME = "DATETIME";
            public static readonly string CONSTANT = "CONSTANT";

            public static readonly string TESTLOG = "TESTLOG";

            public static readonly string BOM = "[Bb][Oo][Mm]@(?<Level>.*)~(?<Method>.*)";

            //for get Method name
            public static readonly string GetExtendedProperty = "GetExtendedProperty";
            public static readonly string GetAttributeValue = "GetAttributeValue";
            public static readonly string GetAttribute = "GetAttribute";
        }

        readonly static IDictionary<string, string> MapMethodNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {ConstName.PRODUCT, ConstName.GetAttribute},
             {ConstName.IPRODUCT, ConstName.GetAttribute},
            //{ConstName.PRODUCTINFO, ConstName.GetExtendedProperty},
            //{ConstName.PRODUCTATTR, ConstName.GetAttributeValue},
            {ConstName.PCB, ConstName.GetAttribute},
             {ConstName.IMB, ConstName.GetAttribute},
             //{ConstName.PCBINFO, ConstName.GetExtendedProperty},
            //{ConstName.PCBATTR, ConstName.GetAttributeValue},
             {ConstName.MODEL, ConstName.GetAttribute},
             //{ConstName.MODELINFO, ConstName.GetAttribute},
             {ConstName.FAMILY, ConstName.GetAttribute},
            //{ConstName.FAMILYINFO, ConstName.GetAttribute},
            {ConstName.DELIVERY, ConstName.GetExtendedProperty},
            //{ConstName.DELIVERYINFO, ConstName.GetExtendedProperty},
            {ConstName.PART, ConstName.GetAttribute},
             {ConstName.IPART, ConstName.GetAttribute}
            //{ConstName.PARTINFO, ConstName.GetAttribute}
        };

        readonly static object syncObject = new object();

        readonly static IDictionary<string, PropertyInfo[]> PropertyInfoList = new Dictionary<string, PropertyInfo[]>();

        readonly static IDictionary<string, ScriptInfo> ParserInfoList = new Dictionary<string, ScriptInfo>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// iMES special property example:ProductInfo.XXXX, ModelInfo.XXXX....
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
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
        /// <returns></returns>
        public static Delegate CompileWithCache(ScriptTypeEnum scriptType, string name, string code, params ParameterExpression[] parameters)
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
        /// arguement sequence:Product,PCB,Delivery,Part,Model,Family,TestLog,BOM
        /// </summary>
        /// <param name="scriptType"></param>
        /// <param name="code"></param>
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
                retType = typeof(string);
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
