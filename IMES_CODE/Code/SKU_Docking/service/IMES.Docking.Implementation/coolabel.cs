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
using IMES.Docking.Interface.DockingIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.MO;
namespace IMES.Docking.Implementation
{
    /// <summary>
    /// IMES service for CooLable.
    /// </summary>
    public class _CooLabel : MarshalByRefObject, ICooLabel
    {
        private const Session.SessionType SessionType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeliveryRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
        private IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        private IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
        /// <summary>
        /// 得DN
        /// </summary>
        /// <param name="mode">mode</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public IList<DNForUI> GetDeliveryListByModel(string mode, string status)
        {
            IList<DNForUI> ret = new List<DNForUI>();
            IList<Delivery> getList = null;
            getList = currentRepository.GetDeliveryListByModel(mode, status);
            foreach (Delivery tmp in getList)
            {
                DNForUI ele = new DNForUI();
                if (tmp.Qty <= productRepository.GetCombinedQtyByDN(tmp.DeliveryNo))
                {
                    continue;
                }
                ele.Qty = tmp.Qty;
                ele.PoNo = tmp.PoNo;
                ele.ShipDate = tmp.ShipDate;
                ele.Status = tmp.Status;
                ele.ModelName = tmp.ModelName;
                ele.DeliveryNo = tmp.DeliveryNo;
                ret.Add(ele);
            }
            return ret;
        }
        /// <summary>
        /// 判断是否更新drop
        /// </summary>
        /// <param name="DN">DN</param>
        /// <returns></returns>
        public bool ISDNChange(string DN)
        {
            Delivery oldDelivery = currentRepository.Find(DN);
            if (null == oldDelivery)
            {
                return true;
            }
            if (oldDelivery.Status != "00")
            {
                 return true;
            }
            return false;
        }
        /// <summary>
        /// 判断是否是日本
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="model">model</param>
        /// <param name="valuePrefix">valuePrefix</param>
        /// <returns></returns>
        public bool ISJapan(string name, string model, string valuePrefix)
        {
            bool ret = modelRep.CheckExistModelInfo(name, model, valuePrefix);
            return ret;
        }
        /// <summary>
        /// 得product
        /// </summary>
        /// <param name="customerSn">customerSn</param>
        /// <param name="station">station</param> 
        /// <returns></returns>
        public S_CooLabel GetProductBySN(string customerSn, string station)
        {
            try
            {
                S_CooLabel ret = new S_CooLabel();
                ret.IsCombineDN = "";
                IProduct temp = productRepository.GetProductByCustomSn(customerSn);
                if (null == temp)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(customerSn);
                    throw new FisException("SFC002", errpara);
                }
                if (temp != null)
                {
                    ret.CustomerSN = temp.CUSTSN;
                    ret.Model = temp.Model;
                    ret.ProductID = temp.ProId;
                    ret.PalletNo = temp.PalletNo;
                    ret.CartonSN = temp.CartonSN;
                    ret.Mo = temp.MO;
                    var currentMO = moRepository.Find(temp.MO);
                    if (null != currentMO)
                    {
                        ret.Total = currentMO.Qty.ToString();
                        
                    }
                    //ret.Pass = productRepository.GetCountOfProductLogByMo(station, temp.MO).ToString();
                    ret.Pass = "0";
                    ret.IsCombineDN = temp.DeliveryNo;
                }
                return ret;
             }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// 获取Product表相关信息
        /// </summary>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="customerSN">customer SN</param>
        /// <param name="prod">prod</param> 
        public S_CooLabel GetProduct(string editor, string station, string customer, string customerSN, string prod)
        {
            logger.Debug("(_CooLabel)GetProduct start.customerSN:" + customerSN);
            string keyStr = "";
            try
            {
                S_CooLabel getProductInfo;
                if (station == null)
                {
                    station = "";
                }
                if (customerSN == "")
                {
                    getProductInfo = GetProductByProd(prod, station);
                }
                else
                {
                    getProductInfo = GetProductBySN(customerSN, station);
                }
                customerSN = getProductInfo.CustomerSN;
                prod = getProductInfo.ProductID;
                string line = "";
                ProductStatusInfo statusIfo = productRepository.GetProductStatusInfo(prod);
                if (statusIfo.pdLine != null)
                {
                    line = statusIfo.pdLine;
                }
                string sessionKey = prod;
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
                RouteManagementUtils.GetWorkflow(station, "CooLabelBlock.xoml", "", out wfName, out rlName);
                WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                currentSession.AddValue(Session.SessionKeys.CustSN, customerSN);
                currentSession.AddValue(Session.SessionKeys.ProductIDOrCustSN, prod);
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
                bool retJapan = ISJapan("PN", getProductInfo.Model, "#ABJ");
                if (retJapan == true)
                {
                    getProductInfo.IsJapan = "japantrue";
                }
                else
                {
                    getProductInfo.IsJapan = "japanfalse";
                }
                return getProductInfo;
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
                logger.Debug("(_CooLabel)GetProduct end, customerSN:" + customerSN);
            }
        }
        /// <summary>
        /// 获取Product表相关信息
        /// </summary>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="customerSN">customer SN</param>
        /// <param name="prod">prod</param> 
        public S_CooLabel GetProductOnly(string editor, string station, string customer, string customerSN, string prod)
        {
            logger.Debug("(_CooLabel)GetProductOnly start.customerSN:" + customerSN);
            try
            {
                S_CooLabel getProductInfo;
                if (station == null)
                {
                    station = "";  
                }
                if (customerSN == "")
                {
                    getProductInfo = GetProductByProd(prod, station);
                }
                else
                {
                    getProductInfo = GetProductBySN(customerSN, station);
                }
                customerSN = getProductInfo.CustomerSN;
                prod = getProductInfo.ProductID;
                bool retJapan = ISJapan("PN", getProductInfo.Model, "#ABJ");
                if (retJapan == true)
                {
                    getProductInfo.IsJapan = "japantrue";
                }
                else
                {
                    getProductInfo.IsJapan = "japanfalse";
                }
                return getProductInfo;
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
                logger.Debug("(_CooLabel)GetProductOnly end, customerSN:" + customerSN);
            }
        }
        /// <summary>
        /// 得product
        /// </summary>
        /// <param name="prod">name</param>
        /// <param name="station">station</param>  
        /// <returns></returns>
        public S_CooLabel GetProductByProd(string prod, string station)
        {
            try
            {
                S_CooLabel ret = new S_CooLabel();
                ret.IsCombineDN = "";
                IProduct temp = productRepository.Find(prod);
                if (null == temp)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(prod);
                    throw new FisException("SFC002", errpara);
                }
                if (temp != null)
                {
                    ret.Model = temp.Model;
                    ret.ProductID = temp.ProId;
                    ret.CustomerSN = temp.CUSTSN;
                    ret.PalletNo = temp.PalletNo;
                    ret.CartonSN = temp.CartonSN;
                    ret.Mo = temp.MO;
                    var currentMO = moRepository.Find(temp.MO);
                    if (null != currentMO)
                    {
                        ret.Total = currentMO.Qty.ToString();
                      
                    }
                    //ret.Pass = productRepository.GetCountOfProductLogByMo(station, temp.MO).ToString();
                    ret.Pass = "0";
                    ret.IsCombineDN = temp.DeliveryNo;
                }
                return ret;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// 得QTY
        /// </summary>
        /// <param name="deliveryNo">deliveryNo</name>
        /// <returns></returns>
        public int GetQTY(string deliveryNo)
        {
            int ret = -99999999;
            if (deliveryNo == "")
            {
                return ret;
            }
            try
            {
                ret = productRepository.GetCombinedQtyByDN(deliveryNo);
                Delivery newDelivery = currentRepository.Find(deliveryNo);
                if (ret >= newDelivery.Qty || newDelivery.Status != "00")
                {
                    ret = 0 - ret;
                }
                return ret;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// 更新ProductStatus
        /// </summary>
        /// <param name="station">station</name>
        /// <param name="status">status</name>
        /// <param name="editor">editor</name>
        /// <param name="prod">prod</name>
        /// <returns></returns>
        public void UpdateProStatus(string station, string status, string editor, string prod)
        {
            IMES.FisObject.FA.Product.ProductStatus newStatus = new IMES.FisObject.FA.Product.ProductStatus();
            newStatus.Editor = status;
            newStatus.StationId = station;
            newStatus.Udt = DateTime.Now;
            string[] prodIds = new string[1];
            prodIds[0] = prod;
            try
            {
                 productRepository.UpdateProductStatuses(newStatus, prodIds);
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// 更新ProductStatus
        /// </summary>
        /// <param name="station">station</param>
        /// <param name="editor">editor</param> 
        /// <param name="prod">prod</param>
        /// <returns></returns>
        public void InsertProLog(string station, string editor, string prod)
        {
            try
            {
                ProductStatusInfo statusIfo = productRepository.GetProductStatusInfo(prod);
                ProductLog[] items = new ProductLog[1];
                items[0].Line = statusIfo.pdLine;
                items[0].Station = station;
                items[0].ProductID = prod;
                items[0].Editor = editor;
                items[0].Cdt = DateTime.Now;
                productRepository.InsertProductLogs(items);
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// 更新RecordCKK
        /// </summary>
        /// <param name="editor">editor</param> 
        /// <param name="prod">prod</param>
        /// <param name="CKK">CKK</param> 
        /// <returns></returns>
        public void RecordCKK(string editor, string prod, string CKK)
        {
            try
            {
                bool have = productRepository.CheckExistProductInfo(prod, "CKK");
                if (have)
                { 
                    IMES.FisObject.FA.Product.ProductInfo setValue = new IMES.FisObject.FA.Product.ProductInfo();
                    IMES.FisObject.FA.Product.ProductInfo condition = new IMES.FisObject.FA.Product.ProductInfo();
                    setValue.InfoValue = CKK;
                    setValue.Editor = editor;
                    setValue.Udt = DateTime.Now;
                    condition.ProductID = prod;
                    condition.InfoType = "CKK";
                    productRepository.UpdateProductInfo(setValue, condition);
                }
                else
                {
                    IMES.FisObject.FA.Product.ProductInfo newValue = new IMES.FisObject.FA.Product.ProductInfo();
                    newValue.ProductID = prod;
                    newValue.InfoType = "CKK";
                    newValue.InfoValue = CKK;
                    newValue.Editor = editor;
                    newValue.Udt = DateTime.Now;
                    newValue.Cdt = DateTime.Now;
                    productRepository.InsertProductInfo(newValue);
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// 更新RecordCKK
        /// </summary>
        /// <param name="prod">prod</param>
        /// <returns></returns>
        public string DoPAQC(string prod)
        {
            try
            {
                string QCIs = "false";
                int doAQC = 0;
                IProduct currentProduct = productRepository.Find(prod);
                if (null == currentProduct)
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(prod);
                    throw new FisException("SFC002", errpara);
                }
                ProductStatusInfo statusIfo = productRepository.GetProductStatusInfo(currentProduct.ProId);
                doAQC = productRepository.GetCountOfQCStatusByTpAndPdLineAndPnoToday("PAQC", statusIfo.pdLine, currentProduct.Model);
                int QCRatio = 1;
                
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
                if (doAQC % QCRatio == 0)
                {
                    QCIs = "true";
                }
                return QCIs;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// 获取CombineDN表相关信息
        /// </summary>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="customerSn">customerSn</param>
        /// <param name="prod">prod</param> 
        /// <param name="DN">DN</param> 
        /// <param name="IsChk">IsChk</param> 
        public string CombineDN(string editor, string station, string customer, string customerSn, string prod, string DN, string IsChk)
		{
			return CombineDN(editor, station, customer, customerSn, prod, DN, IsChk, "false");
		}
		
		public string CombineDN(string editor, string station, string customer, string customerSn, string prod, string DN, string IsChk, string IsBTChk)
        {
            logger.Debug("(_CooLabel)CombineDN start.customerSN:" + customerSn);
            string keyStr = "";
            try
            {
                if (station == null)
                {
                    station = "";
                }
                S_CooLabel getProductInfo;
                if (customerSn == "")
                {
                    getProductInfo = GetProductByProd(prod, station);
                }
                else
                {
                    getProductInfo = GetProductBySN(customerSn, station);
                }
                customerSn = getProductInfo.CustomerSN;
                prod = getProductInfo.ProductID;
                if (IsChk == "true")
                {
                    
                }
                else
                {
                    Delivery newDelivery = currentRepository.Find(DN);
                    if (newDelivery == null)
                    {
                        throw new FisException("CHK190", new string[] { DN });//DN不存在
                    }
                    if (productRepository.GetCombinedQtyByDN(DN) >= newDelivery.Qty)
                    {
                        //throw new FisException("CHK875", new string[] { });
                    }
                    if (station != "96")
                    {
                        if (getProductInfo.PalletNo == null)
                        {
                        }
                        else if (getProductInfo.PalletNo != "")
                        {
                            throw new FisException("CHK876", new string[] { });
                        }
                        if (getProductInfo.CartonSN == null)
                        {
                        }
                        else if (getProductInfo.CartonSN != "")
                        {
                            throw new FisException("CHK876", new string[] { });
                        }
                    }
                    
                }

                string line = "";
                ProductStatusInfo statusIfo = productRepository.GetProductStatusInfo(prod);
                if (statusIfo.pdLine != null)
                {
                    line = statusIfo.pdLine;
                }
                if (line == null)
                {
                    line = "";
                }
                string sessionKey = prod;
                
                IProduct currentProduct = null;
                if (customerSn != "")
                {
                    currentProduct = productRepository.GetProductByCustomSn(customerSn);
                }
                else if (prod != "")
                {
                    currentProduct = productRepository.Find(prod);
                }
                if (null == currentProduct)
                {
                    List<string> errpara = new List<string>();
                    if (customerSn != "")
                    {
                        errpara.Add(customerSn);
                    }
                    else if (prod != "")
                    {
                        errpara.Add(prod);
                    }
                    throw new FisException("SFC002", errpara);
                }
				
				if ((IsChk == "true") && (IsBTChk != "true") && (!"2PR".Equals(currentProduct.Model.Substring(0,3)))) {
					List<string> errpara = new List<string>();
					throw new FisException("DCK005", errpara);
				}
				
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
                RouteManagementUtils.GetWorkflow(station, "CooLabelRun.xoml", "CooLabelRun.rules", out wfName, out rlName);
                WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                currentSession.AddValue(Session.SessionKeys.CustSN, customerSn);
                currentSession.AddValue(Session.SessionKeys.ProductIDOrCustSN, prod);
                currentSession.AddValue(Session.SessionKeys.DeliveryNo, DN);
                
                currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                string QCIs = "false";
                if (station != "96")
                {
                    if (DoPAQC(prod) == "true")
                    {
                        currentSession.AddValue("COOQCStatus", "true");
                        QCIs = "true";
                    }
                    else
                    {
                        currentSession.AddValue("COOQCStatus", "false");
                    }
                }
                else
                {
                    currentSession.AddValue("COOQCStatus", "undo");
                    IProductRepository repProduct = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    string[] tps = new string[1];
                    tps[0] = "PAQC";
                    IList<ProductQCStatus> QCStatusList = new List<ProductQCStatus>();
                    QCStatusList = repProduct.GetQCStatusOrderByUdtDesc(getProductInfo.ProductID, tps);
                    foreach (ProductQCStatus tmp in QCStatusList)
                    {
                        if (tmp.Status == "8")
                        {
                            QCIs = "true";
                            break;
                        }
                        break;
                    }
                }
                if (IsChk == "true")
                {
                    currentSession.AddValue(Session.SessionKeys.IsBT, false);
					if (IsBTChk == "true")
					{
						currentSession.AddValue("isBT", "Y");
					}
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.IsBT, true);
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
                logger.Debug("(_CooLabel)CombineDN end, customerSN:" + customerSn);
            }
        }
        /// <summary>
        /// 获取CombineDN表相关信息
        /// </summary>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="customerSn">customerSn</param>
        /// <param name="prod">prod</param> 
        /// <param name="DN">DN</param> 
        public void CombineDNNotCheck(string editor, string station, string customer, string customerSn, string prod, string DN)
        {
            try
            {
                Delivery newDelivery = currentRepository.Find(DN);
                if (newDelivery == null)
                {
                    throw new FisException("CHK190", new string[] { DN });//DN不存在
                }
                IProduct currentProduct = null;
                if (customerSn != "")
                {
                    currentProduct = productRepository.GetProductByCustomSn(customerSn);
                }
                else if (prod != "")
                {
                    currentProduct = productRepository.Find(prod);
                }
                if (null == currentProduct)
                {
                    List<string> errpara = new List<string>();
                    if (customerSn != "")
                    {
                        errpara.Add(customerSn);
                    }
                    else if (prod != "")
                    {
                        errpara.Add(prod);
                    }
                    throw new FisException("SFC002", errpara);
                }
                if (currentProduct.DeliveryNo != "" && currentProduct.DeliveryNo != DN)
                {
                    Delivery oldDelivery = currentRepository.Find(currentProduct.DeliveryNo);
                    if (oldDelivery != null)
                    {
                        oldDelivery.Status = "00";
                        IUnitOfWork uow = new UnitOfWork();
                        currentRepository.Update(oldDelivery, uow);
                        uow.Commit();
                    }
                }
                IList<string> productList = new List<string>();
                productList.Add(currentProduct.ProId);
                productRepository.BindDN(DN, productList, newDelivery.Qty);
                int packedQty = productRepository.GetCombinedQtyByDN(DN);
                if (packedQty == newDelivery.Qty)
                {
                    newDelivery.Status = "87";
                    IUnitOfWork uow = new UnitOfWork();
                    currentRepository.Update(newDelivery, uow);
                    uow.Commit();
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// 印标签
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
            logger.Debug("(_CooLabel)PrintCOOLabel start, [CustomerSN]:" + custSN
                + " [line]: " + line
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custSN;

            try
            {

                if (station == null)
                {
                    station = "";
                }
                IProduct currentProduct = null;
                currentProduct = productRepository.Find(custSN);

                if (null == currentProduct)
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(custSN);

                    throw new FisException("SFC002", errpara);
                }
                ProductStatusInfo statusIfo = productRepository.GetProductStatusInfo(custSN);
                if (statusIfo.pdLine != null)
                {
                    line = statusIfo.pdLine;
                }
                if (line == null)
                {
                    line = "";
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
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.MB);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("COOLabelPrint.xoml", string.Empty, wfArguments);

                    session.AddValue(Session.SessionKeys.CustSN, custSN);

                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.PrintLogName, "COO Label");
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
                logger.Debug("(_CooLabel)PrintCOOLabel end, [CustomerSN]:" + custSN
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
        public IList<PrintItem> RePrintCOO(string reason, string custSN, string line, string editor, string station, string customer,string pcode,IList<PrintItem> printItems)
        {
            logger.Debug("(_CooLabel)RePrintCOO start, [CustomerSN]:" + custSN
                + " [line]: " + line
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custSN;

            try
            {
                string code = "";
                if (station == null)
                {
                    station = "";
                }
                IProduct currentProduct = null;
                currentProduct = productRepository.Find(custSN);
                code = custSN;
                if (null == currentProduct)
                {
                    currentProduct = productRepository.GetProductByCustomSn(custSN);
                    if (null == currentProduct)
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(custSN);
                        throw new FisException("SFC002", errpara);
                    }
                    code = custSN;
                }
                custSN = currentProduct.ProId;
                ProductStatusInfo statusIfo = productRepository.GetProductStatusInfo(currentProduct.ProId);
                if (statusIfo.pdLine != null)
                {
                    line = statusIfo.pdLine;
                }
                if (line == null)
                {
                    line = "";
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
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("COOLabelRePrint.xoml", "COOLabelRePrint.rules", wfArguments);

                    session.AddValue(Session.SessionKeys.CustSN, custSN);
                    session.AddValue(Session.SessionKeys.Product, currentProduct);
                    session.AddValue(Session.SessionKeys.Reason, reason);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    string logstr = "";
                    if (pcode == "OPDK024")
                    {
                        logstr = "COO Label";
                    }
                    else
                    {
                        logstr = "JAN Label";
                   }

                    session.AddValue(Session.SessionKeys.PrintLogName, logstr);
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, code);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, code);
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
                logger.Debug("(_CooLabel)RePrintCOO end, [CustomerSN]:" + custSN
                    + " [line]: " + line
                    + " [editor]:" + editor
                    + " [station]:" + station
                    + " [customer]:" + customer);
            }
        }
    }

}
