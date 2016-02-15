
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Workflow.Runtime;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;   

namespace IMES.Station.Implementation
{
    public class MBCTCheck : MarshalByRefObject, IMBCTCheck
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

        #region IMBCTCheck Members

       /// <summary>
       /// 
       /// </summary>
       /// <param name="product"></param>
       /// <param name="pdLine"></param>
       /// <param name="editor"></param>
       /// <param name="stationId"></param>
       /// <param name="customerId"></param>
       /// <returns></returns>
        public IList<string> InputProduct(string product, string pdLine, string editor, string stationId, string customerId)
        {
            logger.Debug("(MBCTCheck)InputProduct start, product:" + product + " pdLine:" + pdLine + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            List<string> ret = new List<string>();
            try
            {
                string sessionKey = product;
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

                    RouteManagementUtils.GetWorkflow(stationId, "MBCTCheck.xoml", "MBCTCheck.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    CurrentSession.AddValue(Session.SessionKeys.ProductIDOrCustSN, product);
                    //CurrentSession.AddValue(Session.SessionKeys.ProductIDOrCustSN, product);
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

                string LCM = "";
                string MBCT = "";
                string CUSTSN = "";
                if (CurrentSession.GetValue("CUSTSN") == null)
                {
                    //error code
                    throw new FisException("CHK1069", new string[] { product, "CUSTSN" });
                }

                if (CurrentSession.GetValue("LCM") == null)
                {
                    //error code
                    throw new FisException("CHK1069", new string[] { product, "LCM" });
                }

                if (CurrentSession.GetValue(Session.SessionKeys.MBCT) == null)
                {
                    //error code
                    throw new FisException("CHK1069", new string[] { product, "MBCT" });
                }

                
                LCM = (string)CurrentSession.GetValue("LCM");
                MBCT = (string)CurrentSession.GetValue(Session.SessionKeys.MBCT);
                CUSTSN = (string)CurrentSession.GetValue("CUSTSN");                
                ret.Add(LCM);
                ret.Add(MBCT);
                ret.Add(CUSTSN);
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(MBCTCheck)InputProduct end, product:" + product + " pdLine:" + pdLine + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Save(string product, string pdLine, string editor, string stationId, string customerId)
		{
            logger.Debug("(MBCTCheck)Save start, product:" + product + " pdLine:" + pdLine + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            //FisException ex;
            //List<string> erpara = new List<string>();
            string ret = "OK";
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(product, TheType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(product);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    //currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    //currentSession.AddValue(Session.SessionKeys.IsComplete, true);
                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(MBCTCheck)Save end, product:" + product + " pdLine:" + pdLine + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            }
            
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string pcbno)
        {
            string sessionKey = "";
            try
            {
                logger.Debug("(MBCTCheck)Cancel start, pcbno:" + pcbno);

                sessionKey = pcbno;
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(MBCTCheck)Cancel end, pcbno:" + pcbno + " ,sessionKey:" + sessionKey);
            }
        }

         #endregion
    }
}
