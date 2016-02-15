/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006            Create 
 * 2009-11-03   207006            ITC-1103-0093
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
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Route;

namespace IMES.Station.Implementation
{
    public class _MACReprint : MarshalByRefObject, IICTInput
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private static readonly string station;
        private static readonly Session.SessionType theType=Session.SessionType.MB ;

        #region IICTInput Members

        public IList<PrintItem> Reprint(
            string pdLine, string MB_SNo, string editor, string stationId, string customerId, string reason, IList<PrintItem> printItems)
        {
            logger.Debug("(MACReprint)Print start, pdLine:" + pdLine + " MB_SNo:" + MB_SNo + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId + " reason:" + reason);

            FisException ex;
            List<string> erpara = new List<string>();
            IList<PrintItem> printList;
            try
            {
                //2009-11-03   207006            ITC-1103-0093
                string sessionKey = MB_SNo;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, theType,editor,stationId,pdLine,customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", theType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "003Reprint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    Session.AddValue(Session.SessionKeys.Reason, reason);
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, MB_SNo);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, MB_SNo);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                   
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
                logger.Debug("(MACReprint)Print end, pdLine:" + pdLine + " MB_SNo:" + MB_SNo + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId + " reason:" + reason);
            }
        }
        #endregion
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

        #region #region IICTInput Members Update by Dean 20110331 
        public void InputMBSNo(
           string pdLine,
          out string custVersion,
           out string IECVersion,
           int multiQty,
           string MB_SNo,
           string editor, string stationId, string customerId, string ecr, string shipmode,out bool IsQuantity)
        {
            throw new NotImplementedException();
        }

        #endregion

        public IList<PrintItem> InputDefectCodeList(
     string MB_SNo,
     IList<string> defectList, IList<PrintItem> printItems, out IList<string> subMBSNs, string IECVersion, string CustVersion, out IList<string> subMBCustSNs)
        {
            throw new NotImplementedException();
        }

        #region IICTInput Members

        public void  InputMBSNo(
            string pdLine,
           out string custVersion,
            out string IECVersion,
            int multiQty,
            string MB_SNo,
            string editor, string stationId, string customerId, string ecr, out bool IsQuantity)
        {
            throw new NotImplementedException();
        }

        public void InputDeviceNo(string MB_SNo, string deviceNo)
        {
            throw new NotImplementedException();
        }

        public IList<PrintItem> InputDefectCodeList(
     string MB_SNo,
     IList<string> defectList, IList<PrintItem> printItems, out IList<string> subMBSNs, string IECVersion, string CustVersion)
        {
            throw new NotImplementedException();
        }

        //public string GetCustVersion(string MB_CODE, string ECR, out string IECVersion)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region IICTInput Members

        public void InputMBSNo(string pdLine, out string custVersion, out string IECVersion, int multiQty, string MB_SNo, string editor, string stationId, string customerId, string ecr)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, theType);

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

    }
}
