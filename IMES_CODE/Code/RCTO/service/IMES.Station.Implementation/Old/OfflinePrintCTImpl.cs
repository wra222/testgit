/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: OfflinePrintCTImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-12-04   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.CommonIntf;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IOfflinePrintCT接口的实现类
    /// </summary>
    public class OfflinePrintCTImpl : MarshalByRefObject, IOfflinePrintCT 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Common;

        #region IOfflinePrintCT members

        /// <summary>
        /// 1.1	UC-FA-OPC-01 Offline Print CT
        /// 根据产线生产安排，选择CT Part，再根据维护的part基本信息打印出label
        /// 目的：便于后期parts的追踪
        /// </summary>
        /// <param name="PN">Part Number</param>
        /// <param name="DateCode">Date Code</param>
        /// <param name="Qty">要打印的数量</param>
        /// <param name="editor">operator</param>
        /// <returns>Print Items</returns>
        public IList<PrintItem> PrintCTForDell(string PN, string DateCode, int Qty, string editor, string stationId, string customer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据产线生产安排，选择CT Part，再根据维护的part基本信息打印出label
        /// 目的：便于后期parts的追踪
        /// </summary>
        /// <param name="AssemblyCode">AssemblyCode</param>
        /// <param name="DateCode">DateCode</param>
        /// <param name="VendoeDCode">VendoeDCode</param>
        /// <param name="qty">qty</param>
        /// <param name="editor">editor</param>
        /// <returns>Print Items</returns>
        public IList<PrintItem> PrintCTForTSB(string AssemblyCode, string DateCode, string VendorDCode, int qty, string editor, string stationId, string customer, IList<PrintItem> printItems, out IList<string> ctList)
        {
            logger.Debug("(OfflinePrintCTImpl)PrintCTForTSB start, [AssemblyCode]:" + AssemblyCode
                + " [DateCode]: " + DateCode
                + " [VendorDCode]: " + VendorDCode
                + " [qty]: " + qty
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = Guid.NewGuid().ToString();

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, SessionType, editor, stationId, string.Empty, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", string.Empty);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "012PrintCT.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.IsComplete, false);
                    session.AddValue(Session.SessionKeys.AssemblyCode, AssemblyCode);
                    session.AddValue(Session.SessionKeys.DateCode, DateCode);
                    session.AddValue(Session.SessionKeys.VendorDCode, VendorDCode);
                    session.AddValue(Session.SessionKeys.Qty, qty);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.SelectedWarrantyRuleID, int.Parse(DateCode));
                    session.AddValue(Session.SessionKeys.PCode, string.Empty);
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

                ctList = (IList<string>)session.GetValue(Session.SessionKeys.CTList);
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
                logger.Debug("(OfflinePrintCTImpl)PrintCTForTSB end, [AssemblyCode]:" + AssemblyCode
                    + " [DateCode]: " + DateCode
                    + " [VendorDCode]: " + VendorDCode
                    + " [qty]: " + qty
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// 根据产线生产安排，选择CT Part，再根据维护的part基本信息打印出label
        /// 目的：便于后期parts的追踪
        /// </summary>
        /// <param name="AssemblyCode">AssemblyCode</param>
        /// <param name="DateCode">DateCode</param>
        /// <param name="VendorDCode">VendorDCode</param>
        /// <param name="VendorCTLen">VendorCTLen</param>
        /// <param name="VendorSN">VendorSN</param>
        /// <returns>Print Items</returns>
        public IList<PrintItem> PrintCTForTSBCombine(string AssemblyCode, string DateCode, string VendorDCode, int VendorCTLen, string VendorSN, string editor, string stationId, string customer, IList<PrintItem> printItems, out IList<string> ctList, string pcode)
        {
            logger.Debug("(OfflinePrintCTImpl)PrintCTForTSBCombine start, [AssemblyCode]:" + AssemblyCode
                + " [DateCode]: " + DateCode
                + " [VendorDCode]: " + VendorDCode
                + " [VendorCTLen]: " + VendorCTLen
                + " [VendorSN]: " + VendorSN
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = Guid.NewGuid().ToString();

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, SessionType, editor, stationId, string.Empty, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", string.Empty);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "012PrintCT.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.IsComplete, false);
                    session.AddValue(Session.SessionKeys.AssemblyCode, AssemblyCode);
                    session.AddValue(Session.SessionKeys.DateCode, DateCode);
                    session.AddValue(Session.SessionKeys.VendorSN, VendorSN);
                    session.AddValue(Session.SessionKeys.VendorDCode, VendorDCode);
                    session.AddValue(Session.SessionKeys.Qty, 1);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.SelectedWarrantyRuleID, int.Parse(DateCode));
                    session.AddValue(Session.SessionKeys.PCode, pcode);
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

                ctList = (IList<string>)session.GetValue(Session.SessionKeys.CTList);
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
                logger.Debug("(OfflinePrintCTImpl)PrintCTForTSBCombine end, [AssemblyCode]:" + AssemblyCode
                    + " [DateCode]: " + DateCode
                    + " [VendorDCode]: " + VendorDCode
                    + " [VendorCTLen]: " + VendorCTLen
                    + " [VendorSN]: " + VendorSN
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

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

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

        #endregion
    }
}
