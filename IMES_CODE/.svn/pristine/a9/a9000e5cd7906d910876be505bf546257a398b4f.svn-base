/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description: PCA OQC Output
 * UI:CI-MES12-SPEC-SA-UI PCA OQC Output.docx 
 * UC:CI-MES12-SPEC-SA-UC PCA OQC Output.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-13   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1.	记录SAOQC 结果，若有不良，则记录不良信息
 *               
 * UC Revision:  3382
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
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Line;



namespace IMES.Station.Implementation
{
    /// <summary>
    /// PCA OQC Output
    /// </summary> 

    public class PCAOQCOutput : MarshalByRefObject, IPCAOQCOutput
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        #region IPCAOQCOutput Members

        /// <summary>
        /// 刷mbsno，调用该方法启动工作流，根据输入mbsno获取model
        /// 返回model
        /// </summary>
        /// <param name="mbsno">mbsno</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="curMBInfo">curMBInfo</param>
        /// <returns>model</returns>
        public string inputMBSno(string mbsno, string editor, string station, string customer, out IMES.DataModel.MBInfo curMBInfo)
        {
            logger.Debug("(PCAOQCOutputImpl)InputCustSNOnCooLabel start, mbsno:" + mbsno + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retLst = new ArrayList();

            try
            {
                string pdline = null;

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
                    RouteManagementUtils.GetWorkflow(station, "PCAOQCOutput.xoml", "PCAOQCOutput.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.MBSN, mbsno);
                    
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

               string line = string.Empty;
               MB currentMB = (MB)currentSession.GetValue(Session.SessionKeys.MB);
               if (string.IsNullOrEmpty(currentMB.MBStatus.Line))
               {
                   erpara.Add(sessionKey);
                   ex = new FisException("PAK124", erpara); //该PCB %1 没有PdLine，请确认！
                   throw ex;
               }

               line = currentMB.MBStatus.Line;
               
               ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
               Line lineInfo = lineRepository.Find(line);
               if (lineInfo == null)
               {
                   erpara.Add(sessionKey);
                   ex = new FisException("PAK124", erpara); //该PCB %1 没有PdLine，请确认！
                   throw ex;
               }

               curMBInfo = new IMES.DataModel.MBInfo();
               curMBInfo.line = line;
               curMBInfo.lineDesc = lineInfo.Descr;

               string model = string.Empty;
               model = (string)currentSession.GetValue(Session.SessionKeys.PCBModelID);

               return model;
                       

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
                logger.Debug("(PCAOQCOutputImpl)InputCustSNOnCooLabel end, mbsno:" + mbsno + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }

        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="mbsno">mbsno</param>
        /// <param name="defectCodeList">defectCodeList</param>
        public void save(string mbsno, IList<string> defectCodeList)
        {
            logger.Debug("(PCAOQCOutputImpl)save start,"
                + " mbsno: " + mbsno
                + " defectList:" + defectCodeList);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = mbsno;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    session.AddValue(Session.SessionKeys.DefectList, defectCodeList);
                    session.AddValue(Session.SessionKeys.HasDefect, (defectCodeList != null && defectCodeList.Count != 0) ? false : true);
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
                logger.Debug("(PCAOQCOutputImpl)save end,"
                   + " mbsno: " + mbsno
                   + " defectList:" + defectCodeList);
            }
        }
        

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn">mbsno</param>
        public void Cancel(string mbsno)
        {
            logger.Debug("(PCAOQCOutputImpl)Cancel Start," + "mbsno:" + mbsno);
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
                logger.Debug("(PCAOQCOutputImpl)Cancel End," + "mbsno:" + mbsno);
            }

        }


        #endregion

   
    }

    
}
