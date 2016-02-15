/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: PAQC Repair
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-04-06   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure;
using System.Workflow.Runtime;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.Common.Repair;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IPAQCRepair接口的实现类
    /// </summary>
    public class PAQCRepairImpl : MarshalByRefObject, IPAQCRepair
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;

        #region IPAQCRepair members

        /// <summary>
        /// 刷uutSn，启动工作流，检查输入的uutSn，卡站，获取ProductModel
        /// </summary>
        /// <param name="prodIdOrSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>ProductModel</returns>
        public IMES.DataModel.ProductModel Input(string prodIdOrSN, string line, string editor, string station, string customer)
        {
            logger.Debug("(PAQCRepairImpl)Input start, [prodIdOrSN]:" + prodIdOrSN
                + " [line]: " + line
                + " [editor]:" + editor
                + " [station]: " + station
                + " [customer]: " + customer);

            try
            {
                var currentProduct = CommonImpl.GetProductByInput(prodIdOrSN, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                string sessionKey = currentProduct.ProId;
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
                    RouteManagementUtils.GetWorkflow(station, "042PAQCRepair.xoml", "042PAQCRepair.rules", out wfName, out rlName);
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

                IMES.DataModel.ProductModel currentModel = new IMES.DataModel.ProductModel();

                currentModel.CustSN = currentProduct.CUSTSN;
                currentModel.ProductID = currentProduct.ProId;
                currentModel.Model = currentProduct.Model;
                return currentModel;

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
                logger.Debug("(PAQCRepairImpl)Input end, [prodIdOrSN]:" + prodIdOrSN
                    + " [line]: " + line
                    + " [editor]:" + editor
                    + " [station]: " + station
                    + " [customer]: " + customer);
            }
        }

        /// <summary>
        /// 修改不良信息
        /// </summary>
        /// <param name="ProductID">Product Id</param>
        /// <param name="updateRepairDefect">改变的Repair Defect</param>
        public void Edit(string ProductID, IMES.DataModel.RepairInfo updateRepairDefect)
        {
            logger.Debug("(PAQCRepairImpl)Edit start, [ProductID]:" + ProductID
                + " [updateRepairDefect]: " + updateRepairDefect);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = ProductID;

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
                    RepairDefect defect = getPAQCRepairDefect(updateRepairDefect);

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
                logger.Debug("(PAQCRepairImpl)Edit end, [ProductID]:" + ProductID
                    + " [updateRepairDefect]: " + updateRepairDefect);
            }
        }

        /// <summary>
        /// 修改不良信息
        /// </summary>
        /// <param name="ProductID">Product Id</param>
        /// <param name="newRepairDefect">新增的Repair Logs</param>
        public void Add(string ProductID, IMES.DataModel.RepairInfo newRepairDefect)
        {
            logger.Debug("(PAQCRepairImpl)Add start, [ProductID]:" + ProductID
                + " [newRepairDefect]: " + newRepairDefect);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = ProductID;

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
                    RepairDefect defect = getPAQCRepairDefect(newRepairDefect);

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
                logger.Debug("(PAQCRepairImpl)Add end, [ProductID]:" + ProductID
                    + " [newRepairDefect]: " + newRepairDefect);
            }
        }

        /// <summary>
        /// 完成维修并保存
        /// </summary>
        /// <param name="ProductID">Product Id</param>
        public void Save(string ProductID, string returnStation)
        {
            logger.Debug("(PAQCRepairImpl)Save start, [ProductID]:" + ProductID
                + " [returnStation]: " + returnStation);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = ProductID;

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
                    session.AddValue(Session.SessionKeys.ReturnStation, returnStation);

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
                logger.Debug("(PAQCRepairImpl)Save end, [ProductID]:" + ProductID
                    + " [returnStation]: " + returnStation);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="ProductID"></param>
        public void cancel(string ProductID)
        {
            logger.Debug("(PAQCRepairImpl)Cancel start, [ProductID]:" + ProductID);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = ProductID;

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
                logger.Debug("(PAQCRepairImpl)Cancel end, [ProductID]:" + ProductID);
            } 
        }

        #endregion

        private RepairDefect getPAQCRepairDefect(IMES.DataModel.RepairInfo repairLogInfo)
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
