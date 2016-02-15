/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: Pallet Verify FDEOnly
 * UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx 
 * UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx                        
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================
 * 2011-11-22   Chen Xu (eB1-4)     create
 * Known issues:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Workflow.Runtime;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Collections;
using IMES.FisObject.Common;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;
using IMES.Route;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using IMES.FisObject.Common.Part;
using System.Net;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure.UnitOfWork;



namespace IMES.Station.Implementation
{
    /// <summary>
    /// Pallet Verify FDEOnly
    /// station : 99
    /// 本站实现的功能：
    ///     检查Pallet 上的所有SKU；
    ///     列印Pallet SN Label；Delivery Label
    /// </summary> 

    public class PalletVerifyFDEOnly : MarshalByRefObject, IPalletVerifyFDEOnly
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        

        #region IPalletVerify Members

        /// <summary>
        /// 刷第一个SN时，调用该方法启动工作流，根据输入CustSN获取PalletInfo,成功后调用InputCustSn
        /// 将custSN放到Session.CustSN中(string)
        /// 返回ArrayList
        /// </summary>
        public ArrayList InputFirstCustSn(string firstSn, string line, string editor, string station, string customer, out int index)
        {
            logger.Debug("(PalletVerifyOnlyImpl)InputCustSNOnCooLabel start, custsn:" + firstSn + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retLst = new ArrayList();

            try
            {
                //根据输入的Customer S/N, 获得对应的Product
                var currentProduct = CommonImpl.GetProductByInput(firstSn, CommonImpl.InputTypeEnum.CustSN);
                if (string.IsNullOrEmpty(currentProduct.ProId))
                {
                    erpara.Add(firstSn);
                    ex = new FisException("CHK152", erpara);    //您所刷入的Customer S/N 无效 ,请重新刷入！
                    throw ex;

                }
                

                //取得Customer S/N 绑定的Delivery No :IMES_FA..Product.DeliveryNo
                if (string.IsNullOrEmpty(currentProduct.DeliveryNo))
                {
                    erpara.Add(firstSn);
                    ex = new FisException("PAK020", erpara);    //该Customer S/N还未与DN绑定!
                    throw ex;
                }
                string curDn = currentProduct.DeliveryNo;

                //获取Pallet No : IMES_FA..Product.PalletNo
                if (string.IsNullOrEmpty(currentProduct.PalletNo))
                {
                    erpara.Add(firstSn);
                    ex = new FisException("PAK021", erpara);     //该Customer S/N还未与Pallet绑定!
                    throw ex;
                }
                string currentPalletNo = currentProduct.PalletNo;
                
     

                string sessionKey = currentProduct.ProId;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.Product, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "PalletVerifyFDEOnly.xoml", "PalletVerifyFDEOnly.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.CustSN, firstSn);
                    currentSession.AddValue(Session.SessionKeys.PalletNo, currentPalletNo);
                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, curDn);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);

                    //For Check OA3
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                    IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    IList<ConstValueInfo> cvInfo = new List<ConstValueInfo>();
           
                    //ConstValueInfo cvCond = new ConstValueInfo();
                    //cvCond.type = "CheckOA3State";
                    //cvCond.name = "OA3UpSAP";
                    //bool bNeedCheckEOA = partRepository.GetConstValueInfoList(cvCond).Any(x => x.value == "Y");
    
                    string fkiPath = "";
                    string fkiUser = "";
                    string fkiPwd = "";
                    bool bNoNeedCheckEOA=false;
                    GetAndCheckOA3Setting(currentProduct.Model, out fkiPath, out fkiUser, out fkiPwd, out bNoNeedCheckEOA);

                    //string fkiPath = System.Configuration.ConfigurationManager.AppSettings["FKIServicePath"].Trim();
                    //string fkiUser = System.Configuration.ConfigurationManager.AppSettings["FKIAuthUser"].Trim();
                    //string fkiPwd = System.Configuration.ConfigurationManager.AppSettings["FKIAuthPassword"].Trim();
                    currentSession.AddValue("FKIServicePath", fkiPath);
                    currentSession.AddValue("FKIAuthUser", fkiUser);
                    currentSession.AddValue("FKIAuthPassword", fkiPwd);
                    CommonImpl2 cmm2 = new CommonImpl2();
                    CredentialCache FKICredentialCache = new CredentialCache();
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
                    IMES.FisObject.FA.Product.IProduct prd = (IMES.FisObject.FA.Product.IProduct)currentProduct;
                    IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                   // bool bNoNeedCheckEOA = partRepository.GetValueFromSysSettingByName("DisableOA3CheckOnPallet").
                                                             //  Any(x => x == "Y");
                    IList<IMES.DataModel.ConstValueTypeInfo> lstConst = partRep.GetConstValueTypeList("NoCheckOA3SN");
                    if (!lstConst.Any(x => x.value == currentProduct.CUSTSN) && !bNoNeedCheckEOA)
                    {
                         IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currentProduct.Model);
                        if (!cmm2.CheckOA3Key(bom, prd, fkiPath, FKICredentialCache, editor, (UnitOfWork)currentSession.UnitOfWork))
                        {
                            throw new FisException("PAK158", new string[] { currentProduct.CUSTSN, currentProduct.DeliveryNo });
                        }
                    }

                    //For Check OA3

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }
               

                // 获取该Pallet 上的SKU 列表
                // 该Pallet 上的SKU 列表:IMES_FA..Product.PalletNo = @PalletNo的所有记录
                // SKU 列表显示如下信息：Product ID | Customer S/N | PAQC | POD | WC | Collection Data
                // 按照Customer S/N 升序排序
                IList<string> productNoList = new List<string>();
                IList<string> custSnList = new List<string>();
                
                // retLst[0]-[2]
                string dummyPalletNo = string.Empty;
                dummyPalletNo = (string)currentSession.GetValue(Session.SessionKeys.DummyPalletNo);
                productNoList = (IList<string>) currentSession.GetValue(Session.SessionKeys.ProdNoList);
                custSnList = (IList<string>) currentSession.GetValue(Session.SessionKeys.CustomSnList);
                retLst.Add(dummyPalletNo);
                retLst.Add(productNoList);
                retLst.Add(custSnList);

                IProductRepository iproductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                
                // Mantis Bug :http://10.99.183.26/Mantis/view.php?id=627 (改为前台页面收集scanProductNoList)
                // Update IMES_FA..ProductStatus ( 更新Pallet 上已收集的PRODUCT 的状态)
                //IList<string> scanProductNoList = new List<string>();
                //scanProductNoList.Add(currentProduct.ProId);
                //currentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, scanProductNoList);
                
                int i = 0;
                index = 0;
                foreach (string iprod in productNoList)
                {
                  
                  if (iprod== currentProduct.ProId && custSnList[i] == firstSn)
                    {
                        index = i+1;
                        break;
                    }
                  else i++;
                }
               
                // PAQC –retLst[3]: 使用ProductID = @ProductId and Tp = 'PAQC' 查询IMES_FA..QCStatus 表取Udt 最新的记录，如果该记录的Status = '8'，则PAQC 栏位显示'PAQC'；Status = '9'，则PAQC 栏位显示'Pass'；Status = 'A'，则PAQC 栏位显示'Fail'；Status = '1'，则PAQC 栏位显示'No Check'；
                IDeliveryRepository ideliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                IList<string> PAQCStatusLst = new List<string>();
                foreach (string iprod in productNoList)
                {
                    ProductQCStatus qcsStatus = ideliveryRepository.GetQCStatus(iprod, "PAQC");
                    if (qcsStatus == null || string.IsNullOrEmpty(qcsStatus.Status))
                    {
                        erpara.Add(iprod);
                        erpara.Add("PAQC");
                        ex = new FisException("PAK051", erpara);    //QCStatus 中Product%1 的参数 %2 不存在！
                        throw ex;
                    }
                    switch (qcsStatus.Status)
                    {
                        case "8":
                        case "B":
                        case "C":
                            PAQCStatusLst.Add("PAQC");
                            break;
                        case "9":
                            // ITC-1360-1235 : 小写
                            // PAQCStatusLst.Add("PASS");
                            PAQCStatusLst.Add("Pass");
                            break;
                        case "A":
                            PAQCStatusLst.Add("Fail");
                            break;
                        case "1":
                            PAQCStatusLst.Add("No Check");
                            break;
                        default:
                            PAQCStatusLst.Add("");
                            break;
                    }

                }

                retLst.Add(PAQCStatusLst);      


                // POD –retLst[4]: 检查IMES_FA..ProductLog，如果存在Station = 'PD' 的记录，则POD 栏位显示'PD'；如果存在Station = '86' 的记录，则POD 栏位显示'86'；
                // WC –retLst[5]: IMES_ProductStatus.Station
                // UC Revision: 7422:检查IMES_FA..ProductLog，如果存在Station = ’86' 的记录，则POD 栏位显示'86'；否则如果存在Station = 'PD' 的记录，则POD 栏位显示'PD'；两者都不存在时，显示为空
                IList<string> PODLst = new List<string>();
                IList<string> WCLst = new List<string>();
                foreach (string prodid in productNoList)
                {
                    IList<ProductLog> prodLogLst = new List<ProductLog>();
                    prodLogLst = iproductRepository.GetProductLogs(prodid, "86");
                    if (prodLogLst.Count> 0)
                    {
                        PODLst.Add("86");
                    }
                    else
                    {
                        prodLogLst = iproductRepository.GetProductLogs(prodid, "PD");
                        if (prodLogLst.Count > 0)
                        {
                            PODLst.Add("PD");
                        }
                        else
                        {
                            PODLst.Add("");
                        }
                    }

                    ProductStatusInfo productStatus = iproductRepository.GetProductStatusInfo(prodid);
                    if (productStatus.station != null)
                    {
                        WCLst.Add(productStatus.station);
                    }
                    else
                    {
                        erpara.Add(prodid);
                        ex = new FisException("PAK026", erpara);    //没有Product Status 站数据！
                        throw ex;
                    }

                }
                retLst.Add(PODLst);
                retLst.Add(WCLst);
                
                // Collection Data 为空 - retLst[6]
                retLst.Add("");

                // Pallet Qty -retLst[7]: (Sum(IMES_PAK..Delivery_Pallet.DeliveryQty))
                //int palletQty =0;
                //IList<DeliveryPallet> dnpltlist = new List<DeliveryPallet>();
                //IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                //dnpltlist= iPalletRepository.GetDeliveryPallet(currentPalletNo);
                //if (dnpltlist != null && dnpltlist.Count > 0)
                //{
                //   foreach (DeliveryPallet dp in dnpltlist)
                //   {
                //       palletQty += dp.DeliveryQty;
                //   }
                //}
                //else
                //{
                //    erpara.Add(currentPalletNo);
                //    ex = new FisException("PAK027", erpara);    //没有获得当前Pallet的DeliveryQty！
                //    throw ex;
                //}

               string palletQty = (string)currentSession.GetValue(Session.SessionKeys.PalletQty);
               retLst.Add(palletQty);

               // Scan Qty - retLst[8]: 
               string scanQty = (string)currentSession.GetValue(Session.SessionKeys.ScanQty);
               retLst.Add(scanQty);
               
               // DPC - retLst[9]: 
               string DPC = (string)currentSession.GetValue(Session.SessionKeys.DummyPalletCase);
               retLst.Add(DPC);
               
               // total Qty - retLst[10]:
               string totalQty = (string)currentSession.GetValue(Session.SessionKeys.Qty);
               retLst.Add(totalQty);

               return retLst;             

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
                logger.Debug("(PalletVerifyOnlyImpl)InputCustSNOnCooLabel end, custsn:" + firstSn + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }

        /// <summary>
        /// 每次刷SN都调用该方法,SFC
        /// 将custSN放到Session.CustSN中(string)
        /// 如果匹配到SKU List 中的某条记录，如果该记录的Collection Data为空，(页面处理)： 如果该记录的PAQC 栏位等于'PAQC'，则报告错误：“该机器尚未完成PAQC！”；
        ///                                                                                如果该记录的PAQC 栏位等于'Fail'，则报告错误：“该机器PAQC Fail！”；
        ///                                                                                如果该记录的PD 栏位等于'PD'，则报告错误：“该机器尚未完成POD Label Check！”；
        ///                                                                                如果上述错误均未发生，则该记录的Collection Data 栏位显示刷入的Customer S/N，并且Scan Qty + 1；
        ///                                  如果该记录的Collection Data 不为空，则报告错误：“You had duplicate scan.”。(页面处理)
        /// 如果没有匹配到SKU List 中的任何记录，则报告错误：“非法数据！”（service处理）
        /// </summary>
        /// <param name="firstSn">firstSn</param>
        /// <param name="custSn">custSn</param>
        /// <returns></returns>
        public int InputCustSn(string firstSn, string custSn)
        {
            logger.Debug("(PalletVerifyOnlyImpl)ScanSN start, firstSn:" + firstSn + " custSn:" + custSn);

            try
            {
                FisException ex;
                List<string> erpara = new List<string>();

                var currentProduct = CommonImpl.GetProductByInput(firstSn, CommonImpl.InputTypeEnum.CustSN);
                string sessionKey = currentProduct.ProId;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);


                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.CustSN, custSn);
                    //SFC : 根据刷入的 Another Customer S/N （custSn）, 获得其对应的Product
                    var newProduct = CommonImpl.GetProductByInput(custSn, CommonImpl.InputTypeEnum.CustSN);
                    if (string.IsNullOrEmpty(newProduct.ProId))
                    {
                        erpara.Add(custSn);
                        ex = new FisException("CHK152", erpara);    //您所刷入的Customer S/N 无效 ,请重新刷入！
                        throw ex;

                    }
       
                    IList<string> productNoList = (List<string>)(currentSession.GetValue(Session.SessionKeys.ProdNoList));
                    IList<string> custSnList = (List<string>)(currentSession.GetValue(Session.SessionKeys.CustomSnList));
                    int index = 0;
                    Boolean flag =false;
                    foreach (string sn in custSnList)
                    {
                        if (sn == custSn)
                        {
                            flag = true;
                            break;
                        }
                        index++;
                    }
                    if (!flag)
                    {
                        erpara.Add(custSn);
                        ex = new FisException("PAK022", erpara);    //非法数据！
                        throw ex;
                    }

                    if ((string.IsNullOrEmpty(newProduct.ProId)) || (newProduct.ProId != productNoList[index]))
                    {
                        erpara.Add(custSn);
                        ex = new FisException("PAK022", erpara);    //非法数据！
                        throw ex;
                    }


                    //For Check OA3
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                    IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    IList<ConstValueInfo> cvInfo = new List<ConstValueInfo>();
                    IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                    string fkiPath = "";
                    string fkiUser = "";
                    string fkiPwd = "";
                    bool bNoNeedCheckEOA = false;
                    GetAndCheckOA3Setting(newProduct.Model, out fkiPath, out fkiUser, out fkiPwd, out bNoNeedCheckEOA);

                    //string fkiPath = System.Configuration.ConfigurationManager.AppSettings["FKIServicePath"].Trim();
                    //string fkiUser = System.Configuration.ConfigurationManager.AppSettings["FKIAuthUser"].Trim();
                    //string fkiPwd = System.Configuration.ConfigurationManager.AppSettings["FKIAuthPassword"].Trim();

                    CommonImpl2 cmm2 = new CommonImpl2();
                    CredentialCache FKICredentialCache = new CredentialCache();
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
                    IMES.FisObject.FA.Product.IProduct prd = (IMES.FisObject.FA.Product.IProduct)currentProduct;
                    IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                   // bool bNoNeedCheckEOA = partRepository.GetValueFromSysSettingByName("DisableOA3CheckOnPallet").
                                                              // Any(x => x == "Y");
                    IList<IMES.DataModel.ConstValueTypeInfo> lstConst = partRep.GetConstValueTypeList("NoCheckOA3SN");
                    if (!lstConst.Any(x => x.value == newProduct.CUSTSN) && !bNoNeedCheckEOA)
                    {
                        IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(newProduct.Model);
                        if (!cmm2.CheckOA3Key(bom, newProduct, fkiPath, FKICredentialCache, currentSession.Editor, (UnitOfWork)currentSession.UnitOfWork))
                        {
                            throw new FisException("PAK158", new string[] { newProduct.CUSTSN, newProduct.DeliveryNo });
                        }
                    }

                    //For Check OA3

                    currentSession.AddValue(Session.SessionKeys.Product, newProduct);
                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();

                    if (currentSession.Exception != null)
                    {
                        if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            currentSession.ResumeWorkFlow();
                        }
                        throw currentSession.Exception;
                    }
                    
                    //string currentPalletNo = (string)(currentSession.GetValue(Session.SessionKeys.PalletNo));
                    
                    //ITC-1360-1277:
                    //if ((string.IsNullOrEmpty(newProduct.ProId)) || (newProduct.PalletNo != currentPalletNo) || (newProduct.ProId != productNoList[index]))
                    //{
                    //    erpara.Add(custSn);
                    //    ex = new FisException("PAK022", erpara);    //非法数据！
                    //    throw ex;
                    //}

                   

                    //Mantis Bug :http://10.99.183.26/Mantis/view.php?id=627 (改为前台页面收集scanProductNoList)
                    //IList<string> scanProductNoList = (IList<string>)currentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);
                    //Boolean exitFlag = false;
                    //foreach (string iprodid in scanProductNoList)
                    //{
                    //    if (iprodid == newProduct.ProId)
                    //    {
                    //        exitFlag = true;
                    //        break;
                    //    }
                    //}
                    //if (!exitFlag)
                    //{
                    //    scanProductNoList.Add(newProduct.ProId);
                    //    currentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, scanProductNoList);
                    //}

                    return index;

                }
            }

            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(PalletVerifyOnlyImpl)ScanSN end, firstSn:" + firstSn + " custSn:" + custSn);
            }
        }

        /// <summary>
        /// Save Data，返回打印重量标签的PrintItem，结束工作流
        /// Update IMES_FA..ProductStatus – 更新Pallet 上所有SKU 的状态
        /// Station – Pallet Verify 站号
        /// Status – '1'
        /// Editor – Editor (from UI)
        /// Udt – Current Time
        /// Insert IMES_FA..ProductLog – 记录Pallet 上所有SKU 的Log
        /// 列印allet SN Label
        /// </summary>
        /// <param name="firstSn">firstSn</param>
        /// <param name="printItems">printItems</param> 
        /// <param name="ScanProductNoList">ScanProductNoList</param>
        /// <param name="DummyPalletNo">DummyPalletNo</param>
        /// <param name="printParams">printParams</param>
        /// <returns></returns>
        public IList<PrintItem> Save(string firstSn, IList<PrintItem> printItems, IList<string> ScanProductNoList, out string DummyPalletNo, out ArrayList printParams)
        {
            logger.Debug("(PalletVerifyOnlyImpl)Save start, firstSN:" + firstSn);

            FisException ex;
            List<string> erpara = new List<string>();
            
            var currentProduct = CommonImpl.GetProductByInput(firstSn, CommonImpl.InputTypeEnum.CustSN);
            string sessionKey = currentProduct.ProId;
            Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
            if (currentSession == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                throw ex;
            }
            try
            {

                currentSession.Exception = null;
                currentSession.AddValue(Session.SessionKeys.IsComplete, true);
                currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                currentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, ScanProductNoList);
                
                // WriteProductLog (2.	Insert IMES_FA..ProductLog – 记录Pallet 上已收集的PRODUCT的Log)
                IProductRepository iproductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                //Mantis Bug :http://10.99.183.26/Mantis/view.php?id=627 (改为前台页面收集scanProductNoList)
                //IList<string> scanProductNoList = (IList<string>)currentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);

                IList<IMES.FisObject.FA.Product.IProduct> prodLst = iproductRepository.GetProductListByIdList(ScanProductNoList);
                currentSession.AddValue(Session.SessionKeys.ProdList, prodLst);

                currentSession.SwitchToWorkFlow();

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                DummyPalletNo = string.Empty;
                string DPC = (string)currentSession.GetValue(Session.SessionKeys.DummyPalletCase);
                if (DPC == "NA" || DPC == "BA")
                {
                    DummyPalletNo = (string)currentSession.GetValue(Session.SessionKeys.DummyPalletNo);
                }
                else if (DPC == "NAN" || DPC == "BAN")
                {
                    DummyPalletNo = (string)currentSession.GetValue(Session.SessionKeys.GenerateDummyPalletNo);
                }

                printParams = new ArrayList();

                //IDeliveryRepository ideliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                //IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                //IList<DummyShipDetInfo> dummyshipdetLst = new List<DummyShipDetInfo>();
                //dummyshipdetLst = iPalletRepository.GetDummyShipDetListByPlt(DummyPalletNo);
                //IList<string> BOLlist = new List<string>();
                //IList<string> BOLQtyList = new List<string>();
                //IList<string> DNList = new List<string>();
                //IList<string> DNQtyList = new List<string>();
                //IList<string> SNOList = new List<string>();
                //IList<DeliveryNoAndQtyEntity> DNandQtlyList = new List<DeliveryNoAndQtyEntity>();
                //foreach (DummyShipDetInfo iDSD in dummyshipdetLst)
                //{
                //    BOLlist.Add(iDSD.bol);
                //    int bolqty = ideliveryRepository.GetBOLQty(iDSD.bol);
                //    BOLQtyList.Add(bolqty.ToString());
                //}
                //printParams.Add(BOLlist);
                //printParams.Add(BOLlist.Count());
                //printParams.Add(BOLQtyList);

                //DNandQtlyList = ideliveryRepository.GetDNAndQtyByDummyPalletNo(DummyPalletNo);
                //if (DNandQtlyList == null || DNandQtlyList.Count <= 0)
                //{
                //    erpara.Add("Delivery Label");
                //    erpara.Add("DNQty");
                //    ex = new FisException("PAK040", erpara);    //%1 Label print parameter does not contain %2！
                //    throw ex;
                //}
                //foreach (DeliveryNoAndQtyEntity iDnQty in DNandQtlyList)
                //{
                //    DNList.Add(iDnQty.DeliveryNo);
                //    DNQtyList.Add(iDnQty.Qty.ToString());
                //}

                //printParams.Add(DNList);
                //printParams.Add(DNQtyList);

                //SNOList=ideliveryRepository.GetCustSnListByDummyPalletNo(DummyPalletNo);
                //if (SNOList == null || SNOList.Count <= 0)
                //{
                //    erpara.Add("Pallet SN Label");
                //    erpara.Add("SNO");
                //    ex = new FisException("PAK040", erpara);    //%1 Label print parameter does not contain %2！
                //    throw ex;
                //}
                //printParams.Add(SNOList);

                return (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
            }

            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(PalletVerifyOnlyImpl)Save end, firstSn:" + firstSn);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn">uutSn</param>
        public void Cancel(string uutSn)
        {
            logger.Debug("(PalletVerifyOnlyImpl)Cancel Start," + "uutSn:" + uutSn);
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(uutSn, CommonImpl.InputTypeEnum.CustSN);
                string sessionKey = currentProduct.ProId;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(PalletVerifyOnlyImpl)Cancel End," + "uutSn:" + uutSn);
            }

        }


        #endregion
        private void GetAndCheckOA3Setting(string Model, out string url, out string user, out string password, out bool bNoNeedCheckEOA)
        {
            CommonImpl cmmn1 = new CommonImpl();
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            if (cmmn1.CheckModelByProcReg(Model, "ThinClient"))
            {
                IList<string> lstUrl = partRepository.GetValueFromSysSettingByName("TCOA3URL");
                IList<string> lstUser = partRepository.GetValueFromSysSettingByName("TCOA3User");
                IList<string> lstPwd = partRepository.GetValueFromSysSettingByName("TCOA3Password");
                url = lstUrl.Count == 0 ? "" : lstUrl[0];
                user = lstUser.Count == 0 ? "" : lstUser[0];
                password = lstPwd.Count == 0 ? "" : lstPwd[0];
                bNoNeedCheckEOA = partRepository.GetValueFromSysSettingByName("DisableOA3CheckOnPalletForTC").Any(x => x == "Y");
                if ("" == url || "" == user || "" == password)
                { throw new FisException("Wrong ThinClientOA3 setting"); }
            }
            else
            {
                IList<string> lstUrl = partRepository.GetValueFromSysSettingByName("OA3URL");
                IList<string> lstUser = partRepository.GetValueFromSysSettingByName("OA3User");
                IList<string> lstPwd = partRepository.GetValueFromSysSettingByName("OA3Password");
                url = lstUrl.Count == 0 ? "" : lstUrl[0];
                user = lstUser.Count == 0 ? "" : lstUser[0];
                password = lstPwd.Count == 0 ? "" : lstPwd[0];
                bNoNeedCheckEOA = partRepository.GetValueFromSysSettingByName("DisableOA3CheckOnPallet").Any(x => x == "Y");
                if ("" == url || "" == user || "" == password)
                { throw new FisException("Wrong OA3 setting"); }
            }
        }
        #region rePrint


        /// <summary>
        /// 重印
        /// </summary>
        /// <param name="dummyPalletNo">dummyPalletNo</param>
        /// <param name="reason">reason</param>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="printItems">printItems</param>
        public IList<PrintItem> rePrint(string dummyPalletNo, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(PalletVerifyOnlyImpl)rePrint Start," + "dummyPalletNo:" + dummyPalletNo + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer + "printItems" + printItems);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                IProductRepository iproductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IList<DummyShipDetInfo> dummyshipdetLst = new List<DummyShipDetInfo>();
                dummyshipdetLst = iPalletRepository.GetDummyShipDetListByPlt(dummyPalletNo);
                if (dummyshipdetLst == null || dummyshipdetLst.Count <= 0)
                {
                    erpara.Add(dummyPalletNo);
                    ex = new FisException("PAK049", erpara);    //此Dummy Pallet No 不存在！
                    throw ex;
                }
                else
                {
                   // Boolean exitFlag = false;
                    IList<ProductLog> prodLogLst =  new List<ProductLog>();
                    foreach (DummyShipDetInfo iDSD in dummyshipdetLst)
                    {
                       // prodLogLst = iproductRepository.GetProductLogs(iDSD.snoId, "99");
                        prodLogLst = iproductRepository.GetProductLogs(iDSD.snoId, station);
                        //if (prodLogLst.Count > 0)
                        //{
                        //    exitFlag = true;
                        //    break;
                        //}
                        if (prodLogLst.Count <= 0)
                        {
                            //ITC-1360-1642:存在尚未Pass Pallet Verify (FDE Only) 的Product，请先刷Pallet Verify (FDE Only)！
                            FisException fe = new FisException("PAK121", new string[] { iDSD.snoId });
                            throw fe;
                            
                            //exitFlag = true;
                            //break;
                        }
                    }

                    //if (!exitFlag)
                    //{
                    //    //沒有任何Pass Pallet Verify  (FDE Only)的Product，请先刷Pallet Verify！
                    //    FisException fe = new FisException("PAK050", new string[] { });  
                    //    throw fe;
                    //}
                    //if (exitFlag)
                    //{
                    //    //ITC-1360-1642:存在尚未Pass Pallet Verify (FDE Only) 的Product，请先刷Pallet Verify (FDE Only)！
                    //    FisException fe = new FisException("PAK121", new string[] { });
                    //    throw fe;
                    //}
                }



                string sessionKey = dummyPalletNo;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.Common, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Common);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PalletVerifyFDEOnlyReprint.xoml", "", wfArguments);
                    currentSession.AddValue(Session.SessionKeys.DummyPalletNo, dummyPalletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, "PalletVerifyFDEOnly");
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, dummyPalletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, dummyPalletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "PalletVerifyFDEOnlyReprint");
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }


                return (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(PalletVerifyOnlyImpl)rePrint End," + "dummyPalletNo:" + dummyPalletNo + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer + "printItems" + printItems);
            }

        }


        #endregion
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {   // 总是接受    
            return true;
        }
    }

    
}
