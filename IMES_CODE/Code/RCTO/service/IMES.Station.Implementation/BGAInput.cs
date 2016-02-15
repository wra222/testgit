// INVENTEC corporation ©2011 all rights reserved. 
// Description:PAQC Input 
// UI:CI-MES12-SPEC-SA-UC BGA Input.docx –2012/1/04 
// UC:CI-MES12-SPEC-SA-UC BGA Input.docx –2012/1/04                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-1-04   liuqingbiao                  Create 
// Known issues:
// TODO：

using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.DataModel;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// BGAOutput接口的实现类
    /// </summary>
    public class BGAInput : MarshalByRefObject, IBGAInput 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.MB;

        #region members

        /// <summary>
        /// 刷MBSno，启动工作流，检查输入的Sno，卡站，获取RepairList
        /// </summary>
        /// <param name="sno"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList InputSno(string sno, string line, string editor, string station, string customer)
        {
            logger.Debug("(BGAInput)InputSno start, MBSno:" + sno);
            try
            {
                ArrayList retList = new ArrayList();
                string sessionKey = sno;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "BGAInput.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    //currentSession.AddValue(Session.SessionKeys.IsComplete, false);
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
                //==============================================================================

                //Get [Repair List] By [MBSno]
                //select SnoId,Remark,Username,Cdt,Udt from rpt_PCARep nolock 
                //where Tp='BGA' and SnoId=@MBSNo order by Cdt

                IMB curMb = (IMB)currentSession.GetValue(Session.SessionKeys.MB);
                IList<MBRptRepair> repairList = curMb.MBRptRepairs;
                //IList<RptPcaRepInfo> infoList = new List<RptPcaRepInfo>();
                IList<Rework> infoList = new List<Rework>();
#if false
                for (int i = 0; i < 20; i++)
                {
                        Rework info = new Rework() ;
                        info.ReworkCode = "abcd";
                        info.Cdt = DateTime.Now;
                        info.Udt = DateTime.UtcNow;
                        infoList.Add(info);
                }
#else
                foreach (MBRptRepair node in repairList)
                {
                    if (node.Tp == "BGA")
                    {
                        Rework info = new Rework() ;
                        info.ReworkCode = node.Remark;
                        info.Cdt = node.Cdt;
                        info.Udt = node.Cdt;
                        infoList.Add(info);
                    }
                }
#endif

                retList.Add(sno);
                retList.Add(infoList);
                //===============================================================================
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
                logger.Debug("(BGAInput)InputSno end, MBSno:" + sno);
            }
        }

        /// <summary>
        /// 显示过 list 到页面后，调用此过程返回到 workflow，存数据
        /// </summary>
        /// <param name="snoScaned"></param>
        public void save(string snoScaned)//, IList<string> defectCodeList)
        {
            logger.Debug("(BGAInput)save start,"
                + " [snoScaned]: " + snoScaned);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = snoScaned;
#if false
            erpara.Add(sessionKey);
            ex = new FisException("CHK021", erpara);
            //ex.logErr("", "", "", "", "83");
            //logger.Error(ex);
            throw ex;
#else
            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                else
                {
                    //session.AddValue(Session.SessionKeys.DefectList, defectCodeList);
                    //session.AddValue(Session.SessionKeys.HasDefect, (defectCodeList != null && defectCodeList.Count != 0) ? true : false);
                    session.Exception = null;
                    session.SwitchToWorkFlow();

                    //check workflow exception
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
                logger.Debug("(BGAInput)save end,"
                   + " [snoScaned]: " + snoScaned);
            }
#endif
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sno"></param>
        public void cancel(string sno)
        {
            logger.Debug("(BGAInput)Cancel start, [MbSno]:" + sno);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = sno;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
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
                logger.Debug("(BGAInput)Cancel end, [MbSno]:" + sno);
            } 
        }

        #endregion
    }
}
