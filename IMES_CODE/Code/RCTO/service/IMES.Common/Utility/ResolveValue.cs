using System;
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


namespace IMES.Common
{
    /// <summary>
    /// Resolve value from object name 
    /// </summary>
    public sealed class ResolveValue
    {
        private static readonly IFamilyRepository familyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
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
        public static string GetValue(Session session, 
                                                    IProduct product, 
                                                    IMB mb, 
                                                    Delivery delivery, 
                                                    IPart part,
                                                    string name, char spliter)
        {
            int index = name.IndexOf(spliter);
            if (index < 1)
            {
                throw new Exception("wrong method name : " + name);
            }

            string objName = name.Substring(0, index).ToUpper();
            string objMethod = name.Substring(index + 1).Trim();

            if (objName == GlobalConstName.ResolveValue.PRODUCTINFO)
            {
                if (product == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Product" });
                }
                return product.ProductInfoes
                                            .Where(x => x.InfoType == objMethod)
                                            .Select(y => y.InfoValue).FirstOrDefault();
            }
            if (objName == GlobalConstName.ResolveValue.PRODUCTATTR)
            {
                if (product == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Product" });
                }
                if (product.ProductAttributes == null ||
                    !product.ProductAttributes.Any(x => x.AttributeName == objMethod))
                {
                    throw new FisException("CHK1036", new List<string> { product.ProId, name });
                }
                return product.ProductAttributes
                                            .Where(x => x.AttributeName == objMethod)
                                            .Select(y => y.AttributeValue).FirstOrDefault().Trim();
            }

            if (objName == GlobalConstName.ResolveValue.MODEL)
            {
                if (product == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Product" });                   
                }

                if (product.ModelObj == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Product.ModelObj" });
                }

                object value = product.ModelObj.GetProperty(objMethod);
                if (value == null)
                {
                    return null;
                }
                return value.ToString().Trim();

            }

            if (objName == GlobalConstName.ResolveValue.MODELINFO)
            {
                if (product == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Product" });
                }

                if (product.ModelObj == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Product.ModelObj" });
                }

                return product.ModelObj.Attributes
                                            .Where(x => x.Name == objMethod)
                                            .Select(y => y.Value).FirstOrDefault();
            }

            if (objName == GlobalConstName.ResolveValue.FAMILYINFO)
            {
                if (product == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Product" });
                }
                string family = product.Family;
                //IFamilyRepository familyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
                IList<FamilyInfoDef> familyInfoList = familyRep.GetExistFamilyInfo(new FamilyInfoDef { family = family, name = objMethod });

                if (familyInfoList == null || familyInfoList.Count == 0)
                {
                    throw new FisException("CHK1036", new List<string> { family, objMethod });
                }

                return familyInfoList[0].value;
            }

            if (objName == GlobalConstName.ResolveValue.PRODUCT)
            {
                if (product == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Product" });
                }
                object value = product.GetProperty(objMethod);
                if (value==null)
                {
                    throw new FisException("not exists property name:" + name);
                }
                return value.ToString();
            }

            #region disable for ICI HTC case
            //if (objName == GlobalConstName.ResolveValue.IMGOSVER)
            //{
            //    if (product == null)
            //    {
            //       throw new FisException("CQCHK0006", new List<string> { "Product" });                   
            //    }

            //    if (product.ModelObj == null ||
            //        string.IsNullOrEmpty(product.ModelObj.OSCode) ||
            //        product.ModelObj.LastEffectiveOSVer == null)
            //    {
            //        throw new FisException("CQCHK0006", new List<string> { "No setup ImgOSVer" });
            //    }

            //    object value = product.ModelObj.LastEffectiveOSVer.GetField(objMethod);
            //    if (value == null)
            //    {
            //        throw new FisException("not exists field name:" + name);
            //    }

            //    return value.ToString().Trim();
            //} 
            #endregion

            if (objName == GlobalConstName.ResolveValue.PCB)
            {
                if (mb == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "PCB" });
                }
                object value = mb.GetProperty(objMethod);
                if (value == null)
                {
                    throw new FisException("not exists property name:" + name);
                }
                return value.ToString();
            }

            if (objName == GlobalConstName.ResolveValue.PCBINFO)
            {
                if (mb == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "PCB" });
                }

                return mb.MBInfos
                               .Where(x => x.InfoType == objMethod)
                               .Select(y => y.InfoValue).FirstOrDefault();
            }

            if (objName == GlobalConstName.ResolveValue.DELIVERY)
            {
                if (delivery == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Delivery" });
                }
                object value = delivery.GetProperty(objMethod);
                if (value == null)
                {
                    throw new FisException("not exists property name:" + name);
                }
                return value.ToString();
            }

            if (objName == GlobalConstName.ResolveValue.DELIVERYINFO)
            {
                if (delivery == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Delivery" });
                }
                return delivery.DeliveryInfoes
                              .Where(x => x.InfoType == objMethod)
                              .Select(y => y.InfoValue).FirstOrDefault();
            }


            if (objName == GlobalConstName.ResolveValue.PART)
            {
                if (part == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Delivery" });
                }
                object value = part.GetProperty(objMethod);
                if (value==null)
                {
                    throw new FisException("not exists property name:" + name);
                }
                return value.ToString();
            }

            if (objName == GlobalConstName.ResolveValue.PARTINFO)
            {
                if (part == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Delivery" });
                }
                return part.Attributes
                             .Where(x => x.InfoType == objMethod)
                             .Select(y => y.InfoValue).FirstOrDefault();
            }

            if (objName == GlobalConstName.ResolveValue.SESSION)
            {
                if (session == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Session" });                    
                }
                object value = session.GetValue(objMethod);
                if (value == null)
                {
                    throw new FisException("not exists session key name:" + name);
                }
                return value.ToString().Trim();
            }
            if (objName == GlobalConstName.ResolveValue.DATETIME)
            {
                DateTime now = DateTime.Now;
                return now.ToString(objMethod);
            }

            if (objName == GlobalConstName.ResolveValue.CONSTANT)
            {
                return objMethod;
            } 
            throw new Exception("not support resolve name:" + name);
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
        public static string GetValueWithoutError(Session session,IProduct product, IMB mb, Delivery delivery, IPart part,
                                                   string name, char spliter)
        {
            int index = name.IndexOf(spliter);
            if (index < 1)
            {
                throw new Exception("wrong method name : " + name);
            }

            string objName = name.Substring(0, index).ToUpper();
            string objMethod = name.Substring(index + 1).Trim();

            if (objName == GlobalConstName.ResolveValue.PRODUCTINFO)
            {
                if (product == null)
                {
                    //throw new FisException("CQCHK0006", new List<string> { "Product" });
                    return null;
                }
                if (product.ProductInfoes == null ||
                    !product.ProductInfoes.Any(x => x.InfoType == objMethod))
                {
                    return null;
                }
                return product.ProductInfoes
                                            .Where(x => x.InfoType == objMethod)
                                            .Select(y => y.InfoValue).FirstOrDefault().Trim();
            }

            if (objName == GlobalConstName.ResolveValue.PRODUCTATTR)
            {
                if (product == null)
                {
                    //throw new FisException("CQCHK0006", new List<string> { "Product" });
                    return null;
                }
                if (product.ProductAttributes == null ||
                    !product.ProductAttributes.Any(x => x.AttributeName == objMethod))
                {
                    return null;
                }
                return product.ProductAttributes
                                            .Where(x => x.AttributeName == objMethod)
                                            .Select(y => y.AttributeValue).FirstOrDefault().Trim();
            }

            if (objName == GlobalConstName.ResolveValue.MODEL)
            {
                if (product == null)
                {
                    //throw new FisException("CQCHK0006", new List<string> { "Product" });
                    return null;
                }

                if (product.ModelObj == null )
                {
                    return null;
                }

                object value = product.ModelObj.GetProperty(objMethod);
                if (value == null)
                {
                    return null;
                }
                return value.ToString().Trim();
                                           
            }
           
            if (objName == GlobalConstName.ResolveValue.MODELINFO)
            {
                if (product == null)
                {
                    //throw new FisException("CQCHK0006", new List<string> { "Product" });
                    return null;
                }

                if (product.ModelObj == null ||
                    product.ModelObj.Attributes == null ||
                    !product.ModelObj.Attributes.Any(x => x.Name == objMethod))
                {
                    return null;
                }

                return product.ModelObj.Attributes
                                            .Where(x => x.Name == objMethod)
                                            .Select(y => y.Value).FirstOrDefault().Trim();
            }            

            if (objName == GlobalConstName.ResolveValue.FAMILYINFO)
            {
                if (product == null)
                {
                    //throw new FisException("CQCHK0006", new List<string> { "Product" });
                    return null;
                }
                string family = product.Family;
                //IFamilyRepository familyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
                IList<FamilyInfoDef> familyInfoList = familyRep.GetExistFamilyInfo(new FamilyInfoDef { family = family, name = objMethod });

                if (familyInfoList == null || familyInfoList.Count == 0)
                {
                    return null;
                }

                return familyInfoList[0].value.Trim();
            }

            if (objName == GlobalConstName.ResolveValue.PRODUCT)
            {
                if (product == null)
                {
                    //throw new FisException("CQCHK0006", new List<string> { "Product" });
                    return null;
                }
                object value = product.GetProperty(objMethod);
                if (value==null )
                {
                    return null;
                }
                return value.ToString().Trim();
            }

            #region disable Code for ICI HTC
            //if (objName == GlobalConstName.ResolveValue.IMGOSVER)
            //{
            //    if (product == null)
            //    {
            //        //throw new FisException("CQCHK0006", new List<string> { "Product" });
            //        return null;
            //    }

            //    if (product.ModelObj == null ||
            //        string.IsNullOrEmpty(product.ModelObj.OSCode) ||
            //        product.ModelObj.LastEffectiveOSVer==null )
            //    {
            //        return null;
            //    }

            //    object value = product.ModelObj.LastEffectiveOSVer.GetField(objMethod);
            //    if (value == null)
            //    {
            //        return null;
            //    }
            //    return value.ToString().Trim();
            //}
            #endregion

            if (objName == GlobalConstName.ResolveValue.PCB)
            {
                if (mb == null)
                {
                    //throw new FisException("CQCHK0006", new List<string> { "PCB" });
                    return null;
                }
                object value = mb.GetProperty(objMethod);
                if (value == null)
                {
                    return null;
                }
                return value.ToString().Trim();
            }

            if (objName == GlobalConstName.ResolveValue.PCBINFO)
            {
                if (mb == null)
                {
                    // throw new FisException("CQCHK0006", new List<string> { "PCB" });
                    return null;
                }

                if (mb.MBInfos == null ||
                    !mb.MBInfos.Any(x => x.InfoType == objMethod))
                {
                    return null;
                }

                return mb.MBInfos
                               .Where(x => x.InfoType == objMethod)
                               .Select(y => y.InfoValue).FirstOrDefault().Trim();
            }

            if (objName == GlobalConstName.ResolveValue.DELIVERY)
            {
                if (delivery == null)
                {
                    // throw new FisException("CQCHK0006", new List<string> { "Delivery" });
                    return null;
                }

                object value = delivery.GetProperty(objMethod);
                if (value == null)
                {
                    return null;
                }
                return value.ToString().Trim();
            }

            if (objName == GlobalConstName.ResolveValue.DELIVERYINFO)
            {
                if (delivery == null)
                {
                    // throw new FisException("CQCHK0006", new List<string> { "Delivery" });
                    return null;
                }

                if (delivery.DeliveryInfoes == null ||
                     !delivery.DeliveryInfoes.Any(x => x.InfoType == objMethod))
                {
                    return null;
                }

                return delivery.DeliveryInfoes
                              .Where(x => x.InfoType == objMethod)
                              .Select(y => y.InfoValue).FirstOrDefault().Trim();
            }


            if (objName == GlobalConstName.ResolveValue.PART)
            {
                if (part == null)
                {
                    //throw new FisException("CQCHK0006", new List<string> { "Part" });
                    return null;
                }

                object value = part.GetProperty(objMethod);
                if (value == null)
                {
                    return null;
                }
                return value.ToString().Trim();
            }

            if (objName == GlobalConstName.ResolveValue.PARTINFO)
            {
                if (part == null)
                {
                    //throw new FisException("CQCHK0006", new List<string> { "Part" });
                    return null;
                }

                if (part.Attributes == null ||
                    !part.Attributes.Any(x => x.InfoType == objMethod))
                {
                    return null;
                }

                return part.Attributes
                             .Where(x => x.InfoType == objMethod)
                             .Select(y => y.InfoValue).FirstOrDefault().Trim();
            }
            if (objName == GlobalConstName.ResolveValue.SESSION)
            {
                if (session == null)
                {
                    //throw new FisException("CQCHK0006", new List<string> { "Product" });
                    return null;
                }
                object value = session.GetValue(objMethod);
                if (value == null)
                {
                    return null;
                }
                return value.ToString().Trim();
            }

            if (objName == GlobalConstName.ResolveValue.DATETIME)
            {
                DateTime now = DateTime.Now;
                return now.ToString(objMethod);
            }

            if (objName == GlobalConstName.ResolveValue.CONSTANT)
            {
                return objMethod;
            } 
            throw new Exception("not support resolve name:" + name);
        }

		/// <summary>
        /// Resolve UPS Parameter Name/Value
        /// </summary>
        /// <param name="srcData"></param>
        /// <param name="session"></param>
        /// <param name="product"></param>
        /// <param name="dn"></param>
        /// <param name="part"></param>
        /// <param name="hpPOInfo"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static string replaceUPSParameter(string[] srcData, 
                                                                     Session session, 
                                                                      IProduct product, 
                                                                      Delivery dn, 
                                                                      IPart part,
                                                                      UPSHPPOInfo  hpPOInfo,                                                                        
                                                                      char spliter)
        {
           
            int index = 0;
            foreach (string value in srcData)
            {
                if (value.Contains(spliter))
                {
                    string s = GetValue(hpPOInfo, value, spliter);
                    if (string.IsNullOrEmpty(s))
                    {
                        s = ResolveValue.GetValue(session, product, null, dn, part, value, spliter);
                    }

                    if (string.IsNullOrEmpty(s))
                    {
                        throw new FisException("CQCHK0006", new List<string> { value });
                    }
                    srcData[index] = s;
                }
                index++;
            }
            return string.Join(",", srcData);
        }

        /// <summary>
        /// Get Value by UPSInfo
        /// </summary>
        /// <param name="hpPOInfo"></param>
        /// <param name="name"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static string GetValue(UPSHPPOInfo hpPOInfo,
                                                   string name,
                                                   char spliter)
        {
            int index = name.IndexOf(spliter);
            if (index < 1)
            {
                throw new Exception("wrong method name : " + name);
            }

            string objName = name.Substring(0, index).ToUpper();
            string objMethod = name.Substring(index + 1).Trim();

            if (objName == "UPSHPPOINFO")
            {
                if (hpPOInfo == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "UPSHPPOInfo" });
                }
                string value = (string)hpPOInfo.GetProperty(objMethod);
                if (string.IsNullOrEmpty(value))
                {
                    throw new FisException("not exists property name:" + name);
                }
                return value;
            }
            return null;
        }
    }
}
