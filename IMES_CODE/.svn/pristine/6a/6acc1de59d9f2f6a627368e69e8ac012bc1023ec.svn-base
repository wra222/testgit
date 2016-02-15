/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for UploadShipmentData Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI Upload Shipment Data to SAP.docx –2011/10/26 
 * UC:CI-MES12-SPEC-PAK-UC Upload Shipment Data to SAP.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-11  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
                //与BOM相关的业务
*/

using System;
using System.Collections;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using log4net;
using System.Net;
using System.IO;
using System.Xml;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for UploadShipmentData.
    /// </summary>
    public class _UploadShipmentData : MarshalByRefObject, IUploadShipmentData
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        private IPizzaRepository pizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
        private IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
        private IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        private IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();

        #region IUploadShipmentData Members
        /// <summary>
        /// 获取表格数据信息
        /// </summary>
        public IList<S_RowData_UploadShipmentData> GetTableData(DateTime fromDate, DateTime toDate, bool bAllData)
        {
            logger.Debug("(_UploadShipmentData)GetTableData start, fromDate:" + fromDate.ToString("yyyy-MM-dd") + " toDate:" + toDate.ToString("yyyy-MM-dd") + " bAllData:" + bAllData);
            try
            {
                //系统允许站点定义在SysSetting表，参考如下方法获取：
                //SELECT @UploadSAPStation = Value FROM SysSetting NOLOCK
                //WHERE Name = 'UploadSAPStation'
                //其格式是逗号分隔的站号，例如：85,86,9A,PO,97
                IList<string> allowStationList = new List<string>();
                IList<string> stationStringList = partRepository.GetValueFromSysSettingByName("UploadSAPStation");
                if (stationStringList.Count > 0)
                {
                    allowStationList = stationStringList[0].Split(',');
                }

                IList<S_RowData_UploadShipmentData> ret = new List<S_RowData_UploadShipmentData>();
                IList<Delivery> dlvList = deliveryRepository.GetDeliveriesByShipDateRange(fromDate, toDate);
                foreach (Delivery tmp in dlvList)
                {
                    if (!bAllData && tmp.Status.StartsWith("9"))
                    {
                        continue;
                    }

                    //UC updated(2012-07-31):整机UpLoad Shipment Data to SAP页面除掉Docking的船务数据
                    if (!tmp.ModelName.StartsWith("PC"))
                    {
                        continue;
                    }

                    S_RowData_UploadShipmentData ele;
                    ele.m_bAllowUpload = true;
                    ele.m_date = tmp.ShipDate;
                    ele.m_dn = tmp.DeliveryNo;
                    ele.m_pn = tmp.PoNo;
                    ele.m_model = tmp.ModelName;
                    ele.m_qty = tmp.Qty;
                    ele.m_status = tmp.Status;
                    ele.m_pack = productRepository.GetCombinedQtyByDN(tmp.DeliveryNo);
                    ele.m_paqc = "";
                    IList<string> toCheckList = productRepository.GetProductIDListNeedToCheck(tmp.DeliveryNo);
                    foreach (string str in toCheckList)
                    {
                        ProductQCStatus qcs = productRepository.GetNewestProductQCStatus(str);
                        /*
                         * Answer to: ITC-1360-1840
                         * Description: UC updated.
                         */
                        if (qcs != null && (qcs.Status.Equals("8") || qcs.Status.Equals("A")))
                        {
                            ele.m_paqc = "PQ";
                            break;
                        }
                    }

                    //当Qty<>Pack，或者Status <> ‘88’ 和‘98’时，则该条记录不能被选择，即UI上的Check Box不可选
                    //当PAQC 显示为”PQ” 时，则该条记录也不能被选择
                    if (ele.m_qty != ele.m_pack ||
                        (ele.m_status != "88" && ele.m_status != "98") ||
                        ele.m_paqc == "PQ")
                    {
                        ele.m_bAllowUpload = false;
                    }

                    //当Delivery 结合的Product 的ProductStatus.Station非系统允许站点时，则该条记录不能被选择
                    //实现上：只在之前ele.m_bAllowUpload未置false的情况下才做这步判断，以尽可能减少多余的处理
                    if (ele.m_bAllowUpload)
                    {
                        IList<IProduct> proList = productRepository.GetProductListByDeliveryNo(ele.m_dn);
                        foreach (IProduct p in proList)
                        {
                            if (!allowStationList.Contains(p.Status.StationId))
                            {
                                ele.m_bAllowUpload = false;
                                break;
                            }
                        }
                    }
                    ret.Add(ele);
                }

                return ret;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_UploadShipmentData)GetTableData end, fromDate:" + fromDate.ToString("yyyy-MM-dd") + " toDate:" + toDate.ToString("yyyy-MM-dd") + " bAllData:" + bAllData);
            }            
        }

        /// <summary>
        /// 获取记入c:\serial.txt的信息
        /// </summary>
        /// <param name="dns">DN list</param>
        public IList<string> GetFileData(IList<string> dns)
        {
            logger.Debug("(_UploadShipmentData)GetFileData start");
            bool bNeedCheckEOA = false;
            string fkiPath = "";
            CredentialCache FKICredentialCache = new CredentialCache();

            try
            {
                fkiPath = System.Configuration.ConfigurationManager.AppSettings["FKIServicePath"].Trim();
                if (fkiPath != "")
                {
                    try
                    {
                        string fkiUser = System.Configuration.ConfigurationManager.AppSettings["FKIAuthUser"].Trim();
                        string fkiPwd = System.Configuration.ConfigurationManager.AppSettings["FKIAuthPassword"].Trim();
                        if (fkiUser != "")
                        {
                            FKICredentialCache.Add(new System.Uri(fkiPath), "NTLM", new System.Net.NetworkCredential(fkiUser, fkiPwd));
                        }
                    }
                    catch { }
                    bNeedCheckEOA = true;
                }
            }
            catch { }

            try
            {
                IList<string> ret = new List<string>();
                foreach (string tmpDn in dns)
                {
                    IList<string> subRet = new List<string>();
                    int caseNoByDN = 2; //0 - case A; 1 - case B; 2 - case C
                    
                    Delivery dlv = deliveryRepository.Find(tmpDn);
                    //GetBom,与BOM相关的都尚未确定
                    IList<DNInfoForUI> dlvInfoList = deliveryRepository.GetDNInfoList(tmpDn);
                    string SW = "";
                    foreach (DNInfoForUI tmpInfo in dlvInfoList)
                    {
                        //SW：DeliveryInfo.InfoType=’PartNo’对应的InfoValue值，14位开始取7位
                        if (tmpInfo.InfoType.Equals("PartNo"))
                        {
                            SW = tmpInfo.InfoValue.Substring(13, 7);
                        }
                        if (tmpInfo.InfoType.Equals("Flag") && tmpInfo.InfoValue.Equals("N"))
                        {
                            caseNoByDN = 1;
                        }
                    }
                    IList<IProduct> proList = productRepository.GetProductListByDeliveryNo(tmpDn);
                    foreach (IProduct p in proList)
                    {
                        int caseNo = caseNoByDN;
                        IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(p.Model);
                        IList<IBOMNode> PLBomNodeList = bom.GetFirstLevelNodesByNodeType("PL");
                        foreach (IBOMNode bomNode in PLBomNodeList)
                        {
                            if (bomNode.Part.Descr == "ATT LABEL" || bomNode.Part.Descr == "VERIZON LABEL")
                            {
                                caseNo = 0;
                                break;
                            }
                        }

                        string s = "";

                        string palletNo = "";
                        //UC Exception 1
                        if (p.PalletNo.Equals(""))
                        {
                            throw new FisException("PAK111", new string[] { tmpDn });
                            //subRet.Clear();
                            //subRet.Add(tmpDn + " Pallet Qty is error!");
                            //break;
                        }
                        else
                        {
                            //PalletNo：若Product.PalletNo的前两位等于NA or BA时，PalletNo=’~’+Product.PalletNo从第3位开始取10位；否则PalletNo=Product.PalletNo+’~’
                            if (p.PalletNo.StartsWith("NA") || p.PalletNo.StartsWith("BA"))
                            {
                                palletNo = "~" + p.PalletNo.Substring(2, 10);
                            }
                            else
                            {
                                palletNo = p.PalletNo + "~";
                            }
                        }

                        if (!CheckCOACombine(bom, p, tmpDn))
                        {
                            throw new FisException("PAK158", new string[] { p.CUSTSN, tmpDn });
                        }

                        if (bNeedCheckEOA && !CheckOA3Key(bom, p, fkiPath, FKICredentialCache))
                        {
                            throw new FisException("PAK158", new string[] { p.CUSTSN, tmpDn });
                        }
                        
                        string boxId = "";
                        string MEID = "";
                        string UUID = "";
                        string WM = "";
                        string IMEI = "";
                        string ICCID = "";

                        IList<IMES.FisObject.FA.Product.ProductInfo> pInfoList = new List<IMES.FisObject.FA.Product.ProductInfo>(); 
                        IProduct tmpPrd = productRepository.Find(p.ProId);
                        if (tmpPrd != null)
                        {
                            pInfoList = tmpPrd.ProductInfoes;
                        }

                        foreach (IMES.FisObject.FA.Product.ProductInfo tmpInfo in pInfoList)
                        {
                            //Boxid：ProductInfo里InfoType=BoxId或UCC对应的InfoValue
                            if (tmpInfo.InfoType.Equals("BoxId") || tmpInfo.InfoType.Equals("UCC"))
                            {
                                boxId = tmpInfo.InfoValue;
                                continue;
                            }

                            //MEID：ProductInfo.InfoValue where infoType=’MEID’ (情况B和情况C为空)
                            if (tmpInfo.InfoType.Equals("MEID"))
                            {
                                MEID = (caseNo == 0) ? tmpInfo.InfoValue : "";
                                continue;
                            }

                            //UUID：ProductInfo.InfoValue where infoType=’UUID’ (情况B和情况C为空)
                            if (tmpInfo.InfoType.Equals("UUID"))
                            {
                                UUID = (caseNo == 0) ? tmpInfo.InfoValue : "";
                                continue;
                            }

                            //WM：ProductInfo.InfoValue where infoType=’WM’ (情况B和情况C为空)
                            if (tmpInfo.InfoType.Equals("WM"))
                            {
                                WM = (caseNo == 0) ? tmpInfo.InfoValue : "";
                                continue;
                            }

                            //IMEI：ProductInfo.InfoValue where infoType=’IMEI’(情况B和情况C为空)
                            if (tmpInfo.InfoType.Equals("IMEI"))
                            {
                                IMEI = (caseNo == 0) ? tmpInfo.InfoValue : "";
                                continue;
                            }

                            //ICCID：ProductInfo.InfoValue where infoType=’ICCID’ (情况B和情况C为空)
                            if (tmpInfo.InfoType.Equals("ICCID"))
                            {
                                ICCID = (caseNo == 0) ? tmpInfo.InfoValue : "";
                                continue;
                            }
                        }

                        //Descr：不知道如何获取?? 暂时写””
                        string Descr = "";

                        //ODD:
                        string ODD = "";
                        //try
                        //{
                            ODD = GetODD(bom, tmpDn);
                        //}
                        //catch (FisException e)
                        //{
                            //subRet.Clear();
                            /*
                             * Answer to: ITC-1360-0910
                             * Description: Should use mErrmsg to get error msg of FisException.
                             */
                            //subRet.Add(e.mErrmsg);
                            //break;
                        //}

                        //Wireless and FCCID:
                        string Wireless = "";
                        string FCCID = "";
                        //try
                        //{
                            IList<string> wlAndFcc = GetWirelessAndFCCID(bom, p, tmpDn);
                            Wireless = wlAndFcc[0];
                            FCCID = wlAndFcc[1];
                        //}
                        //catch (FisException e)
                        //{
                            //subRet.Clear();
                            /*
                             * Answer to: ITC-1360-0903, ITC-1360-0913, ITC-1360-0914, ITC-1360-0916, ITC-1360-0922
                             * Description: Should use mErrmsg to get error msg of FisException.
                             */
                            //subRet.Add(e.mErrmsg);
                            //break;
                        //}

                        //ODD_Accession and ODD_Mftld
                        string ODD_Accession = "";
                        string ODD_Mftld = "";
                        //try
                        //{
                            IList<string> oddAccAndMft = GetODDAccessionAndMftId(p);
                            ODD_Accession = oddAccAndMft[0];
                            ODD_Mftld = oddAccAndMft[1];
                        //}
                        //catch (FisException e)
                        //{
                            //subRet.Clear();
                            //subRet.Add(e.mErrmsg);
                            //break;
                        //}

                        //UC Exception 4 (deleted UC)
                        /*  Delete by kaisheng --2012/03/20：miniRun need --
                        if (ODD != "" && (ODD_Accession == "" || ODD_Mftld == ""))
                        {
                            subRet.Clear();
                            subRet.Add("ODD Accession No. or ManufactureId missing!!" + tmpDn);
                            break;
                        }
                        */
                        //AST:unit绑定的AST sn，IMES_FA.Product_Part.PartSn where BomNodeType=’AT’
                        string AST = "";
                        foreach (IProductPart ele in p.ProductParts)
                        {
                            if (ele.BomNodeType == "AT")
                            {
                                AST = ele.PartSn;
                                break;
                            }
                        }

                        //MFD：机器产生CustomerSN的时间，格式为YYYYMMDD。数据获取方法ProductLog.Cdt where Station=’58’(情况B为空)
                        string MFD = "";
                        if (caseNo != 1)
                        {
                            /*
                             * Answer to: ITC-1360-1449
                             * Description: Get logs by station.
                             */
                            IList<ProductLog> logs = productRepository.GetProductLogs(p.ProId, "58");
                            if (logs.Count > 0)
                            {
                                MFD = logs[0].Cdt.ToString("yyyyMMdd");
                            }
                        }
                        
                        s += dlv.DeliveryNo.Substring(0, 10);
                        s += "~";
                        s += dlv.DeliveryNo.Substring(dlv.DeliveryNo.Length - 6);
                        s += "~";
                        s += dlv.PoNo;
                        s += "~";
                        s += palletNo;
                        s += "~";
                        s += p.CUSTSN;
                        s += "~";
                        s += boxId;
                        s += "~1~";
                        s += p.Model.StartsWith("PC") ? "C" : "O";
                        s += "~";
                        s += Descr;
                        s += "~";
                        s += AST;
                        s += "~";
                        s += Wireless;
                        s += "~";
                        s += ODD;
                        s += "~";
                        s += ODD_Accession;
                        s += "~";
                        s += ODD_Mftld;
                        s += "~";
                        s += FCCID;
                        s += "~~~";
                        s += (caseNo == 0) ? p.MAC : "";
                        s += "~~~~~";
                        s += MEID;
                        s += "~";
                        s += MFD;
                        s += "~";
                        s += UUID;
                        s += "~";
                        s += WM;
                        s += "~";
                        s += IMEI;
                        s += "~";
                        s += (caseNo == 0) ? "1.0" : "";
                        s += "~";
                        s += (caseNo == 0) ? "EMBDSIM" : "";
                        s += "~";
                        s += (caseNo == 0) ? SW : "";
                        s += "~";
                        s += ICCID;
                        s += "~";
                        subRet.Add(s);
                    }
                    foreach (string ele in subRet)
                    {
                        ret.Add(ele);
                    }
                }
                return ret;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_UploadShipmentData)GetFileData end");
            }
        }
        
        /// <summary>
        /// 成功生成文件后，每个Delivery 需要更新状态
        /// </summary>
        public void ChangeDNStatus(IList<string> dns)
        {
            logger.Debug("(_UploadShipmentData)ChangeDNStatus start");
            try
            {
                string[] input = new string[dns.Count];
                for (int i = 0; i < dns.Count; i++)
                {
                    input[i] = dns[i];
                }
                deliveryRepository.UpdateDeliveryForStatusChange(input, "9");
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_UploadShipmentData)ChangeDNStatus end");
            }
        }

        private string GetODD(IHierarchicalBOM bom, string dn)
        {
            //ODD:在Model BOM中按照Model取下阶，得到
            //BomNodeType=’AV’ and PartInfo.InfoType=’Descr’ and PartInfo.InfoValue=’ODD’
            //的记录，获取PartInfo.InfoType=’AV’对应的InfoValue值，当存在多条时，用’\’分割串起来
            string ret = "";
            IList<IBOMNode> AVBomNodeList = bom.GetFirstLevelNodesByNodeType("AV");
            foreach (IBOMNode bomNode in AVBomNodeList)
            {
                /*
                 * Answer to: ITC-1360-0907
                 */
                string attrDescr = getAttrValue(bomNode.Part, "Descr");
                if (attrDescr != null && attrDescr == "ODD")
                {
                    string val = getAttrValue(bomNode.Part, "AV");
                    if (val != null && val != "") ret += val + "\\";
                    //UC Exception 3
                    else if (!bomNode.Part.PN.StartsWith("ZM"))
                    {
                        throw new FisException("PAK113", new string[] { dn });
                    }
                }
            }

            if (ret.EndsWith("\\")) ret = ret.Remove(ret.Length - 1);
            return ret;
        }

        private bool CheckCOACombine(IHierarchicalBOM bom, IProduct p, string dn)
        {
            bool bNeedCOA = false;
            string pnCOA = "";
            IList<IBOMNode> P1BomNodeList = bom.GetFirstLevelNodesByNodeType("P1");
            foreach (IBOMNode bomNode in P1BomNodeList)
            {
                if (bomNode.Part.Descr.StartsWith("COA"))
                {
                    bNeedCOA = true;
                    pnCOA = bomNode.Part.PN;
                    break;
                }
            }

            if (!bNeedCOA) return true;
            
            if (partRepository.CheckExistInternalCOA(dn, "DN")) return true;

            if (partRepository.CheckExistInternalCOA(p.CUSTSN, "SN")) return true;

            foreach (IProductPart tmp in p.ProductParts)
            {
                if (tmp.PartID == pnCOA) return true;
            }

            return false;
        }

        private bool CheckOA3Key(IHierarchicalBOM bom, IProduct p, string fkiPath, CredentialCache FKICredentialCache)
        {
            bool bWIN8 = false;
            IList<IBOMNode> P1BomNodeList = bom.GetFirstLevelNodesByNodeType("P1");
            foreach (IBOMNode bomNode in P1BomNodeList)
            {
                if (bomNode.Part.Descr.StartsWith("ECOA"))
                {
                    bWIN8 = true;
                    break;
                }
            }

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
            req.AllowAutoRedirect = false;
            req.CookieContainer = new CookieContainer();
            //req.Headers.Add("Content-Type", "application/plain; charset=utf-8");
            req.Accept = "*/*";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            req.KeepAlive = false;
            req.Method = "POST";
            req.Credentials = FKICredentialCache;

            string XMLInputData = "<?xml version='1.0' encoding='utf-8' ?>";
            XMLInputData += "<UnitStatusRequest xmlns='http://HP.ITTS.OA30/digitaldistribution/2011/08'>";
            XMLInputData += "<HPSerialNumber>";
            XMLInputData += p.CUSTSN;
            XMLInputData += "</HPSerialNumber>";
            XMLInputData += "<ProductKeyID>";
            XMLInputData += p.ProId;
            XMLInputData += "</ProductKeyID>";
            XMLInputData += "</UnitStatusRequest>";


            MemoryStream memoryStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(memoryStream);
            writer.Write(XMLInputData);
            using (Stream stream = req.GetRequestStream())
            {
                memoryStream.WriteTo(stream);
            }

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            XmlTextReader xmlr = new XmlTextReader(res.GetResponseStream());
            string status = "";
            bool bFound = false;
            while (xmlr.Read())
            {
                if (xmlr.NodeType == System.Xml.XmlNodeType.Element && xmlr.LocalName.Equals("ProductKeyStateName"))
                {
                    xmlr.Read();
                    status = xmlr.Value;

                    if (status == "Bound" || status == "NotifiedBound" || status == "PendingBound")
                    {
                        bFound = true;
                        continue;
                    }

                    if (status != "Returned" && status != "NotifiedReturned" && status != "PendingReturn")
                    {
                        xmlr.Close();
                        return false;
                    }
                }
            }
            xmlr.Close();

            return bFound;
        }

        private IList<string> GetWirelessAndFCCID(IHierarchicalBOM bom, IProduct p, string dn)
        {
            //Wireless:在Model BOM中按照Model取下阶，得到
            //BomNodeType=’AV’ and PartInfo.InfoType=’Descr’ and PartInfo.InfoValue=’BLUETOOTH’
            //的记录，Wireless=获取PartInfo.InfoType=’AV’对应的InfoValue值(只考虑得到一条)。
            string ret1 = "";
            string ret2 = "";
            IList<IBOMNode> AVBomNodeList = bom.GetFirstLevelNodesByNodeType("AV");
            foreach (IBOMNode bomNode in AVBomNodeList)
            {
                string attrDescr = getAttrValue(bomNode.Part, "Descr");
                if (attrDescr != null && attrDescr == "BLUETOOTH")
                {
                    string attrAV = getAttrValue(bomNode.Part, "AV");
                    if (attrAV != null && attrAV != "")
                    {
                        ret1 = attrAV;
                        break;
                    }
                }
            }

            //FCCID:当Wireless<>””时且该model在Model BOM中的下阶存在
            //BomNodeType in('P1','BM','KP') and Part.Descr like ’BLUETOOTH%’
            //时，才需要获取FCCID
            bool bNeedFCCID = false;
            for (int loop = 0; loop < 3; loop++)
            {
                if (bNeedFCCID) break;
                IList<IBOMNode> BomNodeList = new List<IBOMNode>();
                if (loop == 0) BomNodeList = bom.GetFirstLevelNodesByNodeType("P1");
                if (loop == 1) BomNodeList = bom.GetFirstLevelNodesByNodeType("BM");
                if (loop == 2) BomNodeList = bom.GetFirstLevelNodesByNodeType("KP");

                foreach (IBOMNode bomNode in BomNodeList)
                {
                    if (bomNode.Part.Descr.StartsWith("BLUETOOTH"))
                    {
                        if (ret1 != "")
                        {
                            bNeedFCCID = true;
                        }
                        else
                        {
                            //UC Exception 7
                            throw new FisException("PAK116", new string[] { dn });
                        }
                        break;
                    }
                }
            }

            //FCCID获取方法： Model BOM从model展2阶，得到
            //BomNodeType=’KP’ and Part.Descr like ’BLUETOOTH%’ and 该part对应的’FCC ID’不等于空
            //的记录，FCCID=’FCC ID’对应的属性值
            if (bNeedFCCID)
            {
                bool bFoundFCCID = false;
                IList<IBOMNode> L1BomNodeList = bom.FirstLevelNodes;    //展1阶
                foreach (IBOMNode bomNode in L1BomNodeList)
                {
                    if (bFoundFCCID) break;
                    IList<IBOMNode> L2BomNodeList = bomNode.Children;   //展2阶
                    foreach (IBOMNode bomNode1 in L2BomNodeList)
                    {
                        if (bFoundFCCID) break;
                        IPart tmpPart = bomNode1.Part;
                        if (tmpPart.BOMNodeType == "KP" && tmpPart.Descr.StartsWith("BLUETOOTH"))
                        {
                            string val = getAttrValue(tmpPart, "FCC ID");
                            if (val != null && val != "")
                            {
                                ret2 = val;
                                bFoundFCCID = true;
                            }
                        }
                    }
                }
                //UC Exception 2
                if (!bFoundFCCID)
                {
                    throw new FisException("PAK112", new string[] { p.Model });
                }
            }

            //注：只要FCCID=’’时，Wireless=’’
            if (ret2 == "") ret1 = "";

            //再来获取Wireless part:当Model BOM按照Model展下阶存在Part.Descr=’WIRELESS’时，
            //若获取Model下阶BomNodeType= ’AV’ and 其Descr属性值like ‘WIRELESS%’的part对应的AV属性值@wireless不等于””时，
            //作以下处理：
            bool bExistWLPart = false;
            string wlval = "";
            foreach (IBOMNode bomNode in bom.FirstLevelNodes)
            {
                if (bomNode.Part.Descr == "WIRELESS")
                {
                    bExistWLPart = true;
                    break;
                }
            }

            if (bExistWLPart)
            {
                foreach (IBOMNode bomNode in AVBomNodeList)
                {
                    string attrDescr = getAttrValue(bomNode.Part, "Descr");
                    if (attrDescr != null && attrDescr.StartsWith("WIRELESS"))
                    {
                        string attrAV = getAttrValue(bomNode.Part, "AV");
                        if (attrAV != null && attrAV != "")
                        {
                            wlval = attrAV;
                            break;
                        }
                        //UC Exception 5
                        else
                        {
                            throw new FisException("PAK114", new string[] { dn });
                        }
                    }
                }
            }

            if (wlval != "")
            {
                //If Wireless=’’
                //  Wireless=@wireless
                //  FCCID=IMES_FA..Product_Part.PartNo对应的Part.BomNodeType=’KP’ and Part.Descr like ‘WIRELESS%’ and [FCC ID]属性值不等于空的part，得到其[FCC ID]属性值@fccid
                //Else
                //  Wireless=Wireless+’\’+@wireless
                //  FCCID=FCCID+’\’+@fccid

                string fccid = "";
                bool bFoundFCCID = false;
                foreach (IProductPart part in p.ProductParts)
                {
                    IPart tmp = partRepository.GetPartByPartNo(part.PartID);
                    if (tmp.BOMNodeType == "KP" && tmp.Descr.StartsWith("WIRELESS"))
                    {
                        string attrFCCID = getAttrValue(tmp, "FCC ID");
                        if (attrFCCID != null && attrFCCID != "")
                        {
                            bFoundFCCID = true;
                            fccid = attrFCCID;
                            break;
                        }
                        //UC Exception 6
                        else
                        {
                            throw new FisException("PAK115", new string[] { dn });
                        }
                    }
                }

                /*
                 * Answer to: ITC-1360-0903
                 * Description: Throw fis exception if WIRELESS not empty but FCCID is null.
                 */
                //UC Exception 8
                if (!bFoundFCCID)
                {
                    throw new FisException("PAK117", new string[] { dn });
                }

                if (ret1 == "")
                {
                    ret1 = wlval;
                    ret2 = fccid;
                }
                else
                {
                    ret1 += "\\" + wlval;
                    ret2 += "\\" + fccid;
                }
            }

            IList<string> ret = new List<string>();
            ret.Add(ret1);
            ret.Add(ret2);
            return ret;
        }

        private IList<string> GetODDAccessionAndMftId(IProduct p)
        {
            //ODD_Accession and ODD_Mftld：
            //IMES_FA..Product_Part(Product_Part.PartSn)和Pizza_Part(Product.PizaaID和ProductInfo.InfoValue where InfoType=’KIT2’得到绑定的PizzaID，再从Pizza_Part得到PartSn)中的Value第一位是’7’或’N’，且Part.BomNodeType=’KP’的所有part
            //根据所有PartNo得到其对应的VENDOR属性值@vendor(PartInfo.InfoValue where InfoType=’VENDOR’)，再在DescType表中得到其AcsNo属性值@ascno及MftId属性值@mftid(DescType.Description where Tp=’VD’ and Code=@vendor，Description是用”~”分割的属性值对)
            //对获取的记录集作distinct处理
            //ODD_Accession=@ascno(存在多个时用’\’分割串起来)
            //ODD_MftId=@mftid(存在多个时用’\’分割串起来)
            string ret1 = "";
            string ret2 = "";
            for (int loop = 0; loop < 2; loop++)
            {
                IList<IProductPart> partList = new List<IProductPart>();
                if (loop == 0) partList = p.ProductParts;
                if (loop == 1)
                {
                    Pizza findPizza = pizzaRepository.Find(p.PizzaID);
                    if (findPizza != null)
                    {
                        partList = findPizza.PizzaParts;
                    }
                }
                foreach (IProductPart part in partList)
                {
                    if ((part.PartSn.StartsWith("7") || part.PartSn.StartsWith("N")) && part.BomNodeType == ("KP"))
                    {
                        string vendor = getAttrValue(partRepository.GetPartByPartNo(part.PartID), "VENDOR");
                        if (vendor != null && vendor != "")
                        {
                            IList<string> descList = bomRep.GetDescriptionOfDescTypeListByTpAndCode("VD", vendor);
                            foreach (string str in descList)
                            {
                                foreach (string str1 in str.Split('~'))
                                {
                                    if (str1.StartsWith("AcsNo="))
                                    {
                                        string thisval = str1.Substring(6) + "\\";
                                        //Distinct处理
                                        if (!ret1.StartsWith(thisval) && !ret1.Contains("\\" + thisval))
                                        {
                                            ret1 += thisval;
                                        }
                                    }
                                    if (str1.StartsWith("MftId="))
                                    {
                                        string thisval = str1.Substring(6) + "\\";
                                        //Distinct处理
                                        if (!ret2.StartsWith(thisval) && !ret2.Contains("\\" + thisval))
                                        {
                                            ret2 += thisval;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (ret1.EndsWith("\\")) ret1 = ret1.Remove(ret1.Length - 1);
            if (ret2.EndsWith("\\")) ret2 = ret2.Remove(ret2.Length - 1);
            IList<string> ret = new List<string>();
            ret.Add(ret1);
            ret.Add(ret2);
            return ret;
        }

        private string getAttrValue(IPart part, string name)
        {
            foreach (PartInfo ele in part.Attributes)
            {
                if (ele.InfoType == name)
                {
                    return ele.InfoValue;
                }
            }
            return null;
        }
        #endregion
    }
}
