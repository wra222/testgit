/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PalletDataCollection New
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2011-04-01 Tong.Zhi-Yong         Create 
 * Known issues:
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using log4net;
using System.Workflow.Runtime;
using IMES.Infrastructure.WorkflowRuntime;
using System.Collections;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IPalletDataCollectionNew接口的实现类
    /// </summary>
    public class PalletDataCollectionImpl : MarshalByRefObject, IPalletDataCollection
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Common;


        #region IPalletDataCollectionNew members

        public IList startWorkflow(string cartonNo, string palletNo, string line, string editor, string station, string customer, Boolean isCombined)
        {
            logger.Debug("(PalletDataCollectionImpl)startWorkflow start, [cartonNo]:" + cartonNo
                + "[palletNo]: " + palletNo
                + "[line]: " + line
                + "[editor]: " + editor
                + "[station]: " + station
                + "[customer]: " + customer);

            try
            {

                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IProduct currentProduct = null;
                Pallet pallet = null;
                List<string> productIDList = productRepository.GetProductIDListByCarton(cartonNo);
                IList ret = new ArrayList();
                if (productIDList == null || productIDList.Count == 0)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    //erpara.Add(sessionKey);
                    ex = new FisException("CHK105", erpara);
                    throw ex;
                }
                currentProduct = productRepository.Find(productIDList[0]);
                pallet = palletRepository.Find(palletNo);

                if (pallet == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(palletNo);
                    ex = new FisException("CHK106", erpara);
                    throw ex;
                }

                //string sessionKey = palletNo;
                //产生sessionKey，requirement change ,pallet could be changing. choose guid as key
                string sessionKey = Guid.NewGuid().ToString();
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                Delivery delivery = null;

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
                    RouteManagementUtils.GetWorkflow(station, "045PalletDataCollection.xoml", "045PalletDataCollection.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue("IsFirst", true);
                    currentSession.AddValue(Session.SessionKeys.Carton, cartonNo);
                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, currentProduct.DeliveryNo);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.AddValue(Session.SessionKeys.PalletNo, palletNo);
                    
                    //Dean 20110920 for FRU Flow
                    List<string> PalletIdList=new  List<string>();
                    PalletIdList.Add(palletNo);
                    currentSession.AddValue(Session.SessionKeys.PalletIdList, PalletIdList);
                    //Dean 20110920 for FRU Flow

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

                delivery = (Delivery)currentSession.GetValue(Session.SessionKeys.Delivery);


                ret.Add(delivery.GetDNPalletQty(palletNo));
                ret.Add(cartonNo);
                ret.Add(currentProduct.DeliveryNo);//++DN return
                ret.Add(sessionKey);
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
                logger.Debug("(PalletDataCollectionImpl)startWorkflow end, [cartonNo]:" + cartonNo
                    + "[line]: " + line
                    + "[editor]: " + editor
                    + "[station]: " + station
                    + "[customer]: " + customer);
            }
        }

        public string inputCartonNo(string dn, string palletNo, string cartonNo)
        {
            logger.Debug("(PalletDataCollectionImpl)inputCartonNo start,"
                + " [dn]: " + dn
                + " [palletNo]:" + palletNo
                + " [cartonNo]:" + cartonNo);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = dn; //change dn content as requirement changing

            try
            {
                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                List<string> productIDList = productRepository.GetProductIDListByCarton(cartonNo);
                IList ret = new ArrayList();
                if (productIDList == null || productIDList.Count == 0)
                {
                    FisException ex0;
                    List<string> erpara0 = new List<string>();
                    //erpara.Add(sessionKey);
                    ex0 = new FisException("CHK105", erpara0);
                    throw ex0;
                }
                IProduct   currentProduct = productRepository.Find(productIDList[0]);

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
                    session.AddValue(Session.SessionKeys.Carton, cartonNo);
                    string prePltno = (string)session.GetValue(Session.SessionKeys.PalletNo);
                    string preDn = (string)session.GetValue(Session.SessionKeys.DeliveryNo);
                    //List<IProduct> prdlist = (List<IProduct>)session.GetValue(Session.SessionKeys.ProdList);
                    //int dnpacked = 0;
                    //foreach (Product prod in prdlist)
                    //{
                    //    if (prod.DeliveryNo == currentProduct.DeliveryNo)
                    //    {
                    //        dnpacked = dnpacked + 1;
                    //    }
                    //}
                    //change plt
                    if (prePltno != palletNo)
                    {
                        session.AddValue(Session.SessionKeys.PalletNo, palletNo);
                        session.AddValue(Session.SessionKeys.IsPalletChange, true);
                        //change session??
                    }
                    else
                    {
                        session.AddValue(Session.SessionKeys.IsPalletChange, false);                        
                    }
                    if (currentProduct.DeliveryNo != preDn)
                    {
                        session.AddValue(Session.SessionKeys.DeliveryNo, currentProduct.DeliveryNo);
                        IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                        Delivery thisDelivery = DeliveryRepository.Find(currentProduct.DeliveryNo);
                        if (thisDelivery == null)
                        {
                            List<string> errpara = new List<string>();
                            errpara.Add(currentProduct.DeliveryNo);
                            throw new FisException("CHK107", errpara);
                        }
                        session.AddValue(Session.SessionKeys.Delivery, thisDelivery);
                        ////isPalletDnfull(dnpacked, palletNo, thisDelivery);

                    }
                    else
                    {
                        Delivery thisDelivery = (Delivery)session.GetValue(Session.SessionKeys.Delivery);
                        //isPalletDnfull(dnpacked, palletNo, thisDelivery);
                    }

                    session.AddValue("IsFirst", false);
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

                return currentProduct.DeliveryNo;
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
                logger.Debug("(PalletDataCollectionImpl)inputCartonNo start,"
                    + " [dn]: " + dn
                    + " [palletNo]:" + palletNo
                    + " [cartonNo]:" + cartonNo);
            }
        }

        public System.Collections.IList save(string dn, string palletNo, IList<PrintItem> printItems)
        {
            logger.Debug("(PalletDataCollectionImpl)save start,"
                + " [guid]: " + dn
                + " [palletNo]: " + palletNo);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = dn;
            //Pallet pallet = null;
            IList ret = new ArrayList();
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
                    session.AddValue(Session.SessionKeys.Qty, 1);
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
                    ret.Add(session.GetValue(Session.SessionKeys.PrintItems));
                    ret.Add(session.GetValue(Session.SessionKeys.PalletIdList));
                    return ret;
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
                logger.Debug("(PalletDataCollectionImpl)save end,"
                    + " [guid]: " + dn
                    + " [palletNo]: " + palletNo);
            }
        }

        public void cancel(string dn, string palletNo)
        {
            logger.Debug("(PalletDataCollectionImpl)cancel start, [dn]:" + dn
                + "[palletNo]:" + palletNo);
           // FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = dn; ;

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
                logger.Debug("(PalletDataCollectionImpl)cancel end, [dn]:" + dn
                    + "[palletNo]:" + palletNo);
            }
        }

        public IList<PrintItem> ReprintLabel(string pltNoOrCartonNo, string line, string editor, string station, string customer, IList<PrintItem> printItems, string reason)
        {
            logger.Debug("(PalletDataCollectionImpl)ReprintLabel start, [pltNoOrCartonNo:] " + pltNoOrCartonNo
                + "[line:] " + line
                + "[editor:] " + editor
                + "[station:] " + station
                + "[customer:] " + customer);

            try
            {
                string sessionKey = Guid.NewGuid().ToString();
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                IProductRepository iprr = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IPalletRepository ipr = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                Pallet CurrentPallet = null;
                IProduct currentProduct = null;

                List<string> productIDList = iprr.GetProductIDListByCarton(pltNoOrCartonNo);

                if (productIDList != null && productIDList.Count != 0)
                {
                    currentProduct = iprr.Find(productIDList[0]);
                }
                else
                {
                    currentProduct = iprr.FindOneProductWithProductIDOrCustSNOrPallet(pltNoOrCartonNo);
                }

                if (currentProduct == null)
                {
                    FisException fe = new FisException("CHK079", new string[] { pltNoOrCartonNo });
                    throw fe;
                }

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
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("045PalletDataCollectionRePrint.xoml", string.Empty, wfArguments);

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
                logger.Debug("(PalletDataCollectionImpl)ReprintLabel end, [pltNoOrCartonNo:] " + pltNoOrCartonNo
                    + "[line:] " + line
                    + "[editor:] " + editor
                    + "[station:] " + station
                    + "[customer:] " + customer);
            }
        }

        public IList<string> getPalletListByCarton(string cartonNo)
        {
            try
            {
                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                List<string> productIDList = productRepository.GetProductIDListByCarton(cartonNo);
                 IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                List<string> ret = new List<string>();
                IList<PalletInfo> lstPalletInfo = null;

                IProduct firstProduct = null;
                string deliveryNo = string.Empty;


                if (productIDList != null && productIDList.Count != 0)
                {
                    firstProduct = productRepository.Find(productIDList[0]);
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(cartonNo);
                    ex = new FisException("CHK105", erpara);
                    throw ex;
                }
       
               lstPalletInfo = palletRepository.GetPalletList(firstProduct.DeliveryNo);
               IList<string> pltlist = DeliveryRepository.GetPalletNoListByDn(firstProduct.DeliveryNo);
               if (lstPalletInfo != null && lstPalletInfo.Count != 0)
               {
                   foreach (PalletInfo pltInfo in lstPalletInfo)
                   {
                       //not combined plt
                       if (!pltlist.Contains(pltInfo.friendlyName))
                       {
                           ret.Add(pltInfo.friendlyName);
                       }                      
                   }
                   foreach (string pltno in pltlist)
                   {
                       ret.Add(pltno + " *");
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
        }

        /// <summary>
        /// get dn palletinfo by palletno
        /// </summary>
        /// <param name="pltno"></param>
        /// <returns></returns>
        public IList<DNForUI> getInfoByPallet(string pltno)
        {
            try
            {
                IList<DNForUI> Infolist = new List<DNForUI>();
                IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IList<DeliveryPallet> dnpltlist = palletRepository.GetDeliveryPallet(pltno);
                if(dnpltlist!=null && dnpltlist.Count >0)
                {
                    DNForUI dpinfo=null;
                    foreach (DeliveryPallet dp in dnpltlist)
                    {
                        dpinfo = new DNForUI();
                        dpinfo.DeliveryNo =dp.DeliveryID;
                        dpinfo.Qty =dp.DeliveryQty;
                        dpinfo.PoNo = pltno; //pallet no
                        Infolist.Add(dpinfo); 
                    }
                }                
                return Infolist;
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

        

        #endregion
    }
}
