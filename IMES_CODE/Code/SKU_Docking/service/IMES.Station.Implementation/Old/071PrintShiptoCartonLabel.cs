// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 071PrintShiptoCartonLabel
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-03-22   Chen Xu (eB1-4)              create
// 2011-04-06   Chen Xu                      Modify: ITC-1268-0020
// 2011-04-06   Chen Xu                      Modify: ITC-1268-0024
// Known issues:

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.MO;
using IMES.FisObject.Common.Model;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.FisObject.PAK.DN;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// PrintShiptoCartonLabel
    /// </summary>
    public class PrintShiptoCartonLabel : MarshalByRefObject, IPrintShiptoCartonLabel
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Common;
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region IPrintShiptoCartonLabel Members


        /// <summary>
        /// 刷CartonSN，选择相应的DN后，调用该方法
        /// 更新Carton 中所有Product 的状态，记录Carton 中所有Product Log，记录Carton 中所有Product 与DN 的绑定， DN 满，则更新DN 的状态
        /// 返回打印重量标签的PrintItem，结束工作流。
        /// 将CartonSN,deliveryNo放到Session.DeliveryNo里
        /// 以CartonSN为SessionKey创建工作流071PrintShiptoCartonLabel.xoml
        /// </summary>
        public IList<IMES.DataModel.PrintItem> Save(string PdLine, string CartonSN, string deliveryNo, IList<PrintItem> printItems, string editor, string station, string customer)
        {
            logger.Debug("(PrintShiptoCartonLabelImpl)Save start, PdLine:" + PdLine + "CartonSN" + CartonSN + "deliveryNo" + deliveryNo + "printItems" + printItems + "editor:" + editor + "station:" + station + "customer" + customer);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                var currentProduct = CommonImpl.GetProductByInput(CartonSN, CommonImpl.InputTypeEnum.Carton);

                if (!(currentProduct.DeliveryNo == "" || currentProduct.DeliveryNo == null))        
                {
                    erpara.Add(CartonSN);
                    throw new FisException("CHK156", erpara);//该Carton No已经与DN绑定! //ITC-1268-0024
                }

                string sessionKey = CartonSN;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);
               
                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                List<string> productIDList = new List<string>();
                productIDList = productRepository.GetProductIDListByCarton(CartonSN);
                int pcs = productIDList.Count;
               

                IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                Delivery currentDelivery = DeliveryRepository.Find(deliveryNo);


                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, currentSessionType, editor, station, PdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", PdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "071PrintShiptoCartonLabel.xoml", "071PrintShiptoCartonLabel.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.Carton, CartonSN);
                    currentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, productIDList);
                    currentSession.AddValue(Session.SessionKeys.ProdList, productIDList);
                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, deliveryNo);
                    currentSession.AddValue(Session.SessionKeys.Delivery, currentDelivery);
                    currentSession.AddValue(Session.SessionKeys.PCS, pcs);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.isDNFull, false);
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

                IList<PrintItem> returnList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                return returnList;

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
                logger.Debug("(PrintShiptoCartonLabelImpl)Save end,PdLine:" + PdLine + "CartonSN" + CartonSN + "deliveryNo" + deliveryNo + "printItems" + printItems + "editor:" + editor + "station:" + station + "customer" + customer);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        public void Cancel(string cartonSN)
        {
            try
            {
                logger.Debug("(PrintShiptoCartonLabelImpl)Cancel start, cartonSN:" + cartonSN);

                var currentProduct = CommonImpl.GetProductByInput(cartonSN, CommonImpl.InputTypeEnum.Carton);
                string sessionKey = cartonSN;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
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
            finally
            {
                logger.Debug("(PrintShiptoCartonLabelImpl)Cancel end, cartonSN:" + cartonSN);
            }
        }

        /// <summary>
        /// 输入CartonNo,选择Reason
        /// 使用工作流071ReprintShiptoCartonLabelImpl.xoml
        /// </summary>
        public IList<IMES.DataModel.PrintItem> ReprintShiptoCartonLabel(string cartonNo, string reason, IList<PrintItem> printItems, string line, string editor, string station, string customer)
        {

            logger.Debug("(ReprintShiptoCartonLabelImpl)ReprintShiptoCartonLabel start, cartonNo:" + cartonNo + "reason" + reason + "printItems:" + printItems + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                var currentProduct = CommonImpl.GetProductByInput(cartonNo, CommonImpl.InputTypeEnum.Carton);
                if ((currentProduct.DeliveryNo == "" || currentProduct.DeliveryNo == null)) 
                {
                    throw new FisException("CHK143", new string[] { cartonNo });//该Carton No还未与DN绑定! //ITC-1268-0020
                }
                string sessionKey = cartonNo;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("071ReprintShiptoCartonLabel.xoml", "", wfArguments);


                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.Carton, cartonNo);
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, cartonNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, cartonNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "ShiptoCartonLabel");
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.isDNFull, false);
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
                logger.Debug("(ReprintShiptoCartonLabelImpl)ReprintShiptoCartonLabel end,cartonNo:" + cartonNo + "reason" + reason + "printItems:" + printItems + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }


        #endregion

        #region
        
        /// <summary>
        /// 检查DN状态是否已经满了
        /// 总DN数量： Delivery.Qty
        /// 已经使用的DN数量： select (*) count  from IMES_FA..Product WHERE DeliveryNo = @DN
        /// </summary>
        /// <returns></returns>
        public Boolean CheckDNisFull(string deliveryNo)
        {
            try
            {
                logger.Debug("(PrintShiptoCartonLabelImpl)CheckDNisFull start, deliveryNo:" + deliveryNo);

                IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                int combinedQty = -1;
                combinedQty = currentProductRepository.GetCombinedQtyByDN(deliveryNo);
                if (combinedQty < 0)
                {
                    throw new FisException("CHK146", new List<string>());
                }

                IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                Delivery currentDelivery = DeliveryRepository.Find(deliveryNo);
                //Boolean isDnfull = currentDelivery.IsDNFull(combinedQty);
                //return isDnfull;
                int totalQty =-1;
                totalQty = currentDelivery.Qty;
                if (totalQty > combinedQty)
                {
                    return false;
                }
                else
                {
                    return true;
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
            finally
            {
                logger.Debug("(PrintShiptoCartonLabelImpl)CheckDNisFull end, deliveryNo:" + deliveryNo);
            }
        }


        /// <summary>
        /// 检查用户输入的[Carton No] 在IMES_FA..Product 表中是否存在
        /// </summary>
        /// <param name="cartonSN"></param>
        /// <returns>model</returns>
        public string CheckCartonSN(string cartonSN)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                logger.Debug("(PrintShiptoCartonLabelImpl)CheckDNisFull start, cartonSN:" + cartonSN);

                IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IMES.FisObject.FA.Product.IProduct currentProduct = currentProductRepository.FindOneProductWithProductIDOrCustSNOrCarton(cartonSN);
                if ((currentProduct!=null)&&(currentProduct.CartonSN.Trim() == cartonSN))
                {
                    string currentModel = currentProduct.Model.Trim();
                    if (currentModel != null)
                    {
                        return currentModel;
                    }
                    else
                    {
                        erpara.Add(cartonSN);
                        ex = new FisException("CHK144", erpara);
                        throw ex;
                    }
                }
                else
                {
                    throw new FisException("CHK105", new List<string>());
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
            finally
            {
                logger.Debug("(PrintShiptoCartonLabelImpl)CheckDNisFull end, cartonSN:" + cartonSN);
            }
        }


        /// <summary>
        /// 显示被选DN的total 数量，已结合数量和剩余数量
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        public IList<string> GetDNQtyInfo(string deliveryNo)
        {
            try
            {
                logger.Debug("(PrintShiptoCartonLabelImpl)GetDNQtyInfo start, deliveryNo:" + deliveryNo);

                IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                Delivery currentDelivery = DeliveryRepository.Find(deliveryNo);
                int totalQty = currentDelivery.Qty;

                IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                int combinedQty = currentProductRepository.GetCombinedQtyByDN(deliveryNo);

                int remainQty = totalQty - combinedQty;

                if (remainQty < 0)
                {
                    throw new FisException("CHK145", new List<string>());
                }

                IList<string> DNQtyInfoLst = new List<string>();
                DNQtyInfoLst.Add(totalQty.ToString());
                DNQtyInfoLst.Add(combinedQty.ToString());
                DNQtyInfoLst.Add(remainQty.ToString());
                return DNQtyInfoLst;

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
                logger.Debug("(PrintShiptoCartonLabelImpl)GetDNQtyInfo end, deliveryNo:" + deliveryNo);
            }
        }


        /// <summary>
        /// 获取PrintShiptoCartonLabel的DN列表
        /// 调用 IDeliveryRepository.getDeliveryNoListFor071()
        /// DN 按照Model（按照Carton No 查询IMES_FA..Product 得到）, Status='00', Ship Date大于等于当天为条件进行查询，结果按照ShipDate, Delivery No 进行排序
        /// </summary>
        public IList<string> getDNList(string Model)
        {
            try
            {
                logger.Debug("(PrintShiptoCartonLabelImpl)getDNList start, Model:" + Model);

                IDeliveryRepository iDeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                IList<string> DNLst = new List<string>();
                DNLst = iDeliveryRepository.GetDeliveryNoListFor071(Model);  
                return DNLst;
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
                logger.Debug("(PrintShiptoCartonLabelImpl)getDNList end, Model:" + Model);
            }
        }

        #endregion
    }
}
