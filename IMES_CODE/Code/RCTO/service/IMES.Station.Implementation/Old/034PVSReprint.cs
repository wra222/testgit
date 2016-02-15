// INVENTEC corporation (c)2010 all rights reserved. 
// Description:  PVSReprint bll
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-04-06   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// 实现IPVSReprint接口，PVSReprint BLL实现类,实现PVS标签重印功能
    /// </summary>
    public class PVSReprint : MarshalByRefObject, IPVSReprint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Product;

        #region IPVSReprint Members

        /// <summary>
        /// Reprint Pizza Label
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="reason"></param>
        /// <param name="printItems"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public IList<PrintItem> ReprintLabel(ref string custSN, string reason, IList<PrintItem> printItems, string editor, string line, string station, string customer)
        {
            logger.Debug(" ReprintLabel start, custSN:" + custSN);
            try
            {

                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                custSN = currentProduct.CUSTSN;
                string sessionKey = currentProduct.ProId;
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
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "034PVSReprint.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currentProduct.PizzaID);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currentProduct.PizzaID);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "Pizza");
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(custSN);
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
                logger.Debug(" ReprintLabel end, custSN:" + custSN);
            }
        }

        #endregion
    }
}
