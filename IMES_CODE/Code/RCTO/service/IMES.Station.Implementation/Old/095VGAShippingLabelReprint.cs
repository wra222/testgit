//INVENTEC corporation (c)2009 all rights reserved. 
//Description:  VGAShippingLabelReprint bll
//                        
//Update: 
//Date         Name                         Reason 
//==========   =======================      ============
//2010-01-04   Yuan XiaoWei                 create
//Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using log4net;
using IMES.FisObject.Common.HDDCopyInfo;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Route;
namespace IMES.Station.Implementation
{
    public class VGAShippingLabelReprint : MarshalByRefObject, IVGAShippingLabelReprint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Common;

        #region IVGAShippingLabelReprint Members

        SVBInfo IVGAShippingLabelReprint.CheckSVB(int dcode, string version, string svbsno, string editor, string line, string station, string customer)
        {
            logger.Debug(" CheckSVB start, svbsno:" + svbsno);
            try
            {
                Session currentSVBSession = SessionManager.GetInstance.GetSession(svbsno, currentSessionType);

                if (currentSVBSession == null)
                {
                    currentSVBSession = new Session(svbsno, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", svbsno);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", currentSVBSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "095VGALabelReprint.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSVBSession.AddValue(Session.SessionKeys.SelectedWarrantyRuleID, dcode);
                    currentSVBSession.AddValue(Session.SessionKeys.Version, version);
                    currentSVBSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSVBSession))
                    {
                        currentSVBSession.WorkflowInstance.Terminate("Session:" + svbsno + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(svbsno);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSVBSession.WorkflowInstance.Start();
                    currentSVBSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(svbsno);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                if (currentSVBSession.Exception != null)
                {
                    if (currentSVBSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSVBSession.ResumeWorkFlow();
                    }

                    throw currentSVBSession.Exception;
                }
                SVBInfo result = new SVBInfo();
                result.FruNo = (string)currentSVBSession.GetValue(Session.SessionKeys.FRUNO);
                result.PartNo = (string)currentSVBSession.GetValue(Session.SessionKeys.PN111);
                return result;

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
                logger.Debug(" CheckSVB end, svbsno:" + svbsno);
            }
        }

        IList<PrintItem> IVGAShippingLabelReprint.Print(string svbsno, IList<PrintItem> printItems, string reason, out string dcode)
        {
            logger.Debug("Print start, svbsno:" + svbsno);

            try
            {
                Session currentSVBSession = SessionManager.GetInstance.GetSession(svbsno, currentSessionType);

                if (currentSVBSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(svbsno);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    currentSVBSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSVBSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSVBSession.Exception = null;
                    currentSVBSession.SwitchToWorkFlow();

                    if (currentSVBSession.Exception != null)
                    {
                        if (currentSVBSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            currentSVBSession.ResumeWorkFlow();
                        }
                        throw currentSVBSession.Exception;
                    }

                    dcode = (string)currentSVBSession.GetValue(Session.SessionKeys.DCode);
                    return (IList<PrintItem>)currentSVBSession.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug("Print end, svbsno:" + svbsno);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="svbsno"></param>
        void IVGAShippingLabelReprint.Cancel(string svbsno)
        {
            try
            {
                logger.Debug("Cancel start, svbsno:" + svbsno);

                Session currentSVBSession = SessionManager.GetInstance.GetSession(svbsno, currentSessionType);

                if (currentSVBSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSVBSession);
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
                logger.Debug("Cancel end, svbsno:" + svbsno);
            }
        }
        #endregion
    }
}
