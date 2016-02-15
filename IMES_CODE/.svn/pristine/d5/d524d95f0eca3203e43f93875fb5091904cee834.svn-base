/*
 * INVENTEC corporation (c)2011all rights reserved. 
 * Description: PAK PakUnitWeightNew interface implement
 * UI:CI-MES12-SPEC-PAK-UI Unit Weight.docx
 * UC:CI-MES12-SPEC-PAK-UC Unit Weight.docx                 
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2011-11-29 Chen Xu itc208014     Create 
 * Known issues:
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.StandardWeight;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IProduct = IMES.FisObject.FA.Product.IProduct;
using ModelInfo = IMES.FisObject.Common.Model.ModelInfo;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure.Repository.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PAK.Pizza;
using System.Data;
using System.Linq;

namespace IMES.Station.Implementation
{
    /// <summary>
    ///  UC 具体业务：  本站站号：85
    ///                1. Unit 称重；
    ///                2. Print Config Label; Print POD Label
    ///                3. 上传数据至SAP
    /// </summary>
    public class PakUnitWeightNew : MarshalByRefObject, IPakUnitWeightNew
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region GetCOMSettingInfo

        /// <summary>
        /// 获取COMSettingInfo
        /// </summary>
        /// <param name="hostname">hostname</param>
        /// <returns></returns>
        public IList<COMSettingDef> GetWeightSettingInfo(string hostname)
        {
            IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IList<COMSettingInfo> UnitWeighSettingInfo = new List<COMSettingInfo>();
            UnitWeighSettingInfo = iPalletRepository.FindCOMSettingByName(hostname);


            IList<COMSettingDef> UnitWeighInfoList = new List<COMSettingDef>();
            foreach (COMSettingInfo wsInfo in UnitWeighSettingInfo)
            {
                COMSettingDef wsd = new COMSettingDef();
                wsd.id = wsInfo.id;
                wsd.name = wsInfo.name;
                wsd.commport = wsInfo.commPort;
                wsd.baudRate = wsInfo.baudRate;
                wsd.rthreshold = wsInfo.rthreshold.ToString();
                wsd.sthreshold = wsInfo.sthreshold.ToString();
                wsd.handshaking = wsInfo.handshaking.ToString();
                wsd.editor = wsInfo.editor;
                wsd.cdt = wsInfo.cdt.ToString("yyyy-MM-dd hh:mm:ss");
                wsd.udt = wsInfo.udt.ToString("yyyy-MM-dd hh:mm:ss");

                UnitWeighInfoList.Add(wsd);

            }
            if (UnitWeighInfoList == null || UnitWeighInfoList.Count <= 0)
            {
                return null;
            }
            return UnitWeighInfoList;
        }

        #endregion

        #region PakUnitWeightNew

        /// <summary>
        /// 此站输入的是SN，需要在BLL中先根据SN获取Product调用CommonImpl.GetProductByInput()
        /// 如果获取不到，报CHK079！
        /// 用ProductID启动工作流
        /// 将获得的Product放到Session.Product中
        /// 获取Model和标准重量和ProductID
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="actualWeight"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="configParams"></param>
        /// <returns>ArrayList对象</returns>
        public ArrayList InputCustsn(string custSN, decimal actualWeight, string line, string editor, string station, string customer, out List<string> configParams)
        {
            logger.Debug("(PakUnitWeightNew)InputCustsn Start,"
                + " [custSN]:" + custSN
                + " [line]:" + line
                + " [editor]:" + editor
                + " [station]:" + station
                 + " [actualWeight]:" + actualWeight.ToString()
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                ArrayList retLst = new ArrayList();
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                if (currentProduct == null)
                {
                    FisException fe = new FisException("CHK079", new string[] { custSN });
                    throw fe;
                }
                string productID = currentProduct.ProId;
                string sessionKey = productID;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                //Mantis 961
                //获取Pallet No : IMES_FA..Product.PalletNo
                if (!currentProduct.IsBT && string.IsNullOrEmpty(currentProduct.PalletNo))
                {
                    erpara.Add(custSN);
                    ex = new FisException("PAK021", erpara);     //该Customer S/N还未与Pallet绑定!
                    throw ex;
                }
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
                    RouteManagementUtils.GetWorkflow(station, "PakUnitWeightNew.xoml", "PakUnitWeightNew.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.ActuralWeight, actualWeight);
                    currentSession.AddValue(Session.SessionKeys.PalletNo, currentProduct.PalletNo);
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

                string modelTolerance = currentSession.GetValue(Session.SessionKeys.Tolerance).ToString();
                decimal standardWeight = -1;
                if (currentSession.GetValue(Session.SessionKeys.StandardWeight) != null)
                {
                    standardWeight = (decimal)currentSession.GetValue(Session.SessionKeys.StandardWeight);
                }
              

                //PAQC 抽检: 查询IMES_FA..QCStatus 取Udt 最新的记录，如果该记录的Status = ‘8’，则在UI 上显示'PAQC 抽检'，否则该位置显示' '
                string labelType1 = string.Empty;

                IDeliveryRepository iDeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                ProductQCStatus qcStatus = iDeliveryRepository.GetQCStatus(productID, "PAQC");
                //Vincent for ICC special PAQC/PAQC
                if (qcStatus != null && (qcStatus.Status == "8" || qcStatus.Status == "B" || qcStatus.Status == "C"))
                {
                    labelType1 = "P" + qcStatus.Status;
                }

                //1.判断是否为BT Product, 2.于非BT Product
                string delievery = currentProduct.DeliveryNo;
                string model = string.IsNullOrEmpty(currentProduct.Model) ? "" : currentProduct.Model.Trim();
                string country = string.Empty;
                string printlabeltype1 = string.Empty; // Config Label
                string printlabeltype2 = string.Empty; // POD Label

                if (!currentProduct.IsBT)
                {
                    //获取与Customer S/N 绑定的Delivery No :IMES_FA..Product.DeliveryNo
                    if (string.IsNullOrEmpty(delievery))
                    {
                        erpara.Add(custSN);
                        ex = new FisException("PAK020", erpara);    //该Customer S/N还未与DN绑定!
                        throw ex;
                    }
                }
                else
                {
                    //获取Model : IMES_FA..Product.Model
                    if (string.IsNullOrEmpty(model))
                    {
                        erpara.Add(custSN);
                        ex = new FisException("PAK028", erpara);     //该Customer S/N还未与Model绑定!
                        throw ex;
                    }
                }
               
                IProductRepository iproductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
             
                // 4. Adaptor Label / India Label 判定
                string labelType2 = string.Empty;
                if (model.Length >= 11 && (model.Substring(9, 2) == "16" || model.Substring(9, 2) == "DM" || model.Substring(9, 2) == "D0"))
                {
                    labelType2 = "A";
                }
                else if (country == "INDIA")
                {
                    labelType2 = "I";
                }
      
                IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IMES.FisObject.Common.FisBOM.IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
                IHierarchicalBOM sessionBOM = null;
                sessionBOM = ibomRepository.GetHierarchicalBOMByModel(currentProduct.Model);
                IList<IBOMNode> bomNodeLst = new List<IBOMNode>();
                bomNodeLst = sessionBOM.FirstLevelNodes;
                CommonImpl2 cm2 = new CommonImpl2();
                string site = cm2.GetValueFromSysSetting("Site");
                currentSession.AddValue("Site", "ICC");
                currentSession.AddValue("BlackLabel",true);
                IList<string> lstValue
                         =cm2.GetConstValueTypeByType("POD_White_Lable_PN").Where(x=>x.value!="").Select(x=>x.value).ToList();
                    if (lstValue != null && lstValue.Count > 0 )
                    {
                        foreach (IBOMNode ibomnode in bomNodeLst)
                        {
                            if (lstValue.Contains(ibomnode.Part.PN))
                            { currentSession.AddValue("BlackLabel", false); break; }

                        }
                    }
                
                if (bomNodeLst == null || bomNodeLst.Count <= 0)
                {
                    erpara.Add(currentProduct.Model);
                    ex = new FisException("PAK039", erpara);
                    throw ex;
                }
                IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                string type = string.Empty;
        //       Model modelObject = iModelRepository.Find(currentProduct.Model);
                string family = currentProduct.Family;
                
              // Print =====如果为BT 产品，当满足如下条件时，需要列印Config Label=====

                currentSession.AddValue(Session.SessionKeys.labelBranch, "");
     //           delievery = currentProduct.DeliveryNo;
                configParams = new List<string>();
               
                //Revision: 9810:	修改列印Config Label 的条件为非BT
                if (!currentProduct.IsBT)
                {
                    string BTRegId = iDeliveryRepository.GetDeliveryInfoValue(delievery, "RegId");
                    string BTShipTp = iDeliveryRepository.GetDeliveryInfoValue(delievery, "ShipTp");
                    string BTCountry = iDeliveryRepository.GetDeliveryInfoValue(delievery, "Country");

                    if (!string.IsNullOrEmpty(BTRegId) && !string.IsNullOrEmpty(BTShipTp) && !string.IsNullOrEmpty(BTCountry))
                    {
                        //ITC-1360-1527:RegId 为'SNE'时，未能打印CONFIG LABEL
                        IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                         //bool bRegId = new[] { "SCN", "SAF", "SNE", "SCE", "QCN", "QAF", "QNE", "QCE" }.Any(BTRegId.Trim().Equals);
                       
                        IList<ConstValueTypeInfo> constValueTypeList = partRep.GetConstValueTypeList("ConfigLabelRegionId", BTRegId.Trim());
                        bool bRegId = (constValueTypeList != null && constValueTypeList.Count > 0);
                        
                       // if ((BTRegId == "SCN" || BTRegId == "SAF" || BTRegId == "SNE" || BTRegId == "SCE") && (BTShipTp == "CTO") && (BTCountry != "JAPAN"))
                        if (bRegId && (BTShipTp == "CTO") && (BTCountry != "JAPAN"))
                         {
                            printlabeltype1 = "ConfigLabel";
                            currentSession.AddValue(Session.SessionKeys.labelBranch, printlabeltype1);
                        }
                    }
                }

                //  Print =====如果IMES_PAK..PODLabelPart中有维护PartNo等于Model的前几位字符的记录 and (Model的第7位不是数字)，则需要列印POD Label====

                IList<PODLabelPartDef> podLabelPartLst = new List<PODLabelPartDef>();
                podLabelPartLst = ipartRepository.GetPODLabelPartListByPartNo(model);
                if (podLabelPartLst.Count > 0)
                {
                    string number = "0123456789";
                    string modelbit = currentProduct.Model.Substring(6, 1);
                    if (!number.Contains(modelbit)) //Model的第7位不是数字
                    {
                        printlabeltype2 = "PODLabel";
                        currentSession.AddValue(Session.SessionKeys.labelBranch, printlabeltype2);  //如果列印PodLabel，则需要再记录一次ProductLog (Station = 'PD'，Line = 'POD Label Print')
                    }
                }
                IList<string> EditsFISAddrLst = new List<string>();
                string EditsFISAddr = string.Empty;
                EditsFISAddrLst = ipartRepository.GetValueFromSysSettingByName("EditsFISAddr");
                if (EditsFISAddrLst != null && EditsFISAddrLst.Count > 0)
                {
                    EditsFISAddr = EditsFISAddrLst[0];
                } 
               
                IList<BomItemInfo> bomItemList = PartCollection.GeBOM(sessionKey, Session.SessionType.Product);
                S_UnitWeightNew sUnitWeightNew = new S_UnitWeightNew
                                                                       {
                                                                           ProductID = productID,
                                                                           Model = model,
                                                                           StandardWeight = standardWeight,
                                                                           PAQC = labelType1,
                                                                           FAI=CheckFAI(model).Replace(currentProduct.CUSTSN,"").Replace("[","").Replace("]",""),
                                                                           IndiaLabel = labelType2,
                                                                           ConfigLabel = printlabeltype1,
                                                                           PODLabel = printlabeltype2,
                                                                           EditsFISAddr = EditsFISAddr,
                                                                           BomItemList = bomItemList
                                                                       };
                      
                retLst.Add(sUnitWeightNew);  
                return retLst;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                if (e.mErrcode == "CHK020") //序號已被刷入
                {
                    throw e;
                }
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(PakUnitWeightNew)InputCustsn End,"
                    + " [custSN]:" + custSN
                    + " [line]:" + line
                    + " [editor]:" + editor
                    + " [station]:" + station
                    + " [customer]:" + customer);
            }

        }

        public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue)
        {
            return PartCollection.TryPartMatchCheck(sessionKey, Session.SessionType.Product, checkValue);
        }

        
    

        /// <summary>
        /// 保存机器重量，更新机器状态，返回打印重量标签的PrintItem，结束工作流。
        /// 将printItems放到Session.PrintItems中
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public IList<PrintItem> Save(string productID, IList<PrintItem> printItems)
        {
            logger.Debug("(PakUnitWeightNew)Save Start,"
                + " [productID]:" + productID + "[printItems]" + printItems);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(productID, Session.SessionType.Product);

                if (currentSession == null)
                {
                    erpara.Add(productID);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {

                    //1.	如果Product Model 是'PC' 开头，并且非BT 产品，如果该Product 尚未绑定Delivery，则报告错误：“尚未结合Delivery!”
                    Product currentProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                    //if (currentProduct.Model.Substring(0, 2) == "PC" && !currentProduct.IsBT)
                    // mantis 2071
                    if (CommonImpl.GetInstance().CheckModelByProcReg(currentProduct.Model, "SKU,ThinClient") && !currentProduct.IsBT)
                    {
                        if (string.IsNullOrEmpty(currentProduct.DeliveryNo))
                        {
                            erpara.Add(productID);
                            ex = new FisException("PAK036", erpara);
                            throw ex;
                        }
                    }
                    currentSession.AddValue("HavePrintItem", false);
                    if (printItems != null && printItems.Count > 0)
                    {
                        currentSession.AddValue("HavePrintItem", true);
                    }

                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, true);
                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();
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
                logger.Debug("(PakUnitWeightNew)Save End,"
                    + " [productID]:" + productID + "[printItems]" + printItems);
            }
        }



        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="custSN"></param>
        public void Cancel(string custSN)
        {
            logger.Debug("(PakUnitWeightNew)Cancel Start,"
               + " [custSN]:" + custSN);

            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
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
                logger.Debug("(PakUnitWeightNew)Cancel End,"
                    + " [productID]:" + custSN);
            }

        }
        #endregion




        #region Reprint

        /// <summary>
        /// 重印Unit Weight Label
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="reason"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItemList"></param>
        /// <param name="printlabeltype"></param>
        /// <param name="model"></param>
        /// <param name="printexepath"></param>
        /// <returns></returns>
        public IList<PrintItem> ReprintLabel(string custSN, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItemList, out string printlabeltype, out string model, out string printexepath)
        {
            logger.Debug("(PakUnitWeightNew)ReprintLabel Start,"
                            + " [custSN]:" + custSN
                            + " [reason]:" + reason
                            + " [line]:" + line
                            + " [editor]:" + editor
                            + " [station]:" + station
                            + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                //if (currentProduct == null)
                //{
                //    FisException fe = new FisException("CHK079", new string[] { custSN });
                //    throw fe;
                //}
                if (currentProduct == null)
                {
                    erpara.Add(custSN);
                    ex = new FisException("PAK042", erpara);
                    throw ex;
                }

                //获取与Customer S/N 绑定的Delivery No :IMES_FA..Product.DeliveryNo
                if (!currentProduct.IsBT && string.IsNullOrEmpty(currentProduct.DeliveryNo))
                {
                    erpara.Add(custSN);
                    ex = new FisException("PAK020", erpara);    //该Customer S/N还未与DN绑定!
                    throw ex;
                }
                string delievery = currentProduct.DeliveryNo;

                //获取Pallet No : IMES_FA..Product.PalletNo
                if (!currentProduct.IsBT && string.IsNullOrEmpty(currentProduct.PalletNo))
                {
                    erpara.Add(custSN);
                    ex = new FisException("PAK021", erpara);     //该Customer S/N还未与Pallet绑定!
                    throw ex;
                }
                string palletno = currentProduct.PalletNo;

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
                    RouteManagementUtils.GetWorkflow(station, "PakUnitWeightReprint.xoml", "PakUnitWeightReprint.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.CustSN, custSN);
                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, custSN);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, custSN);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "UnitWeightReprint");
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItemList);
                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, delievery);
                    currentSession.AddValue(Session.SessionKeys.PalletNo, palletno);
                    currentSession.AddValue("HavePrintItem", false);
                    if (printItemList != null && printItemList.Count > 0)
                    {
                        currentSession.AddValue("HavePrintItem", true);
                    }

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(custSN);
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

                printlabeltype = string.Empty;
                printlabeltype = (string)currentSession.GetValue(Session.SessionKeys.labelBranch);

                model = string.Empty;
                model = currentProduct.Model;

                printexepath = string.Empty;
                IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> printexepathLst = ipartRepository.GetValueFromSysSettingByName("PDFPrintPath");
                if (printexepathLst != null && printexepathLst.Count > 0)
                {
                    printexepath = printexepathLst[0];
                }
                else printexepath = "C:\\FIS\\";

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
                logger.Debug("(PakUnitWeightNew)ReprintLabel End,"
            + " [custSN]:" + custSN
            + " [reason]:" + reason
            + " [line]:" + line
            + " [editor]:" + editor
            + " [station]:" + station
            + " [customer]:" + customer);
            }
        }
        #endregion

        /// <summary>
        /// 获取ModelWeight
        /// </summary>
        /// <param name="inputData">inputData</param>
        /// <returns></returns>
        public ModelWeightDef GetModelWeightByModelorCustSN(string inputData)
        {
            //检查合法model
            //看取得的数据是否有数据
            String result = "";
            try
            {
                string model = string.Empty;
                IModelRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();


                //3)	如果刷入为10位，如果是CN打头的初步认定为Customer SN，如在Product．CustSN中不存在，则提示“非法的Customer SN”
                //if (inputData.Length == 10 && inputData.Substring(0, 2) == "CN")
                if (CommonImpl.CheckCustSNPreFix2(inputData))
                {
                    var currentProduct = CommonImpl.GetProductByInput(inputData, CommonImpl.InputTypeEnum.CustSN);
                    if (currentProduct == null)
                    {
                        FisException fe = new FisException("PAK042", new string[] { inputData });  //此Customer S/N %1 不存在！
                        throw fe;
                    }
                    else if (string.IsNullOrEmpty(currentProduct.Model))
                    {
                        FisException fe = new FisException("PAK028", new string[] { inputData });  //该Customer SN %1 还未与Model绑定！
                        throw fe;
                    }
                    else model = currentProduct.Model;
                }
                else
                {
                    Model modelItem = itemRepository.Find(inputData);
                    if (modelItem == null)
                    {
                        var currentProduct = CommonImpl.GetProductByInput(inputData, CommonImpl.InputTypeEnum.CustSN);
                        if (currentProduct == null || string.IsNullOrEmpty(currentProduct.Model))
                        {
                            FisException fe = new FisException("CHK079", new string[] { inputData });   //找不到与此序号 %1 匹配的Product! 
                            throw fe;
                        }
                        else model = currentProduct.Model;

                    }
                    else model = inputData;

                }

                IModelWeightRepository itemRepositoryModelWeight = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository>();

                DataTable modelWeight = itemRepositoryModelWeight.GetModelWeightItem(model);
                if (modelWeight == null || modelWeight.Rows.Count == 0)
                {
                    //This Model, there is no standard weight, please go to the weighing.
                    //该Model尚无标准重量，请先去称重。
                    List<string> erpara = new List<string>();
                    erpara.Add(model);
                    FisException ex;
                    ex = new FisException("PAK123", erpara);
                    throw ex;
                }

                result = modelWeight.Rows[0][1].ToString();

                if (result == "")
                {
                    //该Model尚无标准重量，请先去称重。
                    List<string> erpara = new List<string>();
                    erpara.Add(model);
                    FisException ex;
                    ex = new FisException("PAK123", erpara);
                    throw ex;
                }

                ModelWeightDef item = new ModelWeightDef();
                item.Model = model;
                item.UnitWeight = result;
                return item;
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
        /// 保存修改的ModelWeight
        /// </summary>
        /// <param name="item">item</param>
        /// <returns></returns>
        public void SaveModelWeightItem(ModelWeightDef item)
        {

            try
            {

                IModelWeightRepository itemRepositoryModelWeight = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository>();
                //看取得的数据是否有, 防止update的是空记录，但[PAK_SkuMasterWeight_FIS]中加入了记录
                DataTable modelWeight = itemRepositoryModelWeight.GetModelWeightItem(item.Model);
                if (modelWeight == null || modelWeight.Rows.Count == 0)
                {
                    //该Model尚无标准重量，请先去称重。
                    List<string> erpara = new List<string>();
                    erpara.Add(item.Model);
                    FisException ex;
                    ex = new FisException("PAK123", erpara);
                    throw ex;
                }

                IPizzaRepository itemRepositoryPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository>();

                ModelWeightInfo setValue = new ModelWeightInfo();
                setValue.unitWeight = Decimal.Parse(item.UnitWeight);
                setValue.editor = item.Editor;
                setValue.udt = DateTime.Now;

                ModelWeightInfo condition = new ModelWeightInfo();
                condition.model = item.Model;

                PakSkuMasterWeightFisInfo pakSkuMasterWeight = new PakSkuMasterWeightFisInfo();
                pakSkuMasterWeight.model = item.Model;
                pakSkuMasterWeight.weight = setValue.unitWeight;
                pakSkuMasterWeight.cdt = setValue.udt;

                UnitOfWork uow = new UnitOfWork();
                itemRepositoryModelWeight.UpdateModelWeightDefered(uow, setValue, condition);
                itemRepositoryPizza.DeletetPakSkuMasterWeightFisByModelDefered(uow, item.Model);
                itemRepositoryPizza.InsertPakSkuMasterWeightFisDefered(uow, pakSkuMasterWeight);
                uow.Commit();

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
        public ArrayList GetPODLabelPathAndSite()
        {
           ArrayList arr = new ArrayList();
           string path = "";
           CommonImpl2 cm2 = new CommonImpl2();
            path = cm2.GetValueFromSysSetting("PODLabelPath"); 
           arr.Add(path);
           return arr;
        }

        public string GetCqPodLabelColor(string model)
        {
           
            IMES.FisObject.Common.FisBOM.IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
            IHierarchicalBOM sessionBOM = null;
            sessionBOM = ibomRepository.GetHierarchicalBOMByModel(model);
            IList<IBOMNode> bomNodeLst = new List<IBOMNode>();
            bomNodeLst = sessionBOM.FirstLevelNodes;
            CommonImpl2 cm2 = new CommonImpl2();
            string color = "Black";
            IList<string> lstValue
                     = cm2.GetConstValueTypeByType("POD_White_Lable_PN").Where(x => x.value != "").Select(x => x.value).ToList();
                if (lstValue != null && lstValue.Count > 0)
                {
                    foreach (IBOMNode ibomnode in bomNodeLst)
                    {
                        if (lstValue.Contains(ibomnode.Part.PN))
                        { color = "White"; ; break; }

                    }
                }
                return color;
        }

        public string CheckFAI(string model)
        {
            IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
            FAIModelInfo faimodel = iModelRepository.GetFAIModelByModelWithTrans(model);
            string r = "";
            if (faimodel != null && faimodel.PAKState == "Hold")
            {
                FisException err = new FisException("CQCHK1096", new List<string>());
                r = err.mErrmsg;
            }
            return r;
        }
    }
}
