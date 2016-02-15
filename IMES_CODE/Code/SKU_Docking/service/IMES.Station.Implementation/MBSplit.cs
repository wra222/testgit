/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:MB Split
 * UI:CI-MES12-SPEC-SA-UI MB Split.docx 
 * UC:CI-MES12-SPEC-SA-UC MB Split.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-13  Chen Xu               Create
 * 2012-01-13  Chen Xu               ITC-1360-0854
 * Known issues:
 * TODO：
 * UC 具体业务：  1.	连板切割入口，实现连板的切割
 *                2.	打印子板标签
 * UC Revision: 3924-> 4824
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
using IMES.FisObject.Common;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;
using IMES.Route;
using IMES.FisObject.PAK.DN;



namespace IMES.Station.Implementation
{
    /// <summary>
    /// MB Split
    /// Station ="09"
    /// </summary> 

    public class MBSplit : MarshalByRefObject, IMBSplit
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        #region IMBSplit Members

        /// <summary>
        ///刷mbsno，调用该方法启动工作流，根据输入mbsno获取model
        /// </summary>
        public IList<PrintItem> inputMBSno(string pdline, string mbsno, string editor, string station, string customer, IList<PrintItem> printItems, out string model,out IList<string> MBObjectList)
        {
            logger.Debug("(MBSplitImpl)InputCustSNOnCooLabel start, pdline:" + pdline + "mbsno:" + mbsno + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retLst = new ArrayList();

            try
            {
                string sessionKey = mbsno;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.MB, editor, station, pdline, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.MB);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "MBSplit.xoml", "mbsplit.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.MBSN, mbsno);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                                       
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
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


               model = (string)currentSession.GetValue(Session.SessionKeys.PCBModelID);

 
               MBObjectList = currentSession.GetValue(Session.SessionKeys.MBSNOList) as List<string>;

               
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
                logger.Debug("(MBSplitImpl)InputCustSNOnCooLabel end, pdline:" + pdline + "mbsno:" + mbsno + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="mbsno">mbsno</param>
        public void Cancel(string mbsno)
        {
            logger.Debug("(MBSplitImpl)Cancel Start," + "mbsno:" + mbsno);
            try
            {
                string sessionKey = mbsno;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB);

                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(MBSplitImpl)Cancel End," + "mbsno:" + mbsno);
            }

        }


        #endregion

        #region rePrint


        /// <summary>
        /// 重印
        /// </summary>
        /// <param name="mbsno">mbsno</param>
        /// <param name="reason">reason</param>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="printItems">printItems</param>
        public IList<PrintItem> rePrint(string mbsno, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(MBSplitImpl)rePrint Start," + "mbsno:" + mbsno + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer + "printItems" + printItems);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {

                string sessionKey = mbsno;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.MB, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.MB);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("MBSplitReprint.xoml", "", wfArguments);
                    currentSession.AddValue(Session.SessionKeys.MBSN, mbsno);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, "MBSplit");
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, mbsno);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, mbsno);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "MBSplitReprint");
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
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


                return (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(MBSplitImpl)rePrint End," + "mbsno:" + mbsno + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer + "printItems" + printItems);
            }

        }


        #endregion
    }

    
}
