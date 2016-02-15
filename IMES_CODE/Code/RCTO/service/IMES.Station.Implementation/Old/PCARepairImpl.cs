/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   Tong.Zhi-Yong     Create 
 * 2010-01-08   Tong.Zhi-Yong     Modify ITC-1103-0075
 * 2010-01-31   Tong.Zhi-Yong     Modify ITC-1103-0143
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.DataModel;
using log4net;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IPCARepair接口的实现类
    /// </summary>
    public class PCARepairImpl : MarshalByRefObject, IPCARepair 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType MBSessionType = Session.SessionType.MB;

        #region IPCARepair members

        /// <summary>
        /// 1.1	UC-PCA-PCAR-01 Query
        /// 查询并显示MB 当前维修记录
        /// 查询成功后调用getMBInfo和getRepairLogList来取得查询结果
        /// </summary>
        /// <param name="MB_SNo"></param>
        /// <param name="pdLine"></param>
        /// <param name="editor">operator</param>
        public void Query(
            string MB_SNo,
            string pdLine,
            string editor,
            string stationId, string customer)
        {
            logger.Debug("(PCARepairImpl)Query start, MB_SNo:" + MB_SNo);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = MB_SNo;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, MBSessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, MBSessionType, editor, stationId, pdLine,customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);

                    //ITC-1103-0075 Tong.Zhi-Yong 2010-01-08
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", MBSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "PCARepair.xoml", "PCARepair.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.IsComplete, false);
                    session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(session))
                    {
                        session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    
                    session.WorkflowInstance.Start();
                    session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //session.SwitchToWorkFlow();

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
                logger.Debug("(PCARepairImpl)Query end, MB_SNo:" + MB_SNo);
            }
        }

        /// <summary>
        /// 1.2	UC-PCA-PCAR-02 Edit
        /// 修改指定的MB 维修记录
        /// </summary>
        /// <param name="log">被修改的RepairLogInfo</param>
        /// <param name="editor">operator</param>
        /// <returns>已维修的次数</returns>
        public int Edit(
            string MB_SNo,
            RepairInfo log,
            string editor,
            string stationId,
            string customer)
        {
            logger.Debug("(PCARepairImpl)Edit start, MB_SNo:" + MB_SNo);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = MB_SNo;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, MBSessionType);

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
                    RepairDefect defect = getMBRepairDefect(log);

                    session.AddValue(Session.SessionKeys.CurrentRepairdefect, defect);
                    session.AddValue(Session.SessionKeys.MaintainAction, 1);

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

                    return (int) session.GetValue(Session.SessionKeys.RepairTimes);
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
                logger.Debug("(PCARepairImpl)Edit end, MB_SNo:" + MB_SNo);
            }
        }

        /// <summary>
        /// 1.3	UC-PCA-PCAR-03 Add
        /// 添加 MB 维修记录
        /// </summary>
        /// <param name="log">待添加的RepairLogInfo</param>
        /// <param name="editor">operator</param>
        /// <returns>已维修的次数</returns>
        public int Add(
            string MB_SNo,
            RepairInfo log,
            string editor,
            string stationId,
            string customer)
        {
            logger.Debug("(PCARepairImpl)Add start, MB_SNo:" + MB_SNo);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = MB_SNo;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, MBSessionType);

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
                    RepairDefect defect = getMBRepairDefect(log);

                    session.AddValue(Session.SessionKeys.CurrentRepairdefect, defect);
                    session.AddValue(Session.SessionKeys.MaintainAction, 0);

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

                    return (int)session.GetValue(Session.SessionKeys.RepairTimes);
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
                logger.Debug("(PCARepairImpl)Add end, MB_SNo:" + MB_SNo);
            }
        }

        /// <summary>
        /// 1.4	UC-PCA-PCAR-04 Delete
        /// 删除指定的MB 维修记录
        /// </summary>
        /// <param name="repairLogId">待删除的RepairLogInfo</param>
        /// <param name="editor">operator</param>
        public void Delete(
            string MB_SNo,
            string repairLogId,
            string editor,
            string stationId,
            string customer)
        {
            //FisException ex;
            //List<string> erpara = new List<string>();
            //string sessionKey = MB_SNo;

            //try
            //{
            //    Session session = SessionManager.GetInstance.GetSession(sessionKey, MBSessionType);

            //    if (session == null)
            //    {
            //        erpara.Add(sessionKey);
            //        ex = new FisException("CHK021", erpara);
            //        //ex.logErr("", "", "", "", "83");
            //        //logger.Error(ex);
            //        throw ex;
            //    }
            //    else
            //    {
            //        RepairDefect defect = getMBRepairDefect(log);

            //        session.AddValue(Session.SessionKeys.RepairDefectID, repairLogId);
            //        session.AddValue(Session.SessionKeys.MaintainAction, "2");

            //        session.Exception = null;
            //        session.SwitchToWorkFlow();

            //        //check workflow exception
            //        if (session.Exception != null)
            //        {
            //            if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
            //            {
            //                session.ResumeWorkFlow();
            //            }

            //            throw session.Exception;
            //        }
            //    }
            //}
            //catch (FisException e)
            //{
            //    throw e;
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
            //finally
            //{ }
        }

        /// <summary>
        /// 1.5	UC-PCA-PCAR-05 Save
        /// 已经完成对MB 的维修
        /// </summary>
        public void Save(string MB_SNo)
        {
            logger.Debug("(PCARepairInputImpl)Save start, MB_SNo:" + MB_SNo);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = MB_SNo;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, MBSessionType);

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
                logger.Debug("(PCARepairInputImpl)Save end, MB_SNo:" + MB_SNo);
            }            
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string MB_SNo)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = MB_SNo;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, MBSessionType);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
                }
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            { } 
        }

        #endregion

        private RepairDefect getMBRepairDefect(RepairInfo repairLogInfo)
        {
            RepairDefect defect = new RepairDefect();

            defect.ID = string.IsNullOrEmpty(repairLogInfo.id) ? 0 : int.Parse(repairLogInfo.id);
            defect._4M = repairLogInfo._4M;
            defect.Action = repairLogInfo.action;
            defect.Cause = repairLogInfo.cause;
            defect.Cdt = repairLogInfo.cdt;
            defect.Component = repairLogInfo.component;
            defect.Cover = repairLogInfo.cover;
            defect.DefectCodeID = repairLogInfo.defectCodeID;
            defect.Distribution = repairLogInfo.distribution;
            defect.Editor = repairLogInfo.editor;
            defect.IsManual = (!string.IsNullOrEmpty(repairLogInfo.isManual) && repairLogInfo.isManual.Equals("1")) ? true : false;
            defect.MajorPart = repairLogInfo.majorPart;
            defect.Manufacture = repairLogInfo.manufacture;
            defect.Mark = repairLogInfo.mark;
            defect.NewPart = repairLogInfo.newPart;
            defect.NewPartSno = repairLogInfo.newPartSno;
            defect.Obligation = repairLogInfo.obligation;
            defect.OldPart = repairLogInfo.oldPart;
            defect.OldPartSno = repairLogInfo.oldPartSno;
            defect.PartType = repairLogInfo.partType;
            defect.PIAStation = repairLogInfo.piaStation;
            defect.Remark = repairLogInfo.remark;
            defect.RepairID = string.IsNullOrEmpty(repairLogInfo.repairID) ? 0 : int.Parse(repairLogInfo.repairID);
            defect.Responsibility = repairLogInfo.responsibility;
            defect.ReturnSign = repairLogInfo.returnSign;
            defect.Site = repairLogInfo.site;
            defect.SubDefect = repairLogInfo.subDefect;
            defect.TrackingStatus = repairLogInfo.trackingStatus;
            defect.Type = repairLogInfo.type;
            defect.Udt = repairLogInfo.udt;
            defect.Uncover = repairLogInfo.uncover;
            defect.VendorCT = repairLogInfo.vendorCT;
            defect.VersionA = repairLogInfo.versionA;
            defect.VersionB = repairLogInfo.versionB;
            defect.NewPartDateCode = repairLogInfo.newPartDateCode;

            //ITC-1103-0143 Tong.Zhi-Yong 2010-01-31
            defect.Location = repairLogInfo.component + repairLogInfo.site;
            defect.Side = repairLogInfo.side;

            return defect;
        }
    }
}
