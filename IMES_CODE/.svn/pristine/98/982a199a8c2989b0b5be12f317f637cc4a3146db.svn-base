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

namespace IMES.Activity
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

            if (objName == "PRODUCTINFO")
            {
                if (product == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "ProductInfo" });
                }
                return product.ProductInfoes
                                            .Where(x => x.InfoType == objMethod)
                                            .Select(y => y.InfoValue).FirstOrDefault();
            }

            if (objName == "MODELINFO")
            {
                if (product == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Product" });
                }
                return product.ModelObj.Attributes
                                            .Where(x => x.Name == objMethod)
                                            .Select(y => y.Value).FirstOrDefault();
            }

            if (objName == "FAMILYINFO")
            {
                if (product == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Product" });
                }
                string family = product.Family;
               
                IList<FamilyInfoDef> familyInfoList = familyRep.GetExistFamilyInfo(new FamilyInfoDef { family = family, name = objMethod });

                if (familyInfoList == null || familyInfoList.Count == 0)
                {
                    throw new FisException("CHK1036", new List<string> { family, objMethod });
                }

                return familyInfoList[0].value;
            }

            if (objName == "PRODUCT")
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
                return value;
            }

            if (objName == "PCB")
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
                return value;
            }

            if (objName == "PCBINFO")
            {
                if (mb == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "PCB" });
                }

                return mb.MBInfos
                               .Where(x => x.InfoType == objMethod)
                               .Select(y => y.InfoValue).FirstOrDefault();
            }

            if (objName == "DELIVERY")
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
                return value;
            }

            if (objName == "DELIVERYINFO")
            {
                if (delivery == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Delivery" });
                }
                return delivery.DeliveryInfoes
                              .Where(x => x.InfoType == objMethod)
                              .Select(y => y.InfoValue).FirstOrDefault();
            }


            if (objName == "PART")
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
                return value;
            }

            if (objName == "PARTINFO")
            {
                if (part == null)
                {
                    throw new FisException("CQCHK0006", new List<string> { "Part" });
                }
                return part.Attributes
                             .Where(x => x.InfoType == objMethod)
                             .Select(y => y.InfoValue).FirstOrDefault();
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
