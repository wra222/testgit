/*
* INVENTEC corporation ©2011 all rights reserved. 

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using System.Collections;
using log4net;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.CommonIntf;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Route;
using IMES.Infrastructure.Extend;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.TestLog;

namespace IMES.Station.Implementation
{
    public class PCAOQCCollection : MarshalByRefObject, IPCAOQCCollection
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType sessionType = Session.SessionType.MB;

        public ArrayList InputMBNo(string mbNo, string line, string editor, string station, string customer)
        {
            logger.Debug("(PCAOQCCollection)InputMBNo start, MB No:" + mbNo
                          + "editor:" + editor + "station:" + station + "MB No:" + mbNo);

            try
            {
          
                ArrayList retList = new ArrayList();

                string sessionKey = mbNo;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, sessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, sessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", sessionType );

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "PCAOQCCollection.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, sessionKey);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
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
                //===============================================================================
              
                 var mb = (IMES.FisObject.PCA.MB.IMB)currentSession.GetValue(Session.SessionKeys.MB);
                 retList.Add(mb.ECR);
                 retList.Add(mb.Model);
                 retList.Add(mb.Family);
                return retList;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void Save(string mbNo, string actionName, string type, string status, string result, string cause, string remark)
        {
            logger.Debug("(PCAOQCCollection)save start, mbNo: " + mbNo);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = mbNo;
            
            try
            {
                IMBRepository rep = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMES.FisObject.PCA.MB.IMB>();
                IList<TestLog> listTestLog= rep.GetPCBTestLogListFromPCBTestLog(mbNo);
                if(listTestLog.Any(x=>x.ActionName==actionName && x.Type==type &&x.ErrorCode=="OK"))
                {
                    throw new FisException("This had been scaned pass, it cant not scan again!");
                }


                Session session = SessionManager.GetInstance.GetSession(sessionKey, sessionType);
                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
               
                    var mb = (IMES.FisObject.PCA.MB.IMB)session.GetValue(Session.SessionKeys.MB);
                    session.AddValue(ExtendSession.SessionKeys.TestLogActionName, actionName);
                    session.AddValue(Session.SessionKeys.FixtureID, mb.PCBModelID);
                    session.AddValue(ExtendSession.SessionKeys.TestLogErrorCode, result);
                    session.AddValue(ExtendSession.SessionKeys.TestLogDescr, cause);
                    session.AddValue(ExtendSession.SessionKeys.TestLogRemark, remark);
              
                   
                    session.Exception = null;
                    session.SwitchToWorkFlow();

                    if (session.Exception != null)
                    {
                        if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            session.ResumeWorkFlow();
                        }

                        throw session.Exception;
                    }
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
                logger.Debug("(PCAOQCCollection)save end,"
                   + " mbsno: " + mbNo);
            }
        
        }

        public void Cancel(string mbNo)
        {
            string sessionKey = mbNo;
            try
            {
                logger.Debug("(PCAOQCCollection)Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, sessionType);

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
                logger.Debug("(PCAOQCCollection)Cancel end, sessionKey:" + sessionKey);
            }
        }

    
    }
}
