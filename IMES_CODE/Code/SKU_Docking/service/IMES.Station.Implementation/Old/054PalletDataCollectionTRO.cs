/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: PalletDataCollectionTRO
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2010-04-09 Tong.Zhi-Yong         Create 
 * Known issues:
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.DataModel;
using log4net;
using IMES.FisObject.FA.Product;
using System.Workflow.Runtime;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IPalletDataCollectionTRO接口的实现类
    /// </summary>
    public class PalletDataCollectionTROImpl : MarshalByRefObject, IPalletDataCollectionTRO
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Common;

        #region IPalletDataCollectionTRO members

        /// <summary>
        /// 刷第一个SN时，调用该方法启动工作流，根据输入CustSN获取ProductModel,
        /// 将SN放到Session.CustSN里
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="palletNo"></param>
        /// <param name="firstSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public IMES.DataModel.ProductModel StartWorkFlow(string dn, string palletNo, string firstSN, string line, string editor, string station, string customer)
        {
            logger.Debug("(PalletDataCollectionTROImpl)StartWorkFlow start, [dn]:" + dn
                + "[palletNo]: " + palletNo
                + "[firstSN]: " + firstSN
                + "[line]: " + line
                + "[editor]: " + editor
                + "[station]: " + station
                + "[customer]: " + customer);

            try
            {
                string sessionKey = palletNo;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                //Delivery delivery = null;

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "054PalletDataCollectionTRO.xoml", "054PalletDataCollectionTRO.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);


                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, dn);
                    currentSession.AddValue(Session.SessionKeys.PalletNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.CustSN, firstSN);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
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
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
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

                IProduct currentProduct = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);
                IMES.DataModel.ProductModel currentModel = new IMES.DataModel.ProductModel();

                currentModel.CustSN = currentProduct.CUSTSN;
                currentModel.ProductID = currentProduct.ProId;
                currentModel.Model = currentProduct.Model;
                return currentModel;
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
                logger.Debug("(PalletDataCollectionTROImpl)StartWorkFlow end, [dn]:" + dn
                    + "[palletNo]: " + palletNo
                    + "[firstSN]: " + firstSN
                    + "[line]: " + line
                    + "[editor]: " + editor
                    + "[station]: " + station
                    + "[customer]: " + customer);
            }
        }

        /// <summary>
        /// 除首次外，每次刷SN都调用该方法，进行SFC,判断DN是否和但前选定DN相同,刷SN返回ProductModel
        /// 将SN放到Session.CustSN里
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="palletNo"></param>
        /// <param name="uutSN"></param>
        /// <returns>返回ProductModel</returns>
        public IMES.DataModel.ProductModel ScanSN(string dn, string palletNo, string uutSN)
        {
            logger.Debug("(PalletDataCollectionTROImpl)ScanSN start,"
                + " [dn]: " + dn
                + " [palletNo]:" + palletNo
                + " [uutSN]:" + uutSN);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = palletNo;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                else
                {
                    session.AddValue(Session.SessionKeys.CustSN, uutSN);
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
                }

                IProduct currentProduct = (IProduct)session.GetValue(Session.SessionKeys.Product);
                IMES.DataModel.ProductModel currentModel = new IMES.DataModel.ProductModel();

                currentModel.CustSN = currentProduct.CUSTSN;
                currentModel.ProductID = currentProduct.ProId;
                currentModel.Model = currentProduct.Model;
                return currentModel;
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
                logger.Debug("(PalletDataCollectionTROImpl)ScanSN start,"
                    + " [dn]: " + dn
                    + " [palletNo]:" + palletNo
                    + " [uutSN]:" + uutSN);
            }
        }

        /// <summary>
        ///  更新所有机器状态，记录所有机器Log，绑定Pallet，返回打印重量标签的PrintItem，结束工作流。
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="palletNo"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public IList<IMES.DataModel.PrintItem> Save(string dn, string palletNo, IList<IMES.DataModel.PrintItem> printItems, out bool isPalletFull)
        {
            logger.Debug("(PalletDataCollectionTROImpl)save start,"
                + " [dn]: " + dn
                + " [palletNo]: " + palletNo);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = palletNo;
            Pallet pallet = null;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                else
                {
                    session.AddValue(Session.SessionKeys.IsComplete, true);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
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

                    //palletQty = 0;
                    pallet = (Pallet)session.GetValue(Session.SessionKeys.Pallet);
                    isPalletFull = pallet.IsPalletFull(dn);
                    return (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug("(PalletDataCollectionTROImpl)save end,"
                    + " [dn]: " + dn
                    + " [palletNo]: " + palletNo);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="palletNo"></param>
        public void Cancel(string dn, string palletNo)
        {
            logger.Debug("(PalletDataCollectionTROImpl)Cancel start, [dn]:" + dn
                + "[palletNo]:" + palletNo);
            //FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = palletNo; ;

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
                logger.Debug("(PalletDataCollectionTROImpl)Cancel end, [dn]:" + dn
                    + "[palletNo]:" + palletNo);
            } 
        }

        /// <summary>
        /// 获取可以做PalletDataCollectionTRO的DN列表，成功后调用ScanSN
        /// 调用 IDeliveryRepository.getDeliveryNoListFor054()
        /// </summary>
        /// <returns></returns>
        public IList<string> getDNList()
        {
            try
            {
                IDeliveryRepository idr = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                IList<string> ret = null;

                ret = idr.GetDeliveryNoListFor054();
                return ret;
            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据DN获取Pallet列表
        /// </summary>
        /// <returns></returns>
        public IList<string> getPalletList(string dn)
        {
            try
            {
                IPalletRepository ipr = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IList<PalletInfo> retFromRep = null;
                IList<string> ret = null;

                retFromRep = ipr.GetPalletList(dn);

                if (retFromRep != null && retFromRep.Count != 0)
                {
                    ret = new List<string>();

                    foreach (PalletInfo temp in retFromRep)
                    {
                        ret.Add(temp.friendlyName);
                    }
                }

                return ret;
            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 输入的可以是PalletNo，也可以是ProductID
        /// </summary>
        /// <param name="ProductIDOrPalletNo"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public IList<IMES.DataModel.PrintItem> ReprintLabel(string ProductIDOrPalletNo, IList<IMES.DataModel.PrintItem> printItems, string line, string editor, string station, string customer, string reason, out bool isPalletFull, out string out_palletNo)
        {
            logger.Debug("(PalletDataCollectionTROImpl)ReprintLabel start, [ProductIDOrPalletNo:] " + ProductIDOrPalletNo
                + "[line:] " + line
                + "[editor:] " + editor
                + "[station:] " + station
                + "[customer:] " + customer);

            try
            {
                var currentProduct = CommonImpl.GetProductByInput(ProductIDOrPalletNo, CommonImpl.InputTypeEnum.ProductIDOrCustSNOrPallet);
                string sessionKey = Guid.NewGuid().ToString();
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                IPalletRepository ipr = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                Pallet CurrentPallet = null;

                CurrentPallet = ipr.Find(currentProduct.PalletNo);

                if (CurrentPallet == null)
                {
                    FisException fe = new FisException("CHK106", new string[] { currentProduct.PalletNo });
                    throw fe;
                }

                if (!CurrentPallet.IsPalletFull(string.Empty))
                {
                    FisException fe = new FisException("CHK122", new string[] { currentProduct.PalletNo });
                    throw fe;
                }

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("054PalletDataCollectionTRORePrint.xoml", string.Empty, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currentProduct.PalletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currentProduct.PalletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "");
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
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
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

                out_palletNo = CurrentPallet.PalletNo; 
                isPalletFull = false;
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
                logger.Debug("(PalletDataCollectionTROImpl)ReprintLabel end, [ProductIDOrPalletNo:] " + ProductIDOrPalletNo
                    + "[line:] " + line
                    + "[editor:] " + editor
                    + "[station:] " + station
                    + "[customer:] " + customer);
            }
        }

        /// <summary>
        /// 获取属于该DN,Pallet的所有Product
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="pallet"></param>
        /// <returns></returns>
        public IList<IMES.DataModel.ProductModel> getProductByDnPallet(string dn, string pallet)
        {
            try
            {
                IProductRepository ipr = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IList<ProductModel> ret = null;

                ret = ipr.GetProductByDnPallet(dn, pallet);

                return ret;
            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取属于该Pallet的所有CustSN
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        public IList<string> getProductByPallet(string palletNo)
        {
            try
            {
                IProductRepository ipr = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IList<ProductModel> backList = null;
                List<string> lstCustSN = new List<string>();

                backList = ipr.GetProductListByPalletNo(palletNo);

                if (backList != null && backList.Count != 0)
                {
                    foreach (ProductModel temp in backList)
                    {
                        lstCustSN.Add(temp.CustSN);
                    }
                }

                return lstCustSN;
            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据palletNo获取Delivery_Pallet中的DN和Qty
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        public Dictionary<string, int> getDeliveryAndQtyByPalletNo(string palletNo)
        {
            try
            {
                Dictionary<string, int> dic = null;
                IPalletRepository ipr = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IList<DeliveryPallet> lstDeliveryPallet = ipr.GetDeliveryPallet(palletNo);

                if (lstDeliveryPallet != null && lstDeliveryPallet.Count != 0)
                {
                    dic = new Dictionary<string, int>();

                    foreach (DeliveryPallet dp in lstDeliveryPallet)
                    {
                        dic.Add(dp.DeliveryID, dp.DeliveryQty);
                    }
                }

                return dic;
            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取Delivery_Pallet的qty
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        public int getDNPalletQty(string deliveryNo, string palletNo)
        {
            try
            {
                IDeliveryRepository idr = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                Delivery delivery = idr.Find(deliveryNo);
                int qty = 0;

                if (delivery != null)
                {
                    qty = delivery.GetDNPalletQty(palletNo);
                }

                return qty;
            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
