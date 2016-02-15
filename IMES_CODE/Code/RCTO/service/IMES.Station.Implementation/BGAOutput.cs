// INVENTEC corporation ©2011 all rights reserved. 
// Description:PAQC Output 
// UI:CI-MES12-SPEC-SA-UC BGA Output.docx –2012/1/04 
// UC:CI-MES12-SPEC-SA-UC BGA Output.docx –2012/1/04                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-1-04   Du.Xuan                      Create 
// Known issues:
// ITC-1360-0176 rework插入rpt_PCARep表status应该为1
// ITC-1360-0191 修改status和log状态为pass
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
using IMES.FisObject.PCA.MB;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// BGAOutput接口的实现类
    /// </summary>
    public class BGAOutputImpl : MarshalByRefObject, BGAOutput 
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
            logger.Debug("(BGAOutputImpl)InputSno start, MBSno:" + sno);
            
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
                    RouteManagementUtils.GetWorkflow(station, "BGAOutput.xoml", "BGAOutput.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

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
                //==============================================================================
                
                //Get [Repair List] By [MBSno]
                //select SnoId,Remark,Username,Cdt,Udt from rpt_PCARep nolock 
                //where Tp='BGA' and SnoId=@MBSNo order by Cdt
              
                IMB curMb = (IMB)currentSession.GetValue(Session.SessionKeys.MB);
                IList<MBRptRepair>  repairList = curMb.MBRptRepairs;
                IList<Rework> infoList = new List<Rework>();
                //DEBUG ITC-1360-0736
                //order by Cdt
                var repairListOrderbyCdt =
                            from item in repairList
                            orderby item.Cdt 
                            select item;


                foreach (MBRptRepair tmpNode in repairListOrderbyCdt)
                {   
                    if (tmpNode.Tp == "BGA")
                    {
                        Rework info = new Rework() ;
                        info.ReworkCode = tmpNode.Remark;
                        info.Cdt = tmpNode.Cdt;
                        //DEBUG ITC-1360-0736
                        //info.Udt = tmpNode.Cdt;
                        info.Udt = tmpNode.Udt;
                        infoList.Add(info);
                    }
                }

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
                logger.Debug("(BGAOutputImpl)InputSno end, MBSno:" + sno);
            }
        }

        /// <summary>
        /// 检查rpt_PCARep是否存在同样的维修记录 ,Save Repair Item 
        /// </summary>
        /// <param name="sno"></param>
        /// <param name="reworkStation"></param>
        /// <returns></returns>
        public Rework addNew(string sno, string reworkStation)
        {
            logger.Debug("(BGAOutputImpl)addNew start, [MbSno]:" + sno + "[Station]:" + reworkStation);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = sno;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    session.AddValue(Session.SessionKeys.Remark, reworkStation);
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
                //===========================================================================
                MBRptRepair newMb = (MBRptRepair)session.GetValue(Session.SessionKeys.NewMB);
                Rework rework = new Rework();

                rework.ReworkCode = newMb.Remark;
                rework.Cdt = newMb.Cdt;
                rework.Udt = newMb.Udt;
                rework.Status = "1";

                return rework;

                //===========================================================================
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
                logger.Debug("(BGAOutputImpl)Cancel end, [MbSno]:" + sno + "[Station]:" + reworkStation);
            } 
        }

        /// <summary>
        /// 扫描9999，结束工作流
        /// </summary>
        /// <param name="mbSno"></param>
        public void save(string mbSno)
        {
            logger.Debug("(BGAOutputImpl)save start, [MBSno]: " + mbSno);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = mbSno;

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
                    session.AddValue(Session.SessionKeys.IsComplete, true);
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
                logger.Debug("(BGAOutputImpl)save end, [MBSno]: " + mbSno);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sno"></param>
        public void cancel(string sno)
        {
            logger.Debug("(BGAOutputImpl)Cancel start, [MbSno]:" + sno);
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
                logger.Debug("(BGAOutputImpl)Cancel end, [MbSno]:" + sno);
            } 
        }

        #endregion
    }
}
