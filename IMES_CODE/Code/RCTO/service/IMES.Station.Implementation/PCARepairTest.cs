/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: PCARepairTest
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-10-20   zhu lei           Create 
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
using IMES.FisObject.PCA.MB;
using log4net;
using IMES.Route;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// IPCARepairTest接口的实现类
    /// </summary>
    public class PCARepairTest : MarshalByRefObject, IPCARepairTest
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region IPCARepairTest members

        /// <summary>
        /// 输入MBSno相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="mbSno">MBSno</param>
        /// <param name="editor">Editor</param>
        /// <param name="stationId">Station</param>
        /// <param name="customer">Customer</param>
        public void InputMBSno(string pdLine, string mbSno, string editor, string stationId, string customer)
        {
            logger.Debug("(PCARepairTest)InputmbSno start, [pdLine]:" + pdLine
                + " [mbSno]: " + mbSno
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = mbSno;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

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
                    RouteManagementUtils.GetWorkflow(stationId, "PCARepairTest.xoml", "PCARepairTest.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.IsComplete, false);
                    session.AddValue(Session.SessionKeys.MBSN, sessionKey);
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
                logger.Debug("(PCARepairTest)InputmbSno end, [pdLine]:" + pdLine
                    + " [mbSno]: " + mbSno
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// 不良品信息。
        /// </summary>
        /// <param name="mbSno">MBSno</param>
        /// <param name="defectList">Defect IList</param>
        /// <param name="scanCode">scanCode</param>
        /// <returns>string</returns>
        public string InputDefectCodeList(string mbSno, IList<string> defectList, string scanCode)
        {
            logger.Debug("(PCARepairTest)InputDefectCodeList start,"
                + " [mbSno]: " + mbSno
                + " [defectList]:" + defectList
                + " [scanCode]:" + scanCode);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = mbSno;
            string ret = "";
            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

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
                    session.AddValue(Session.SessionKeys.IsComplete, false);
                    session.AddValue(Session.SessionKeys.DefectList, defectList);
                    session.AddValue(Session.SessionKeys.C, scanCode);
                    if (scanCode == "9999")
                    {
                        if (defectList.Count > 0)
                        {
                            session.AddValue(Session.SessionKeys.ifElseBranch, false);
                        }
                        else
                        {
                            session.AddValue(Session.SessionKeys.ifElseBranch, true);
                        }
                    }

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
                    bool flag = (bool)session.GetValue(Session.SessionKeys.IsComplete);
                    if (flag)
                    {
                        ret = "SUCCESS";
                    }
                    else
                    {
                        ret = "ERROR";
                    }
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
                logger.Debug("(PCARepairTest)InputDefectCodeList end,"
                   + " [mbSno]: " + mbSno
                   + " [defectList]:" + defectList
                   + " [scanCode]:" + scanCode);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            logger.Debug("(PCARepairTest)Cancel start, [mbSno]:" + sessionKey);
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

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
                logger.Debug("(PCARepairTest)Cancel end, [mbSno]:" + sessionKey);
            }
        }

        #endregion

    }
}