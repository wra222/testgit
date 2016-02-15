/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: TPCBTPLabelImpl
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================
 * 2010-03-26   Chen Xu (eB1-4)     create
 * 2010-04-07	Chen Xu				Modify: ITC-1122-0239
 * Known issues:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Workflow.Runtime;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Collections;
using IMES.FisObject.Common.TPCB;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;
using IMES.Route;


namespace IMES.Station.Implementation
{

    /// <summary>
    /// Maintain T/P AND TPCB Vendor Code 的绑定数据，并列印Label
    /// </summary>
    public class TPCBTPLabel : MarshalByRefObject, ITPCBTPLabel
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType currentSessionType = Session.SessionType.Common;

        /// <summary>
        /// 通过tpcb和tp，获得VCode
        /// </summary>
        string ITPCBTPLabel.GetVCode(string tpcb, string tp, string editor, string station, string line, string customer)
        {
            logger.Debug("(TPCBTPLabelImpl)GetVCode start, tpcb:" + tpcb + "tp:" + tp  + "editor:" + editor + "station:" + station + "line:" + line + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(tpcb, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(tpcb, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", tpcb);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "097TPCBTPLabel.xoml", "097TPCBTPLabel.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.TPCB, tpcb);
                    currentSession.AddValue(Session.SessionKeys.TP,tp);
                    currentSession.AddValue(Session.SessionKeys.ifElseBranch, 1);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, true);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + tpcb + " Exists.");
                        erpara.Add(tpcb);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(tpcb);
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

                return ((string)currentSession.GetValue(Session.SessionKeys.VCode));

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
                logger.Debug("(TPCBTPLabelImpl)GetVCode end, tpcb:" + tpcb + "tp:" + tp + "editor:" + editor + "station:" + station + "line:" + line + "customer:" + customer);
            }
        }

        /// <summary>
        /// Maintain T/P AND TPCB Vendor Code 的绑定数据
        /// 保存 VCode Info
        /// </summary>
        void ITPCBTPLabel.Save(string tpcb, string tp, string vcode, string editor, string station, string line, string customer)
        {
            logger.Debug("(TPCBTPLabelImpl)Save start, tpcb:" + tpcb + "tp:" + tp + "vcode:" + vcode + "editor:" + editor + "station:" + station + "line:" + line + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(tpcb, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(tpcb, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", tpcb);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "097TPCBTPLabel.xoml", "097TPCBTPLabel.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.TPCB, tpcb);
                    currentSession.AddValue(Session.SessionKeys.TP, tp);
                    currentSession.AddValue(Session.SessionKeys.VCode, vcode);
                    currentSession.AddValue(Session.SessionKeys.Editor, editor);
                    currentSession.AddValue(Session.SessionKeys.ifElseBranch, 2);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + tpcb + " Exists.");
                        erpara.Add(tpcb);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(tpcb);
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
                logger.Debug("(TPCBTPLabelImpl)Save end, tpcb:" + tpcb + "tp:" + tp + "vcode:" + vcode + "editor:" + editor + "station:" + station + "line:" + line + "customer:" + customer);
            }
        }

        /// <summary>
        /// T/P AND TPCB Label Print
        /// 检查通过后，获取PrintItem
        /// </summary>

        IList<PrintItem> ITPCBTPLabel.Print(string tpcb, string tp, string vcode, string qty, IList<PrintItem> printItems, string editor, string station, string line, string customer)
        {
            logger.Debug("(TPCBTPLabelImpl)Print start, tpcb:" + tpcb + "tp:" + tp + "vcode:" + vcode + "qty:" + qty + "editor:" + editor + "station:" + station + "line:" + line + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(tpcb, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(tpcb, currentSessionType,editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", tpcb);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "097TPCBTPLabel.xoml", "097TPCBTPLabel.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.TPCB, tpcb);
                    currentSession.AddValue(Session.SessionKeys.TP, tp);
                    currentSession.AddValue(Session.SessionKeys.VCode, vcode);
                    currentSession.AddValue(Session.SessionKeys.Qty, qty);	//ITC-1122-0239

                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.ifElseBranch, 5);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + tpcb + " Exists.");
                        erpara.Add(tpcb);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(tpcb);
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
                logger.Debug("(TPCBTPLabelImpl)Print end, tpcb:" + tpcb + "tp:" + tp + "vcode:" + vcode + "qty:" + qty + "editor:" + editor + "station:" + station + "line:" + line + "customer:" + customer);
            }
        
        }


        /// <summary>
        /// Maintain T/P AND TPCB Vendor Code 的绑定数据
        /// Delete
        /// </summary>
        void ITPCBTPLabel.DeleteVcode(string vcode, string editor, string station, string line, string customer)
        {
            logger.Debug("(TPCBTPLabelImpl)Delete start, vcode:"  + vcode + "editor:" + editor + "station:" + station + "line:" + line + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(vcode, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(vcode, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", vcode);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "097TPCBTPLabel.xoml", "097TPCBTPLabel.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.VCode, vcode);
                    currentSession.AddValue(Session.SessionKeys.ifElseBranch, 3);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + vcode + " Exists.");
                        erpara.Add(vcode);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(vcode);
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
                logger.Debug("(TPCBTPLabelImpl)Delete end, vcode:" + vcode + "editor:" + editor + "station:" + station + "line" + line + "customer:" + customer);
            }
        
        }


        /// <summary>
        /// Maintain T/P AND TPCB Vendor Code 的绑定数据
        /// Query
        /// </summary>
        IList<VCodeInfo> ITPCBTPLabel.Query()
        {
            logger.Debug("(TPCBTPLabelImpl)Query start");

            try
            {
                ITPCBRepository TPCBRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBRepository, TPCB>();
                IList<VCodeInfo> currentVCodeInfoLst = TPCBRepository.QueryAll();
                return currentVCodeInfoLst;
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
                logger.Debug("(TPCBTPLabelImpl)Query end");
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void ITPCBTPLabel.Cancel(string sessionKey)
        {
           
            try
            {
                logger.Debug("(TPCBTPLabelImpl)Cancel start, sessionKey:" + sessionKey);

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
                logger.Debug("(TPCBTPLabelImpl)Cancel end, sessionKey:" + sessionKey);
            }        
        }
    }
}
