/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Service for KP Repair Page
 *             
 * UI:CI-MES12-SPEC-FA-UI KeyParts Repair.docx
 * UC:CI-MES12-SPEC-FA-UC KeyParts Repair.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-26  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
using System;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using log4net;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IKPRepair接口的实现类
    /// </summary>
    public class _KPRepairImpl : MarshalByRefObject, IKPRepair 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        private const Session.SessionType SessionType = Session.SessionType.Common;

        #region IKPRepair members

        /// <summary>
        /// 取得界面显示数据
        /// </summary>
        public void InputCTNo(
            string ctno,
            string line,
            string editor,
            string station, 
            string customer)
        {
            logger.Debug("(KPRepairImpl)InputCTNo start, CTNO:" + ctno);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = ctno;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, SessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "KPRepair.xoml", "KPRepair.rules", out wfName, out rlName);
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
                logger.Debug("(KPRepairImpl)InputCTNo end, CTNO:" + ctno);
            }
        }

        /// <summary>
        /// 取得界面显示数据
        /// </summary>
        public IList<RepairInfo> GetKPRepairList(string ctno)
        {
            try
            {
                return partRepository.GetKPRepairDefectList(ctno);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改指定的KP 维修记录
        /// </summary>
        public int Edit(string ctno, RepairInfo repInfo)
        {
            logger.Debug("(KPRepairImpl)Edit start, CTNO:" + ctno);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = ctno;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                RepairDefect defect = GetKPRepairDefect(repInfo);

                session.AddValue(Session.SessionKeys.CurrentRepairdefect, defect);
                session.AddValue("isAdd", false);

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
                logger.Debug("(KPRepairImpl)Edit end, CTNO:" + ctno);
            }
        }

        /// <summary>
        /// 添加 KP 维修记录
        /// </summary>
        public int Add(string ctno, RepairInfo repInfo)
        {
            logger.Debug("(KPRepairImpl)Add start, CTNO:" + ctno);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = ctno;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                RepairDefect defect = GetKPRepairDefect(repInfo);

                session.AddValue(Session.SessionKeys.CurrentRepairdefect, defect);
                session.AddValue("isAdd", true);

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
                logger.Debug("(KPRepairImpl)Add end, CTNO:" + ctno);
            }
        }

        /// <summary>
        /// 已经完成对KP 的维修
        /// </summary>
        public void Save(string ctno)
        {
            logger.Debug("(KPRepairImpl)Save start, CTNO:" + ctno);
            List<string> erpara = new List<string>();
            string sessionKey = ctno;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    var ex = new FisException("CHK021", erpara);
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
                logger.Debug("(KPRepairImpl)Save end, CTNO:" + ctno);
            }            
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string ctno)
        {
            logger.Debug("(KPRepairImpl)Cancel start, CTNO:" + ctno);
            string sessionKey = ctno;

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
                throw;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(KPRepairImpl)Cancel end, CTNO:" + ctno);
            } 
        }

        #endregion

        private RepairDefect GetKPRepairDefect(RepairInfo repairLogInfo)
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

            defect.Location = (repairLogInfo.majorPart + "   ").Substring(0, 3)
                + (repairLogInfo.component + "  ").Substring(0, 2)
                + repairLogInfo.site.TrimEnd();
            defect.Side = repairLogInfo.side;

            return defect;
        }
    }
}
