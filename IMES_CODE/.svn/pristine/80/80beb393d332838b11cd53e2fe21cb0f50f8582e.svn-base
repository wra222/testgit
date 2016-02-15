using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using System.Collections;
using log4net;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.FisBOM;
using IMES.Route;
using IMES.Infrastructure.Extend;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.StandardWeight;
using System.Net;
using System.Xml;
using IMES.Infrastructure.UnitOfWork;


namespace IMES.Station.Implementation
{
    public class CommonImpl2 : MarshalByRefObject, ICommonImpl2
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IList<ConstValueTypeInfo> GetConstValueTypeByType(string type)
        {
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueTypeInfo> constValueTypeList=partRep.GetConstValueTypeList(type);
            return constValueTypeList;
        }

        public IList<PalletType> GetPalletType(string palletNo,string dnNo)
        {
            return IMES.Infrastructure.Utility.Common.CommonUti.GetPalletType(palletNo, dnNo);
            #region disable code
            //IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            //IPalletRepository palletRepository =RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            //IPalletTypeRepository palletTypeRepository =RepositoryFactory.GetInstance().GetRepository<IPalletTypeRepository, PalletType>();
            //string ShipWay="";
            //string RegId="";
            //string StdPltFullQty = "";
            //if (string.IsNullOrEmpty(dnNo))
            //{ 
            //    IList<DeliveryPallet> dpList = palletRepository.GetDeliveryPallet(palletNo);
            //    if ((dpList == null) || (dpList.Count == 0))
            //    {
            //        List<string> errpara1 = new List<string>();
            //        errpara1.Add(palletNo);
            //        throw new FisException("CHK241", errpara1);
            //    }
            //    dnNo = dpList[0].DeliveryID;
            //}
            //Delivery dn = deliveryRep.GetDelivery(dnNo);
            //string model = dn.ModelName;
            //int pltDeliveryQty = deliveryRep.GetSumDeliveryQtyOfACertainPallet(palletNo);
            //DeliveryEx dnEx = dn.DeliveryEx;
            //if (dn.ModelName.Substring(0,2)=="PC")
            //{
            //    if (dnEx.Carrier == "FDE" || dnEx.Carrier == "FEPL")
            //    {
            //        RegId = dnEx.Carrier;
            //    }
            //    else
            //    {
            //        RegId = dnEx.MessageCode;
            //    }
            //    StdPltFullQty = dnEx.StdPltFullQty;
            //    ShipWay = dnEx.ShipWay;
            //}
            //else
            //{
            //    IList<DeliveryInfo> lstDnInfo = dn.DeliveryInfoes;
            //    IList<string> tmpArr;
            //    tmpArr = lstDnInfo.Where(x => x.InfoType == "ShipWay").Select(x => x.InfoValue).ToList();
            //    ShipWay = tmpArr.Count == 0 ? "" : tmpArr[0];
            //    tmpArr = lstDnInfo.Where(x => x.InfoType == "RegId").Select(x => x.InfoValue).ToList();
            //    RegId = tmpArr.Count == 0 ? "" : tmpArr[0];
            //    tmpArr = lstDnInfo.Where(x => x.InfoType == "PalletQty").Select(x => x.InfoValue).ToList();
            //    StdPltFullQty = tmpArr.Count == 0 ? "0" : tmpArr[0];
            //    double d;
            //    if (double.TryParse(StdPltFullQty, out d))
            //    {
            //        StdPltFullQty = Math.Floor(Math.Abs(d)).ToString();
            //    }
            //}

            //IList<PalletType> lstPalletType = palletTypeRepository.GetPalletType(ShipWay, RegId, StdPltFullQty, pltDeliveryQty);
            //return lstPalletType;
            #endregion
        }
        public string GetSite()
        {
            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> valueList = PartRepository.GetValueFromSysSettingByName("Site");
            if (valueList.Count == 0)
            { 
                throw new FisException("PAK095", new string[] { "Site" }); 
            }
            return valueList[0].Trim(); //
        }
        public bool CheckIsWIN8(IHierarchicalBOM bom)
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
       public string GetValueFromSysSetting(string name)
        {
            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> valueList = PartRepository.GetValueFromSysSettingByName(name);
            if (valueList.Count == 0)
            { throw new FisException("PAK095", new string[] { name }); }
            return valueList[0].Trim(); //
        
        }
        public string CheckPodLabel(string custsn)
        {
            IProduct product=null;
            try
            {
                
                IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                product = CommonImpl.GetProductByInput(custsn, CommonImpl.InputTypeEnum.CustSN);
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
        public string CheckConfigLabel(string custsn)
        {
            IProduct product = null;
            try
            {
                IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                product = CommonImpl.GetProductByInput(custsn, CommonImpl.InputTypeEnum.CustSN);
                IDeliveryRepository iDeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                string label = "";
              //  string site = GetValueFromSysSetting("Site");
                string delievery = product.DeliveryNo.Trim();
                if (!product.IsBT)
                {
                    string BTRegId = (string)iDeliveryRepository.GetDeliveryInfoValue(delievery, "RegId");

                    //if (BTRegId != null && BTRegId.Length == 3)
                    //{ BTRegId = BTRegId.Substring(1, 2); }
                    //else
                    //{ BTRegId = ""; }

                    string BTShipTp = iDeliveryRepository.GetDeliveryInfoValue(delievery, "ShipTp");
                    string BTCountry = iDeliveryRepository.GetDeliveryInfoValue(delievery, "Country");

                    if (!string.IsNullOrEmpty(BTRegId) && !string.IsNullOrEmpty(BTShipTp) && !string.IsNullOrEmpty(BTCountry))
                    {
                        //ITC-1360-1527:RegId 为'SNE'时，未能打印CONFIG LABEL
                        //if ((BTRegId == "CN" || BTRegId == "AF" || BTRegId == "NE" || BTRegId == "CE") && (BTShipTp == "CTO") && (BTCountry != "JAPAN"))
                        bool bRegId = false;
                        IList<ConstValueTypeInfo> constValueTypeList = PartRepository.GetConstValueTypeList("ConfigLabelRegionId", BTRegId.Trim());
                        if (constValueTypeList != null && constValueTypeList.Count > 0)
                        {
                            bRegId = true;
                        }

                       // if ((CommonUtl.CheckDomesticDN(BTRegId) || BTRegId == "AF" || BTRegId == "NE" || BTRegId == "CE") && (BTShipTp == "CTO") && (BTCountry != "JAPAN"))

                        if (bRegId  && BTShipTp == "CTO" &&  BTCountry != "JAPAN")
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

        public bool CheckOA3Key(IHierarchicalBOM bom,  IProduct p, string fkiPath, CredentialCache FKICredentialCache, string editor, UnitOfWork uow)
        {
            bool bWIN8 = false;
         //   IList<IBOMNode> P1BomNodeList = bom.GetFirstLevelNodesByNodeType("P1");
            //foreach (IBOMNode bomNode in P1BomNodeList)
            //{
            //    if (bomNode.Part.Descr.StartsWith("ECOA"))
            //    {
            //        bWIN8 = true;
            //        break;
            //    }
            //}
            CommonImpl2 cm2 = new CommonImpl2();
            bWIN8 = cm2.CheckIsWIN8(bom);


            if (!bWIN8) return true;

            if (fkiPath == "") return true; //Switch off checking by FKIServer.
            string thisURI = "";
            if (fkiPath.EndsWith("/"))
            {
                thisURI = fkiPath + "UnitStatus";
            }
            else
            {
                thisURI = fkiPath + "/UnitStatus";
            }

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(thisURI);
            req.AllowAutoRedirect = true;
            req.CookieContainer = new CookieContainer();
            req.ContentType = "application/plain; charset=utf-8";
            req.Accept = "*/*";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            req.KeepAlive = true;
            req.Method = "POST";
            req.Credentials = FKICredentialCache.GetCredential(new Uri(fkiPath), "NTLM");

            string XMLInputData = "<?xml version='1.0' encoding='utf-8' ?>";
            XMLInputData += "<UnitStatusRequest xmlns='http://HP.ITTS.OA30/digitaldistribution/2011/08'>";
            XMLInputData += "<HPSerialNumber>";
            XMLInputData += p.CUSTSN;
            XMLInputData += "</HPSerialNumber>";
            XMLInputData += "<ProductKeyID>";
            XMLInputData += "";
            XMLInputData += "</ProductKeyID>";
            XMLInputData += "</UnitStatusRequest>";
            Encoding encoding = Encoding.Default;
            byte[] buffer = encoding.GetBytes(XMLInputData);
            req.ContentLength = buffer.Length;
            req.GetRequestStream().Write(buffer, 0, buffer.Length);

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            XmlTextReader xmlr = new XmlTextReader(res.GetResponseStream());
            string status = "";
            string rc = "";
            bool bError = false;
            bool bFound = false;
            StringBuilder str = new StringBuilder("Formatted Response:\n");

            while (xmlr.Read())
            {
                switch (xmlr.NodeType)
                {
                    case XmlNodeType.Element:
                        if (xmlr.IsEmptyElement)
                            str.AppendFormat("<{0}/>", xmlr.Name);
                        else
                            str.AppendFormat("<{0}>", xmlr.Name);
                        break;
                    case XmlNodeType.Text:
                        str.Append(xmlr.Value);
                        break;
                    case XmlNodeType.CDATA:
                        str.AppendFormat("<![CDATA[{0}]]>", xmlr.Value);
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        str.AppendFormat("<?{0} {1}?>", xmlr.Name, xmlr.Value);
                        break;
                    case XmlNodeType.Comment:
                        str.AppendFormat("<!--{0}-->", xmlr.Value);
                        break;
                    case XmlNodeType.XmlDeclaration:
                        str.AppendFormat("<?xml version='1.0'?>");
                        break;
                    case XmlNodeType.DocumentType:
                        str.AppendFormat("<!DOCTYPE{0} [{1}]>", xmlr.Name, xmlr.Value);
                        break;
                    case XmlNodeType.EntityReference:
                        str.Append(xmlr.Name);
                        break;
                    case XmlNodeType.EndElement:
                        str.AppendFormat("</{0}>", xmlr.Name);
                        break;
                    case XmlNodeType.Whitespace:
                        str.Append("\n");
                        break;
                }

                if (xmlr.NodeType == System.Xml.XmlNodeType.Element && xmlr.LocalName.Equals("ReturnCode"))
                {
                    xmlr.Read();
                    str.Append(xmlr.Value);
                    rc = xmlr.Value.Trim();
                    if (rc != "000") bError = true;
                    continue;
                }

                if (xmlr.NodeType == System.Xml.XmlNodeType.Element && xmlr.LocalName.Equals("ReturnMessage"))
                {
                    if (bError)
                    {
                        xmlr.Read();
                        str.Append(xmlr.Value);
                        string msg = xmlr.Value;
                        xmlr.Close();
                        throw new Exception("[" + rc + "]:" + msg);
                    }
                }

                if (xmlr.NodeType == System.Xml.XmlNodeType.Element && xmlr.LocalName.Equals("ProductKeyStateName"))
                {
                    xmlr.Read();
                    str.Append(xmlr.Value);
                    status = xmlr.Value;
                    // Marked this by Benson at 2013/3/8
                    //  if (status == "NotifiedBound")
                    if (status == "NotifiedBound" || status == "FKIErrorBound")
                    {
                        //只能有一行状态为"Bound"/"NotifiedBound"/"PendingBound" --OLD
                        //只能有一行状态为"Bound"/"NotifiedBound"/"PendingBound/FKIErrorBound" --NEW add by Benson at 2013/3/8

                        if (bFound)
                        {
                            xmlr.Close();
                            return false;
                        }
                        bFound = true;
                        continue;
                    }


                    if (status == "Bound" || status == "PendingBound")
                    {
                        //只能有一行状态为"Bound"/"NotifiedBound"/"PendingBound"
                        if (bFound)
                        {
                            xmlr.Close();
                            return false;
                        }
                        bFound = true;

                        //对于状态为Bound/PendingBound的Product需要记录到ProductInfo中
                        IMES.FisObject.FA.Product.ProductInfo item = new IMES.FisObject.FA.Product.ProductInfo();
                        item.ProductID = p.ProId;
                        item.InfoType = "Win8KeyState";
                        item.InfoValue = status;
                        item.Editor = editor;
                       IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                        if (productRepository.CheckExistProductInfo(p.ProId, "Win8KeyState"))
                        {
                            IMES.FisObject.FA.Product.ProductInfo cond = new IMES.FisObject.FA.Product.ProductInfo();
                            cond.ProductID = p.ProId;
                            cond.InfoType = "Win8KeyState";
                            productRepository.UpdateProductInfoDefered(uow, item, cond);
                        }
                        else
                        {
                            productRepository.InsertProductInfoDefered(uow, item);
                        }

                        continue;
                    }
                    //Returned 或PendingReturn或NotifiedReturned或FKIErrorReturn --Mantis0001691
                    // Marked this by Benson at 2013/3/8
                    //   if (status != "Returned" && status != "NotifiedReturned" && status != "PendingReturn")
                    if (status != "Returned" && status != "NotifiedReturned" && status != "PendingReturn" && status != "FKIErrorReturn")
                    {
                        xmlr.Close();
                        return false;
                    }
                }
            }
            xmlr.Close();
            Console.Write(str.ToString());
            return bFound;
        }
    }
      
}
