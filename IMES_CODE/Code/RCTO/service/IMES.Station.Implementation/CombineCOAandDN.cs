/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for CombineCOAandDN Page            
 * CI-MES12-SPEC-PAK Combine COA and DN.docx
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
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using IMES.FisObject.PAK.COA;
using log4net;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.Route;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.UnitOfWork;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for CombineCOAandDN.
    /// </summary>
    public class _CombineCOAandDN : MarshalByRefObject, ICombineCOAandDN
    {
        private const Session.SessionType SessionType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeliveryRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        private ICOAStatusRepository coaRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
        private IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        private IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
        private IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
        #region ICombineCOAandDN Members
        /// <summary>
        /// 获取DN表相关信息
        /// </summary>
        public IList<S_RowData_COAandDN> GetDNListQuick(string model, string pono)
        {
            logger.Debug("(_CombineCOAandDN)GetDNListQuick start.");
            try
            {
                IList<S_RowData_COAandDN> ret = new List<S_RowData_COAandDN>();
                if (model == "")
                {
                    return ret;
                }
                if (pono == "")
                {
                    Delivery assignDelivery = null;
                    int assignQty = 0;

                    IList<Delivery> deliveryList = currentRepository.GetDeliveryListByModel(model, "PC", 12, "00");
                   
                    //a)	ShipDate 越早，越优先
                    //b)	散装优先于非散装
                    //c)	剩余未包装Product数量越少的越优先

                    bool assignNA = false;
                    foreach (Delivery dvNode in deliveryList)
                    {
                        int curqty = productRepository.GetCombinedQtyByDN(dvNode.DeliveryNo);
                        int tmpqty = dvNode.Qty - curqty;
                        int curQty = productRepository.GetCombinedQtyByDN(dvNode.DeliveryNo);
                        if (tmpqty > 0)
                        {
                            bool naflag = false;
                            foreach (DeliveryPallet dpNode in dvNode.DnPalletList)
                            {
                                if (dpNode.PalletID.Substring(0, 2) == "NA")
                                {
                                    naflag = true;
                                    break;
                                }
                            }
                            if (assignDelivery == null)
                            {
                                assignDelivery = dvNode;
                                assignQty = tmpqty;
                                assignNA = naflag;
                                continue;
                            }
                            if (DateTime.Compare(assignDelivery.ShipDate, dvNode.ShipDate) < 0)
                            {
                                continue;
                            }
                            if (!assignNA && naflag)
                            {
                                assignDelivery = dvNode;
                                assignQty = tmpqty;
                                assignNA = naflag;
                            }
                            else if (assignNA == naflag)
                            {
                                if (tmpqty < assignQty)
                                {
                                    assignDelivery = dvNode;
                                    assignQty = tmpqty;
                                    assignNA = naflag;
                                }
                            }
                        }
                    }
                    if (assignDelivery == null)
                    {
                        FisException fe = new FisException("PAK103", new string[] { });   //没找到可分配的delivery
                        throw fe;
                    }

                    S_RowData_COAandDN ele = new S_RowData_COAandDN();
                    ele.DeliveryNO = assignDelivery.DeliveryNo;

                    ele.Model = assignDelivery.ModelName;
                    ele.CustomerPN = currentRepository.GetDeliveryInfoValue(assignDelivery.DeliveryNo, "PartNo");
                    ele.PoNo = assignDelivery.PoNo;

                    ele.Date = assignDelivery.ShipDate.ToString("yyyy/MM/dd");
                    ele.Qty = assignDelivery.Qty.ToString();
                    int qty = 0;
                    int packedQty = 0;
                    qty = assignDelivery.Qty;
                    IList<IProduct> productList = new List<IProduct>();
                    productList = productRepository.GetProductListByDeliveryNo(assignDelivery.DeliveryNo);
                    if (null != productList)
                    {
                        ele.PackedQty = productList.Count.ToString();
                        packedQty = productList.Count;
                    }
                    ret.Add(ele);
                }
                else
                {
                    DNQueryCondition condition = new DNQueryCondition();
                    DateTime temp = DateTime.Now;
                    temp = temp.AddDays(-3);
                    condition.ShipDateFrom = new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0, 0);
                    condition.Model = model;
                    IList<DNForUI> dnList = currentRepository.GetDNListByConditionWithSorting(condition);
                    foreach (DNForUI tmp in dnList)
                    {
                        S_RowData_COAandDN ele = new S_RowData_COAandDN();
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
                        if (pono != "")
                        {
                            if (pono != tmp.PoNo)
                            {
                                continue;
                            }
                        }
                        ele.Date = tmp.ShipDate.ToString("yyyy/MM/dd");
                        ele.Qty = tmp.Qty.ToString();
                        int qty = 0;
                        int packedQty = 0;
                        qty = tmp.Qty;
                        IList<IProduct> productList = new List<IProduct>();
                        productList = productRepository.GetProductListByDeliveryNo(tmp.DeliveryNo);
                        if (null != productList)
                        {
                            ele.PackedQty = productList.Count.ToString();
                            packedQty = productList.Count;
                        }
                        if (packedQty > qty)
                        {
                            continue;
                        }
                        ret.Add(ele);
                        break;
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
                logger.Debug("(_CombineCOAandDN)GetDNListQuick end.");
            }
        }
       /*
        /// <summary>
        /// 获取DN表相关信息
        /// </summary>
        public IList<S_RowData_COAandDN> GetDNList()
        {
            logger.Debug("(_CombineCOAandDN)GetDNList start.");
            try
            {
                IList<S_RowData_COAandDN> ret = new List<S_RowData_COAandDN>();
                DNQueryCondition condition = new DNQueryCondition();
                DateTime temp = DateTime.Now;
                temp = temp.AddDays(-3);
                condition.ShipDateFrom = new DateTime(temp.Year, temp.Month, temp.Day, 0,0,0,0);
                IList<Srd4CoaAndDn> dnList = currentRepository.GetDNListByConditionForPerformance(condition);
                foreach (Srd4CoaAndDn tmp in dnList)
                {
                    S_RowData_COAandDN ele = new S_RowData_COAandDN();
                    ele.DeliveryNO = tmp.DeliveryNO;
                    ele.Model = tmp.Model;
                    ele.CustomerPN = tmp.CustomerPN;
                    ele.PoNo = tmp.PoNo;
                    ele.Date = tmp.ShipDate.ToString("yyyy/MM/dd");
                    ele.Qty = tmp.Qty.ToString();
                    ele.PackedQty = tmp.PackedQty.ToString();
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
                logger.Debug("(_CombineCOAandDN)GetDNList end.");
            }
        }*/
        /// <summary>
        /// 获取DN表相关信息
        /// </summary>
        public IList<S_RowData_COAandDN> GetDNList()
        {
            logger.Debug("(_CombineCOAandDN)GetDNList start.");
            try
            {
                IList<S_RowData_COAandDN> ret = new List<S_RowData_COAandDN>();
                DNQueryCondition condition = new DNQueryCondition();
                DateTime temp = DateTime.Now;
                temp = temp.AddDays(-3);
                condition.ShipDateFrom = new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0, 0);
                IList<DNForUI> dnList = currentRepository.GetDNListByCondition(condition);
                foreach (DNForUI tmp in dnList)
                {
                    S_RowData_COAandDN ele = new S_RowData_COAandDN();
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
                    ele.Date = tmp.ShipDate.ToString("yyyy/MM/dd");
                    ele.Qty = tmp.Qty.ToString();
                    int qty = 0;
                    int packedQty = 0;
                    qty = tmp.Qty;
                    IList<IProduct> productList = new List<IProduct>();
                    productList = productRepository.GetProductListByDeliveryNo(tmp.DeliveryNo);
                    if (null != productList)
                    {
                        ele.PackedQty = productList.Count.ToString();
                        packedQty = productList.Count;
                    }
                    if (packedQty > qty)
                    {
                        continue;
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
                logger.Debug("(_CombineCOAandDN)GetDNList end.");
            }
        }
        /// <summary>
        /// 获取DN表相关信息
        /// </summary>
        /// <param name="aDN">aDN</param>
        public S_RowData_COAandDN GetADN(string aDN)
        {
            logger.Debug("(_CombineCOAandDN)GetADN start.");
            try
            {
                S_RowData_COAandDN ele = new S_RowData_COAandDN();
                Delivery reDelivery = currentRepository.Find(aDN);
                if (null != reDelivery)
                {
                    ele.DeliveryNO = reDelivery.DeliveryNo;
                    ele.Model = reDelivery.ModelName;
                    ele.CustomerPN = currentRepository.GetDeliveryInfoValue(reDelivery.DeliveryNo, "PartNo");
                    ele.PoNo = reDelivery.PoNo;
                    ele.Date = reDelivery.ShipDate.ToString("yyyy/MM/dd");
                    ele.Qty = reDelivery.Qty.ToString();
                    int qty = 0;
                    int packedQty = 0;
                    qty = reDelivery.Qty;
                    IList<IProduct> productList = new List<IProduct>();
                    productList = productRepository.GetProductListByDeliveryNo(reDelivery.DeliveryNo);
                    if (null != productList)
                    {
                        ele.PackedQty = productList.Count.ToString();
                        packedQty = productList.Count;
                    }
                }
                return ele;
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
                logger.Debug("(_CombineCOAandDN)GetADN end.");
            }
        }
        /// <summary>
        /// 获取Product表相关信息
        /// </summary>
        /// <param name="customerSN">customer SN</param>
        public S_RowData_Product GetProductOnly(string customerSN)
        {
            logger.Debug("(_CombineCOAandDN)GetProductOnly start.customerSN:" + customerSN);
            try
            {
                S_RowData_Product ret = new S_RowData_Product();
                ret.DN = "";
                ret.Model = "";
                ret.ProductID = "";
                ret.isBT = "false";
                ret.isCDSI = "";
                ret.isFactoryPo = "";
                IProduct temp = productRepository.GetProductByCustomSn(customerSN);
                if (null != temp)
                {
                    ret.ProductID = temp.ProId;
                    ret.isBT = temp.IsBT.ToString();
                    ret.Model = temp.Model;
                    IList<IMES.FisObject.Common.Model.ModelInfo> isPO = modelRep.GetModelInfoByModelAndName(temp.Model, "PO");

                    foreach (IMES.FisObject.Common.Model.ModelInfo tmpPO in isPO)
                    {
                        if (tmpPO.Value == "Y")
                        {
                            ret.isCDSI = "true";
                        }
                        else
                        {
                            IList<IMES.FisObject.Common.Model.ModelInfo> isATSNAV = modelRep.GetModelInfoByModelAndName(temp.Model, "ATSNAV");
                            foreach (IMES.FisObject.Common.Model.ModelInfo tmpATSNAV in isATSNAV)
                            {
                                if (tmpATSNAV.Value == null)
                                { 
                                }
                                else if (tmpATSNAV.Value != "")
                                {
                                    ret.isCDSI = "true";
                                }
                                break;
                            }
                        }
                        break;
                    }
                    if (ret.isCDSI == "true")
                    {

                        CdsiastInfo condition = new CdsiastInfo();
                        condition.snoId = temp.ProId;
                        condition.tp = "FactoryPO";
                        IList<CdsiastInfo> isCdsiastInfo = productRepository.GetCdsiastInfoList(condition);
                        ret.isFactoryPo = "";
                        foreach (CdsiastInfo tmpCdsiastInfo in isCdsiastInfo)
                        {
                            ret.isFactoryPo = tmpCdsiastInfo.sno;
                            break;
                        }
                        if (ret.isBT == "true" || ret.isBT == "True")
                        {
                        }
                        else
                        {
                            if (ret.isFactoryPo == "" || ret.isFactoryPo == null)
                            {
                                throw new FisException("CHK882", new string[] { });
                            }
                        }
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
                logger.Debug("(_CombineCOAandDN)GetProductOnly end. customerSN:" + customerSN);
            }
        }
        /// <summary>
        /// 获取Product表相关信息
        /// </summary>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="customerSN">customer SN</param>
        public S_RowData_Product GetProduct(string line, string editor, string station, string customer,string customerSN)
        {
            logger.Debug("(_CombineCOAandDN)GetProduct start.customerSN:" + customerSN);
           
            string keyStr = "";
            try
            {
                string sessionKey = customerSN;
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
                RouteManagementUtils.GetWorkflow(station, "CombineCOAandDNBlock.xoml", "", out wfName, out rlName);
                WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                currentSession.AddValue(Session.SessionKeys.CustSN, customerSN);
                currentSession.SetInstance(instance);
                if (!SessionManager.GetInstance.AddSession(currentSession))
                {
                    currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");

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
                S_RowData_Product ret = new S_RowData_Product();
                ret.DN = "";
                ret.Model = "";
                ret.ProductID = "";
                ret.isBT = "false";
                ret.isCDSI = "";
                ret.isFactoryPo = "";
                ret.isWin8 = "";
                IProduct temp = productRepository.GetProductByCustomSn(customerSN);
                if (null != temp)
                {
                    ret.ProductID = temp.ProId;
                    ret.isBT = temp.IsBT.ToString();
                    ret.Model = temp.Model;
                    
                    IList<IMES.FisObject.Common.Model.ModelInfo> isPO = modelRep.GetModelInfoByModelAndName(temp.Model, "PO");
                    foreach (IMES.FisObject.Common.Model.ModelInfo tmpPO in isPO)
                    {
                        if (tmpPO.Value == "Y")
                        {
                            ret.isCDSI = "true";
                        }
                        else
                        {
                            IList<IMES.FisObject.Common.Model.ModelInfo> isATSNAV = modelRep.GetModelInfoByModelAndName(temp.Model, "ATSNAV");
                            foreach (IMES.FisObject.Common.Model.ModelInfo tmpATSNAV in isATSNAV)
                            {
                                if (tmpATSNAV.Value == null)
                                { 
                                }
                                else if (tmpATSNAV.Value != "")
                                {
                                    ret.isCDSI = "true";
                                }
                                break;
                            }
                        }
                        break;
                    }
                    if (ret.isCDSI == "true")
                    {
                        CdsiastInfo condition = new CdsiastInfo();
                        condition.snoId = temp.ProId;
                        condition.tp = "FactoryPO";
                        IList<CdsiastInfo> isCdsiastInfo = productRepository.GetCdsiastInfoList(condition);
                        ret.isFactoryPo = "";
                        foreach (CdsiastInfo tmpCdsiastInfo in isCdsiastInfo)
                        {
                            ret.isFactoryPo = tmpCdsiastInfo.sno;
                            break;
                        }
                        if (ret.isBT == "true" || ret.isBT == "True")
                        {
                        }
                        else
                        {
                            if (ret.isFactoryPo == "" || ret.isFactoryPo == null)
                            {
                                throw new FisException("CHK882", new string[] { });
                            }
                        }
                    } 
                    IList<MoBOMInfo>  win8list = bomRepository.GetPnListByModelAndBomNodeTypeAndDescr(temp.Model, "P1", "ECOA");
                    if (win8list != null && win8list.Count > 0)
                    {
                        IList<string> typeList = new List<string>();
                        typeList.Add("P/N");
                        typeList.Add("Key");
                        typeList.Add("Hash");
                        IList<IMES.FisObject.FA.Product.ProductInfo> infoList = productRepository.GetProductInfoList(temp.ProId, typeList);
                        if (infoList == null || infoList.Count != 3)
                        {
                            throw new FisException("CHK885", new string[] { });
                            //ret.isWin8 = "true";
                        }
                        else
                        {
                            ret.isWin8 = "true";
                        }
                    }
                    if (ret.isCDSI == "true")
                    {
                        DNQueryCondition conditionDN = new DNQueryCondition();
                        DateTime aTime = DateTime.Now;
                        aTime = aTime.AddDays(-3);
                        conditionDN.ShipDateFrom = new DateTime(aTime.Year, aTime.Month, aTime.Day, 0, 0, 0, 0);
                        conditionDN.Model = ret.Model;
                        if (conditionDN.Model != "")
                        {
                            IList<Srd4CoaAndDn> dnList = currentRepository.GetDNListByConditionForPerformanceWithSorting(conditionDN);
                            foreach (Srd4CoaAndDn tmp in dnList)
                            {
                                if (ret.isFactoryPo != "")
                                {
                                    if (ret.isFactoryPo != tmp.PoNo)
                                    {
                                        continue;
                                    }
                                }
                                ret.DN = tmp.DeliveryNO;
                                break;
                            }
                        }
                    }
                    else
                    { 
                        Delivery assignDelivery = null;
                        int assignQty = 0;

                        IList<Delivery> deliveryList = currentRepository.GetDeliveryListByModel(ret.Model,"PC",12,"00");
                        if (deliveryList.Count == 0)
                        {

                            if (temp.IsBT)
                            {
                            }
                            else
                            {
                                List<string> errpara = new List<string>();
                                errpara.Add(customerSN);
                                throw new FisException("PAK101", errpara);//无此机型Delivery!
                            }
                        }

                        //a)	ShipDate 越早，越优先
                        //b)	散装优先于非散装
                        //c)	剩余未包装Product数量越少的越优先

                        bool assignNA = false;
                        foreach (Delivery dvNode in deliveryList)
                        {
                            int curqty = productRepository.GetCombinedQtyByDN(dvNode.DeliveryNo);
                            int tmpqty = dvNode.Qty - curqty;
                            int curQty = productRepository.GetCombinedQtyByDN(dvNode.DeliveryNo);
                            if (tmpqty > 0 )
                            {
                                bool naflag = false;
                                foreach (DeliveryPallet dpNode in dvNode.DnPalletList)
                                {
                                    if (dpNode.PalletID.Substring(0, 2) == "NA")
                                    {
                                        naflag = true;
                                        break;
                                    }
                                }
                                if (assignDelivery == null)
                                {
                                    assignDelivery = dvNode;
                                    assignQty = tmpqty;
                                    assignNA = naflag;
                                    continue;
                                }
                                if (DateTime.Compare(assignDelivery.ShipDate, dvNode.ShipDate) < 0)
                                {
                                    continue;
                                }
                                if (!assignNA && naflag)
                                {
                                    assignDelivery = dvNode;
                                    assignQty = tmpqty;
                                    assignNA = naflag;
                                }
                                else if (assignNA == naflag)
                                {
                                    if (tmpqty < assignQty)
                                    {
                                        assignDelivery = dvNode;
                                        assignQty = tmpqty;
                                        assignNA = naflag;
                                    }
                                }
                            }
                        }
                        if (assignDelivery == null)
                        {
                            if (temp.IsBT)
                            {
                            }
                            else
                            {
                                FisException fe = new FisException("PAK103", new string[] { });   //没找到可分配的delivery
                                throw fe;
                            }
                        }
                        else
                        {
                            ret.DN = assignDelivery.DeliveryNo;
                        }
                    }
                }

                return ret;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                Session sessionDelete = SessionManager.GetInstance.GetSession(keyStr, SessionType); ;
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(_CombineCOAandDN)GetProduct end, customerSN:" + customerSN);
            }          
        }
        /// <summary>
        /// 取ModelBOM 中Model 直接下阶中有BomNodeType 为'P1' 的Part
        /// </summary>
        /// <param name="custSN">custSN</param>
        public string CheckBOM(string custSN)
        {
            string ret = "NotFind";
            try 
            {
                IProduct product = productRepository.GetProductByCustomSn(custSN);
                if (product == null)
                {
                    return ret;
                }
                string model = product.Model;
                // 取ModelBOM 中Model 直接下阶中有BomNodeType 为'P1' 的Part
                IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
                IHierarchicalBOM sessionBOM = null;
                sessionBOM = ibomRepository.GetHierarchicalBOMByModel(product.Model);
                IList<IBOMNode> bomNodeLst = new List<IBOMNode>();
                bomNodeLst = sessionBOM.FirstLevelNodes;
                if (bomNodeLst != null && bomNodeLst.Count > 0)
                {
                    foreach (IBOMNode ibomnode in bomNodeLst)
                    {
                        IPart currentPart = ibomnode.Part;
                        if (currentPart.BOMNodeType == "P1" && currentPart.Descr.IndexOf("COA") == 0)
                        {
                            return currentPart.PN;
                        }
                    }
                }
                return ret;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// Product_Part 中看是否有绑定COA – 存在BomNodeType = 'P1' ，IMES_GetData..Part.Descr LIKE 'COA%' 的Part
        /// </summary>
        /// <param name="custSN">custSN</param>
        public string CheckPart(string custSN)
        {
            try
            {
                IProduct currentProduct = productRepository.GetProductByCustomSn(custSN);
                if (null == currentProduct)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(custSN);
                    throw new FisException("SFC002", errpara);
                }

                // IMES_FA..Product_Part 表中与当前Product 绑定的Parts 
                IList<IProductPart> productParts = new List<IProductPart>();
                productParts = currentProduct.ProductParts;
                if (productParts == null || productParts.Count <= 0)
                {
                    /*List<string> errpara = new List<string>();
                    errpara.Add(currentProduct.ProId);
                    throw new FisException("PAK038", errpara);  *///该Product尚未绑定Part！
                    return "false";
                }

                foreach (ProductPart iprodpart in productParts)
                {
                    IPart curPart = ipartRepository.GetPartByPartNo(iprodpart.PartID);
                    //if (iprodpart.PartType == "P1")
                    if (curPart.BOMNodeType == "P1" && curPart.Descr.IndexOf("COA") == 0)
                    {
                        return curPart.PN;
                    }
                }
                return "false";
                /*IProduct product = productRepository.GetProductByCustomSn(custSN);
                if (product == null)
                {
                    return ret;
                }
                string productID = product.ProId;
                string[] descr = new string[1];
                descr[0] = "COA%";
                IList<PartDef> partLst = ipartRepository.GetPartByBomNodeTypeAndDescr(productID, "P1", descr);
                
                if (null == partLst || partLst.Count == 0)
                {
                    return ret;
                }
                else 
                {
                    return true;
                }*/
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// Product_Part 中看是否有绑定COA相等
        /// </summary>
        /// <param name="custSN">custSN</param>
        /// <param name="coaSN">coaSN</param> 
        public int CheckPartCoa(string custSN, string coaSN)
        {
            try
            {
 

                IProduct currentProduct = productRepository.GetProductByCustomSn(custSN);
                if (null == currentProduct)
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(custSN);
                    throw new FisException("SFC002", errpara);
                }


                IList<ProductPart> parts = new List<ProductPart>();
                parts = productRepository.GetProductPartsByPartSn(coaSN);
                foreach (ProductPart iparts in parts)
                {
                    if (iparts.PartSn == coaSN && iparts.ProductID != currentProduct.ProId)
                    {
                        return -2;
                    }
                }

                // IMES_FA..Product_Part 表中与当前Product 绑定的Parts 
                IList<IProductPart> productParts = new List<IProductPart>();
                productParts = currentProduct.ProductParts;
                if (productParts == null || productParts.Count <= 0)
                {
                    /*List<string> errpara = new List<string>();
                    errpara.Add(currentProduct.ProId);
                    throw new FisException("PAK038", errpara);  */
                    //该Product尚未绑定Part！
                    return -1;
                }

                foreach (ProductPart iprodpart in productParts)
                {
                    if (iprodpart.PartSn == coaSN)
                    {
                        return 1;
                    }
                }
                
                return -1;
                /*IProduct product = productRepository.GetProductByCustomSn(custSN);
                if (product == null)
                {
                    return ret;
                }
                string productID = product.ProId;
                string[] descr = new string[1];
                descr[0] = "COA%";
                IList<PartDef> partLst = ipartRepository.GetPartByBomNodeTypeAndDescr(productID, "P1", descr);
                
                if (null == partLst || partLst.Count == 0)
                {
                    return ret;
                }
                else 
                {
                    return true;
                }*/
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// 使用Code = @CustomerSN and Type =  'SN' 或者Code = @DeliveryNo and Type =  'DN'查询InternalCOA 表
        /// </summary>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="deliveryNo">DeliveryNo</param>
        public bool CheckInternalCOA(string custSN, string deliveryNo)
        {
            bool ret = false;
            try
            {
                List<InternalCOADef> retLst = new List<InternalCOADef>();
                IList<InternalCOAInfo> getData = ipartRepository.FindAllInternalCOA();
                if (getData != null)
                {
                    for (int i = 0; i < getData.Count; i++)
                    {
                        InternalCOAInfo item = getData[i];
                        if (item.code == custSN && item.type == "SN")
                        {
                            return true;
                        }
                        if (item.code == deliveryNo && item.type == "DN")
                        {
                            return true;
                        }
                    }
                }
                return ret;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// check coa
        /// </summary>
        /// <param name="coaSN">coaSN</param> 
        /// <param name="partNO">partNO</param>
        public int CheckCOA(string coaSN, string partNO)
        {
            COAStatus reCOA = coaRepository.Find(coaSN);
            if (null == reCOA)
            {
                return 1;
            }
            if (partNO != reCOA.IECPN)
            {
                return 2;
            }
            if ("P1" != reCOA.IECPN)
            {
                return 3;
            }
            return 4;
        }
        /// <summary>
        /// GetModel
        /// </summary>
        /// <param name="DN">DN</param>
        public string GetModel(string DN)
        {
            try
            {
                Delivery CurrentDelivery = currentRepository.Find(DN);
                if (CurrentDelivery == null)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(DN);
                    throw new FisException("CHK107", errpara);
                }
                return CurrentDelivery.ModelName;
                
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
        /// Product.DeliveryNo – Delivery No (from UI)
        /// </summary>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="deliveryNo">DeliveryNo</param> 
        public void UpdateProduct(string custSN, string deliveryNo)
        {
            try
            {
                IProduct product = productRepository.GetProductByCustomSn(custSN);
                if (product == null)
                {
                    return;
                }
                string prod = product.ProId;
                IList<string> ProductIDList = new List<string>();
                ProductIDList.Add(prod);
                productRepository.BindDN(deliveryNo, ProductIDList);
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        private static object _syncObj4UpdateDeliveryStatus = new object();
        /// <summary>
        /// IMES_PAK..Delivery.Status = '87'
        /// </summary>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="DN">DN</param>
        /// <param name="custSN">custSN</param>
        /// <param name="coaSN">coaSN</param>
        /// <param name="printItems"></param> 
        public ArrayList UpdateDeliveryStatusAndPrint(string line, string editor, string station, string customer, string DN, string custSN, string coaSN, IList<PrintItem> printItems)
        {
            ArrayList retList = new ArrayList();
            string keyStr = "";
            try 
            {
                if (null == line)
                {
                    line = "";
                }
                if (null == station)
                {
                    station = "";
                }
                if (null == editor)
                {
                    editor = "";
                }
                if (null == customer)
                {
                    customer = "";
                }
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                if (null == currentProduct)
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(custSN);
                    throw new FisException("SFC002", errpara);
                }
                Delivery reDelivery = currentRepository.Find(DN);
                
                S_RowData_Product productInfo = GetProductOnly(custSN);
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
                RouteManagementUtils.GetWorkflow(station, "CombineCOAandDNAndPrint.xoml", "combinecoaanddnandprint.rules", out wfName, out rlName);
                WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                currentSession.AddValue(Session.SessionKeys.COASN, coaSN);
                if (currentProduct.IsBT)
                {
                    currentSession.AddValue(Session.SessionKeys.IsBT, true);
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.IsBT, false);
                }
                if (productInfo.isCDSI == "true")
                {
                    currentSession.AddValue(Session.SessionKeys.Pno, productInfo.isFactoryPo);
                }
                currentSession.AddValue(Session.SessionKeys.CustSN, custSN);
                currentSession.AddValue(Session.SessionKeys.DeliveryNo, DN);
                currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                currentSession.AddValue(Session.SessionKeys.Delivery, reDelivery);
                currentSession.AddValue(Session.SessionKeys.ReturnStation, "1");
                currentSession.AddValue(Session.SessionKeys.IsComplete, true);
                currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                
                currentSession.SetInstance(instance);
                if (!SessionManager.GetInstance.AddSession(currentSession))
                {
                    currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                    
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
                IUnitOfWork uow = new UnitOfWork();
                var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
                if (currentProduct != null)
                {
                    var item = new PrintLog
                    {
                        Name = "PIZZA Label-1",
                        BeginNo = custSN,
                        EndNo = custSN,
                        Descr = "",
                        Editor = editor
                    };
                    repository.Add(item, uow);
                }
                if (currentProduct.IsBT)
                {
                    var item = new PrintLog
                    {
                        Name = "BT COO Label",
                        BeginNo = custSN,
                        EndNo = custSN,
                        Descr = "",
                        Editor = editor
                    };
                    repository.Add(item, uow);
                }
               
                string DNTemp = (string)currentSession.GetValue(Session.SessionKeys.DeliveryNo);
                logger.Debug("(_CombineCOAandDN)UpdateDeliveryStatus DNTemp:" + DNTemp);
                logger.Debug("(_CombineCOAandDN)UpdateDeliveryStatus DN:" + DN);
                string QCIs = "noqcis";
                if (DN != DNTemp)
                {
                    QCIs = QCIs + "#@$#" + DNTemp;
                }
                IList<PrintItem> printList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

                //Model 的第10，11码是”29” 或者”39” 的产品是出货日本的产品；否则，是非出货日本的产品
                string jpmodel = currentProduct.Model.Substring(9, 2);
                bool jpflag = false;
                if (jpmodel == "29" || jpmodel == "39")
                {
                    jpflag = true;
                    var item = new PrintLog
                    {
                        Name = "PIZZA Label-2",
                        BeginNo = custSN,
                        EndNo = custSN,
                        Descr = "",
                        Editor = editor
                    };
                    repository.Add(item, uow);
                }
                uow.Commit();
                retList.Add(printList);
                retList.Add(jpflag);
                retList.Add(QCIs);
                return retList;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                Session sessionDelete = SessionManager.GetInstance.GetSession(keyStr, SessionType);
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(_CombineCOAandDN)UpdateDeliveryStatusAndPrint end, DN:" + DN);
            }
        }
        /// <summary>
        /// IMES_PAK..Delivery.Status = '87'
        /// </summary>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="DN">DN</param>
        /// <param name="custSN">custSN</param>
        /// <param name="coaSN">coaSN</param>
        public string UpdateDeliveryStatus(string line, string editor, string station, string customer, string DN, string custSN, string coaSN)
        {
            string keyStr = "";
            try
            {
                if (null == line)
                {
                    line = "";
                }
                if (null == station)
                {
                    station = "";
                }
                if (null == editor)
                {
                    editor = "";
                }
                if (null == customer)
                {
                    customer = "";
                }
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                if (null == currentProduct)
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(custSN);
                    throw new FisException("SFC002", errpara);
                }
                Delivery reDelivery = currentRepository.Find(DN);

                S_RowData_Product productInfo = GetProductOnly(custSN);
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
                RouteManagementUtils.GetWorkflow(station, "CombineCOAandDN.xoml", "combinecoaanddn.rules", out wfName, out rlName);
                WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                currentSession.AddValue(Session.SessionKeys.COASN, coaSN);
                if (currentProduct.IsBT)
                {
                    currentSession.AddValue(Session.SessionKeys.IsBT, true);
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.IsBT, false);
                }
                if (productInfo.isCDSI == "true")
                {
                    currentSession.AddValue(Session.SessionKeys.Pno, productInfo.isFactoryPo);
                }
                currentSession.AddValue(Session.SessionKeys.CustSN, custSN);
                currentSession.AddValue(Session.SessionKeys.DeliveryNo, DN);
                currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                currentSession.AddValue(Session.SessionKeys.Delivery, reDelivery);
                currentSession.AddValue(Session.SessionKeys.ReturnStation, "1");
                currentSession.AddValue(Session.SessionKeys.IsComplete, true);
                string QCIs = "false";
                currentSession.SetInstance(instance);
                if (!SessionManager.GetInstance.AddSession(currentSession))
                {
                    currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");

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
                string DNTemp = (string)currentSession.GetValue(Session.SessionKeys.DeliveryNo);
                logger.Debug("(_CombineCOAandDN)UpdateDeliveryStatus DNTemp:" + DNTemp);
                logger.Debug("(_CombineCOAandDN)UpdateDeliveryStatus DN:" + DN);
                if (DN != DNTemp)
                {
                    QCIs = QCIs + "#@$#" + DNTemp;
                }
                return QCIs;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                Session sessionDelete = SessionManager.GetInstance.GetSession(keyStr, SessionType);
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(_CombineCOAandDN)UpdateDeliveryStatus end, DN:" + DN);
            }
        }
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="custSN"></param>
        public void Cancel(string custSN)
        {
            logger.Debug("(_CombineCOAandDN)Cancel start, [custSN]:" + custSN);
            List<string> erpara = new List<string>();
            string sessionKey = custSN;
            
            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
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
                throw e;
            }
            finally
            {
                logger.Debug("(_CombineCOAandDN)Cancel end, [custSN]:" + custSN);
            }
        }
        /// <summary>
        /// 印第一个pizza标签
        /// </summary>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>  
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        public IList<PrintItem> PrintPizzaLabelFinal(string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(_CombineCOAandDN)PrintPizzaLabelFinal start, [CustomerSN]:" + custSN
                + " [line]: " + line
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custSN;

            try
            {
                string DNTemp = "";
                if (custSN.IndexOf("#@#") != -1)
                {
                    string temp = custSN;
                    custSN = temp.Substring(0, temp.IndexOf("#@#"));
                    DNTemp = temp.Substring(temp.IndexOf("#@#") + "#@#".Length);
                    DNTemp = DNTemp.Trim();
                }
                sessionKey = custSN;
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                
                if (null == currentProduct)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(custSN);
                    throw new FisException("SFC002", errpara);
                }
                if (currentProduct.IsBT != true)
                {
                    if (currentProduct.DeliveryNo.Trim() != DNTemp)
                    {
                        logger.Debug("(_CombineCOAandDN)PrintPizzaLabelFinalnull:" + currentProduct.DeliveryNo + "^" + DNTemp + "^" + currentProduct.DeliveryNo.Trim() + "##" + DNTemp.Trim() + "##");
                        //return null;
                    }
                    if (null == currentProduct.DeliveryNo || currentProduct.DeliveryNo == "")
                    {
                        return null;
                    }
                }
                
                
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB);

                if (session == null)
                {
                    session = new Session(sessionKey, Session.SessionType.MB, editor, station, string.Empty, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", string.Empty);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.MB);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("CombineCOAandDNPrint1.xoml", string.Empty, wfArguments);

                    session.AddValue(Session.SessionKeys.CustSN, custSN);

                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.PrintLogName, "PIZZA Label");
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, custSN);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, custSN);
                    session.AddValue(Session.SessionKeys.PrintLogDescr, "");
                    session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(session))
                    {
                        session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    session.WorkflowInstance.Start();
                    session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }

                return (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                Session sessionDelete = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB); ;
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(_CombineCOAandDN)PrintPizzaLabelFinal end, [CustomerSN]:" + custSN
                    + " [line]: " + line
                    + " [editor]:" + editor
                    + " [station]:" + station
                    + " [customer]:" + customer);
            }
        }
        /*
        /// <summary>
        /// 印第一个pizza标签
        /// </summary>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>  
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        public IList<PrintItem> PrintPizzaLabelFinal(string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(_CombineCOAandDN)PrintPizzaLabelFinal start, [CustomerSN]:" + custSN
                + " [line]: " + line
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);

            string sessionKey = custSN;
            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                if (session != null)
                {
                    string doDelivery = (string)session.GetValue("doDelivery");
                    if (doDelivery != null && doDelivery == "true")
                    {
                        var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                        if (null == currentProduct)
                        {
                            List<string> errpara = new List<string>();
                            errpara.Add(custSN);
                            throw new FisException("SFC002", errpara);
                        }
                        string DNTemp = (string)session.GetValue(Session.SessionKeys.DeliveryNo);
                        if (currentProduct.DeliveryNo != DNTemp)
                        {
                            return null;
                        }
                    }
                    session.AddValue(Session.SessionKeys.IsComplete, true);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.PrintLogName, "PIZZA Label");
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, custSN);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, custSN);
                    session.AddValue(Session.SessionKeys.PrintLogDescr, "");
                    session.Exception = null;
                    session.SwitchToWorkFlow();

                    //check workflow exception
                    if (session.Exception != null)
                    {
                        if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            session.ResumeWorkFlow();
                        }

                        throw session.Exception;
                    }
                    return (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
                    //return null;
                }
                else
                {
                    return null;
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
                throw e;
            }
            finally
            {
                Session sessionDelete = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(_CombineCOAandDN)PrintPizzaLabelFinal end, [CustomerSN]:" + custSN
                    + " [line]: " + line
                    + " [editor]:" + editor
                    + " [station]:" + station
                    + " [customer]:" + customer);
            }
        }*/

        /// <summary>
        /// Insert Product_Part - Combine COA 
        /// </summary>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="coaSN">coaSN</param>
        /// <param name="editor">editor</param>
        public void BindPart(string custSN, string coaSN,string editor)
        { 
            try 
            {
                COAStatus reCOA = coaRepository.Find(coaSN);
                IProduct product = productRepository.GetProductByCustomSn(custSN);
                if (product == null)
                {
                    return ;
                }
                IProductPart bindPart = new ProductPart();
                bindPart.ProductID = product.ProId;
                bindPart.PartID = reCOA.IECPN;
                bindPart.PartSn = coaSN;
                bindPart.Cdt = DateTime.Now;
                if (editor == null)
                {
                    editor = "";
                }
                bindPart.Editor = editor;
                product.AddPart(bindPart);
                
                reCOA.Status = "A1";
                reCOA.Editor = editor;
                coaRepository.UpdateCOAStatus(reCOA);
                COALog newItem = new COALog();
                newItem.COASN = coaSN;
                newItem.LineID = custSN;
                newItem.Editor = editor;
                newItem.StationID = "A1";
                coaRepository.InsertCOALog(newItem);
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// Generate Pizza
        /// </summary>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        public string  GeneratePizza(string custSN, string line, string editor, string station, string customer)
        {
          
            string keyStr = "";
            try
            {
                if (null == line)
                {
                    line = "";
                }
                if (null == station)
                {
                    station = "";
                }
                if (null == editor)
                {
                    editor = "";
                }
                if (null == customer)
                {
                    customer = "";
                }
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                if (null == currentProduct)
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(custSN);
                    throw new FisException("SFC002", errpara);
                }
                string sessionKey = currentProduct.ProId;
                keyStr = sessionKey;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);
                if (null == currentSession)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.Common, editor, station, line, customer);
                }
                Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                wfArguments.Add("Key", sessionKey);
                wfArguments.Add("Station", station);
                wfArguments.Add("CurrentFlowSession", currentSession);
                wfArguments.Add("Editor", editor);
                wfArguments.Add("PdLine", line);
                wfArguments.Add("Customer", customer);
                wfArguments.Add("SessionType", Session.SessionType.Common);

                string wfName, rlName;
                RouteManagementUtils.GetWorkflow(station, "CombineCOAandDNPizza.xoml", "CombineCOAandDNPizza.rules", out wfName, out rlName);
                WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                currentSession.AddValue(Session.SessionKeys.ReturnStation, "1");

                int patern = 1;// 2,CTO, 1,BTO
                if (currentProduct.Model.Length == 12)
                {
                    string seven = currentProduct.Model.Substring(6, 1);
                    if (seven == "0"
                        || seven == "1"
                        || seven == "2"
                        || seven == "3"
                        || seven == "4"
                        || seven == "5"
                        || seven == "6"
                        || seven == "7"
                        || seven == "8"
                        || seven == "9")
                    {
                        string PC = currentProduct.Model.Substring(0, 2);
                        if (PC == "PC")
                        {
                            patern = 2;
                        }
                    }
                }
                int doAQC = 0;
                if (patern == 2)
                {
                    doAQC = productRepository.GetCountOfQCStatusByTpAndPdLineAndModelToday("PAQC", line, currentProduct.Model);
                }
                else
                {
                    doAQC = productRepository.GetCountOfQCStatusByTpAndPdLineAndPnoToday("PAQC", line, currentProduct.Model);
                }
                patern = 1;//3,japan 2,CTO, 1,common

                if (currentProduct.Model.Length > 6)
                {
                    string seven = currentProduct.Model.Substring(6, 1);
                    if (seven == "0"
                        || seven == "1"
                        || seven == "2"
                        || seven == "3"
                        || seven == "4"
                        || seven == "5"
                        || seven == "6"
                        || seven == "7"
                        || seven == "8"
                        || seven == "9")
                    {
                        patern = 2;
                    }
                }
                if (currentProduct.Model.Length > 11)
                {
                    string japan = currentProduct.Model.Substring(9, 3);
                    if (japan == "29Y" || japan == "39Y")
                    {
                        patern = 3;
                    }
                }
                int QCRatio = 1;
                if (patern == 1)
                {
                    IFamilyRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
                    QCRatio currentQCRatio = CurrentRepository.GetQCRatio(currentProduct.Model);
                    if (currentQCRatio == null)
                    {
                        currentQCRatio = CurrentRepository.GetQCRatio(currentProduct.Family);
                    }
                    if (currentQCRatio == null)
                    {
                        currentQCRatio = CurrentRepository.GetQCRatio(currentProduct.Customer);
                    }
                    if (currentQCRatio == null)
                    {
                        List<string> errpara = new List<string>();
                        throw new FisException("CHK040", errpara);
                    }
                    QCRatio = currentQCRatio.PAQCRatio;
                }

                if (patern == 2)
                {
                    IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    IList<string> valueList = new List<string>();
                    valueList = partRepository.GetValueFromSysSettingByName("CTONonJapanPAQCRatio");
                    if (valueList.Count > 0)
                    {
                        QCRatio = int.Parse(valueList[0]);
                    }
                }
                if (patern == 3)
                {
                    IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    IList<string> valueList = new List<string>();
                    valueList = partRepository.GetValueFromSysSettingByName("JapanPAQCRatio");
                    if (valueList.Count > 0)
                    {
                        QCRatio = int.Parse(valueList[0]);
                    }
                }
                string QCIs = "false";
                if (doAQC % QCRatio == 0)
                {
                    currentSession.AddValue(Session.SessionKeys.QCStatus, true);
                    //QCIs = "true";
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.QCStatus, false);
                }

                
               
                currentSession.SetInstance(instance);
                if (!SessionManager.GetInstance.AddSession(currentSession))
                {
                    currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
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
                return QCIs;
            }
            catch (Exception ee)
            {
                throw ee;
            }
            finally
            {
                Session sessionDelete = SessionManager.GetInstance.GetSession(keyStr, Session.SessionType.Common); ;
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(_CombineCOAandDN)GeneratePizza end" );
            }
        }
        /// <summary>
        /// 印第一个pizza标签
        /// </summary>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>  
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        public IList<PrintItem> PrintPizzaLabel(string custSN, string line, string editor, string station, string customer,  IList<PrintItem> printItems)
        {
            logger.Debug("(_CombineCOAandDN)PrintPizzaLabel start, [CustomerSN]:" + custSN
                + " [line]: " + line
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custSN;

            try
            {

                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB);

                if (session == null)
                {
                    session = new Session(sessionKey, Session.SessionType.MB, editor, station, string.Empty, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", string.Empty);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.MB);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("CombineCOAandDNPrint1.xoml", string.Empty, wfArguments);

                    session.AddValue(Session.SessionKeys.CustSN, custSN);
                    
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.PrintLogName, "PIZZA Label");
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, custSN);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, custSN);
                    session.AddValue(Session.SessionKeys.PrintLogDescr, "");
                    session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(session))
                    {
                        session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    session.WorkflowInstance.Start();
                    session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }

                return (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                Session sessionDelete = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB); ;
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(_CombineCOAandDN)PrintPizzaLabel end, [CustomerSN]:" + custSN
                    + " [line]: " + line
                    + " [editor]:" + editor
                    + " [station]:" + station
                    + " [customer]:" + customer);
            }
        }
        /// <summary>
        /// 印第一个pizza标签
        /// </summary>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>  
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        public IList<PrintItem> PrintCOOLabel(string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(_CombineCOAandDN)PrintCOOLabel start, [CustomerSN]:" + custSN
                + " [line]: " + line
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custSN;

            try
            {

                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB);

                if (session == null)
                {
                    session = new Session(sessionKey, Session.SessionType.MB, editor, station, string.Empty, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", string.Empty);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.MB);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("CombineCOAandDNPrint1.xoml", string.Empty, wfArguments);

                    session.AddValue(Session.SessionKeys.CustSN, custSN);

                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.PrintLogName, "BT COO Label");
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, custSN);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, custSN);
                    session.AddValue(Session.SessionKeys.PrintLogDescr, "");
                    session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(session))
                    {
                        session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    session.WorkflowInstance.Start();
                    session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }

                return (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                Session sessionDelete = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB); ;
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(_CombineCOAandDN)PrintCOOLabel end, [CustomerSN]:" + custSN
                    + " [line]: " + line
                    + " [editor]:" + editor
                    + " [station]:" + station
                    + " [customer]:" + customer);
            }
        }
        /// <summary>
        /// 印RePrint标签
        /// </summary>
        /// <param name="reason">reason</param>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>  
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        public IList<PrintItem> RePrint(string reason, string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(_CombineCOAandDN)RePrint start, [CustomerSN]:" + custSN
                + " [line]: " + line
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custSN;

            try
            {

                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                if (null == currentProduct)
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(custSN);
                    throw new FisException("SFC002", errpara);
                }
                
                
                
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

                if (session == null)
                {
                    session = new Session(sessionKey, Session.SessionType.Product, editor, station, string.Empty, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", string.Empty);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("CombineCOAandDNRePrint.xoml", "CombineCOAandDNRePrint.rules", wfArguments);

                    session.AddValue(Session.SessionKeys.CustSN, custSN);
                    session.AddValue(Session.SessionKeys.Product, currentProduct);
                    session.AddValue(Session.SessionKeys.Reason, reason);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.PrintLogName, "PIZZA Label");
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, custSN);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, custSN);
                    session.AddValue(Session.SessionKeys.PrintLogDescr, "");
                    session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(session))
                    {
                        session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    session.WorkflowInstance.Start();
                    session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }
                return (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                Session sessionDelete = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product); 
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(_CombineCOAandDN)RePrint end, [CustomerSN]:" + custSN
                    + " [line]: " + line
                    + " [editor]:" + editor
                    + " [station]:" + station
                    + " [customer]:" + customer);
            }
        }
        /// <summary>
        /// 印RePrint标签
        /// </summary>
        /// <param name="reason">reason</param>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>  
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        public IList<PrintItem> RePrintCOO(string reason, string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(_CombineCOAandDN)RePrintCOO start, [CustomerSN]:" + custSN
                + " [line]: " + line
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custSN;

            try
            {

                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                if (null == currentProduct)
                {
                    List<string> err = new List<string>();
                    err.Add(custSN);
                    throw new FisException("SFC002", err);
                }

                if (!currentProduct.IsBT)
                {
                    List<string> err = new List<string>();
                    throw new FisException("PAK008", err);
                }
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

                if (session == null)
                {
                    session = new Session(sessionKey, Session.SessionType.Product, editor, station, string.Empty, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", string.Empty);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("CombineCOAandDNRePrint.xoml", "CombineCOAandDNRePrint.rules", wfArguments);

                    session.AddValue(Session.SessionKeys.CustSN, custSN);
                    session.AddValue(Session.SessionKeys.Product, currentProduct);
                    session.AddValue(Session.SessionKeys.Reason, reason);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    session.AddValue(Session.SessionKeys.PrintLogName, "BT COO Label");
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, custSN);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, custSN);
                    session.AddValue(Session.SessionKeys.PrintLogDescr, "");
                    session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(session))
                    {
                        session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    session.WorkflowInstance.Start();
                    session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }

                return (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                Session sessionDelete = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(_CombineCOAandDN)RePrintCOO end, [CustomerSN]:" + custSN
                    + " [line]: " + line
                    + " [editor]:" + editor
                    + " [station]:" + station
                    + " [customer]:" + customer);
            }
        }
        

        /// <summary>
        /// 无意义
        /// </summary>
        public string Start()
        {
            try
            {
                logger.Debug("(_CombineCOAandDN)StartBegin:");
                DateTime current = DateTime.Now;
                current =  current.AddSeconds(1);
                while (true)
                {
                    if (current < DateTime.Now)
                    {
                        break;
                    }
                    else
                    {
                        int aa = 1;
                    }
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
                throw e;
            }
            return "true";
        }
        #endregion
    }
    
}
