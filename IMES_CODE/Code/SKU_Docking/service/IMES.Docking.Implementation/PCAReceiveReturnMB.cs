

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.Extend;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Docking.Interface.DockingIntf;
using log4net;
namespace IMES.Docking.Implementation
{
    /// <summary>
    /// IMES service for PCA Repair Input.
    /// </summary>
    public class PCAReceiveReturnMB : MarshalByRefObject, IPCAReceiveReturnMB
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.MB;
        #region IPCARepairInput Members

        /// <summary>
        /// 输入MBSno，卡站
        /// </summary>
        public IList<PCAReceiveReturnMBInfo> InputMBSno(string MBSno, string line, string editor, string station, string customer)
        {
            logger.Debug("(_PCARepairInput)InputMBSno end, MBSno:" + MBSno + " editor:" + editor + " stationId:" + station + " customerId:" + customer);
            IList<PCAReceiveReturnMBInfo> retList = new List<PCAReceiveReturnMBInfo>();
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(MBSno, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(MBSno, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", MBSno);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "PCAReceiveReturnMB.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.SetInstance(instance);
                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + MBSno + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(MBSno);
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
                    erpara.Add(MBSno);
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

                IList<ArrayList> info = (IList<ArrayList>)currentSession.GetValue("PCAReceiveReturnMBListInfo"); 
                //PCAReceiveReturnMBListInfo
                foreach(ArrayList items in info)//MBSN,LineID,DefectCodeID,Editor,Cdt
                {
                    PCAReceiveReturnMBInfo item = new PCAReceiveReturnMBInfo();
                    item.MBSN = items[0].ToString();
                    item.Line = items[1].ToString();
                    item.Defcet = items[2].ToString();
                    item.Editor = items[3].ToString();
                    item.Cdt = Convert.ToDateTime(items[4]);
                    retList.Add(item);
                }
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
                logger.Debug("(_PCARepairInput)InputMBSno end, MBSno:" + MBSno + " editor:" + editor + " stationId:" + station + " customerId:" + customer);
            }
        }

        /// <summary>
        /// Save
        /// </summary>
        public string Save(string mb, string IsBU,string Rework,string MBLookLike)
        {
            logger.Debug("(_PCARepairInput)Save start, MBSno:" + mb);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = mb;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK194", erpara);
                    throw ex;
                }

                currentSession.AddValue(ExtendSession.SessionKeys.TestLogActionName, "ReceiveReturnMB");
                currentSession.AddValue(Session.SessionKeys.FixtureID, "ReceiveReturnMB");
                currentSession.AddValue(ExtendSession.SessionKeys.TestLogErrorCode, IsBU);
                currentSession.AddValue(ExtendSession.SessionKeys.TestLogDescr, Rework);
                currentSession.AddValue(ExtendSession.SessionKeys.TestLogRemark, MBLookLike);

                currentSession.SwitchToWorkFlow();
                
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }
                string ret = "SUCCESSRET";
                return ret;
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
                logger.Debug("(_PCARepairInput)Save end, MBSno:" + mb);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        public void Cancel(string mb)
        {
            logger.Debug("(_PCARepairInput)Cancel start.");
            string sessionKey = mb;
            Session session = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

            if (session != null)
            {
                SessionManager.GetInstance.RemoveSession(session);
            }
            logger.Debug("(_PCARepairInput)Cancel end.");
        }
        #endregion
    }
}
