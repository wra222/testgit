/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for CombineDNPalletforBT Page            
 * CI-MES12-SPEC-PAK Combine DN & Pallet for BT.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc207003              Create
 * Known issues:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.COA;
using IMES.FisObject.PAK.Pallet;
using log4net;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.FisObject.Common.Model;
using IBOMRepository = IMES.FisObject.Common.FisBOM.IBOMRepository;
using IMES.Route;

using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.Common.MO;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for CombineDNPalletforBT.
    /// </summary>
    public class _CombineDNPalletforBT : MarshalByRefObject, ICombineDNPalletforBT
    {
        private const Session.SessionType SessionType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeliveryRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        private ICOAStatusRepository coaRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
        private IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
        private IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
        private IMORepository moRep = RepositoryFactory.GetInstance().GetRepository<IMORepository>();
        #region ICombineDNPalletforBT Members
        /// <summary>
        /// 获取DN表相关信息GetDeliveryPalletListByDN
        /// </summary>
        public IList<S_RowData_DN> GetDNList()
        {
            logger.Debug("(_CombineDNPalletforBT)GetDNList start.");
            try
            {
                IList<S_RowData_DN> ret = new List<S_RowData_DN>();
                DNQueryCondition condition = new DNQueryCondition();
                DateTime temp = DateTime.Now;
                temp = temp.AddDays(-3);
                condition.ShipDateFrom = new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0, 0);
                IList<DNForUI> dnList = currentRepository.GetDNListByCondition(condition);
                foreach (DNForUI tmp in dnList)
                {
                    S_RowData_DN ele = new S_RowData_DN();
                    ele.DeliveryNO = tmp.DeliveryNo;
                    if (tmp.Status != "00")
                    {
                        continue;
                    }
                    if (!(tmp.ModelName.Length == 12 && tmp.ModelName.Substring(0, 2) == "PC"))
                    {
                        continue;
                    }
                    ele.Model = tmp.ModelName;
                    ele.CustomerPN = currentRepository.GetDeliveryInfoValue(tmp.DeliveryNo, "PartNo");
                    ele.PoNo = tmp.PoNo;
                    ele.Date = tmp.ShipDate.ToString();
                    ele.Qty = tmp.Qty.ToString();
                    IList<IProduct> productList = new List<IProduct>();
                    productList = productRepository.GetProductListByDeliveryNo(tmp.DeliveryNo);
                    if (null != productList)
                    {
                        ele.PackedQty = productList.Count.ToString();
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
                logger.Debug("(_CombineDNPalletforBT)GetDNList end.");
            }
        }
        /// <summary>
        /// 获取Pallet表相关信息GetDeliveryPalletListByDN
        /// </summary>
        /// <param name="DN"DN</param>
        public IList<SelectInfoDef> GetPalletList(string DN)
        {
            logger.Debug("(_CombineDNPalletforBT)GetPalletList start.");
            try
            {
                IList<SelectInfoDef> ret = new List<SelectInfoDef>();
                IList<DeliveryPalletInfo> palletList = currentRepository.GetDeliveryPalletListByDN(DN);
                foreach (DeliveryPalletInfo tmp in palletList)
                {
                    SelectInfoDef ele = new SelectInfoDef();
                    ele.Text = tmp.palletNo;
                    //Pallet CurrentPallet = palletRep.Find(tmp.palletNo);
                    ele.Value = tmp.deliveryQty.ToString();
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
                logger.Debug("(_CombineDNPalletforBT)GetPalletList end.");
            }
        }
        /// <summary>
        /// 获取product表相关信息 GetProductListByDeliveryNoAndPalletNo
        /// </summary>
        /// <param name="PalletNo">PalletNo</param>
        /// <param name="DN">DN</param>
        public IList<ProductModel> GetProductList(string PalletNo, string DN)
        {
            logger.Debug("(_CombineDNPalletforBT)GetProductList start.");
            try
            {
                //IList<ProductModel> ret = new List<ProductModel>();
                IList<ProductModel> retValue = new List<ProductModel>();
                //ret = productRepository.GetProductListByPalletNo(PalletNo);
                if (DN == "------------")
                {
                    retValue = productRepository.GetProductListByPalletNo(PalletNo); 
                }
                else
                {
                    IList<IProduct> ret = new List<IProduct>();
                    ret = productRepository.GetProductListByDeliveryNoAndPalletNo(DN, PalletNo);
                    foreach (IProduct tmp in ret)
                    {
                        ProductModel ele = new ProductModel();
                        ele.CustSN = tmp.CUSTSN;
                        ele.ProductID = tmp.ProId;
                        ele.Model = tmp.Model;
                        retValue.Add(ele);
                    }
                }
                return retValue;
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
                logger.Debug("(_CombineDNPalletforBT)GetProductList end.");
            }
        }
        /// <summary>
        /// CheckDN
        /// </summary>
        /// <param name="DN">DN</param>
        public int CheckDN(string DN)
        {
            try
            {
                if (DN == "------------")
                {
                    return 1;
                }
                int PackedQty = 0;
                int Qty = 0;
                IList<IProduct> productList = new List<IProduct>();
                productList = productRepository.GetProductListByDeliveryNo(DN);
                if (null != productList)
                {
                    PackedQty = productList.Count;
                }
                Delivery CurrentDelivery = currentRepository.Find(DN);
                if (CurrentDelivery != null)
                {
                    Qty = CurrentDelivery.Qty;
                }
                if (PackedQty == Qty )
                {
                    return 2;
                }
                else if (PackedQty > Qty)
                {
                    return 3;
                }
                return 0;
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
        }
        /// <summary>
        /// CheckProductAndDN
        /// </summary>
        /// <param name="custSN">customer SN</param>
        /// <param name="DN">DN</param>
        public string CheckProductAndDN(string custSN, string DN)
        {
           try
            {
                
                IProduct product = null;
                product = productRepository.GetProductByCustomSn(custSN);
                if (null == product)
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(custSN);
                    throw new FisException("SFC002", errpara);
                }
                if (DN == "------------")
                {
                    return product.Model;
                }
                Delivery CurrentDelivery = currentRepository.Find(DN);
                if (CurrentDelivery == null)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(DN);
                    throw new FisException("CHK107", errpara);
                }
               
                string isCDSI = "";
                string isFactoryPo = "";
                IList<IMES.FisObject.Common.Model.ModelInfo> isPO = modelRep.GetModelInfoByModelAndName(product.Model, "PO");
                foreach (IMES.FisObject.Common.Model.ModelInfo tmpPO in isPO)
                {
                    if (tmpPO.Value == "Y")
                    {
                        isCDSI = "true";
                    }
                    else
                    {
                        IList<IMES.FisObject.Common.Model.ModelInfo> isATSNAV = modelRep.GetModelInfoByModelAndName(product.Model, "ATSNAV");
                        foreach (IMES.FisObject.Common.Model.ModelInfo tmpATSNAV in isATSNAV)
                        {
                            if (tmpATSNAV.Value == null)
                            {
                            }
                            else if (tmpATSNAV.Value != "")
                            {
                                isCDSI = "true";
                            }
                            break;
                        }
                    }
                    break;
                }
                if (isCDSI == "true")
                {

                    CdsiastInfo condition = new CdsiastInfo();
                    condition.snoId = product.ProId;
                    condition.tp = "FactoryPO";
                    IList<CdsiastInfo> isCdsiastInfo = productRepository.GetCdsiastInfoList(condition);
                    isFactoryPo = "";
                    foreach (CdsiastInfo tmpCdsiastInfo in isCdsiastInfo)
                    {
                        isFactoryPo = tmpCdsiastInfo.sno;
                        break;
                    }
                    if (isFactoryPo == "" || isFactoryPo == null)
                    {
                        throw new FisException("CHK882", new string[] { });
                    }
                }
                else if (product.IsBindedPo)  //Vincent add check bind  PO case
                {
                    isCDSI = "true";
                    isFactoryPo = product.BindPoNo;
                }

                if (product.Model != CurrentDelivery.ModelName)
                {

                    if (isCDSI == "true")
                    {
                        return product.Model + "#@#" + isFactoryPo;
                    }
                    else
                    {
                        return product.Model;
                    }
                }
                else
                {
                    if (isCDSI == "true")
                    {
                        if (CurrentDelivery.PoNo == isFactoryPo)
                        {
                            return "";
                        }
                        else
                        {
                            return product.Model + "#@#" + isFactoryPo;
                        }
                    }
                    else  //非bind Po case 即為 Normal case
                    {
                        //Vincent 檢查選中的PO 是否Bind PO
                        if (! string.IsNullOrEmpty(CurrentDelivery.PoNo))
                        {
                            IList<string> bindPoNoList = moRep.GetBindPoNoByModel(product.Model);
                            if (bindPoNoList != null && 
                                bindPoNoList.Count > 0  &&
                                 bindPoNoList.Contains(CurrentDelivery.PoNo))
                            {
                                return product.Model;                               
                            }
                        }
                        return "";
                    }
                }
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
        }
        /// <summary>
        /// check product
        /// </summary>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="custSN">customer SN</param>
        public int CheckProduct(string line,string editor, string station, string customer,string custSN)
        {
           
            string keyStr = "";
            try
            {
                string sessionKey = custSN;
                keyStr = sessionKey;
                Session currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);
                Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                wfArguments.Add("Key", sessionKey);
                wfArguments.Add("Station", station);
                wfArguments.Add("CurrentFlowSession", currentSession);
                wfArguments.Add("Editor", editor);
                wfArguments.Add("PdLine", line);
                wfArguments.Add("Customer", customer);
                wfArguments.Add("SessionType", SessionType);

                string wfName, rlName;
                RouteManagementUtils.GetWorkflow(station, "CombineDNPalletforBTBlock.xoml", "", out wfName, out rlName);
                WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                currentSession.SetInstance(instance);
                if (!SessionManager.GetInstance.AddSession(currentSession))
                {
                    currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }
                currentSession.WorkflowInstance.Start();
                currentSession.SetHostWaitOne();
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }
               
                IProduct product = null;
                product = productRepository.GetProductByCustomSn(custSN);
                if (null == product)
                {
                    //no data
                    return -10;
                }
                string isCDSI = "";
                string isFactoryPo = "";
                IList<IMES.FisObject.Common.Model.ModelInfo> isPO = modelRep.GetModelInfoByModelAndName(product.Model, "PO");

                foreach (IMES.FisObject.Common.Model.ModelInfo tmpPO in isPO)
                {
                    if (tmpPO.Value == "Y")
                    {
                        isCDSI = "true";
                    }
                    else
                    {
                        IList<IMES.FisObject.Common.Model.ModelInfo> isATSNAV = modelRep.GetModelInfoByModelAndName(product.Model, "ATSNAV");
                        foreach (IMES.FisObject.Common.Model.ModelInfo tmpATSNAV in isATSNAV)
                        {
                            if (tmpATSNAV.Value == null)
                            { 
                            }
                            else if (tmpATSNAV.Value != "")
                            {
                                isCDSI = "true";
                            }
                            break;
                        }
                    }
                    break;
                }
                if (isCDSI == "true")
                {

                    CdsiastInfo condition = new CdsiastInfo();
                    condition.snoId = product.ProId;
                    condition.tp = "FactoryPO";
                    IList<CdsiastInfo> isCdsiastInfo = productRepository.GetCdsiastInfoList(condition);
                    isFactoryPo = "";
                    foreach (CdsiastInfo tmpCdsiastInfo in isCdsiastInfo)
                    {
                        isFactoryPo = tmpCdsiastInfo.sno;
                        break;
                    }
                    if (isFactoryPo == "" || isFactoryPo == null)
                    {
                        throw new FisException("CHK882", new string[] { });
                    }
                }
                if (!product.IsBT)
                {
                    //非bt
                    return -1;
                }
                bool checkCoa = false;
                // 取ModelBOM 中Model 直接下阶中有BomNodeType 为'P1' 的Part
                IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
                IHierarchicalBOM sessionBOM = null;
                sessionBOM = ibomRepository.GetHierarchicalBOMByModel(product.Model);
                IList<IBOMNode> bomNodeLst = new List<IBOMNode>();

                bomNodeLst = sessionBOM.FirstLevelNodes;
                IList<string> partNOList = new List<string>();
                if (bomNodeLst != null && bomNodeLst.Count > 0)
                {
                    foreach (IBOMNode ibomnode in bomNodeLst)
                    {
                        IPart currentPart = ibomnode.Part;
                        if (currentPart.BOMNodeType == "P1" && currentPart.Descr.IndexOf("COA") != -1)
                        {
                            partNOList.Add(currentPart.PN);
                        }
                    }
                }
                else
                {
                    //no data
                    return -9;
                }

                foreach (IProductPart tmp in product.ProductParts)
                {
                    if (tmp.PartSn != null && tmp.PartID != null)
                    {
                        if (partNOList.Contains(tmp.PartID))
                        {
                            IList<COAStatus> reCoa = coaRepository.GetCOAStatusRange(tmp.PartSn, tmp.PartSn);
                            foreach (COAStatus coaItem in reCoa)
                            {
                                checkCoa = true;
                                if (coaItem.IECPN != tmp.PartID)
                                {
                                    //COA not matches! 
                                    return -2;
                                }
                            }
                        }
                    }
                }
                if (!checkCoa)
                {
                    //success
                    return 1;
                }
                //success
                return 2;
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
                Session sessionDelete = SessionManager.GetInstance.GetSession(keyStr, SessionType); ;
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(_CombineDNPalletforBT)CheckProduct end");
            }
        }

        private static object _syncObj4AssignAll = new object();
        /// <summary>
        /// AssignAll
        /// </summary>
        /// <param name="custSN">customer SN</param>
        /// <param name="line">line</param>
        /// <param name="code">code</param>
        /// <param name="floor">floor</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="DN">DN</param>
        public string AssignAll(string custSN, string line, string code, string floor,
                                                    string editor, string station, string customer, string DN)
        {
            string keyStr = "";
            string prodId = "";
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                if (null == currentProduct)
                {
                    List<string> err = new List<string>();
                    err.Add(custSN);
                    throw new FisException("SFC002", err);
                }
                prodId = currentProduct.ProId;
                string sessionKey = custSN;
                keyStr = sessionKey;
                Session currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);
                Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                wfArguments.Add("Key", sessionKey);
                wfArguments.Add("Station", station);
                wfArguments.Add("CurrentFlowSession", currentSession);
                wfArguments.Add("Editor", editor);
                wfArguments.Add("PdLine", line);
                wfArguments.Add("Customer", customer);
                wfArguments.Add("SessionType", SessionType);

                string wfName, rlName;
                RouteManagementUtils.GetWorkflow(station, "CombineDNPalletforBT.xoml", "CombineDNPalletforBT.rules", out wfName, out rlName);
                WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                currentSession.AddValue(Session.SessionKeys.CustSN, custSN);
                currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                currentSession.AddValue(Session.SessionKeys.Floor, floor);
                currentSession.AddValue(Session.SessionKeys.MBCode, code);
                currentSession.AddValue(Session.SessionKeys.DeliveryNo, DN);
                currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                if (DN == "")
                {
                    //auto is true
                    currentSession.AddValue(Session.SessionKeys.ifElseBranch, true);
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.ifElseBranch, false);
                }
                
               
                currentSession.SetInstance(instance);

                if (!SessionManager.GetInstance.AddSession(currentSession))
                {
                    currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                //Lock The XXX: 2012.04.21 LiuDong
                //Guid gUiD = Guid.Empty;
                //ConcurrentLocksInfo identity = null;
                //bool isFirstBranch = !(bool)currentSession.GetValue(IMES.Infrastructure.Session.SessionKeys.ifElseBranch);
                //if (isFirstBranch)
                //{
                //    identity = new ConcurrentLocksInfo();
                //    identity.clientAddr = "N/A";
                //    identity.customer = customer;
                //    identity.descr = string.Format("ThreadID: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                //    identity.editor = editor;
                //    identity.line = line;
                //    identity.station = station;
                //    identity.timeoutSpan4Hold = new TimeSpan(0, 0, 3).Ticks;
                //    identity.timeoutSpan4Wait = new TimeSpan(0, 0, 5).Ticks;
                //}
                //lock (_syncObj4AssignAll)
                //{
                //    try
                //    {
                //        if (isFirstBranch)
                //            gUiD = productRepository.GrabLockByTransThread("Delivery", DN, identity);

                        currentSession.WorkflowInstance.Start();
                        currentSession.SetHostWaitOne();
                //    }
                //    finally
                //    {
                //        productRepository.ReleaseLockByTransThread("Ucc", (Guid)currentSession.GetValue<Guid>(Session.SessionKeys.lockToken_Ucc));
                //        productRepository.ReleaseLockByTransThread("Box", (Guid)currentSession.GetValue<Guid>(Session.SessionKeys.lockToken_Box));
                //        productRepository.ReleaseLockByTransThread("Loc", (Guid)currentSession.GetValue<Guid>(Session.SessionKeys.lockToken_Loc));
                //        productRepository.ReleaseLockByTransThread("Pallet", (Guid)currentSession.GetValue<Guid>(Session.SessionKeys.lockToken_Pallet));
                //        if (isFirstBranch)
                //            productRepository.ReleaseLockByTransThread("Delivery", gUiD);
                //        else
                //            productRepository.ReleaseLockByTransThread("Delivery", (Guid)currentSession.GetValue<Guid>(Session.SessionKeys.lockToken_DN));
                //    }
                //}
                //Release The XXX: 2012.04.21 LiuDong
                
                
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }
                string temp = "";
                Delivery thisDelivery = (Delivery)currentSession.GetValue(Session.SessionKeys.Delivery);
                if (thisDelivery != null)
                {
                    temp = thisDelivery.DeliveryNo;
                }
            
                return temp;
            }
            catch (FisException e)
            {
                if (!string.IsNullOrEmpty(prodId))
                {
                    IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                    deliveryRep.RollBackAssignBoxId(prodId);
                }

                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                if (!string.IsNullOrEmpty(prodId))
                {
                    IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                    deliveryRep.RollBackAssignBoxId(prodId);
                }
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {          

                Session sessionDelete = SessionManager.GetInstance.GetSession(keyStr, SessionType); ;
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(_CombineDNPalletforBT)AssignAll end, custSn:" + custSN);
            }
        }
        /// <summary>
        /// GetTemplateName
        /// </summary>
        /// <param name="DN">DN</param>
        public string GetTemplateName(string DN)
        {
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            string ret = "";
            try
            {
                string manualOder = currentRepository.GetDeliveryInfoValue(DN, "Flag");
                string para2 = currentRepository.GetDeliveryInfoValue(DN, "RegId");
                //if (!(para1 == "N" && para2 != "SCN"))
                //{
                    //return ret;
                //}
                //得到模板：
                //Vincent add check manua order
                if (manualOder != "N")
                {
                    return ret;
                }

                IList<string> docnumList = new List<string>();
                IList<string> tempList = new List<string>();
                bool bFind = false;
                //select @doc_set_number = DOC_SET_NUMBER  from [PAK.PAKComn] where left(InternalID,10) = @InternalID的前10位
                docnumList = repPizza.GetDocSetNumListFromPakDashPakComnByLikeInternalID(DN.Substring(0, 10));
                foreach (string num in docnumList)
                {
                    //SELECT @templatename = XSL_TEMPLATE_NAME FROM [PAK.PAKRT] WHERE DOC_CAT = @doctpye AND DOC_SET_NUMBER = @doc_set_number
                    tempList = repPizza.GetXslTemplateNameListFromPakDashPakComnByDocCatAndDocSetNumer("Box Ship Label", num);
                    if (tempList != null && tempList.Count != 0)
                    {
                        bFind = true;
                        foreach (string atemp in tempList)
                        {
                            ret = atemp;
                            break;
                        }
                        break;
                    }
                }
                if (bFind == false)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(DN);
                    //ex = new FisException("CHK834", erpara);
                    ex = new FisException("CHK874", new string[] { });
                    //ex.stopWF = false;
                    throw ex;
                }
                return ret;
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

        public ArrayList GetSysSettingList(IList<string> nameList)
        {
            try
            {
                List<string> erpara = new List<string>();
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                ArrayList retList = new ArrayList();

                foreach (string node in nameList)
                {
                    IList<string> valueList = new List<string>();
                    valueList = partRepository.GetValueFromSysSettingByName(node);
                    if (valueList.Count == 0)
                    {
                        retList.Add("");
                    }
                    else
                    {
                        retList.Add(valueList[0]);
                    }
                }
                return retList;
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
                logger.Debug("(_CombineDNPalletforBT)GetSysSetting, name:");
            }

        }

        public ArrayList AssignAllNew(string custSN, string line, string code, string floor,
                                                 string editor, string station, string customer, string DN)
        {
            string keyStr = "";
            string prodId = "";
            ArrayList arrRet = new ArrayList();
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                if (null == currentProduct)
                {
                    List<string> err = new List<string>();
                    err.Add(custSN);
                    throw new FisException("SFC002", err);
                }
                prodId = currentProduct.ProId;
                string sessionKey = custSN;
                keyStr = sessionKey;
                Session currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);
                Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                wfArguments.Add("Key", sessionKey);
                wfArguments.Add("Station", station);
                wfArguments.Add("CurrentFlowSession", currentSession);
                wfArguments.Add("Editor", editor);
                wfArguments.Add("PdLine", line);
                wfArguments.Add("Customer", customer);
                wfArguments.Add("SessionType", SessionType);

                string wfName, rlName;
                RouteManagementUtils.GetWorkflow(station, "CombineDNPalletforBT.xoml", "CombineDNPalletforBT.rules", out wfName, out rlName);
                WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                currentSession.AddValue(Session.SessionKeys.CustSN, custSN);
                currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                currentSession.AddValue(Session.SessionKeys.Floor, floor);
                currentSession.AddValue(Session.SessionKeys.MBCode, code);
                currentSession.AddValue(Session.SessionKeys.DeliveryNo, DN);
                currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                if (DN == "")
                {
                    //auto is true
                    currentSession.AddValue(Session.SessionKeys.ifElseBranch, true);
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.ifElseBranch, false);
                }


                currentSession.SetInstance(instance);

                if (!SessionManager.GetInstance.AddSession(currentSession))
                {
                    currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


             

                currentSession.WorkflowInstance.Start();
                currentSession.SetHostWaitOne();
             

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }
                string temp = "";
                Delivery thisDelivery = (Delivery)currentSession.GetValue(Session.SessionKeys.Delivery);
                if (thisDelivery != null)
                {
                    temp = thisDelivery.DeliveryNo;
                }
                //Get Pallet
                Pallet pallet = (Pallet)currentSession.GetValue(Session.SessionKeys.Pallet);
                int q1 = palletRep.GetCountOfBoundProduct(pallet.PalletNo);
                int q2 = currentRepository.GetSumDeliveryQtyOfACertainPallet(pallet.PalletNo);
                //Get Pallet

                S_RowData_DN ele = new S_RowData_DN();
                ele.DeliveryNO = thisDelivery.DeliveryNo;
                ele.Model = thisDelivery.ModelName;
                ele.CustomerPN = currentRepository.GetDeliveryInfoValue(thisDelivery.DeliveryNo, "PartNo");
                ele.PoNo = thisDelivery.PoNo;
                ele.Date = thisDelivery.ShipDate.ToString("yyyy/MM/dd ");
                ele.Qty = thisDelivery.Qty.ToString();
                IList<IProduct> productList = new List<IProduct>();
                productList = productRepository.GetProductListByDeliveryNo(thisDelivery.DeliveryNo);
                if (null != productList)
                {
                    ele.PackedQty = productList.Count.ToString();
                }
                arrRet.Add(pallet.PalletNo);
                arrRet.Add(q1.ToString());
                arrRet.Add(q2.ToString());
                arrRet.Add(ele);
                return arrRet;
            }
            catch (FisException e)
            {
                if (!string.IsNullOrEmpty(prodId))
                {
                    IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                    deliveryRep.RollBackAssignBoxId(prodId);
                }

                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                if (!string.IsNullOrEmpty(prodId))
                {
                    IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                    deliveryRep.RollBackAssignBoxId(prodId);
                }

                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                Session sessionDelete = SessionManager.GetInstance.GetSession(keyStr, SessionType); ;
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(_CombineDNPalletforBT)AssignAll end, custSn:" + custSN);
            }
        }
        #endregion
    }
}
