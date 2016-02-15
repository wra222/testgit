using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.ExpressionScript;
using IMES.FisObject.FA.Product;
using System.Linq.Expressions;
using IMES.Resolve.Common;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.Part;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.TestLog;

namespace IMES.Resolve.Common
{
    /// <summary>
    /// invoke expression script code
    /// </summary>
    public sealed class ResolveExpression
    {
        readonly static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        readonly static IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
        readonly static IFamilyRepository familyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();

        static class LocalConstName
        {
            //Parameter Type
            public const string Product = "Product";
            public const string MB = "MB";
            public const string TestLog = "TestLog";

            //Expression Parameter Name
            public const string ProductParam = "Product";
            public const string PCBParam = "PCB";
            public const string TestLogParam = "TestLog";
            public const string ModelParam = "Model";
            public const string FamilyParam = "Family";
            public const string DeliveryParam = "Delivery";
            public const string PartParam = "Part";
            public const string BomParam = "BOM";

            //MethodName
            public const string InvokeConditionMethod = "InvokeCondition";
            public const string GetValueMethod = "GetValue";

            //ErrorMessage
            public const string MsgConditionCode = "Parameter Type:{0} {1} Expression Script Name:{2} IsCache:{3} Code:\n{4}"; 
        }
        /// <summary>
        /// invoke condition code
        /// </summary>
        /// <param name="product"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="isCached"></param>
        /// <returns></returns>
        public static bool InvokeCondition(IProduct product,string name, string code, bool isCached)
        {            
            try
            {
                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat(LocalConstName.MsgConditionCode, LocalConstName.Product, FISObjectExpression.ScriptTypeEnum.Condition.ToString(),
                                                                                 name, isCached.ToString(), code);
                }
                var prod = Expression.Parameter(typeof(IProduct), LocalConstName.ProductParam);
                var model = Expression.Parameter(typeof(Model), LocalConstName.ModelParam);
                var family = Expression.Parameter(typeof(Family), LocalConstName.FamilyParam);
                //var mb = Expression.Parameter(typeof(IMB), LocalConstName.MBParam);
                //var dn = Expression.Parameter(typeof(Delivery), LocalConstName.DeliveryParam);
                //var part = Expression.Parameter(typeof(IPart), LocalConstName.PartParam);
                var bom = Expression.Parameter(typeof(ResolveValue), LocalConstName.BomParam);

                Delegate result = null;

                if (isCached)
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        throw new Exception("Expression Script is cached mode need provide script name!");
                    }
                    //not allow parameters contains more than four elements
                    result = FISObjectExpression.CompileWithCache(FISObjectExpression.ScriptTypeEnum.Condition, name, code, 
                                                                                              prod, model, 
                                                                                              family,bom);
                }
                else
                {
                    result = FISObjectExpression.Compile(FISObjectExpression.ScriptTypeEnum.Condition, code, 
                                                                            prod, model, 
                                                                            family,bom);
                }

                //Delivery Dn = null;
                //if (!string.IsNullOrEmpty(product.DeliveryNo))
                //{
                //    Dn = dnRep.Find(product.DeliveryNo);
                //}
                return (bool)result.DynamicInvoke(product, product.ModelObj, 
                                                                    product.FamilyObj, null);
            }
            catch (Exception ex)
            {
                logger.Error(LocalConstName.InvokeConditionMethod, ex);
                throw;
            }
        }
        /// <summary>
        /// invoke condition code
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="isCached"></param>
        /// <returns></returns>
        public static bool InvokeCondition(IMB mb, string name, string code, bool isCached)
        {
            try
            {
                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat(LocalConstName.MsgConditionCode, LocalConstName.MB, FISObjectExpression.ScriptTypeEnum.Condition.ToString(),
                                                                                 name, isCached.ToString(), code);
                }
                var family = Expression.Parameter(typeof(Family), LocalConstName.FamilyParam);
                var pcb = Expression.Parameter(typeof(IMB), LocalConstName.PCBParam);               
                var part = Expression.Parameter(typeof(IPart), LocalConstName.PartParam);
                
               
                Delegate result = null;

                if (isCached)
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        throw new Exception("Expression Script is cached mode need provide script name!");
                    }
                    result = FISObjectExpression.CompileWithCache(FISObjectExpression.ScriptTypeEnum.Condition, name, code, pcb, part, family);
                }
                else
                {
                    result = FISObjectExpression.Compile(FISObjectExpression.ScriptTypeEnum.Condition, code, pcb, part, family);
                }

                Family familyObj = null;
                if (!string.IsNullOrEmpty(mb.Family))
                {
                    familyObj = familyRep.Find(mb.Family);
                }
                return (bool)result.DynamicInvoke(mb, mb.Part, familyObj);
            }
            catch (Exception ex)
            {
                logger.Error(LocalConstName.InvokeConditionMethod, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="part"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="isCached"></param>
        /// <returns></returns>
        public static bool InvokeCondition(IProduct product, IPart part, string name, string code, bool isCached)
        {
            try
            {
                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat(LocalConstName.MsgConditionCode, LocalConstName.PartParam, FISObjectExpression.ScriptTypeEnum.Condition.ToString(),
                                                                                 name, isCached.ToString(), code);
                }

                var prod = Expression.Parameter(typeof(IProduct), LocalConstName.ProductParam);
                var model = Expression.Parameter(typeof(Model), LocalConstName.ModelParam);
                var family = Expression.Parameter(typeof(Family), LocalConstName.FamilyParam);
                var partPara = Expression.Parameter(typeof(IPart), LocalConstName.PartParam);


                Delegate result = null;

                if (isCached)
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        throw new Exception("Expression Script is cached mode need provide script name!");
                    }
                    result = FISObjectExpression.CompileWithCache(FISObjectExpression.ScriptTypeEnum.Condition, name, code, prod, model, family, partPara);
                }
                else
                {
                    result = FISObjectExpression.Compile(FISObjectExpression.ScriptTypeEnum.Condition, code, prod, model, family, partPara);
                }

                return (bool)result.DynamicInvoke(product, product.ModelObj, product.FamilyObj, part);
            }
            catch (Exception ex)
            {
                logger.Error(LocalConstName.InvokeConditionMethod, ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="part"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="isCached"></param>
        /// <returns></returns>
        public static bool InvokeCondition( IPart part, string name, string code, bool isCached)
        {
            try
            {
                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat(LocalConstName.MsgConditionCode, LocalConstName.PartParam, FISObjectExpression.ScriptTypeEnum.Condition.ToString(),
                                                                                 name, isCached.ToString(), code);
                }              
                var partPara = Expression.Parameter(typeof(IPart), LocalConstName.PartParam);


                Delegate result = null;

                if (isCached)
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        throw new Exception("Expression Script is cached mode need provide script name!");
                    }
                    result = FISObjectExpression.CompileWithCache(FISObjectExpression.ScriptTypeEnum.Condition, name, code,  partPara);
                }
                else
                {
                    result = FISObjectExpression.Compile(FISObjectExpression.ScriptTypeEnum.Condition, code,  partPara);
                }

                return (bool)result.DynamicInvoke(part);
            }
            catch (Exception ex)
            {
                logger.Error(LocalConstName.InvokeConditionMethod, ex);
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="testLog"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="isCached"></param>
        /// <returns></returns>
        public static bool InvokeCondition(IMB mb, TestLog testLog, string name, string code, bool isCached)
        {

            try
            {
                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat(LocalConstName.MsgConditionCode, LocalConstName.MB, FISObjectExpression.ScriptTypeEnum.Condition.ToString(),
                                                                                 name, isCached.ToString(), code);
                }
                var family = Expression.Parameter(typeof(Family), LocalConstName.FamilyParam);
                var pcb = Expression.Parameter(typeof(IMB), LocalConstName.PCBParam);
                var part = Expression.Parameter(typeof(IPart), LocalConstName.PartParam);
                var testLogParam = Expression.Parameter(typeof(TestLog), LocalConstName.TestLogParam);

                Delegate result = null;

                if (isCached)
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        throw new Exception("Expression Script is cached mode need provide script name!");
                    }
                    result = FISObjectExpression.CompileWithCache(FISObjectExpression.ScriptTypeEnum.Condition, name, code, pcb, part, family, testLogParam);
                }
                else
                {
                    result = FISObjectExpression.Compile(FISObjectExpression.ScriptTypeEnum.Condition, code, pcb, part, family, testLogParam);
                }

                Family familyObj = null;
                if (!string.IsNullOrEmpty(mb.Family))
                {
                    familyObj = familyRep.Find(mb.Family);
                }
                return (bool)result.DynamicInvoke(mb, mb.Part, familyObj,testLog);
            }
            catch (Exception ex)
            {
                logger.Error(LocalConstName.InvokeConditionMethod, ex);
                throw;
            }            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="dn"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="isCached"></param>
        /// <returns></returns>
        public static bool InvokeCondition(IProduct product, Delivery dn, string name, string code, bool isCached)
        {
            try
            {
                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat(LocalConstName.MsgConditionCode, LocalConstName.PartParam, FISObjectExpression.ScriptTypeEnum.Condition.ToString(),
                                                                                 name, isCached.ToString(), code);
                }

                var prod = Expression.Parameter(typeof(IProduct), LocalConstName.ProductParam);
                var model = Expression.Parameter(typeof(Model), LocalConstName.ModelParam);
                var family = Expression.Parameter(typeof(Family), LocalConstName.FamilyParam);
                var dnPara = Expression.Parameter(typeof(Delivery), LocalConstName.DeliveryParam);


                Delegate result = null;

                if (isCached)
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        throw new Exception("Expression Script is cached mode need provide script name!");
                    }
                    result = FISObjectExpression.CompileWithCache(FISObjectExpression.ScriptTypeEnum.Condition, name, code, prod, model, family, dnPara);
                }
                else
                {
                    result = FISObjectExpression.Compile(FISObjectExpression.ScriptTypeEnum.Condition, code, prod, model, family, dnPara);
                }

                return (bool)result.DynamicInvoke(product, product.ModelObj, product.FamilyObj, dn);
            }
            catch (Exception ex)
            {
                logger.Error(LocalConstName.InvokeConditionMethod, ex);
                throw;
            }
        }
        /// <summary>
        /// Get Value from script code
        /// </summary>
        /// <param name="product"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="isCached"></param>
        /// <returns></returns>
        public static R GetValue<R>(IProduct product, string name, string code, bool isCached)
        {
            try
            {
                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat(LocalConstName.MsgConditionCode, LocalConstName.Product, FISObjectExpression.ScriptTypeEnum.GetValue.ToString(),
                                                                                 name, isCached.ToString(), code);
                }
                var prod = Expression.Parameter(typeof(IProduct), LocalConstName.ProductParam);
                var model = Expression.Parameter(typeof(Model), LocalConstName.ModelParam);
                var family = Expression.Parameter(typeof(Family), LocalConstName.FamilyParam);
                //var mb = Expression.Parameter(typeof(IMB), LocalConstName.MBParam);
                //var dn = Expression.Parameter(typeof(Delivery), LocalConstName.DeliveryParam);
                //var part = Expression.Parameter(typeof(IPart), LocalConstName.PartParam);
                var bom = Expression.Parameter(typeof(ResolveValue), LocalConstName.BomParam);

                Delegate result = null;

                if (isCached)
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        throw new Exception("Expression Script is cached mode need provide script name!");
                    }
                    result = FISObjectExpression.CompileWithCache(FISObjectExpression.ScriptTypeEnum.GetValue, name, code, prod, model, family, bom);
                }
                else
                {
                    result = FISObjectExpression.Compile(FISObjectExpression.ScriptTypeEnum.GetValue, code, prod, model, family,  bom);
                }

                //Delivery Dn = null;
                //if (!string.IsNullOrEmpty(product.DeliveryNo))
                //{
                //    Dn = dnRep.Find(product.DeliveryNo);
                //}
                return (R)result.DynamicInvoke(product, product.ModelObj, product.FamilyObj, null);
            }
            catch (Exception ex)
            {
                logger.Error(LocalConstName.GetValueMethod, ex);
                throw;
            }
        }
        /// <summary>
        /// get value from executed script code
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="isCached"></param>
        /// <returns></returns>
        public static R GetValue<R>(IMB mb, string name, string code, bool isCached)
        {
            try
            {
                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat(LocalConstName.MsgConditionCode, LocalConstName.MB, FISObjectExpression.ScriptTypeEnum.GetValue.ToString(),
                                                                                 name, isCached.ToString(), code);
                }
                var family = Expression.Parameter(typeof(Family), LocalConstName.FamilyParam);
                var pcb = Expression.Parameter(typeof(IMB), LocalConstName.PCBParam);
                var part = Expression.Parameter(typeof(IPart), LocalConstName.PartParam);


                Delegate result = null;

                if (isCached)
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        throw new Exception("Expression Script is cached mode need provide script name!"); 
                    }
                    result = FISObjectExpression.CompileWithCache(FISObjectExpression.ScriptTypeEnum.GetValue, name, code, pcb, part, family);
                }
                else
                {
                    result = FISObjectExpression.Compile(FISObjectExpression.ScriptTypeEnum.GetValue, code, pcb, part, family);
                }

                Family familyObj = null;
                if (!string.IsNullOrEmpty(mb.Family))
                {
                    familyObj = familyRep.Find(mb.Family);
                }
                return (R)result.DynamicInvoke(mb, mb.Part, familyObj);
            }
            catch (Exception ex)
            {
                logger.Error(LocalConstName.GetValueMethod, ex);
                throw;
            }
        }

    }
}
