/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PAQCInputImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-03-15   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using log4net;
using IMES.FisObject.FA.Product;
using System.Collections;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IPAQCInput接口的实现类
    /// </summary>
    public class PAQCInputImpl : MarshalByRefObject, IPAQCInput
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region IPAQCInput members

        /// <summary>
        /// 输入Customer SN相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="custSN">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>抽检结果: "EOQC", "OQC", or "SKIP"</returns>
        public IList InputCustSN(string pdLine, string custSN, string editor, string stationId, string customer, IList<PrintItem> lstPrtItems)
        {
            logger.Debug("(PAQCInputImpl)InputCustSN start, [pdLine]:" + pdLine
                + " [custSN]: " + custSN
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custSN;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);
                IList ret = new ArrayList();
                IProduct product = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);

                sessionKey = product.ProId;

                if (session == null)
                {
                    session = new Session(sessionKey, ProductSessionType, editor, stationId, pdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "069PAQCInput.xoml", "069PAQCInput.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.IsComplete, false);
                    session.AddValue(Session.SessionKeys.PrintItems, lstPrtItems);
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

                //session.SwitchToWorkFlow();

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }

                ret.Add(session.GetValue(Session.SessionKeys.RandomInspectionStation).ToString());
                ret.Add(((Product)session.GetValue(Session.SessionKeys.Product)).ProId);
                ret.Add((IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems));
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
                logger.Debug("(PAQCInputImpl)InputCustSN end, [pdLine]:" + pdLine
                    + " [prodId]: " + custSN
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

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
                logger.Debug("Cancel end, sessionKey:" + sessionKey);
            }
        }

        public IList<PrintItem> ReprintLabel(string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItems, string reason)
        {
            logger.Debug("(PAQCInputImpl)ReprintLabel start, [custSN:] " + custSN
                + "[line:] " + line
                + "[editor:] " + editor
                + "[station:] " + station
                + "[customer:] " + customer);

            try
            {

                string sessionKey = Guid.NewGuid().ToString();
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);
                IProductRepository iprr = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IPalletRepository ipr = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IProduct currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.Common, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType.Common);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("069PAQCInputRePrint.xoml", string.Empty, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, custSN);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, custSN);
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
                logger.Debug("(PAQCInputImpl)ReprintLabel end, [custSN:] " + custSN
                    + "[line:] " + line
                    + "[editor:] " + editor
                    + "[station:] " + station
                    + "[customer:] " + customer);
            }
        }

        #endregion
    }
}
