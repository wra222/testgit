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


namespace IMES.CheckItemModule.Utility
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
        /// <param name="product"></param>
        /// <param name="mb"></param>
        /// <param name="delivery"></param>
        /// <param name="part"></param>
        /// <param name="name"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static string GetValue(IProduct product, IMB mb, Delivery delivery, Part part,
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
                if (product.ProductInfoes == null || 
                    !product.ProductInfoes.Any(x => x.InfoType == objMethod))
                {
                     throw new FisException("CHK1036", new List<string> { product.ProId, name });
                }
                return product.ProductInfoes
                                            .Where(x => x.InfoType == objMethod)
                                            .Select(y => y.InfoValue).FirstOrDefault().Trim();
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

            if (objName == GlobalConstName.ResolveValue.MODELINFO)
            {
                if (product == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Product" });
                }

                if (product.ModelObj ==null||
                    product.ModelObj.Attributes == null ||
                    !product.ModelObj.Attributes.Any(x => x.Name == objMethod))
                {
                     throw new FisException("CHK1036", new List<string> { product.Model, name });
                }

                return product.ModelObj.Attributes
                                            .Where(x => x.Name == objMethod)
                                            .Select(y => y.Value).FirstOrDefault().Trim();
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
                    throw new FisException("CHK1036", new List<string> { family, name });
                }

                return familyInfoList[0].value.Trim();
            }

            if (objName == GlobalConstName.ResolveValue.PRODUCT)
            {
                if (product == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Product" });
                }
                string value = (string)product.GetProperty(objMethod);
                if (string.IsNullOrEmpty(value))
                {
                    throw new FisException("not exists property name:" + name);
                }
                return value.Trim();
            }

            if (objName == GlobalConstName.ResolveValue.PCB)
            {
                if (mb == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "PCB" });
                }
                string value = (string)mb.GetProperty(objMethod);
                if (string.IsNullOrEmpty(value))
                {
                    throw new FisException("not exists property name:" + name);
                }
                return value.Trim();
            }

            if (objName == GlobalConstName.ResolveValue.PCBINFO)
            {
                if (mb == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "PCB" });
                }

                if (mb.MBInfos==null || 
                    !mb.MBInfos.Any(x => x.InfoType == objMethod))
                {
                    throw new FisException("CHK1036", new List<string> { mb.Sn, name });
                }

                return mb.MBInfos
                               .Where(x => x.InfoType == objMethod)
                               .Select(y => y.InfoValue).FirstOrDefault().Trim();
            }

            if (objName == GlobalConstName.ResolveValue.DELIVERY)
            {
                if (delivery == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Delivery" });
                }
                string value = (string)delivery.GetProperty(objMethod);
                if (string.IsNullOrEmpty(value))
                {
                    throw new FisException("not exists property name:" + name);
                }
                return value.Trim();
            }

            if (objName == GlobalConstName.ResolveValue.DELIVERYINFO)
            {
                if (delivery == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Delivery" });
                }

                if (delivery.DeliveryInfoes==null||
                    !delivery.DeliveryInfoes.Any(x => x.InfoType == objMethod))
                {
                    throw new FisException("CHK1036", new List<string> { delivery.DeliveryNo, name });
                }

                return delivery.DeliveryInfoes
                              .Where(x => x.InfoType == objMethod)
                              .Select(y => y.InfoValue).FirstOrDefault().Trim();
            }


            if (objName == GlobalConstName.ResolveValue.PART)
            {
                if (part == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Part" });
                }
                string value = (string)part.GetProperty(objMethod);
                if (string.IsNullOrEmpty(value))
                {
                    throw new FisException("not exists property name:" + name);
                }
                return value.Trim();
            }

            if (objName == GlobalConstName.ResolveValue.PCBINFO)
            {
                if (part == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Part" });
                }

                if (part.Attributes==null ||
                    !part.Attributes.Any(x => x.InfoType == objMethod))
                {
                    throw new FisException("CHK1036", new List<string> { part.PN, name });
                }

                return part.Attributes
                             .Where(x => x.InfoType == objMethod)
                             .Select(y => y.InfoValue).FirstOrDefault().Trim();
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
        public static string GetValueWithoutError(IProduct product, IMB mb, Delivery delivery, Part part,
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
                if (product.ProductInfoes ==null || 
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

            if (objName == GlobalConstName.ResolveValue.MODELINFO)
            {
                if (product == null)
                {
                    //throw new FisException("CQCHK0006", new List<string> { "Product" });
                    return null;
                }

                if (product.ModelObj == null||
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
                string value = (string)product.GetProperty(objMethod);
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                return value.Trim();
            }

            if (objName == GlobalConstName.ResolveValue.PCB)
            {
                if (mb == null)
                {
                    //throw new FisException("CQCHK0006", new List<string> { "PCB" });
                    return null;
                }
                string value = (string)mb.GetProperty(objMethod);
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                return value.Trim();
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
                string value = (string)delivery.GetProperty(objMethod);
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                return value.Trim();
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
                string value = (string)part.GetProperty(objMethod);
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                return value.Trim();
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

            throw new Exception("not support resolve name:" + name);
        }
    }
}
