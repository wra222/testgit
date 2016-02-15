using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using log4net;
using IMES.Infrastructure;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure.Extend;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.StandardWeight;
using System.Text.RegularExpressions;



namespace IMES.Infrastructure.Utility.Common
{
    public  class CommonUti : MarshalByRefObject
    {
        private static  ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IList<ConstValueTypeInfo> GetConstValueTypeByType(string type)
        {
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueTypeInfo> constValueTypeList=partRep.GetConstValueTypeList(type);
            return constValueTypeList;
        }

        public static IList<PalletType> GetPalletType(string palletNo, string dnNo)
        {
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IPalletRepository palletRepository =RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IPalletTypeRepository palletTypeRepository =RepositoryFactory.GetInstance().GetRepository<IPalletTypeRepository, PalletType>();
            string ShipWay="";
            string RegId="";
            string StdPltFullQty = "";
            int palletLayer = 0;
            string oceanType = "";
            if (string.IsNullOrEmpty(dnNo))
            { 
                IList<DeliveryPallet> dpList = palletRepository.GetDeliveryPallet(palletNo);
                if ((dpList == null) || (dpList.Count == 0))
                {
                    List<string> errpara1 = new List<string>();
                    errpara1.Add(palletNo);
                    throw new FisException("CHK241", errpara1);
                }
                dnNo = dpList[0].DeliveryID;
            }
            Delivery dn = deliveryRep.GetDelivery(dnNo);
            string model = dn.ModelName;
            int pltDeliveryQty = deliveryRep.GetSumDeliveryQtyOfACertainPallet(palletNo);
            DeliveryEx dnEx = dn.DeliveryEx;

            string palletLayerInfo = (string)dn.GetExtendedProperty("PalletLayer");
            if (!string.IsNullOrEmpty(palletLayerInfo))
            {
                palletLayer = int.Parse(palletLayerInfo.Trim());
            }

            string oceanTypeInfo = (string)dn.GetExtendedProperty("OceanType");
            if (!string.IsNullOrEmpty(oceanTypeInfo))
            {
                oceanType = oceanTypeInfo.Trim();
            }
            string RegIdInPalletType = GetValueFromSysSetting("RegIdInPalletType");
            if (Regex.IsMatch(dn.ModelName.Trim(),RegIdInPalletType))
           // if (dn.ModelName.Substring(0, 2) == "PC" || dn.ModelName.Substring(0, 2) == "SC")
            {
                if (dnEx.Carrier == "FDE" || dnEx.Carrier == "FEPL")
                {
                    RegId = dnEx.Carrier;
                }
                else
                {
                    RegId = dnEx.MessageCode;
                }
                StdPltFullQty = dnEx.StdPltFullQty;
                ShipWay = dnEx.ShipWay;
            }
            else
            {
                IList<DeliveryInfo> lstDnInfo = dn.DeliveryInfoes;
                IList<string> tmpArr;
                tmpArr = lstDnInfo.Where(x => x.InfoType == "ShipWay").Select(x => x.InfoValue).ToList();
                ShipWay = tmpArr.Count == 0 ? "" : tmpArr[0];
                tmpArr = lstDnInfo.Where(x => x.InfoType == "RegId").Select(x => x.InfoValue).ToList();
                RegId = tmpArr.Count == 0 ? "" : tmpArr[0];
                tmpArr = lstDnInfo.Where(x => x.InfoType == "PalletQty").Select(x => x.InfoValue).ToList();
                StdPltFullQty = tmpArr.Count == 0 ? "0" : tmpArr[0];
                double d;
                if (double.TryParse(StdPltFullQty, out d))
                {
                    StdPltFullQty = Math.Floor(Math.Abs(d)).ToString();
                }
            }

            IList<PalletType> lstPalletType = palletTypeRepository.GetPalletType(ShipWay, RegId, StdPltFullQty,palletLayer,oceanType, pltDeliveryQty);
            return lstPalletType;
        }

        public static string GetSite()
        {
            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> valueList = PartRepository.GetValueFromSysSettingByName("Site");
            if (valueList.Count == 0)
            { throw new FisException("PAK095", new string[] { "Site" }); }
            return valueList[0].Trim(); //
        }
        public static bool CheckIsWIN8(IHierarchicalBOM bom)
        {
            string site = GetSite();
            IList<IBOMNode> P1BomNodeList = bom.GetFirstLevelNodesByNodeType("P1");
            bool bWIN8 = false;
            if (site == "IPC")
            {
                foreach (IBOMNode bomNode in P1BomNodeList)
                {
                    if (bomNode.Part.Descr.StartsWith("ECOA"))
                    {
                        bWIN8 = true;
                        break;
                    }
                }
             }
            else
            {
                foreach (IBOMNode bomNode in P1BomNodeList)
                {
                    if (bomNode.Part.Descr.ToUpper().Contains("COA (WIN8)") )
                    {
                        bWIN8 = true;
                        break;
                    }
                }
            }
            return bWIN8;

        }
        public static string GetValueFromSysSetting(string name)
        {
            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> valueList = PartRepository.GetValueFromSysSettingByName(name);
            if (valueList.Count == 0)
            { throw new FisException("PAK095", new string[] { name }); }
            return valueList[0].Trim(); //
        
        }
        public static string CheckPodLabel(string custsn)
        {
            IProduct product=null;
            try
            {
                IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                product = GetProductByInput(custsn, InputTypeEnum.CustSN);
                string label = "";
                string site = GetValueFromSysSetting("Site");
                string model = product.Model.Trim();
                string number = "0123456789";
                string modelbit = model.Substring(6, 1);
                string delievery = product.DeliveryNo.Trim();
                IList<PODLabelPartDef> podLabelPartLst = new List<PODLabelPartDef>();
                podLabelPartLst = PartRepository.GetPODLabelPartListByPartNo(model);
                if (site == "ICC")
                {
              
                    //如果IMES_PAK..PODLabelPart中有维护PartNo等于Model的前几位字符的记录 and (Model的第7位不是数字)，则需要列印POD Label====
              //      IDeliveryRepository iDeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                    IList<string> valueList =
                           PartRepository.GetConstValueTypeList("ExceptPODModel").Select(x => x.value).Where(y => y != "").Distinct().ToList();
                    if (!number.Contains(modelbit) && podLabelPartLst.Count > 0 && !valueList.Contains(model))
                    { label = "PODLabel"; }

                }
                else
                {
                    if (podLabelPartLst.Count > 0 && !number.Contains(modelbit))
                    {
                        label = "PODLabel"; 
                    }
                
                }
                return label;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(CommonImpl2)CheckPodLabel" + product.ProId);
            }
        
        }
        public enum InputTypeEnum
        {
         
            CustSN = 1,

            Carton = 2,
            ProductIDOrCustSN = 4,
                  
            ProductIDOrCustSNOrCarton = 8,
            ProductIDOrCustSNOrPallet = 16
        }
        public static IMES.FisObject.FA.Product.IProduct GetProductByInput(string inputNo, InputTypeEnum InputType)
        {
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
            IMES.FisObject.FA.Product.IProduct currentProduct = null;

            switch (InputType)
            {
                case InputTypeEnum.CustSN:
                    currentProduct = productRepository.GetProductByCustomSn(inputNo);
                    if (currentProduct == null && inputNo.Length > 1)
                    {
                        currentProduct = productRepository.GetProductByCustomSn(inputNo.Substring(0, inputNo.Length - 1));
                    }
                    break;
                case InputTypeEnum.Carton:
                    List<string> productIDList = productRepository.GetProductIDListByCarton(inputNo);
                    if (productIDList == null || productIDList.Count == 0 && inputNo.Length > 1)
                    {
                        productIDList = productRepository.GetProductIDListByCarton(inputNo.Substring(0, inputNo.Length - 1));
                    }
                    if (productIDList != null && productIDList.Count != 0)
                    {
                        currentProduct = productRepository.Find(productIDList[0]);
                    }
                    break;
                case InputTypeEnum.ProductIDOrCustSN:
                    currentProduct = productRepository.FindOneProductWithProductIDOrCustSN(inputNo);
                    if (currentProduct == null && inputNo.Length > 1)
                    {
                        currentProduct = productRepository.FindOneProductWithProductIDOrCustSN(inputNo.Substring(0, inputNo.Length - 1));
                    }
                    break;
                case InputTypeEnum.ProductIDOrCustSNOrCarton:
                    currentProduct = productRepository.FindOneProductWithProductIDOrCustSNOrCarton(inputNo);
                    if (currentProduct == null && inputNo.Length > 1)
                    {
                        currentProduct = productRepository.FindOneProductWithProductIDOrCustSNOrCarton(inputNo.Substring(0, inputNo.Length - 1));
                    }
                    break;
                case InputTypeEnum.ProductIDOrCustSNOrPallet:
                    currentProduct = productRepository.FindOneProductWithProductIDOrCustSNOrPallet(inputNo);
                    if (currentProduct == null && inputNo.Length > 1)
                    {
                        currentProduct = productRepository.FindOneProductWithProductIDOrCustSNOrPallet(inputNo.Substring(0, inputNo.Length - 1));
                    }
                    break;

            }

            if (currentProduct == null)
            {
                FisException fe = new FisException("CHK079", new string[] { inputNo });
                throw fe;
            }
            return currentProduct;
        }
        public static string CheckConfigLabel(string custsn)
        {
            IProduct product = null;
            try
            {
                IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                product = GetProductByInput(custsn, InputTypeEnum.CustSN);
                IDeliveryRepository iDeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                string label = "";
              //  string site = GetValueFromSysSetting("Site");
                string delievery = product.DeliveryNo.Trim();
                if (!product.IsBT)
                {
                    string BTRegId = (string)iDeliveryRepository.GetDeliveryInfoValue(delievery, "RegId");
                    if (BTRegId != null && BTRegId.Length == 3)
                    { BTRegId = BTRegId.Substring(1, 2); }
                    else
                    { BTRegId = ""; }
                    string BTShipTp = iDeliveryRepository.GetDeliveryInfoValue(delievery, "ShipTp");
                    string BTCountry = iDeliveryRepository.GetDeliveryInfoValue(delievery, "Country");

                    if (!string.IsNullOrEmpty(BTRegId) && !string.IsNullOrEmpty(BTShipTp) && !string.IsNullOrEmpty(BTCountry))
                    {
                        //ITC-1360-1527:RegId 为'SNE'时，未能打印CONFIG LABEL                         
                        //if ((BTRegId == "CN" || BTRegId == "AF" || BTRegId == "NE" || BTRegId == "CE") && (BTShipTp == "CTO") && (BTCountry != "JAPAN"))
                        if ((CheckDomesticDN(BTRegId) || BTRegId == "AF" || BTRegId == "NE" || BTRegId == "CE") && (BTShipTp == "CTO") && (BTCountry != "JAPAN"))
                        {
                            label = "ConfigLabel";
                        }
                    }
                }
                return label;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(CommonImpl2)CheckConfigLabel" + product.ProId);
            }
        }

        static public bool CheckDomesticDN(string regId)
        {
            IList<string> domesticRegIdList = new List<string>(){
                "SCN",
                "QCN",
                "CN",
                "QET",
                "ET"
            };
            return domesticRegIdList.Contains(regId);
        }
    
    }
      
}
