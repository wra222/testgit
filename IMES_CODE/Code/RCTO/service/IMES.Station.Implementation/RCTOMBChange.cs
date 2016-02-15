/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Implementation for RCTO MB Change Page
* UI:CI-MES12-SPEC-SA-UI RCTO MB Change.docx –2012/6/15 
* UC:CI-MES12-SPEC-SA-UC RCTO MB Change.docx –2012/6/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-8-1    Jessica Liu           Create
* Known issues:
* TODO：
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;


namespace IMES.Station.Implementation
{

    public class RCTOMBChange : MarshalByRefObject, IRCTOMBChange
    {
        private const Session.SessionType TheType = Session.SessionType.Common;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IRCTOMBChange Members


        /// <summary>
        /// Check MB SNo
        /// </summary>
        /// <param name="mbSno">mbSno</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <returns>mbSno和Model值</returns>
        public List<string> CheckMBSNo(string mbSno, string pdLine, string stationId, string editor, string customerId)
        {
            logger.Debug("(RCTOMBChange)CheckMBSNo start, MBSno:" + mbSno + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = mbSno;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;

                    RouteManagementUtils.GetWorkflow(stationId, "RCTOMBChangePrint.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.MBSN, mbSno);

                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    Session.WorkflowInstance.Start();
                    Session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }
                    throw Session.Exception;
                }

                string Model = (string)Session.GetValue(Session.SessionKeys.PCBModelID);

                List<string> ret = new List<string>();
                ret.Add(mbSno);
                ret.Add(Model);

                return ret;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(RCTOMBChange)CheckMBSNo end, MBSno:" + mbSno + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
            
        }

        /// <summary>
        /// Save And Print
        /// </summary>
        /// <param name="mbSno">mbSno</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>Print Item列表</returns>
        /// <param name="newMBSno">new MBSno</param>
        public IList<PrintItem> SaveAndPrint(string mbSno, string pdLine, string stationId, string editor, string customerId, IList<PrintItem> printItems, out string newMBSno)
        {
            logger.Debug("(RCTOMBChange)SaveAndPrint start, MBSno:" + mbSno + " pdLine:" + pdLine + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = mbSno;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.PrintItems, printItems);

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

                //2012-9-7, Jessica Liu
                newMBSno = (string)sessionInfo.GetValue(Session.SessionKeys.MBSN);

                IList<PrintItem> returnList = this.getPrintList(sessionInfo);
                return returnList;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(RCTOMBChange)SaveAndPrint end, MBSno:" + mbSno + " pdLine:" + pdLine + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
        }


        /// <summary>
        /// Reprint
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="reason"></param>
        /// <param name="pdLine"></param>
        /// <param name="customerId"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public IList<PrintItem> RePrint(string mbSno, string reason, string pdLine, string customerId, string editor, string stationId, IList<PrintItem> printItems)
        {
            logger.Debug("(RCTOMBChange)Reprint start, MBSno:" + mbSno + " Reason:" + reason + " editor:" + editor + " station:" + stationId + " customerId:" + customerId);
            
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = mbSno;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "RCTOMBChangeReprint.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);                   
                    
                    //Session.AddValue(Session.SessionKeys.MBMONO, mbSno);
                    Session.AddValue(Session.SessionKeys.MBSN, mbSno);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.Reason, reason);
                    /*
                    Session.AddValue(Session.SessionKeys.PrintLogName, "RCTO MB Label");
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, mbSno);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, mbSno);                    
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, "Reprint");
                    */

                    Session.SetInstance(instance);
                    
                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    Session.WorkflowInstance.Start();
                    Session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }
                    throw Session.Exception;
                }

                IList<PrintItem> returnList = this.getPrintList(Session);
                return returnList;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(RCTOMBChange)Reprint end, MBSno:" + mbSno + " Reason:" + reason + " editor:" + editor + " customerId:" + customerId);
            }
        }


        /// <summary>
        /// 从Sessin里获取打印列表
        /// </summary>
        /// <param name="session">session</param>
        /// <returns></returns>
        private IList<PrintItem> getPrintList(Session session)
        {

            try
            {
                object printObject = session.GetValue(Session.SessionKeys.PrintItems);
                session.RemoveValue(Session.SessionKeys.PrintItems);
                if (printObject == null)
                {
                    return null;
                }

                IList<PrintItem> printItems = (IList<PrintItem>)printObject;

                return printItems;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;

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
                logger.Debug("Cancel end, sessionKey:" + sessionKey);
            }
        }

        #endregion
    }
}
