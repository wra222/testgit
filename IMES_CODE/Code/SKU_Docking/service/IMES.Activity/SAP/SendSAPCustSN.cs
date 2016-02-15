using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.FA.Product;
using IMES.SAP.Interface;
using IMES.Infrastructure.Extend.Utility;
using System.Text.RegularExpressions;

using System.Net;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.Common.FisBOM;
using System.Text;
using System.Xml;
using IMES.Infrastructure.UnitOfWork;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// Send SAP CUSTSN
    /// </summary>
    public partial class SendSAPCustSN : BaseActivity
	{
        /// <summary>
        /// Constructor
        /// </summary>
        public SendSAPCustSN()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 单条还是成批插入
        /// </summary>
        public static DependencyProperty HandleRollBackDnStatusProperty = DependencyProperty.Register("HandleRollBackDnStatus", typeof(string), typeof(SendSAPCustSN), new PropertyMetadata("88"));
        /// <summary>
        /// Single PalletNo
        /// </summary>
        [DescriptionAttribute("HandleRollBackDnStatus")]
        [CategoryAttribute("HandleRollBackDnStatus Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string HandleRollBackDnStatus
        {
            get
            {
                return ((string)(base.GetValue(HandleRollBackDnStatusProperty)));
            }
            set
            {
                base.SetValue(HandleRollBackDnStatusProperty, value);
            }
        }

        /// <summary>
        /// Send SAP CUSTSN
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            IList <string> deliveryNoList = null;
            try
            {
                deliveryNoList = utl.IsNull<IList<string>>(session, Session.SessionKeys.DeliveryNoList);
                IList<Delivery> deliveryList = utl.IsNull<IList<Delivery>>(session, Session.SessionKeys.DeliveryList);
              IList<UploadDNInfo> uploadDnInfo = utl.IsNull<IList<UploadDNInfo>>(session, Session.SessionKeys.UploadDNInfoList);

                #region 抓取Product.CUSTSN
                //IList<ProductModel> productModelList=  prodRep.GetProductListByDeliveryNoList(deliveryNoList);
                //IList<ConstValueInfo> procRegList= utl.GetConstValueListByType("ProcReg", "");     
                #endregion

                #region 產生SAP WebService Structure
                IMES.SAP.Interface.ISAPService sap = ServiceAgent.getInstance().GetObjectByName<ISAPService>(ServiceConstant.SAPService);
                
                string txId= DateTime.Now.ToString("yyyyMMddHHmmssfff");
                IList<UploadSAPSN> uploadSAPSNList = GenUploadSNData(uploadDnInfo, this.Editor, txId);

                #region disable code
                //foreach (string item in uploadSnDataList)
                //{
                //    #region Data Spec
                //    //1	DN
                //    //2	Item
                //    //3	Customer PO number
                //    //4	Pallet Number
                //    //5	Carton Number
                //    //6	序號
                //    //7	Box id
                //    //8	Qty
                //    //9	Type
                //    //10	Pallet Box ID for UCC128
                //    //11	ASSET Numbers
                //    //12	Wireless Part Field
                //    //13	Optical Device Field
                //    //14	Assession Number Field
                //    //15	Manufactory ID
                //    //16	FCCID
                //    //17	Warranty Number
                //    //18	Warranty Date
                //    //19	MAC address
                //    //20	Customer Pallet#(FSC)
                //    //21	ordinal num for pallet
                //    //22	COA label
                //    //23	Software PN
                //    //24	Hex MEID
                //    //25	Manufacture date
                //    //26	UUID
                //    //27	MAC address2
                //    //28	IMEI CODE
                //    //29	BUILDER VER
                //    //30	SIMLOCK CODe
                //    //31	SOFTWARE VER
                //    //32	ICCID
                //    //33	ERAN
                //    //34	WWAN / WLAN (2nd MAC)
                //    //35	UPS Tracking Number
                //    #endregion
                //    string[] data = item.Split(new char[] { '~' });
                //    UploadSAPSN sapSN = new UploadSAPSN
                //    {
                //        //SERIALNUMBER = txId,
                //        //DN = delivery.DeliveryNo.Substring(0,10),
                //        //DLV_ITM = delivery.DeliveryNo.Substring(10, 6),
                //        //PO_NO=delivery.PoNo,
                //        //PALLET_NO= item.PalletNo.Substring(0,2)=="NA"?"":  item.PalletNo,
                //        //CARTON_NO = item.PalletNo.Substring(0, 2) == "NA" ?item.PalletNo.Substring(2)  :"",
                //        //SERNO=item.CustSN,
                //        //BOX_ID=item.CartonNo,
                //        //QTY="1",
                //        //TYPE=utl.CheckModelByProcReg(procRegList, item.Model,true  )?"C":"O"
                //    };

                //   uploadSAPSNList.Add(sapSN);
                //}
                #endregion



                #endregion


                #region 送資料給SAP
                ReplyUploadSAPSN reply = sap.UploadSapCustSN(uploadSAPSNList);
                var failHeaderList = reply.HeaderList.Where(x => x.RESULT != "0").ToList();
                if ( failHeaderList!=null &&  failHeaderList.Count>0)
                {
                    var failItemList = reply.ItemList.Where(x => x.RESULT != "0").ToList();

                    throw new FisException("CQCHK50080", new List<string> { string.Join("\n", failItemList.Select(x => x.DLV_NO + x.DLV_ITM + "~" +x.SERNO+"~" + x.MESSAGE).ToArray()) });

                }

                #endregion

            }
            catch (Exception e)
            {
                if (!string.IsNullOrEmpty(this.HandleRollBackDnStatus) && 
                    deliveryNoList!=null && 
                    deliveryNoList.Count>0)
                {
                    dnRep.UpdateMultiDeliveryForStatusChange(deliveryNoList.ToArray(), this.HandleRollBackDnStatus);
                }
                throw e;
            }
            return base.DoExecute(executionContext);
        }


        /// <summary>
        /// 获取记入c:\serial.txt的信息
        /// </summary>
        /// <param name="dns">DN list</param>
        /// <param name="editor">editor</param>
        /// <param name="txId">txId</param>
        private IList<UploadSAPSN> GenUploadSNData(IList<UploadDNInfo> dns, string editor, string txId)
        {
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IList<UploadSAPSN> ret = new List<UploadSAPSN>();

            Session session = CurrentSession;

            bool bNeedCheckEOA = false;
            bool bNeedCheckOA3 = false;
            string fkiPath = "";
            //ignore the certificate check (FOR CQ)
            #region Get OA3 setting value
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
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
                            if (fkiUser.Contains("\\"))
                            {
                                string user = fkiUser.Substring(fkiUser.IndexOf('\\') + 1);
                                string domain = fkiUser.Substring(0, fkiUser.IndexOf('\\'));
                                FKICredentialCache.Add(new System.Uri(fkiPath), "NTLM", new System.Net.NetworkCredential(user, fkiPwd, domain));
                            }
                            else
                            {
                                FKICredentialCache.Add(new System.Uri(fkiPath), "NTLM", new System.Net.NetworkCredential(fkiUser, fkiPwd));
                            }
                        }
                    }
                    catch { }
                    bNeedCheckEOA = true;
                }
            }
            catch { }
            #endregion

            #region Check Need Send OA3 or not
            try
            {

                IList<ConstValueInfo> cvInfo = new List<ConstValueInfo>();

                ConstValueInfo cvCond = new ConstValueInfo();
                cvCond.type = "CheckOA3State";
                cvCond.name = "OA3UpSAP";
                cvInfo = partRepository.GetConstValueInfoList(cvCond);
                if (cvInfo == null || cvInfo.Count == 0)
                {
                    bNeedCheckOA3 = true;
                }
                else
                {

                    bool findN = false;
                    foreach (ConstValueInfo tmp in cvInfo)
                    {
                        if (tmp.value.ToUpper() == "N")
                        {
                            findN = true;
                            bNeedCheckOA3 = false;
                            break;
                        }
                    }
                    if (findN == false)
                    {
                        bNeedCheckOA3 = true;
                    }
                }
            }
            catch { }
            #endregion

            try
            {
                               
                string tmpDn;
                ActivityCommonImpl utl = ActivityCommonImpl.Instance;
                IList<Regex> lstRegexTablet = utl.GetRegexByProcReg("Tablet");
                IList<Regex> lstRegexSKUorThinClient = utl.GetRegexByProcReg("SKU,ThinClient");

                foreach (UploadDNInfo sDnUdt in dns)
                {
                    tmpDn = sDnUdt.DeliveryNo;
                    IList<string> subRet = new List<string>();
                    int caseNoByDN = 2; //0 - case A; 1 - case B; 2 - case C

                    Delivery dlv = deliveryRepository.Find(tmpDn);
                    ////Check Udt..
                    //if (sDnUdt.Udt != dlv.Udt)
                    //{
                    //    throw new FisException("PAK173", new string[] { tmpDn });

                    //}
                    ////Check Udt..


                    //GetBom,与BOM相关的都尚未确定
                    IList<DNInfoForUI> dlvInfoList = deliveryRepository.GetDNInfoList(tmpDn);
                    string SW = "";
                    foreach (DNInfoForUI tmpInfo in dlvInfoList)
                    {
                        //SW：DeliveryInfo.InfoType=’PartNo’对应的InfoValue值，14位开始取7位
                        if (tmpInfo.InfoType.Equals("PartNo"))
                        {
                            //Add by Benson for Mantis0001733: UpLoad Shipment Data to SAP页面在save的时候报错提示不准确
                            string infoV = tmpInfo.InfoValue.Trim();
                            if (infoV.Length < 14 || string.IsNullOrEmpty(infoV.Substring(13, 7)))
                            { throw new FisException("PAK178", new string[] { tmpDn }); }
                            SW = tmpInfo.InfoValue.Substring(13, 7);

                            //SW = tmpInfo.InfoValue.Substring(13, 7);
                        }
                        if (tmpInfo.InfoType.Equals("Flag") && tmpInfo.InfoValue.Equals("N"))
                        {
                            caseNoByDN = 1;
                        }
                    }

                    IList<IProduct> proList = productRepository.GetProductListByDeliveryNo(tmpDn);
                    foreach (IProduct p in proList)
                    {
                        bool isTablet = utl.CheckModelByProcReg(p.Model, ref lstRegexTablet);
                        bool isSKUorThinClient = utl.CheckModelByProcReg(p.Model, ref lstRegexSKUorThinClient);

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

                        //string s = "";
                        UploadSAPSN sapSN = new UploadSAPSN();

                        #region get SN data
                        string palletNo = "";
                        string cartonNo = "";
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
                                //palletNo = "~" + p.PalletNo.Substring(2, 10);
                                cartonNo = p.PalletNo.Substring(2, 10);
                            }
                            else
                            {
                                //palletNo = p.PalletNo + "~";
                                palletNo = p.PalletNo;
                            }
                        }

                        if (isSKUorThinClient) // mantis 2061, Tablet機型不檢查 COA&OA3 狀態
                        {
                            if (!CheckCOACombine(bom, p, tmpDn))
                            {
                                //For Mantis0001733: UpLoad Shipment Data to SAP页面在save的时候报错提示不准确
                                throw new FisException("PAK179", new string[] { p.CUSTSN, tmpDn });
                                //throw new FisException("PAK158", new string[] { p.CUSTSN, tmpDn });
                            }
                            if (bNeedCheckOA3)
                            {
                                if (bNeedCheckEOA && !CheckOA3Key(bom, p, fkiPath, FKICredentialCache, editor, session.UnitOfWork))
                                {
                                    throw new FisException("PAK158", new string[] { p.CUSTSN, tmpDn });
                                }
                            }
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

                        //Add by Benson for fix Mantis 1543


                        foreach (IProductPart ele in p.ProductParts)
                        {
                            if (ele.CheckItemType == "ODD")
                            {
                                if (string.IsNullOrEmpty(ODD_Accession) || string.IsNullOrEmpty(ODD_Mftld) || string.IsNullOrEmpty(ODD))
                                {
                                    throw new FisException("PAK169", new string[] { tmpDn });

                                }

                            }
                        }




                        //Add by Benson for fix Mantis 1543

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
                        #endregion

                        sapSN.SERIALNUMBER = txId;
                        //s += dlv.DeliveryNo.Substring(0, 10);
                        sapSN.DN = dlv.DeliveryNo.Substring(0, 10);
                        //s += "~";
                        //s += dlv.DeliveryNo.Substring(dlv.DeliveryNo.Length - 6);
                        sapSN.DLV_ITM = dlv.DeliveryNo.Substring(dlv.DeliveryNo.Length - 6);
                        //s += "~";
                        //s += dlv.PoNo;
                        sapSN.PO_NO = dlv.PoNo;
                        //s += "~";
                        //s += palletNo;
                        sapSN.PALLET_NO = palletNo;

                        sapSN.CARTON_NO = cartonNo;
                        //s += "~";
                        //s += p.CUSTSN;
                        sapSN.SERNO = p.CUSTSN; 
                        //s += "~";
                        //s += boxId;
                        sapSN.BOX_ID = boxId;
                        //s += "~1~";
                        sapSN.QTY = "1";
                        // mantis 2061
                        //s += p.Model.StartsWith("PC") ? "C" : "O";
                        if (p.Model.Substring(1, 1) == "C")
                            //s += "C";
                            sapSN.TYPE = "C";
                        else
                            //s += "O";
                            sapSN.TYPE = "O";


                        //s += "~";
                        //s += Descr;
                        sapSN.PBOX_ID = Descr;
                        //s += "~";
                        //s += AST;  //10
                        sapSN.ASSET_NO = AST;
                        //s += "~";
                        //s += Wireless;
                        sapSN.WLMAT = Wireless;
                        //s += "~";
                        //s += ODD;
                        sapSN.ODMAT = ODD;
                        //s += "~";
                        //s += ODD_Accession;
                        sapSN.ACNUM = ODD_Accession;
                        //s += "~";
                        //s += ODD_Mftld;
                        sapSN.FDA = ODD_Mftld;
                        //s += "~";
                        //s += FCCID; //5
                        sapSN.FCCID = FCCID;
                        //s += "~~~";//2,7
                        //s += (caseNo == 0) ? p.MAC : ""; //9
                        sapSN.MACADDRESS = (caseNo == 0) ? p.MAC : "";
                        //s += "~~~~~";//4,13
                        //s += MEID;
                        sapSN.HEX_MEID = MEID;
                        //s += "~";
                        //s += MFD;//15
                        sapSN.MANUFACTURE_DATE = MFD;
                        //s += "~";
                        //s += UUID;
                        sapSN.UUID = UUID;
                        //s += "~";
                        //s += WM;
                        sapSN.MACADDRESS2 = WM;
                        //s += "~";
                        //s += IMEI;
                        sapSN.IMEI_CODE = IMEI;
                        //s += "~";
                        //s += (caseNo == 0) ? "1.0" : "";
                        sapSN.BUILDER_VER = (caseNo == 0) ? "1.0" : "";
                        //s += "~";
                        //s += (caseNo == 0) ? "EMBDSIM" : "";
                        sapSN.SIMLOCK_CODE = (caseNo == 0) ? "EMBDSIM" : "";
                        //s += "~";
                        //s += (caseNo == 0) ? SW : "";
                        sapSN.SOFTWARE_VER = (caseNo == 0) ? SW : "";
                        //s += "~";
                        //s += ICCID;
                        sapSN.ICCID = ICCID;
                        //s += "~";//8
                        //subRet.Add(s);
                        ret.Add(sapSN);
                    }
                    //foreach (string ele in subRet)
                    //{
                    //    ret.Add(ele);
                    //}
                }

                //   Check Udt again
                //foreach (UploadDNInfo dnUdt in dns)
                //{
                //    Delivery dnObj = deliveryRepository.Find(dnUdt.DeliveryNo);
                //    if (dnUdt.Udt != dnObj.Udt || (dnObj.Status != "98" && dnObj.Status != "88"))
                //    {
                //        throw new FisException("PAK173", new string[] { dnUdt.DeliveryNo });

                //    }
                //}

                //Check Udt again

                //uow.Commit();
                return ret;
            }
            catch (FisException e)
            {
                throw new Exception(e.mErrmsg);
            }

            catch (System.Net.WebException)
            {
                //throw new SystemException("無法連接HP OA3 Server,請聯繫測試PE處理");
                throw new FisException("CHK1119", new List<string> { "HP" });
            }

            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {

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
                    //UC Exception 
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
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
            IList<SysSettingInfo> siteList = partRep.GetSysSettingInfoes(new SysSettingInfo() { name = "Site" });
            string site = "IPC";
            if (siteList != null && siteList.Count > 0)
            {
                site = siteList[0].value;
            }
            foreach (IBOMNode bomNode in P1BomNodeList)
            {
                //Vincent Change Check WIN7 or WIN8 log
                if (site == "IPC")
                {
                    if (bomNode.Part.Descr.StartsWith("COA"))
                    {
                        bNeedCOA = true;
                        pnCOA = bomNode.Part.PN;
                        break;
                    }
                }
                else
                {
                    if (bomNode.Part.Descr.ToUpper().Contains("WIN7"))
                    {
                        bNeedCOA = true;
                        pnCOA = bomNode.Part.PN;
                        break;
                    }
                }
            }

            if (!bNeedCOA) return true;

            if (partRep.CheckExistInternalCOA(dn, "DN")) return true;

            if (partRep.CheckExistInternalCOA(p.CUSTSN, "SN")) return true;

            foreach (IProductPart tmp in p.ProductParts)
            {
                if (tmp.PartID == pnCOA) return true;
            }

            return false;
        }

        private bool CheckOA3Key(IHierarchicalBOM bom, IProduct p, string fkiPath, CredentialCache FKICredentialCache, string editor, IUnitOfWork uow)
        {
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            bool bWIN8 = false;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            IList<IBOMNode> P1BomNodeList = bom.GetFirstLevelNodesByNodeType("P1");
            //foreach (IBOMNode bomNode in P1BomNodeList)
            //{
            //    if (bomNode.Part.Descr.StartsWith("ECOA"))
            //    {
            //        bWIN8 = true;
            //        break;
            //    }
            //}

            bWIN8 = utl.CheckIsWIN8(bom);


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

        private IList<string> GetWirelessAndFCCID(IHierarchicalBOM bom, IProduct p, string dn)
        {
            //Wireless:在Model BOM中按照Model取下阶，得到
            //BomNodeType=’AV’ and PartInfo.InfoType=’Descr’ and PartInfo.InfoValue=’BLUETOOTH’
            //的记录，Wireless=获取PartInfo.InfoType=’AV’对应的InfoValue值(只考虑得到一条)。
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
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
            IPizzaRepository pizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {   // 总是接受    
            return true;
        }
	}
}
