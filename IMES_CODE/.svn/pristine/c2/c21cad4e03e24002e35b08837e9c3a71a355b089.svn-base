/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PickCardImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-10-27   zhu lei           Create 
 * Known issues:Any restrictions about this file 
 * TODO:
 * 编码完成，但数据库尚无相关可供测试数据。尚未调试，需整合阶段再行调试
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
using System.Collections;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IPickCard接口的实现类
    /// </summary>
    public class PickCardImpl : MarshalByRefObject, IPickCard
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType commonSessionType = Session.SessionType.Common;

        #region IPickCard members

        /// <summary>
        /// 输入truckID相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="truckID">Truck ID</param>
        /// <param name="dateStr">dataStr</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer">customer</param>
        /// <param name="items">items</param>
        /// <returns>List</returns>
        public IList InputTruckID(
            string pdLine,
            string truckID,
            string dateStr,
            string editor, string stationId, string customer, IList<PrintItem> items)
        {
            logger.Debug("(PickCardImpl)InputTruckID start, [pdLine]:" + pdLine
                + " [truckID]: " + truckID
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = truckID;
            IList lstRet = new ArrayList();

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, commonSessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, commonSessionType, editor, stationId, pdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个TruckID对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", commonSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "PickCard.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    session.AddValue(Session.SessionKeys.IsComplete, false);
                    session.AddValue(Session.SessionKeys.PrintItems, items);
                    session.AddValue(Session.SessionKeys.DateCode, dateStr);
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

                //add items to list
                lstRet.Add((ForwarderInfo)session.GetValue(Session.SessionKeys.Forwarder));
                lstRet.Add((IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems));
                lstRet.Add(session.GetValue(Session.SessionKeys.PickIDCtrl).ToString());
                return lstRet;
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
                logger.Debug("(PickCardImpl)InputTruckID end, [pdLine]:" + pdLine
                    + " [truckID]: " + truckID
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

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, commonSessionType);

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

        #region IPickCard Members

        public IList Init(IList param)
        {
            DateTime today = DateTime.Today;
            string date = string.Format("{0:D4}/{1:D2}/{2:D2}", today.Year, today.Month, today.Day);

            IList list = new ArrayList(1);
            list.Add(date);

            return list;
        }

        #endregion
    }
}
