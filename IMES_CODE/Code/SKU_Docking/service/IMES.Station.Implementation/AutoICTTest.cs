using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.PrintLog;
using IMES.Route;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Defect;



namespace IMES.Station.Implementation
{

    /// <summary>
    /// </summary>
    public partial class AutoICTTest : MarshalByRefObject, IAutoICTTest
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region IAutoICTTest Members
        /// <summary>
        /// </summary>                
        public ArrayList CheckWC(string toolId, string userId, string mbSN, string station, string line,string customer)
        {
            logger.Debug("(AutoICTTest)CheckWC Start:" + mbSN);

            ArrayList retValue = new ArrayList();
            string currentSessionKey = mbSN;

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.MB);
                if (currentSession == null)
                {
                    currentSession = new Session(currentSessionKey, Session.SessionType.MB, userId, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", userId);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.MB);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("ICTTestCheckWC.xoml", "", wfArguments);
                    currentSession.AddValue(Session.SessionKeys.FixtureID, toolId);
                    currentSession.AddValue(Session.SessionKeys.MB, mbSN);


                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
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
                    erpara.Add(currentSessionKey);
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

                retValue.Add("0");
                return retValue;
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
                logger.Debug("(AutoICTTest)CheckWC End:" + mbSN);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public  ArrayList ICTTestCompleted(string mbSN, string station, string toolId, string userId,string line,string customer,
                                                     string isPass, string failureCode, string testLogFilename, string actionName, IList<string> defectList, string testLogErrorCode)
        {
            logger.Debug("(AutoICTTest)ICTTestCompleted start, [key]:" + mbSN);
         
            string currentSessionKey = mbSN;
            ArrayList retValue = new ArrayList();
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.MB);

                if (currentSession == null)
                {
                    currentSession = new Session(currentSessionKey, Session.SessionType.MB, userId, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", userId);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.MB);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("ICTTestCompleted.xoml", "ICTTestCompleted.rules", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.MB, mbSN);
                    currentSession.AddValue(Session.SessionKeys.FixtureID, toolId);
                    currentSession.AddValue(ExtendSession.SessionKeys.TestLogRemark, testLogFilename);
                    currentSession.AddValue(ExtendSession.SessionKeys.TestLogActionName, actionName);
                    currentSession.AddValue(ExtendSession.SessionKeys.TestLogErrorCode, testLogErrorCode);
                    currentSession.AddValue(ExtendSession.SessionKeys.TestLogDescr, "");

                    currentSession.AddValue("PCBTestLogRemark", testLogFilename);
                    currentSession.AddValue("PCBTestLogActionName", actionName);
                    currentSession.AddValue("PCBTestLogErrorCode", testLogErrorCode);
                    currentSession.AddValue("PCBTestLogErrorDescr", "");
                    currentSession.AddValue("ICTTestResult", isPass);

                    if (isPass != "0")
                    {
                        //Session.SessionKeys.DefectList ExtendSession.SessionKeys.AllowPass
                   //     IList<string> defectList = new List<string>();
                     //   defectList.Add("OT05");
                        currentSession.AddValue(Session.SessionKeys.DefectList, defectList);
                        currentSession.AddValue(ExtendSession.SessionKeys.AllowPass, "N");
                        currentSession.AddValue(ExtendSession.SessionKeys.TestLogDescr, failureCode);
                        currentSession.AddValue("PCBTestLogErrorDescr", failureCode);
                        currentSession.AddValue(ExtendSession.SessionKeys.DefectStation, station);
                    }

                    currentSession.SetInstance(instance);
                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
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
                    erpara.Add(currentSessionKey);
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

                retValue.Add("0");
                return retValue;
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
                logger.Debug("(AutoICTTest)CheckWC End:" + mbSN);
            }
        }




        #endregion
    }

}
