/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Implementation for          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* Known issues:
* TODO：
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM; 
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Station;
//using System.Linq;
using IMES.Infrastructure.Extend;

namespace IMES.Station.Implementation
{

    public class FAIInput : MarshalByRefObject, IFAIInput
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        /// <summary>
        /// ProdId相关判断处理
        /// </summary>
        /// <param name="prodid">Product ID</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public ArrayList CheckProdId(string custsn, string pdLine, string editor, string stationId, string customerId)
		{
            FisException ex;
            List<string> erpara = new List<string>();

            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}( pdline={1}, editor={2}, stationId={3}, customerId={4}, custsn={5}, )", methodName, pdLine, editor, stationId, customerId, custsn);
            try
            {
                string sessionKey = custsn;
                Session CurrentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (CurrentSession == null)
                {
                    CurrentSession = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", CurrentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;

                    RouteManagementUtils.GetWorkflow(stationId, "FAIInput.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    CurrentSession.AddValue(Session.SessionKeys.ProductIDOrCustSN, custsn);

                    CurrentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(CurrentSession))
                    {
                        CurrentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    CurrentSession.WorkflowInstance.Start();
                    CurrentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (CurrentSession.Exception != null)
                {
                    if (CurrentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        CurrentSession.ResumeWorkFlow();
                    }
                    throw CurrentSession.Exception;
                }

                IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);

                IStationRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
                IStation station = itemRepository.Find(product.Status.StationId);
                if (null == station)
                {
                    throw new FisException("Can Not Found Station!");
                }

                ArrayList lst = new ArrayList();
                lst.Add(product.CUSTSN);
                lst.Add(product.ProId);
                lst.Add(product.Model);
                lst.Add(product.Status.StationId + " - " + station.Descr);

                return lst;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}( pdline={1}, editor={2}, stationId={3}, customerId={4}, custsn={5}, )", methodName, pdLine, editor, stationId, customerId, custsn);
            }
            
        }


        /// <summary>
        /// Check及保存处理
        /// </summary>
        /// <param name="prodid">Product ID</param>
        /// <param name="kbct">KB CT</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public void Save(string custsn, string pdLine, string editor, string stationId, string customerId)
        {
            FisException ex;
            List<string> erpara = new List<string>();

            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}( pdline={1}, editor={2}, stationId={3}, customerId={4}, custsn={5}, )", methodName, pdLine, editor, stationId, customerId, custsn);
            try
            {
                string sessionKey = custsn;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.Exception = null;
                    sessionInfo.SwitchToWorkFlow();
                }

                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }

                    throw sessionInfo.Exception;
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
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}( pdline={1}, editor={2}, stationId={3}, customerId={4}, custsn={5}, )", methodName, pdLine, editor, stationId, customerId, custsn);
            }

        }


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}( sessionKey={1} )", methodName, sessionKey);
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

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
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}( sessionKey={1} )", methodName, sessionKey);
            }
        }

    }
}
