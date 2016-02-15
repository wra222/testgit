// INVENTEC corporation (c)2012 all rights reserved. 
// Description: Change Model
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-06-15   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.DataModel;
using System.Data;

namespace IMES.Station.Implementation
{
    /// <summary>
    ///
    /// </summary>
    public class ChangeModel : MarshalByRefObject, IChangeModel
    {

        #region IChangeModel Members

        public List<StationDescrQty> InputModel1(string model1, string editor, string line, string station, string customer)
        {
            logger.Debug(" InputModel1 start, model1:" + model1);
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(model1, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(model1, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", model1);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "ChangeModel.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + model1 + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(model1);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    currentSession.AddValue(Session.SessionKeys.Model1, model1);
                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(model1);
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

                return currentSession.GetValue(Session.SessionKeys.StationTable) as List<StationDescrQty>;

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
                logger.Debug(" InputModel1 end, model1:" + model1);
            }
        }

        public void InputModel2(string model1, string model2)
        {
            logger.Debug("InputModel2 start, model1:" + model1 + " model2:" + model2);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(model1, currentSessionType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(model1);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.Model2, model2);
                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();
                }

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
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
                logger.Debug("InputModel2 end, model1:" + model1 + " model2:" + model2);

            }
        }

        public List<ProductModel> Change(string model1, string selectStation, int changeQty)
        {
            logger.Debug("Change start, model1:" + model1 + " SelectStation:" + selectStation);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(model1, currentSessionType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(model1);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.SelectStation, selectStation);
                    currentSession.AddValue(Session.SessionKeys.Qty, changeQty);
                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();
                }

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                return currentSession.GetValue(Session.SessionKeys.ProductIDListStr) as List<ProductModel>;
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
                logger.Debug("Change END, model1:" + model1 + " SelectStation:" + selectStation);

            }
        }

        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

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

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType currentSessionType = Session.SessionType.Common;

        #endregion
    }
}
