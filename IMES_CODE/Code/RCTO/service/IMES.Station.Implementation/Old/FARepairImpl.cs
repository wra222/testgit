/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: FARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-24   Tong.Zhi-Yong     Create 
 * 
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
    /// IFARepair接口的实现类
    /// </summary>
    public class FARepairImpl : MarshalByRefObject, IFARepair 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region IFARepair members

        /// <summary>
        /// 输入Product Id和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>Repair Logs</returns>
        public IList<RepairInfo> InputProdId(string pdLine, string prodId, string editor, string stationId, string customer)
        {
            logger.Debug("(FARepairImpl)InputProdId start, [pdLine]:" + pdLine
                + " [prodId]: " + prodId
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, ProductSessionType, editor, stationId, pdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "024FARepair.xoml", "024FARepair.rules", out wfName, out rlName);
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

                return null;
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
                logger.Debug("(FARepairImpl)InputProdId end, [pdLine]:" + pdLine
                    + " [prodId]: " + prodId
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// Edit Repair logs
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="rll">改变的Repair log</param>
        public void Edit(string prodId, RepairInfo rll)
        {
            logger.Debug("(FARepairImpl)Edit start, [prodId]:" + prodId
                + " [rll]: " + rll);                                                
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

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
                    RepairDefect defect = getFARepairDefect(rll);

                    session.AddValue(Session.SessionKeys.CurrentRepairdefect, defect);
                    session.AddValue(Session.SessionKeys.MaintainAction, 1);
                    //session.AddValue(Session.SessionKeys.IsComplete, false);

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
                logger.Debug("(FARepairImpl)Edit end, [prodId]:" + prodId
                    + " [rll]: " + rll);
            }
        }

        /// <summary>
        /// Add Repair logs
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="rll">新增的Repair Log</param>
        public void Add(string prodId, RepairInfo rll)
        {
            logger.Debug("(FARepairImpl)Add start, [prodId]:" + prodId
                + " [rll]: " + rll);  
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

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
                    RepairDefect defect = getFARepairDefect(rll);

                    session.AddValue(Session.SessionKeys.CurrentRepairdefect, defect);
                    session.AddValue(Session.SessionKeys.MaintainAction, 0);
                   // session.AddValue(Session.SessionKeys.IsComplete, false);

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
                logger.Debug("(FARepairImpl)Add end, [prodId]:" + prodId
                    + " [rll]: " + rll);            
            }
        }

        /// <summary>
        /// Delete Repair logs
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="rll">删除的Repair log</param>
        public void Delete(string prodId, RepairInfo rll)
        {
            //FisException ex;
            //List<string> erpara = new List<string>();
            //string sessionKey = prodId;

            //try
            //{
            //    Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

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
            //        RepairDefect defect = getMBRepairDefect(rll);

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
        /// 完成维修并保存
        /// </summary>
        /// <param name="prodId">Product Id</param>
        public void Save(string prodId, string testStation)
        {
            logger.Debug("(FARepairImpl)Save start, [prodId]:" + prodId
                + " [testStation]: " + testStation); 
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

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
                    session.AddValue(Session.SessionKeys.ReturnStation, testStation);

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
                logger.Debug("(FARepairImpl)Save end, [prodId]:" + prodId
                    + " [testStation]: " + testStation);             
            } 
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string prodId)
        {
            logger.Debug("(FARepairImpl)Cancel start, [prodId]:" + prodId);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

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
                logger.Debug("(FARepairImpl)Cancel end, [prodId]:" + prodId);
            } 
        }

        #endregion

        private RepairDefect getFARepairDefect(RepairInfo repairLogInfo)
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

            return defect;
        }
    }
}
