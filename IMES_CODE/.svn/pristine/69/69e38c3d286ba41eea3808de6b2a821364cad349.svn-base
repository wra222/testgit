/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: Pallet Verify Data for docking                   
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================

 * Known issues:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
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
using IMES.FisObject.Common.Part;
using System.Data;
using IMES.FisObject.PAK.Pizza;
using IMES.Docking.Interface.DockingIntf;

namespace IMES.Docking.Implementation
{
    /// <summary>
    /// Pallet Verify
    /// station : 9A
    /// 本站实现的功能：
    ///     检查Pallet 上的所有PRODUCT；
    ///     列印Ship to Pallet Label；
    ///     内销要额外列印一张Pallet CPMO Label
    /// </summary> 

    public class PalletVerifyDataForDocking : MarshalByRefObject, IPalletVerifyDataForDocking
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

     

        #region IPalletVerify Members

        
        public ArrayList InputFirstCustSn(string firstSn, string line, string editor, string station, string customer, out int index, out string labeltypeBranch)
        {
            logger.Debug("(PalletVerifyImpl)InputCustSNOnCooLabel start, custsn:" + firstSn + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retLst = new ArrayList();

            try
            {
                string sessionKey = firstSn;
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
                    RouteManagementUtils.GetWorkflow(station, "PalletVerifyDataForDocking.xoml", "PalletVerifyDataForDocking.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    //currentSession.AddValue(Session.SessionKeys.CustSN, firstSn);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    
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
                /// result[0]: [0],    [1],      [2],     [3],       [4],   [5],     [6],      [7]  
                ///        palletNo,prodIdLst,custSnLst,carton no,palletQty
               IProduct product = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);
                int palletQty = (int)currentSession.GetValue(Session.SessionKeys.PalletQty);
                //*
                retLst.Add(product.PalletNo);
                retLst.Add(product.ProId);
               
                retLst.Add(product.CUSTSN);
                retLst.Add(product.CartonSN);
                retLst.Add(palletQty);

                // for edits
                IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IDeliveryRepository ideliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

                //
                string currentPalletNo = product.PalletNo;
                //int palletQty = 0;
                IList<DeliveryPallet> dnpltlist = new List<DeliveryPallet>();
                IList<string> DeliveryPerPalletList = new List<string>();
                IList<string> modelList = new List<string>();

                dnpltlist = iPalletRepository.GetDeliveryPallet(currentPalletNo);
                if (dnpltlist != null && dnpltlist.Count > 0)
                {
                    foreach (DeliveryPallet dp in dnpltlist)
                    {
                        //palletQty += dp.DeliveryQty;
                        DeliveryPerPalletList.Add(dp.DeliveryID);
                        Delivery currentDelivery = ideliveryRepository.Find(dp.DeliveryID);
                        modelList.Add(currentDelivery.ModelName);
                    }
                }

                //
                string curDn = product.DeliveryNo;
                
                IList<string> PDFPLLst = new List<string>();

                //当EDI_RegId = 'SNE' or 'SCE'时， Emea = 1；否则， Emea = 0
                decimal emea = 0;

                labeltypeBranch = string.Empty;
                string printflag = ideliveryRepository.GetDeliveryInfoValue(curDn, "Flag");
                if (printflag == "SNE" || printflag == "SCE" ||printflag == "QNE" || printflag == "QCE" )
                {
                    emea = 1;
                }
                else emea = 0;

                if (printflag == "C")
                {
                    string cpmoflag = ideliveryRepository.GetDeliveryInfoValue(curDn, "Carrier");
                    if (cpmoflag == "XJ")
                    {
                        labeltypeBranch = "X";   //手动单:Pallet Label + CPMO Label
                    }
                    else labeltypeBranch = "C";   // 手动单:Pallet Label
                }
                else
                {
                    labeltypeBranch = "A"; // 自动单：Pallet Label

                    //UC Revision: 7636:要求Edits 文档所在的网络路径可配置:
                    // （\\hp-iis\OUT\）需要查询SysSetting 表获取, 参考方法：select Value from SysSetting nolock where Name = 'EditsFISAddr'
                    IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    IList<string> PLEditsURLLst = partRepository.GetValueFromSysSettingByName("PLEditsURL");
                    if (PLEditsURLLst != null && PLEditsURLLst.Count > 0)
                    {
                        PDFPLLst.Add(PLEditsURLLst[0]); //PDFPLLst[0]
                    }

                    IList<string> PLEditsTemplateLst = partRepository.GetValueFromSysSettingByName("PLEditsTemplate");
                    if (PLEditsTemplateLst != null && PLEditsTemplateLst.Count > 0)
                    {
                        PDFPLLst.Add(PLEditsTemplateLst[0]);//PDFPLLst[1]
                    }

                    IList<string> PLEditsXMLLst = partRepository.GetValueFromSysSettingByName("PLEditsXML");
                    if (PLEditsXMLLst != null && PLEditsXMLLst.Count > 0)
                    {
                        PDFPLLst.Add(PLEditsXMLLst[0]);//PDFPLLst[2]
                    }

                    IList<string> PLEditsPDFLst = partRepository.GetValueFromSysSettingByName("PLEditsPDF");
                    if (PLEditsPDFLst != null && PLEditsPDFLst.Count > 0)
                    {
                        PDFPLLst.Add(PLEditsPDFLst[0]); //PDFPLLst[3]
                    }
                    IList<string> PLEditsImageLst = partRepository.GetValueFromSysSettingByName("PLEditsImage");
                    if (PLEditsImageLst != null && PLEditsImageLst.Count > 0)
                    {
                        PDFPLLst.Add(PLEditsImageLst[0]); //PDFPLLst[4]
                    }

                    IList<string> FOPFullFileNameLst = partRepository.GetValueFromSysSettingByName("FOPFullFileName");
                    if (FOPFullFileNameLst != null && FOPFullFileNameLst.Count > 0)
                    {
                        PDFPLLst.Add(FOPFullFileNameLst[0]); //PDFPLLst[5]

                    }
                    IList<string> printexepathLst = partRepository.GetValueFromSysSettingByName("PDFPrintPath");
                    if (printexepathLst != null && printexepathLst.Count > 0)
                    {
                        PDFPLLst.Add(printexepathLst[0]); //PDFPLLst[6]
                    }
                }

                retLst.Add(""); // 5
                retLst.Add(""); // 6
                retLst.Add(""); // 7
                retLst.Add(""); // 8

                retLst.Add(PDFPLLst); // 9
                retLst.Add(DeliveryPerPalletList); // 10
                retLst.Add(curDn); // 11
                retLst.Add(emea); // 12
                retLst.Add(modelList); // 13

                index = 0;
                //labeltypeBranch=null;
                return retLst;

            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(PalletVerifyImpl)InputCustSNOnCooLabel end, custsn:" + firstSn + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
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
        public ArrayList InputCustSn(string firstSn, string custSn)
        {
            logger.Debug("(PalletVerifyImpl)ScanSN start, firstSn:" + firstSn + " custSn:" + custSn);
            ArrayList retLst = new ArrayList();
            try
            {
                FisException ex;
                List<string> erpara = new List<string>();
                string sessionKey = firstSn;
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
                    var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    IProduct currentProduct = null;
                    IProduct product = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);
                    currentProduct = productRepository.GetProductByIdOrSn(custSn);
                   

                    if (currentProduct == null)
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(custSn);                        
                        throw new FisException("SFC002", errpara);
                    }
                    string newPalletNo = currentProduct.PalletNo;
                    string newCarton = currentProduct.CartonSN;
                    string oldPalletNo = product.PalletNo;

                    if (newPalletNo == "" || newPalletNo == null)
                    {
                        erpara.Add(currentProduct.ProId);
                        ex = new FisException("CHK881", erpara);
                    }
                    if (newCarton == "" || newCarton == null)
                    {
                        erpara.Add(currentProduct.ProId);
                        ex = new FisException("CHK880", erpara);
                    }

                    if (oldPalletNo != currentProduct.PalletNo)
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(oldPalletNo);
                        errpara.Add(currentProduct.CUSTSN);
                        throw new FisException("PAK144", errpara);
                    }

                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);                  
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
                    /// result[0]: [0],    [1],      [2],     [3],       [4],     
                ///             palletNo,prodIdLst,custSnLst,carton no,palletQty
                    retLst.Add(currentProduct.PalletNo);
                    retLst.Add(currentProduct.ProId);

                    retLst.Add(currentProduct.CUSTSN);
                    retLst.Add(currentProduct.CartonSN);
                 
                    return retLst;

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
                logger.Debug("(PalletVerifyImpl)ScanSN end, firstSn:" + firstSn + " custSn:" + custSn);
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
        /// 列印Ship to Pallet Label；内销要额外列印一张Pallet CPMO Label
        /// Note: 当存在并板(即一个Pallet 结合了多个Delivery，
        ///       使用某个Pallet 在IMES_PAK..Delivery_Pallet 表中可以查询到多条记录的时候，可以认为是并板)情况时，
        ///       每个结合了当前Pallet 的Delivery 都需要列印一张Pallet Label
        /// </summary>
        /// <param name="firstSn">firstSn</param>
        /// <param name="printItems">printItems</param> 
        /// <param name="printparams1">printparams1</param>
        /// <param name="printparams2">printparams2</param>
        /// <returns></returns>
        public ArrayList Save(string firstSn, IList<PrintItem> printItems)
        {
            logger.Debug("(PalletVerifyImpl)Save start, firstSN:" + firstSn + "printItems" + printItems);

            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retLst = new ArrayList();

           
            string sessionKey = firstSn;

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

                currentSession.SwitchToWorkFlow();

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }
               
                IList<PrintItem> resultPrintItems = currentSession.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;
                retLst.Add(resultPrintItems);
                int count = (int)currentSession.GetValue(Session.SessionKeys.DCode);
                retLst.Add(count);
                string loc = (string)currentSession.GetValue(Session.SessionKeys.DOCType);
                retLst.Add(loc);
                string flag = (string)currentSession.GetValue(Session.SessionKeys.EEP);
                retLst.Add(flag);
                return retLst;
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
                logger.Debug("(PalletVerifyImpl)Save end, firstSn:" + firstSn + "printItems" + printItems);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn">uutSn</param>
        public void Cancel(string uutSn)
        {
            logger.Debug("(PalletVerifyImpl)Cancel Start," + "uutSn:" + uutSn);
            try
            {
                string sessionKey = uutSn;
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
                logger.Debug("(PalletVerifyImpl)Cancel End," + "uutSn:" + uutSn);
            }

        }

        /// <summary>
        /// 调存储过程： HP_EDI.dbo.op_TemplateCheck_LANEW 
        /// </summary>
        /// <param name="dn">dn</param>
        /// <param name="docType">docType</param>
        /// <returns>DataTable</returns>
        public DataTable call_Op_TemplateCheckLaNew(string dn, string docType)
        {
            try
            {
                IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                DataTable TempDB = new DataTable();
                TempDB = repPizza.CallTemplateCheckLaNew(dn, docType);
                foreach (DataRow row in TempDB.Rows)
                {
                    if (row[0].ToString() == "ERROR")
                    {
                        FisException fe = new FisException("PAK105", new string[] { });  //存储过程HP_EDI.dbo.op_TemplateCheck_LANEW 报错，请检查！
                        throw fe;
                    }
                }

                return TempDB;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
        }


        /// <summary>
        /// 与Pallet绑定的所有Delivery
        /// </summary>
        /// <param name="palletNo">palletNo</param>
        /// <returns>list</returns>
        public IList<string> getDeliveryPalletList(string palletNo)
        {
            FisException ex;
            List<string> erpara = new List<string>();

            IDeliveryRepository ideliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IList<DeliveryPalletInfo> DeliveryPalletInfoList = ideliveryRepository.GetDeliveryPalletListByPlt(palletNo);
            if (DeliveryPalletInfoList == null || DeliveryPalletInfoList.Count <= 0)
            {
                erpara.Add(palletNo);    //没有获得当前Pallet的DeliveryPallet！
                ex = new FisException("PAK045", erpara);
                throw ex;
            }
            IList<string> DeliveryList = new List<string>();
            foreach (DeliveryPalletInfo iDPI in DeliveryPalletInfoList)
            {
                DeliveryList.Add(iDPI.deliveryNo);
            }
            return DeliveryList;

        }

        #endregion

        #region rePrint


        /// <summary>
        /// 重印
        /// </summary>
        /// <param name="palletNo">palletNo</param>
        /// <param name="reason">reason</param>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="printItems">printItems</param>

        public ArrayList rePrint(string palletNo, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems, out ArrayList printparams1, out string labeltypeBranch, out IList<string> PDFPLLst, out IList<string> modelList)
        {
            logger.Debug("(PalletVerifyOnlyImpl)rePrint Start," + "palletNo:" + palletNo + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer + "printItems" + printItems);
            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retLst = new ArrayList();
            string sessionKey = Guid.NewGuid().ToString();
            try
            {
                //获取Pallet Info
                IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                Pallet CurrentPallet = null;
                CurrentPallet = iPalletRepository.Find(palletNo);
                if (CurrentPallet == null)
                {
                    erpara.Add(palletNo);
                    ex = new FisException("CHK881", erpara);    // Pallet不存在！
                    throw ex;
                }

                var currentProduct = CommonImpl.GetProductByInput(palletNo, CommonImpl.InputTypeEnum.ProductIDOrCustSNOrPallet);

                if (currentProduct == null)
                {
                    erpara.Add(palletNo);
                    ex = new FisException("CHK079", erpara);    //找不到与此序号匹配的Product! 
                    throw ex;
                }

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

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PalletDataVerifyReprint.xoml", "", wfArguments);
                    currentSession.AddValue(Session.SessionKeys.PalletNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, "PalletDataReprint");
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "PalletDataVerifyReprintForDocking");
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

                // for edits begin
                IDeliveryRepository ideliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

                labeltypeBranch = string.Empty;
                string printflag = ideliveryRepository.GetDeliveryInfoValue(currentProduct.DeliveryNo, "Flag");
                if (printflag == "C")
                {
                    string cpmoflag = ideliveryRepository.GetDeliveryInfoValue(currentProduct.DeliveryNo, "Carrier");
                    if (cpmoflag == "XJ")
                    {
                        labeltypeBranch = "X";   //手动单:Pallet Label + CPMO Label
                    }
                    else labeltypeBranch = "C";   // 手动单:Pallet Label 
                }
                else labeltypeBranch = "A"; // 自动单：Pallet Label 

                printparams1 = new ArrayList();
                IList<DeliveryPalletInfo> DeliveryPalletInfoList = ideliveryRepository.GetDeliveryPalletListByPlt(palletNo);
                modelList = new List<string>();
                /*if (DeliveryPalletInfoList == null || DeliveryPalletInfoList.Count <= 0)
                {
                    erpara.Add(palletNo);    //没有获得当前Pallet的DeliveryPallet！
                    ex = new FisException("PAK045", erpara);
                    throw ex;
                }*/
                foreach (DeliveryPalletInfo iDPI in DeliveryPalletInfoList)
                {
                    printparams1.Add(iDPI.deliveryNo);
                    Delivery currentDelivery = ideliveryRepository.Find(iDPI.deliveryNo);
                    modelList.Add(currentDelivery.ModelName);
                }


                PDFPLLst = new List<string>();

                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> PLEditsURLLst = partRepository.GetValueFromSysSettingByName("PLEditsURL");
                if (PLEditsURLLst != null && PLEditsURLLst.Count > 0)
                {
                    PDFPLLst.Add(PLEditsURLLst[0]); //PDFPLLst[0]
                }

                IList<string> PLEditsTemplateLst = partRepository.GetValueFromSysSettingByName("PLEditsTemplate");
                if (PLEditsTemplateLst != null && PLEditsTemplateLst.Count > 0)
                {
                    PDFPLLst.Add(PLEditsTemplateLst[0]);//PDFPLLst[1]
                }

                IList<string> PLEditsXMLLst = partRepository.GetValueFromSysSettingByName("PLEditsXML");
                if (PLEditsXMLLst != null && PLEditsXMLLst.Count > 0)
                {
                    PDFPLLst.Add(PLEditsXMLLst[0]);//PDFPLLst[2]
                }

                IList<string> PLEditsPDFLst = partRepository.GetValueFromSysSettingByName("PLEditsPDF");
                if (PLEditsPDFLst != null && PLEditsPDFLst.Count > 0)
                {
                    PDFPLLst.Add(PLEditsPDFLst[0]); //PDFPLLst[3]
                }
                IList<string> PLEditsImageLst = partRepository.GetValueFromSysSettingByName("PLEditsImage");
                if (PLEditsImageLst != null && PLEditsImageLst.Count > 0)
                {
                    PDFPLLst.Add(PLEditsImageLst[0]); //PDFPLLst[4]
                }

                IList<string> FOPFullFileNameLst = partRepository.GetValueFromSysSettingByName("FOPFullFileName");
                if (FOPFullFileNameLst != null && FOPFullFileNameLst.Count > 0)
                {
                    PDFPLLst.Add(FOPFullFileNameLst[0]); //PDFPLLst[5]

                }
                IList<string> printexepathLst = partRepository.GetValueFromSysSettingByName("PDFPrintPath");
                if (printexepathLst != null && printexepathLst.Count > 0)
                {
                    PDFPLLst.Add(printexepathLst[0]); //PDFPLLst[6]
                }
                // for edits end

                IList<PrintItem> resultPrintItems = currentSession.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;
                retLst.Add(resultPrintItems);
                string flag = (string)currentSession.GetValue(Session.SessionKeys.EEP);
                retLst.Add(flag);
                return retLst;

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
                logger.Debug("(PalletVerifyOnlyImpl)rePrint End," + "palletNo:" + palletNo + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer + "printItems" + printItems);
            }

        }


        #endregion
    }


}
