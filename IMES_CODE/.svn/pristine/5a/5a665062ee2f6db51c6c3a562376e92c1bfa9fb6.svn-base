/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: SmallPartsLabelPrintImpl
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================
 * 2010-05-06   Chen Xu (eB1-4)     create
 *  
 * Known issues:
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
    /// SmallPartsLabelPrint
    /// </summary>
    public class SmallPartsLabelPrint : MarshalByRefObject, ISmallPartsLabelPrint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Common;
        
        #region ISmallPartsLabelPrint Members

        /// <summary>
        /// 列印Small Parts Label
        /// </summary>
        public IList<PrintItem> Print(string iecPn, int qty, string editor, string line, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(SmallPartsLabelPrintImpl) Print start, iecPn:" + iecPn + " , qty:" + qty + " , editor:" + editor + " , line:" + line + " , station:" + station + " , customer:" + customer);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(iecPn, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(iecPn, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", iecPn);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "108SmallPartsLabelPrint.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.IECPN, iecPn);
                    currentSession.AddValue(Session.SessionKeys.Qty, qty);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + iecPn + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(iecPn);
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
                    erpara.Add(iecPn);
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
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(SmallPartsLabelPrintImpl) Print end, iecPn:" + iecPn + " , qty:" + qty + " , editor:" + editor + " , line:" + line + " , station:" + station + " , customer:" + customer);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        void ISmallPartsLabelPrint.Cancel(string iecPn)
        {
            string sessionKey = iecPn;
            try
            {
                logger.Debug("(SmallPartsLabelPrintImpl) Cancel start, sessionKey:" + sessionKey);

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
                logger.Debug("(SmallPartsLabelPrintImpl) Cancel end, sessionKey:" + sessionKey);
            }
        }

        #endregion

  
    }
}
