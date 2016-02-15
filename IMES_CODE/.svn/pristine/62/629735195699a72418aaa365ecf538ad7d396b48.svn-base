/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-01-10   Yang.Wei-Hua     Create 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.Common.Repair;
using IMES.DataModel;
using log4net;
using IMES.Route;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.FisObjectRepositoryFramework;

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
        /// 成功后调用IMES.Station.Interface.CommonIntf.IMB.getMBInfo
        /// 和IMES.Station.Interface.CommonIntf.IRepair.GetMBRepairList来取得界面显示数据
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="line"></param>
        /// <param name="editor">operator</param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        public string InputMBSn(
            string mbSno,
            string line,
            string editor,
            string station, 
            string customer,
            out string secureOn)
        {
            logger.Debug("(PCARepairImpl)InputMBSn start, MB_SNo:" + mbSno);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = mbSno;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, MBSessionType);
                secureOn="";
                if (session == null)
                {
                    session = new Session(sessionKey, MBSessionType, editor, station, line,customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);

                    //ITC-1103-0075 Tong.Zhi-Yong 2010-01-08
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", MBSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "PCARepair.xoml", "PCARepair.rules", out wfName, out rlName);
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

                secureOn=(string)session.GetValue("TabletSecureOn");
                if (string.IsNullOrEmpty(secureOn))
                {
                    secureOn = "N";
                }
                return secureOn;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PCARepairImpl)InputMBSn end, MB_SNo:" + mbSno);
            }
        }

        /// <summary>
        /// Edit
        /// 修改指定的MB 维修记录
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="repInfo">被修改的RepairLogInfo</param>
        /// <returns>已维修的次数</returns>
        public int Edit(string mbSno, RepairInfo repInfo)
        {
            logger.Debug("(PCARepairImpl)Edit start, MB_SNo:" + mbSno);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = mbSno;
            
            try
            {
                string location = "MB " + repInfo.component;
                IMBRepository iMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                int qty = iMBRepository.GetMbRepairCountByLocationAndStation(mbSno, "15", location);
                if (qty >= 2)
                {
                    FisException e = new FisException("CQCHK1052", new string[] { });
                    e.stopWF = false;
                    throw e;
                }
                Session session = SessionManager.GetInstance.GetSession(sessionKey, MBSessionType);
                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                RepairDefect defect = GetMBRepairDefect(repInfo);

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
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PCARepairImpl)Edit end, MB_SNo:" + mbSno);
            }
        }

        /// <summary>
        /// 添加 MB 维修记录
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="repInfo">待添加的RepairLogInfo</param>
        /// <returns>已维修的次数</returns>
        public int Add(string mbSno, RepairInfo repInfo)
        {
            logger.Debug("(PCARepairImpl)Add start, MB_SNo:" + mbSno);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = mbSno;

            try
            {
                string location = "MB " + repInfo.component;
                IMBRepository iMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                int qty = iMBRepository.GetMbRepairCountByLocationAndStation(mbSno, "15", location);
                if (qty >= 2)
                {
                    FisException e = new FisException("CQCHK1052", new string[] { });
                    e.stopWF = false;
                    throw e;
                }
                Session session = SessionManager.GetInstance.GetSession(sessionKey, MBSessionType);
                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                RepairDefect defect = GetMBRepairDefect(repInfo);

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
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PCARepairImpl)Add end, MB_SNo:" + mbSno);
            }
        }

        /// <summary>
        /// Save
        /// 已经完成对MB 的维修
        /// </summary>
        public void Save(string mbSno)
        {
            logger.Debug("(PCARepairImpl)Save start, MB_SNo:" + mbSno);
            List<string> erpara = new List<string>();
            string sessionKey = mbSno;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, MBSessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    var ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
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
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PCARepairImpl)Save end, MB_SNo:" + mbSno);
            }            
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string mbSno)
        {
            logger.Debug("(PCARepairImpl)Cancel start, MB_SNo:" + mbSno);
            string sessionKey = mbSno;

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
                logger.Error(e.mErrmsg);
                throw;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PCARepairImpl)Cancel end, MB_SNo:" + mbSno);
            } 
        }

        #endregion

        private RepairDefect GetMBRepairDefect(RepairInfo repairLogInfo)
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
            defect.IsManual = (!string.IsNullOrEmpty(repairLogInfo.isManual) && repairLogInfo.isManual.Equals("1"));
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
            //defect.Location = repairLogInfo.component + repairLogInfo.site;
            defect.Location = (repairLogInfo.majorPart + "   ").Substring(0, 3)
                + (repairLogInfo.component + "  ").Substring(0, 2)
                + repairLogInfo.site.TrimEnd();
            defect.Side = repairLogInfo.side;

            return defect;
        }
    }
}
