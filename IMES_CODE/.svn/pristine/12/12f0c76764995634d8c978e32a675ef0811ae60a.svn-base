/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Service for LCM Repair Page
 *             
 * UI:CI-MES12-SPEC-FA-UI RCTO LCM Repair.docx
 * UC:CI-MES12-SPEC-FA-UC RCTO LCM Repair.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-08-16  itc202017             (Reference Ebook SourceCode) Create
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
using IMES.FisObject.FA.Product;
using IMES.DataModel;
using log4net;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// ILCMRepair接口的实现类
    /// </summary>
    public class _LCMRepairImpl : MarshalByRefObject, ILCMRepair 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        private const Session.SessionType SessionType = Session.SessionType.Product;

        #region ILCMRepair members

        /// <summary>
        /// 取得界面显示数据
        /// </summary>
        public void InputCTNo(
            string ctno,
            string editor,
            string station,
            string customer,
            out string id,
            out string model,
            out string tStation,
            out string line)
        {
            id = "";
            model = "";
            tStation = "";
            line = "";

            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                logger.Debug("(LCMRepairImpl)InputCTNo start, CTNO:" + ctno);
                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IList<ProductPart> partList = productRepository.GetProductPartsByPartSn(ctno);
                foreach (ProductPart ele in partList)
                {
                    if (ele.CheckItemType == "LCM")
                    {
                        id = ele.ProductID;
                    }
                }

                if (id == "")
                {
                    erpara.Add(ctno);
                    ex = new FisException("CHK940", erpara);    //"错误的CT No.:%1"
                    throw ex;
                }

                string sessionKey = id;

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
                    RouteManagementUtils.GetWorkflow(station, "LCMRepair.xoml", "LCMRepair.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue("CTNO", ctno);
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
                
                IProduct curProduct = session.GetValue(Session.SessionKeys.Product) as IProduct;
                model = curProduct.Model;
                tStation = curProduct.Status.StationId;
                line = curProduct.Status.Line;
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
                logger.Debug("(LCMRepairImpl)InputCTNo end, CTNO:" + ctno);
            }
        }

        /// <summary>
        /// 取得界面显示数据
        /// </summary>
        public IList<RepairInfo> GetLCMRepairList(string id)
        {
            try
            {
                IDefectRepository defectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository, Defect>();
                IDefectInfoRepository defectInfoRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();
                IList<RepairInfo> ret = partRepository.GetKPRepairDefectList(id);
                foreach (RepairInfo ele in ret)
                {
                    var info = defectRepository.GetDefectInfo(ele.defectCodeID);
                    ele.defectCodeDesc = info.description;
                    var infoLst = defectInfoRepository.GetDefectInfoByTypeAndCode("FACause", ele.cause);
                    if (infoLst.Count > 0)
                    {
                        ele.causeDesc = infoLst[0].description;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改指定的LCM 维修记录
        /// </summary>
        public int Edit(string id, RepairInfo repInfo)
        {
            logger.Debug("(LCMRepairImpl)Edit start");
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = id;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                RepairDefect defect = GetLCMRepairDefect(repInfo);

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
                logger.Debug("(LCMRepairImpl)Edit end");
            }
        }

        /// <summary>
        /// 添加 LCM 维修记录
        /// </summary>
        public int Add(string id, RepairInfo repInfo)
        {
            logger.Debug("(LCMRepairImpl)Add start");
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = id;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                RepairDefect defect = GetLCMRepairDefect(repInfo);

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
                logger.Debug("(LCMRepairImpl)Add end");
            }
        }

        /// <summary>
        /// 已经完成对LCM 的维修
        /// </summary>
        public void Save(string id)
        {
            logger.Debug("(LCMRepairImpl)Save start");
            List<string> erpara = new List<string>();
            string sessionKey = id;

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
                logger.Debug("(LCMRepairImpl)Save end");
            }            
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string id)
        {
            logger.Debug("(LCMRepairImpl)Cancel start");
            string sessionKey = id;

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
                logger.Debug("(LCMRepairImpl)Cancel end");
            } 
        }

        #endregion

        private RepairDefect GetLCMRepairDefect(RepairInfo repairLogInfo)
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
