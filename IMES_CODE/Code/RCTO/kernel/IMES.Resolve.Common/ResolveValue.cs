﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.Util;
using IMES.FisObject.Common.TestLog;
using System.Text.RegularExpressions;
using IMES.FisObject.Common.FisBOM;


namespace IMES.Resolve.Common
{
    /// <summary>
    /// Resolve value from object name 
    /// </summary>
    public sealed class ResolveValue
    {
        private static readonly IFamilyRepository familyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
        private static readonly IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();

        private readonly static IList<string> BomResolveNameList = new List<string>{ "Level","Method"};
        
        /// <summary>
        /// GetValue
        /// </summary>
        /// <param name="session"></param>
        /// <param name="product"></param>
        /// <param name="mb"></param>
        /// <param name="delivery"></param>
        /// <param name="part"></param>
        /// <param name="name"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static string GetValue(Session session, IProduct product, IMB mb, Delivery delivery, IPart part,
                                                    string name, char spliter)
        {
            return getValueInner(session, product, mb, delivery, part, name, spliter, true);            
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="mb"></param>
        /// <param name="delivery"></param>
        /// <param name="part"></param>
        /// <param name="name"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static string GetValue(IProduct product, IMB mb, Delivery delivery, IPart part,
                                                   string name, char spliter)
        {
            return getValueInner(null, product, mb, delivery, part, name, spliter, true); 
        }
        /// <summary>
        /// GetValue
        /// </summary>
        /// <param name="session"></param>
        /// <param name="product"></param>
        /// <param name="mb"></param>
        /// <param name="delivery"></param>
        /// <param name="part"></param>
        /// <param name="testLog"></param>
        /// <param name="name"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static string GetValue(Session session, IProduct product, IMB mb, Delivery delivery, IPart part,
                                                    TestLog testLog, string name, char spliter)
        {
            string ret = null;
            int index = name.IndexOf(spliter);
            if (index < 1)
            {
                throw new Exception("wrong method name : " + name);
            }

            string objName = name.Substring(0, index);
            string objMethod = name.Substring(index + 1).Trim();

            if (resolveTestLog(testLog, objName, objMethod, true, out ret))
            {
                return ret;
            }

            return getValueInner(session, product, mb, delivery, part, name, spliter, true); 
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="session"></param>
       /// <param name="product"></param>
       /// <param name="mb"></param>
       /// <param name="delivery"></param>
       /// <param name="part"></param>
       /// <param name="name"></param>
       /// <param name="spliter"></param>
       /// <returns></returns>
        public static string GetValueWithoutError(Session session,IProduct product, IMB mb, Delivery delivery, IPart part,
                                                   string name, char spliter)
        {

            return getValueInner(session, product, mb, delivery, part, name, spliter, false);           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="mb"></param>
        /// <param name="delivery"></param>
        /// <param name="part"></param>
        /// <param name="name"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static string GetValueWithoutError(IProduct product, IMB mb, Delivery delivery, IPart part,
                                                   string name, char spliter)
        {
            return getValueInner(null, product, mb, delivery, part, name, spliter, false); 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="product"></param>
        /// <param name="mb"></param>
        /// <param name="delivery"></param>
        /// <param name="part"></param>
        /// <param name="testLog"></param>
        /// <param name="name"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static string GetValueWithoutError(Session session, IProduct product, IMB mb, Delivery delivery, IPart part,
                                                   TestLog testLog, string name, char spliter)
        {
            string ret =null;
            int index = name.IndexOf(spliter);
            if (index < 1)
            {
                throw new Exception("wrong method name : " + name);
            }
            string objName = name.Substring(0, index);
            string objMethod = name.Substring(index + 1).Trim();

            if (resolveTestLog(testLog, objName, objMethod, false, out ret))
            {
                return ret;
            }
            return getValueInner(session, product, mb, delivery, part, name, spliter,false);          
        }

        private static bool resolveTestLog(TestLog testLog, string objName, string objMethod, bool isThrowError,  out string value)
        {
            value = null;
            if (string.Compare(objName, GlobalConstName.ResolveValue.TESTLOG, true) == 0)
            {
                if (testLog == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "TestLog" });
                }
                object retValue = testLog.GetProperty(objMethod);
                if (retValue == null)
                {
                    if (isThrowError)
                    {
                        throw new FisException(string.Format("{0} not exists property name {1}", objName, objMethod));
                    }
                    else
                    {                        
                        return true;
                    }
                }
                value = retValue.ToString();
                return true;
            }
            return false;
        }

        private static string getValueInner(Session session, IProduct product, IMB mb, Delivery delivery, IPart part,
                                                            string name, char spliter, bool isThrowError)
        {
            int index = name.IndexOf(spliter);
            if (index < 1)
            {
                throw new Exception("wrong method name : " + name);
            }

            string objName = name.Substring(0, index);
            string objMethod = name.Substring(index + 1).Trim();

            if (string.Compare(objName, GlobalConstName.ResolveValue.PRODUCTINFO, true) == 0)
            {
                if (product == null)
                {
                    if (isThrowError)
                    {
                        throw new FisException("CQCHK0006", new List<string> { "Product" });
                    }
                    else
                    {
                        return null;
                    }
                }
                if (product.ProductInfoes == null ||
                   !product.ProductInfoes.Any(x => x.InfoType == objMethod))
                {
                    if (isThrowError)
                    {
                        throw new FisException("CHK1036", new List<string> { product.ProId, name });
                    }
                    else
                    {
                        return null;
                    }
                }

                return product.ProductInfoes
                                            .Where(x => x.InfoType == objMethod)
                                            .Select(y => y.InfoValue).FirstOrDefault();
            }
            if (string.Compare(objName, GlobalConstName.ResolveValue.PRODUCTATTR, true) == 0)
            {
                if (product == null)
                {
                    if (isThrowError)
                    {
                        throw new FisException("CQCHK0006", new List<string> { "Product" });
                    }
                    else
                    {
                        return null;
                    }
                }
                if (product.ProductAttributes == null ||
                    !product.ProductAttributes.Any(x => x.AttributeName == objMethod))
                {
                    if (isThrowError)
                    {
                        throw new FisException("CHK1036", new List<string> { product.ProId, name });
                    }
                    else
                    {
                        return null;
                    }
                }
                return product.ProductAttributes
                                            .Where(x => x.AttributeName == objMethod)
                                            .Select(y => y.AttributeValue).FirstOrDefault().Trim();
            }

            if (string.Compare(objName, GlobalConstName.ResolveValue.MODEL,true) == 0)
            {
                if (product == null)
                {
                    if (isThrowError)
                    {
                        throw new FisException("CQCHK0006", new List<string> { "Product" });
                    }
                    else
                    {
                        return null;
                    }
                }

                if (product.ModelObj == null)
                {
                    if (isThrowError)
                    {
                        throw new FisException("CQCHK0006", new List<string> { "Product.ModelObj" });
                    }
                    else
                    {
                        return null;
                    }                    
                }

                object value = product.ModelObj.GetProperty(objMethod);
                if (value == null)
                {
                    var model = product.ModelObj;
                    value = model.GetAttribute(objMethod);
                    if (value == null)
                    {
                        if (isThrowError)
                        {
                            throw new FisException("CHK1036", new List<string> { product.Model, name });
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                return value.ToString();

            }

            if (string.Compare(objName, GlobalConstName.ResolveValue.MODELINFO, true) == 0)
            {
                if (product == null)
                {
                    if (isThrowError)
                    {
                        throw new FisException("CQCHK0006", new List<string> { "Product" });
                    }
                    else
                    {
                        return null;
                    }
                }

                var model = product.ModelObj;
                if (model == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Product.ModelObj" });
                }

                string value = model.GetAttribute(objMethod);
                if (value == null)
                {
                    if (isThrowError)
                    {
                        throw new FisException("CHK1036", new List<string> { product.Model, name });
                    }
                    else
                    {
                        return null;
                    }
                }
                return value;
            }

            if (string.Compare(objName, GlobalConstName.ResolveValue.FAMILYINFO, true) == 0 ||
                string.Compare(objMethod, GlobalConstName.ResolveValue.FAMILY, true) == 0)
            {
                if (product == null)
                {
                    if (isThrowError)
                    {
                        throw new FisException("CQCHK0006", new List<string> { "Product" });
                    }
                    else
                    {
                        return null;
                    }
                }

                var family = product.FamilyObj;
                if (family == null)
                {                   
                    if (isThrowError)
                    {
                        throw new FisException("CQCHK0006", new List<string> { "Family" });
                    }
                    else
                    {
                        return null;
                    }
                }

                object value = family.GetProperty(objMethod);
                if (value == null)
                {
                    value = family.GetAttribute(objMethod);
                    if (value == null)
                    {
                        if (isThrowError)
                        {
                            throw new FisException("CHK1036", new List<string> { family.FamilyName, objMethod });
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

                return value.ToString();
            }

            if (string.Compare(objName, GlobalConstName.ResolveValue.PRODUCT, true) == 0)
            {
                if (product == null)
                {                   
                    if (isThrowError)
                    {
                        throw new FisException("CQCHK0006", new List<string> { "Product" });
                    }
                    else
                    {
                        return null;
                    }
                }
                object value = product.GetProperty(objMethod);
                if (value == null)
                {
                    value = product.GetAttribute(objMethod);
                    if (value == null)
                    {
                        if (isThrowError)
                        {
                            throw new FisException("not exists property name:" + name);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                return value.ToString();
            }

            #region IMGOSVER for HTC
            //if (string.Compare(objName, GlobalConstName.ResolveValue.IMGOSVER, true) == 0)
            //{
            //    if (product == null)
            //    {                    
            //        if (isThrowError)
            //        {
            //            throw new FisException("CQCHK0006", new List<string> { "Product" });
            //        }
            //        else
            //        {
            //            return null;
            //        }
            //    }

            //    if (product.ModelObj == null ||
            //        string.IsNullOrEmpty(product.ModelObj.OSCode) ||
            //        product.ModelObj.LastEffectiveOSVer == null)
            //    {                    
            //        if (isThrowError)
            //        {
            //            throw new FisException("CQCHK0006", new List<string> { "No setup ImgOSVer" });
            //        }
            //        else
            //        {
            //            return null;
            //        }

            //    }

            //    object value = product.ModelObj.LastEffectiveOSVer.GetField(objMethod);
            //    if (value == null)
            //    {
            //        if (isThrowError)
            //        {
            //            throw new FisException("not exists field name:" + name);
            //        }
            //        else
            //        {
            //            return null;
            //        }
            //    }

            //    return value.ToString().Trim();
            //}
            #endregion

            if (string.Compare(objName, GlobalConstName.ResolveValue.PCB, true) == 0)
            {
                if (mb == null)
                {                    
                    if (isThrowError)
                    {
                        throw new FisException("CQCHK0006", new List<string> { "PCB" });
                    }
                    else
                    {
                        return null;
                    }
                }
                object value = mb.GetProperty(objMethod);
                if (value == null)
                {
                    value = mb.GetAttribute(objMethod);
                    if (value == null)
                    {
                        if (isThrowError)
                        {
                            throw new FisException("not exists property name:" + name);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                return value.ToString();
            }

            if (string.Compare(objName, GlobalConstName.ResolveValue.PCBINFO, true) == 0)
            {
                if (mb == null)
                {
                    if (isThrowError)
                    {
                        throw new FisException("CQCHK0006", new List<string> { "PCB" });
                    }
                    else
                    {
                        return null;
                    }
                }

                if (mb.MBInfos == null ||
                      !mb.MBInfos.Any(x => x.InfoType == objMethod))
                {
                    if (isThrowError)
                    {
                        throw new FisException("CHK1036", new List<string> { mb.Sn, name });
                    }
                    else
                    {
                        return null;
                    }
                }
                return mb.MBInfos
                               .Where(x => x.InfoType == objMethod)
                               .Select(y => y.InfoValue).FirstOrDefault();
            }

            if (string.Compare(objName, GlobalConstName.ResolveValue.PCBATTR, true) == 0)
            {
                if (mb == null)
                {                   
                    if (isThrowError)
                    {
                        throw new FisException("CQCHK0006", new List<string> { "PCB" });
                    }
                    else
                    {
                        return null;
                    }
                }

                if (mb.PCBAttributes == null ||
                    !mb.PCBAttributes.Any(x => x.AttributeName == objMethod))
                {
                    if (isThrowError)
                    {
                        throw new FisException("CHK1036", new List<string> { mb.Sn, name });
                    }
                    else
                    {
                        return null;
                    }
                }

                return mb.PCBAttributes
                               .Where(x => x.AttributeName == objMethod)
                               .Select(y => y.AttributeValue).FirstOrDefault().Trim();
            }

            if (string.Compare(objName, GlobalConstName.ResolveValue.DELIVERY, true) == 0)
            {
                if (delivery == null)
                {
                    if (isThrowError)
                    {
                        throw new FisException("CQCHK0006", new List<string> { "Delivery" });
                    }
                    else
                    {
                        return null;
                    }
                }
                object value = delivery.GetProperty(objMethod);
                if (value == null)
                {
                    value = delivery.GetExtendedProperty(objMethod);
                    if (value == null)
                    {
                        if (isThrowError)
                        {
                            throw new FisException("not exists property name:" + name);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                return value.ToString();
            }

            if (string.Compare(objName, GlobalConstName.ResolveValue.DELIVERYINFO, true) == 0)
            {
                if (delivery == null)
                {                    
                    if (isThrowError)
                    {
                        throw new FisException("CQCHK0006", new List<string> { "Delivery" });
                    }
                    else
                    {
                        return null;
                    }
                }

                if (delivery.DeliveryInfoes == null ||
                    !delivery.DeliveryInfoes.Any(x => x.InfoType == objMethod))
                {
                    if (isThrowError)
                    {
                        throw new FisException("CHK1036", new List<string> { delivery.DeliveryNo, name });
                    }
                    else
                    {
                        return null;
                    }
                }

                return delivery.DeliveryInfoes
                              .Where(x => x.InfoType == objMethod)
                              .Select(y => y.InfoValue).FirstOrDefault();
            }


            if (string.Compare(objName, GlobalConstName.ResolveValue.PART, true) == 0)
            {
                if (part == null)
                {
                    if (isThrowError)
                    {
                        throw new FisException("CQCHK0006", new List<string> { "Part" });
                    }
                    else
                    {
                        return null;
                    }
                }
                object value = part.GetProperty(objMethod);
                if (value == null)
                {
                    value = part.GetAttribute(objMethod);
                    if (value == null)
                    {
                        if (isThrowError)
                        {
                            throw new FisException("not exists property name:" + name);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                return value.ToString();
            }

            if (string.Compare(objName, GlobalConstName.ResolveValue.PARTINFO, true) == 0)
            {
                if (part == null)
                {
                    if (isThrowError)
                    {
                        throw new FisException("CQCHK0006", new List<string> { "Part" });
                    }
                    else
                    {
                        return null;
                    }
                }

                if (part.Attributes == null ||
                  !part.Attributes.Any(x => x.InfoType == objMethod))
                {
                    if (isThrowError)
                    {
                        throw new FisException("CHK1036", new List<string> { part.PN, name });
                    }
                    else
                    {
                        return null;
                    }
                }

                return part.Attributes
                             .Where(x => x.InfoType == objMethod)
                             .Select(y => y.InfoValue).FirstOrDefault();
            }

            if (string.Compare(objName, GlobalConstName.ResolveValue.SESSION, true) == 0)
            {
                if (session == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Session" });
                }
                object value = session.GetValue(objMethod);
                if (value == null)
                {
                    if (isThrowError)
                    {
                        throw new FisException("not exists session key name:" + name);
                    }
                    else
                    {
                        return null;
                    }
                }
                return value.ToString().Trim();
            }

            if (string.Compare(objName, GlobalConstName.ResolveValue.DATETIME, true) == 0)
            {
                DateTime now = DateTime.Now;
                return now.ToString(objMethod);
            }

            if (string.Compare(objName, GlobalConstName.ResolveValue.CONSTANT, true) == 0)
            {
                return objMethod;
            }
            throw new Exception("not support resolve name:" + name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="part"></param>
        /// <param name="name"></param>
        /// <param name="objName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string ResolvePart(IPart part,string name, string objName, string fieldName)
        {
            if (string.Compare(objName, GlobalConstName.ResolveValue.PART,true)==0)
            {
                if (part == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Par" });
                }
                object value = part.GetProperty(fieldName);
                if (value == null)
                {
                    //throw new FisException("not exists property name:" + name);
                    return null;
                }
                return value.ToString();
            }

            if (string.Compare(objName , GlobalConstName.ResolveValue.PARTINFO,true)==0)
            {
                if (part == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Par" });
                }

                if (part.Attributes == null ||
                  !part.Attributes.Any(x => x.InfoType == fieldName))
                {
                    //throw new FisException("CHK1036", new List<string> { part.PN, name });
                    return null;
                }

                return part.Attributes
                             .Where(x => x.InfoType == fieldName)
                             .Select(y => y.InfoValue).FirstOrDefault();
            }

            throw new Exception("not support resolve name:" + name);
            //return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resolveName">BOM@Level~Method</param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public static bool CheckResolveBOM(string resolveName,out IList<string> paramList)
        {
            paramList = GetRegexGroupValue(resolveName, GlobalConstName.ResolveValue.BOM, BomResolveNameList);
            if (paramList == null || paramList.Count != 2)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression">
        /// Bom Expression BOM@Level~Method  
        /// Level~Method 
        /// Level </param>
        /// <param name="paramList"></param>
        /// <returns></returns>
 
        static bool CheckBomExpression(string expression, out IList<string> paramList)
        {
            if (CheckResolveBOM(expression, out paramList))
            {
                return true;
            }
            else 
            {
                paramList = new List<string>();
                int index = expression.IndexOf(GlobalConstName.TildeChar);
                if (index < 0)  // no delimiter ~ case
                {
                    paramList.Add(expression); //as level
                    paramList.Add(string.Empty);// no resolveValue case
                }
                else
                {
                   paramList.Add(expression.Substring(0, index)); //Level
                   paramList.Add(expression.Substring(index+1));// resolveValue case
                }

                return true;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="product"></param>
        /// <param name="bomNodeTypeLink"></param>
        /// <param name="resolveName"></param>
        /// <param name="spliter"></param>
        /// <param name="matchRule"></param>
        /// <returns></returns>
        public static bool MatchBomRule(Session session, 
                                                                    IProduct product, 
                                                                    string bomNodeTypeLink, 
                                                                    string resolveName,
                                                                    char spliter, 
                                                                    string matchRule)
        {
           
            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(product.Model);
            if (bom == null)
            {
                throw new FisException("CQCHK0006", new List<string> { "BOM(" + product.Model + ")" });
            }

            IList<IBOMNode> bomNodes=GetNextBom(bomNodeTypeLink, bom.FirstLevelNodes);
            if (bomNodes.Count > 0)
            {
                if (string.IsNullOrEmpty(resolveName))  // No resolveName case 
                {
                    return true;
                }
                else
                {
                    string objName = null;
                    string objMethod = null;
                    int index = resolveName.IndexOf(spliter); 
                    if (index < 1) //default object is PartInfo
                    {
                        //throw new Exception("wrong method name : " + resolveName);
                        objName = GlobalConstName.ResolveValue.PARTINFO;
                        objMethod = resolveName;
                    }
                    else
                    {
                        objName = resolveName.Substring(0, index);
                        objMethod = resolveName.Substring(index + 1).Trim();
                    }

                    return bomNodes.Any(x => Regex.IsMatch(ResolvePart(x.Part, resolveName, objName, objMethod) ?? string.Empty, matchRule));
                }            
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根據設置bomResolveName及matchRule檢查是否BOM 有包含料件
        /// </summary>
        /// <param name="product"></param>
        /// <param name="bomResolveName">
        /// case1: BOM@Level~ResolveValue ,example: BOM@PL,PartType,PartDesc,PN->KP,PartType,PartDesct,PN~PartInfo.VendorCode
        /// case2: Level~ResolveValue ,example: PL,PartType,PartDesc,PN->KP,PartType,PartDesct,PN~PartInfo.VendorCode
        /// case3: Level ,example: PL,PartType,PartDesc,PN->KP,PartType,PartDesct,PN
        /// </param>
        /// <param name="matchRule">
        /// case1: BOM@Level~ResolveValue ,example: BOM@PL,PartType,PartDesc,PN->KP,PartType,PartDesct,PN~PartInfo.VendorCode
        /// case2: Level~ResolveValue ,example: PL,PartType,PartDesc,PN->KP,PartType,PartDesct,PN~PartInfo.VendorCode
        /// Part 物件 ResolveValue 的Regex 規則
        /// </param>
        /// <returns></returns>
       public static bool CheckBom(IProduct product,
                                                   string bomResolveName,
                                                   string matchRule)
        {
            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(product.Model);
            if (bom == null)
            {
                throw new FisException("CQCHK0006", new List<string> { "BOM" });
            }
            IList<string> bomParamList = null;
            if (!CheckBomExpression(bomResolveName, out bomParamList))  //BOM@Level~Part.XXX
            {
                return false;
            }
            string filterBomRuleLink = bomParamList[0];
            string resolveName = bomParamList[1];


            IList<IBOMNode> bomNodes = GetNextBom(filterBomRuleLink, bom.FirstLevelNodes);
            if (bomNodes.Count > 0)
            {

                if (string.IsNullOrEmpty(resolveName))  // No resolveName case 
                {
                    return true;
                }
                else
                {
                    string objName = null;
                    string objMethod = null;
                    int index = resolveName.IndexOf(GlobalConstName.DotChar);
                    if (index < 1) //default object is PartInfo
                    {
                        //throw new Exception("wrong method name : " + resolveName);
                        objName = GlobalConstName.ResolveValue.PARTINFO;
                        objMethod = resolveName;
                    }
                    else
                    {
                        objName = resolveName.Substring(0, index);
                        objMethod = resolveName.Substring(index + 1).Trim();
                    }

                    return bomNodes.Any(x => Regex.IsMatch(ResolvePart(x.Part, resolveName, objName, objMethod) ?? string.Empty, matchRule));
                }                 
            }
            else
            {
                return false;
            }
      
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="product"></param>
        /// <param name="filterBomRuleLink"></param>
        /// <param name="resolveName"></param>
        /// <param name="spliter"></param>
        /// <param name="matchRule"></param>
        /// <returns></returns>
        public static IList<IBOMNode> MatchBomNodeFromBomRule(Session session,
                                                                    IProduct product,
                                                                    string filterBomRuleLink,
                                                                    string resolveName,
                                                                    char spliter,
                                                                    string matchRule)
        {

            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(product.Model);
            if (bom == null)
            {
                throw new FisException("CQCHK0006", new List<string> { "BOM(" + product.Model + ")" });
            }

            IList<IBOMNode> bomNodes = GetNextBom(filterBomRuleLink, bom.FirstLevelNodes);
            if (bomNodes.Count > 0)
            {
                if (string.IsNullOrEmpty(resolveName))  // No resolveName case 
                {
                    return bomNodes;
                }
                else
                {
                    string objName = null;
                    string objMethod = null;
                    int index =resolveName.IndexOf(spliter); 
                    if (index < 1) //default object is PartInfo
                    {
                        //throw new Exception("wrong method name : " + resolveName);
                        objName = GlobalConstName.ResolveValue.PARTINFO;
                        objMethod = resolveName;
                    }
                    else
                    {
                        objName = resolveName.Substring(0, index);
                        objMethod = resolveName.Substring(index + 1).Trim();
                    }
                    return bomNodes.Where(x => Regex.IsMatch(ResolvePart(x.Part, resolveName, objName, objMethod) ?? string.Empty, matchRule)).ToList();
                }
            }
            else
            {
                return null;
            }

        }

       /// <summary>
        /// Expand Bom to button level by BomNodeTypeRule/partType/partDescr/PartNo setting value
       /// </summary>
        /// <param name="bom">IHierarchicalBOM </param>
       /// <param name="bomNodeTypeRule">必要有值,範例:PL->KP</param>
       /// <param name="partType">最後階層BOM檢查條件,空白或是Null則不檢查,使用Regex</param>
        /// <param name="partDescr">最後階層BOM檢查條件,空白或是Null則不檢查,使用Regex</param>
        /// <param name="partNo">最後階層BOM檢查條件,空白或是Null則不檢查,使用Regex</param>
       /// <returns></returns>
        public static IList<IBOMNode> GetBom(IHierarchicalBOM bom,
                                                                  string bomNodeTypeRule,
                                                                   string partType,
                                                                   string partDescr,
                                                                    string partNo)
        {
            if (bom.FirstLevelNodes.Count > 0 && !string.IsNullOrEmpty(bomNodeTypeRule))
            {
                int index = bomNodeTypeRule.IndexOf(GlobalConstName.ArrowStr);
                string bomNodeType = index < 0 ? bomNodeTypeRule : bomNodeTypeRule.Substring(0, index);
                string nextFilterBomNodeTypeRule = index <= 0 ? null : bomNodeTypeRule.Substring(index + 2);
                if (string.IsNullOrEmpty(nextFilterBomNodeTypeRule))
                {
                    return bom.FilterFirstLevelBomNodes(bomNodeType, partType, partDescr, partNo);
                }
                else  // have next bomNode
                {
                    IList<IBOMNode> ret = new List<IBOMNode>();
                    var bomNodeList = bom.FilterFirstLevelBomNodes(bomNodeType, null, null, null);
                    if (bomNodeList != null && bomNodeList.Count > 0)
                    {
                        foreach (IBOMNode node in bomNodeList)
                        {
                            ret = ret.Concat(GetNextBom(nextFilterBomNodeTypeRule, partType, partDescr, partNo, node)).ToList();
                        }
                    }
                    return ret;
                }
            }
            else
            {
                return new List<IBOMNode>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bomNodeList"></param>
        /// <param name="product"></param>
        /// <param name="expressionName"></param>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        public static IList<IBOMNode> GetBomByExpression( IList<IBOMNode> bomNodeList,
                                                                                         IProduct product,         
                                                                                        string expressionName,                                                                                                                                                                            
                                                                                        string filterExpression)
        {
            if (!string.IsNullOrEmpty(filterExpression))
            {
                IList<IBOMNode> ret = new List<IBOMNode>();
                foreach(IBOMNode node in bomNodeList)
                {
                    if (ResolveExpression.InvokeCondition(product, node.Part, expressionName, filterExpression, true))
                    {
                        ret.Add(node);
                    }
                }
                return ret;
            }
            else
            {
                return bomNodeList;
            }
        }       

        /// <summary>
        /// 展BOM規則根據filterBomRule 規則
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filterBomRule">
        /// 規則: BomNodeType,PartType Regex,PartDesc Regex,PartNo Regex->...->...
        /// 範例: PL,Battery,HP Battery->KP,Battery,HP Battery,645890-001|645890-002
        /// </param>
        /// <returns></returns>
        public static IList<IBOMNode> GetBom(string model,
                                                                    string filterBomRule)
        {
            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(model);
            if (bom == null)
            {
                throw new FisException("CQCHK0006", new List<string> { "BOM(" + model + ")" });
            }
            return GetNextBom(filterBomRule, bom.FirstLevelNodes);  
        }
       /// <summary>
        /// 展下階BOM規則根據輸入參數
       /// </summary>
       /// <param name="bomNodeTypeRule">PL->KP</param>
       /// <param name="partType">最後階PartType條件</param>
        /// <param name="partDescr">最後階PartDescr條件</param>
        /// <param name="partNo">最後階PartNo條件</param>
       /// <param name="parentBomNode">上階BomNode</param>
       /// <returns></returns>
        public static IList<IBOMNode> GetNextBom(string bomNodeTypeRule,
                                                                            string partType,
                                                                            string partDescr,
                                                                            string partNo,                            
                                                                            IBOMNode parentBomNode)
        {
            int index = bomNodeTypeRule.IndexOf(GlobalConstName.ArrowStr);
            string bomNodeType= index < 0 ? bomNodeTypeRule : bomNodeTypeRule.Substring(0, index);
            string nextFilterBomNodeTypeRule = index <= 0 ? null : bomNodeTypeRule.Substring(index + 2);
            if (string.IsNullOrEmpty(nextFilterBomNodeTypeRule))
            {
                return parentBomNode.FilterChildNode(bomNodeType, partType, partDescr, partNo);               
            }
            else  // have next bomNode
            {
                IList<IBOMNode> ret = new List<IBOMNode>();
                var bomNodeList = parentBomNode.FilterChildNode(bomNodeType, null, null,null);
                if (bomNodeList != null && bomNodeList.Count > 0)
                {
                    foreach (IBOMNode node in bomNodeList)
                    {
                        ret = ret.Concat(GetNextBom(nextFilterBomNodeTypeRule, partType, partDescr, partNo, node)).ToList();
                    }
                }               
                return ret;
            }
        }

        /// <summary>
        /// 展下階BOM規則根據filterBomRuleLink 規則
        /// </summary>
        /// <param name="filterBomRuleLink">
        /// PL,PartType,PartDesc,PN->KP,PartType,PartDesc,PN
        /// </param>
        /// <param name="bomNodes"></param>
        /// <returns></returns>
        public static IList<IBOMNode> GetNextBom(string filterBomRuleLink,
                                                                            IList<IBOMNode> bomNodes)
        {
            int index = filterBomRuleLink.IndexOf(GlobalConstName.ArrowStr);
            string filterBomRule = index < 0 ? filterBomRuleLink : filterBomRuleLink.Substring(0, index);
            string nextFilterBomRule = index <= 0 ? null : filterBomRuleLink.Substring(index + 2);
            if (string.IsNullOrEmpty(nextFilterBomRule))
            {
                return bomNodes.Where(x => CheckBomNodeMatch(x.Part, filterBomRule)).ToList();
            }
            else  // have next bomNode
            {
                IList<IBOMNode> ret = new List<IBOMNode>();
                var bomNodeList = bomNodes.Where(x => CheckBomNodeMatch(x.Part, filterBomRule)).ToList();
                if (bomNodeList != null && bomNodeList.Count > 0)
                {
                    foreach (IBOMNode node in bomNodeList)
                    {
                        ret = ret.Concat(GetNextBom(nextFilterBomRule, node.Children)).ToList();
                    }
                }
                return ret;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="part"></param>
        /// <param name="filterRule">PL,PartType,PartDesc,PN</param>
        /// <returns></returns>
        static bool CheckBomNodeMatch(IPart part, string filterRule)
        {
            string[] partRules = filterRule.Split(GlobalConstName.CommaChar);
            int length = partRules.Length;
            if (part.BOMNodeType == partRules[0]) //Level1 check BomNodeType
            {
                if (length>1 )
                {
                    if (RegexMatchRule(part.Type, partRules[1], true))  //Level2 Check PartType
                    {
                        if (length > 2)
                        {
                            if (RegexMatchRule(part.Descr, partRules[2], true)) //Level3 Check PartDescr
                            {
                                if (length > 3)
                                {
                                    return RegexMatchRule(part.PN, partRules[3], true); //Level4 check PN
                                }
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }                   
                }
                else
                {
                    return true;
                }
            }
            return false;
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="isEmptyPatternPass"></param>
        /// <returns></returns>
        public static bool RegexMatchRule(string input, string pattern, bool isEmptyPatternPass)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return isEmptyPatternPass;
            }
            else
            {
                return Regex.IsMatch(input, pattern);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IList<string> GetRegexGroupName(string expression)
        {
            IList<string> ret = new List<string>();
            //string groupNamePattern = @"\(\?<(?<name>.+?)>.+?\)";

            MatchCollection mc = Regex.Matches(expression, GlobalConstName.RegEx.GroupNamePattern, RegexOptions.Compiled);

            foreach (Match m in mc)
            {
                Group group = m.Groups[GlobalConstName.RegEx.GroupName];
                if (group.Success)
                {
                    ret.Add(group.Value);
                }
            }

            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="pattern"></param>
        /// <param name="groupNameList"></param>
        /// <returns></returns>
        public static IList<string> GetRegexGroupValue(string inputValue, string pattern, IList<string> groupNameList)
        {             
            Match matchRegex = Regex.Match(inputValue, pattern);
            if (matchRegex.Success)
            {
                if (groupNameList == null || groupNameList.Count == 0)
                {
                    return null;
                }
                else
                {
                    if (matchRegex.Groups != null &&
                        matchRegex.Groups.Count > 0)
                    {
                        IList<string> valueList = new List<string>();
                        foreach (string groupName in groupNameList)
                        {
                            Group group = matchRegex.Groups[groupName];
                            if (group != null &&
                                group.Success)
                            {
                                valueList.Add(group.Value);
                            }
                            else
                            {
                                return null;
                            }
                        }
                        return valueList;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="pattern"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static string GetRegexGroupValue(string inputValue, string pattern, string groupName)
        {
            Match matchRegex = Regex.Match(inputValue, pattern);
            if (matchRegex.Success)
            {
                if (string.IsNullOrEmpty(groupName))
                {
                    return inputValue;
                }
                else
                {
                    if (matchRegex.Groups != null &&
                        matchRegex.Groups.Count > 0)
                    {
                        Group group = matchRegex.Groups[groupName];
                        if (group != null &&
                            group.Success)
                        {
                            return group.Value;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }
    }
}