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
using System.Text;

using IMES.Infrastructure.Repository._Schema;
using System.Data;
using System.Data.SqlClient;
using IMES.FisObject.PAK.BSam;
using IMES.Infrastructure.Extend;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for ConbimeOfflinePizza.
    /// </summary>
    public class ConbimeOfflinePizza : MarshalByRefObject, IConbimeOfflinePizza
    {
        private const Session.SessionType SessionType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeliveryRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        private ICOAStatusRepository coaRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
        private IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        private IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
        private IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
        private IBSamRepository bsamRepository = RepositoryFactory.GetInstance().GetRepository<IBSamRepository>();
        
        /// <summary>
        /// 获取Product
        /// </summary>
        /// <param name="custSN">custSN</param>
		/// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        public ArrayList getProduct(string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItems)
		{
			logger.Debug("(ConbimeOfflinePizza)getProduct start, [custSN]:" + custSN + " ,[line]:" + line + " ,[editor]:" + editor + " ,[station]:" + station + " ,[customer]:" + customer);
            List<string> errpara = new List<string>();
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
                    errpara.Add(custSN);
                    throw new FisException("SFC002", errpara);
                }
				
                string sessionKey = custSN;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                //string sessionKey = currentProduct.ProId;
                keyStr = sessionKey;
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
                    RouteManagementUtils.GetWorkflow(station, "ConbimeOfflinePizza.xoml", "conbimeofflinepizza.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.CustSN, custSN);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);

                    currentSession.SetInstance(instance);
                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");

                    }

                    //==============================================
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.LineCode, "PAK");
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, "ConbimeOfflinePizza");
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, currentProduct.ProId);
                    //==============================================

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
                
                
                retList.Add(currentProduct.Model); // 1 model

                //get bom
                IList<BomItemInfo> bomItemList = PartCollection.GeBOM(sessionKey, SessionType);
                retList.Add(bomItemList); // 2 bom

                IList<PrintItem> printList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                retList.Add(printList); // 3 printList

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
                logger.Debug("(ConbimeOfflinePizza)getProduct end, [custSN]:" + custSN + " ,[line]:" + line + " ,[editor]:" + editor + " ,[station]:" + station + " ,[customer]:" + customer);
            }
		}

        public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue)
        {
            return PartCollection.TryPartMatchCheck(sessionKey, SessionType, checkValue);
        }
		
		/// <summary>
        /// Save
        /// </summary>
        /// <param name="custSN">custSN</param>
        /// <param name="pizzaId">pizzaId</param>
		/// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="printItems">printItems</param> 
        public ArrayList Save(string custSN, string pizzaId, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(ConbimeOfflinePizza)Save start, [custSN]:" + custSN + " ,[pizzaId]:" + pizzaId  + " ,[line]:" + line + " ,[editor]:" + editor + " ,[station]:" + station + " ,[customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retList = new ArrayList();

            string sessionKey = "";
			
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
				
                sessionKey = custSN;
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }

                session.AddValue(Session.SessionKeys.IsComplete, true);
                session.AddValue(Session.SessionKeys.PizzaID, pizzaId);

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

                var currentProduct = session.GetValue(Session.SessionKeys.Product) as IProduct;

				// mantis 2105, paqc
				IProductRepository repProduct = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IList<ProductQCStatus> QCStatusList = repProduct.GetQCStatusOrderByCdtDesc(currentProduct.ProId, new string[] { "PAQC" });
                string qcStatus = (QCStatusList != null && QCStatusList.Count != 0) ? QCStatusList[0].Status : string.Empty;
				retList.Add(qcStatus); // 1

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
                Session sessionDelete = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(ConbimeOfflinePizza)Save end, [custSN]:" + custSN + " ,[pizzaId]:" + pizzaId  + " ,[line]:" + line + " ,[editor]:" + editor + " ,[station]:" + station + " ,[customer]:" + customer);
            }
        }
        
		/// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="custSN"></param>
        public void Cancel(string custSN)
        {
            logger.Debug("(ConbimeOfflinePizza)Cancel start, [custSN]:" + custSN);
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
                logger.Debug("(ConbimeOfflinePizza)Cancel end, [custSN]:" + custSN);
            }
        }
        
		/// <summary>
        /// 
        /// </summary>
        /// <param name="customerSN"></param>
        /// <param name="reason"></param>
        /// <param name="printItems"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList Reprint(string custSN, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
			logger.Debug("(ConbimeOfflinePizza)Reprint start, [custSN]:" + custSN + " ,[line]:" + line + " ,[editor]:" + editor + " ,[station]:" + station + " ,[customer]:" + customer);
            List<string> erpara = new List<string>();
            
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                if (currentProduct == null)
                {
                    erpara.Add(custSN);
                    throw new FisException("SFC002", erpara);
                }

                string sessionKey = currentProduct.ProId;

                var repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

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
                    RouteManagementUtils.GetWorkflow(station, "ReprintCombineoffpizza.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, sessionKey);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.SetInstance(instance);

                    //==============================================
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.LineCode, "PAK");
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, "ConbimeOfflinePizza");
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "");
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    //==============================================

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
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
                
                //===============================================================================
                //Get infomation
                ArrayList retList = new ArrayList();

                IList<PrintItem> printList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

                retList.Add(printList);

                //===============================================================================
                
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
                logger.Debug("(ConbimeOfflinePizza)Reprint end, [custSN]:" + custSN + " ,[line]:" + line + " ,[editor]:" + editor + " ,[station]:" + station + " ,[customer]:" + customer);
            }
		}
		
    }
    
}
