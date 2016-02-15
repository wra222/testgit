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
using System.Collections;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Docking.Interface.DockingIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.DataModel;
using log4net;
using IMES.Route;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;

namespace IMES.Docking.Implementation
{
    /// <summary>
    ///
    /// </summary>
    public class FARepairForDocking : MarshalByRefObject, IFARepairForDocking 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IFARepair members

        /// <summary>
        /// 输入Product Id和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>Repair Logs</returns>
        public IList<RepairInfo> InputProdId(string pdline, string prodId, string editor, string station, string customer)
        {
            logger.Debug("(FARepair)InputProdId start, [pdLine]:" + pdline
                + " [prodId]: " + prodId
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (session == null)
                {
                    session = new Session(sessionKey, Session.SessionType.Product, editor, station, pdline, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();                    
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "FARepairForDocking.xoml", "FARepairForDocking.rules", out wfName, out rlName);
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
                logger.Debug("(FARepair)InputProdId end, [pdLine]:" + pdline
                    + " [prodId]: " + prodId
                    + " [editor]:" + editor
                    + " [station]:" + station
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="preStn"></param>
        /// <param name="cause"></param>
        /// <returns></returns>
        public IList<DefectCodeStationInfo> GetReturnStation(int id, string code, string preStn, string cause)
        {
            var stationRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Station.IStationRepository, IMES.FisObject.Common.Station.IStation>();
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            DefectCodeStationInfo cond = new DefectCodeStationInfo();
            IList<DefectCodeStationInfo> info = null;
            cond.defect = code;
            cond.pre_stn = preStn;
            cond.crt_stn = "45";
            cond.cause = cause;
            info = stationRepository.GetDefectCodeStationList(cond);
            if (info != null && info.Count > 0)
            {
                //RepairInfo setValue = new RepairInfo();
                //RepairInfo c1 = new RepairInfo();

                //c1.Identity = id;
                //setValue.returnStation = info[0].nxt_stn;

                //productRepository.UpdateProductRepairDefectInfo(setValue, c1);
                return info;
            }
            else
            {
                DefectCodeStationInfo cond1 = new DefectCodeStationInfo();
                
                cond1.defect = code;
                cond1.pre_stn = preStn;
                cond1.crt_stn = "45";

                DefectCodeStationInfo cond2 = new DefectCodeStationInfo();
                cond2.cause = "";

                IList<DefectCodeStationInfo> info1 = null;
                info1 = stationRepository.GetDefectCodeStationList(cond1, cond2);

                if (info1 != null && info1.Count > 0)
                {
                    //RepairInfo setValue = new RepairInfo();
                    //RepairInfo c1 = new RepairInfo();
                    //c1.Identity = id;
                    //setValue.returnStation = info1[0].nxt_stn;
                    //productRepository.UpdateProductRepairDefectInfo(setValue, c1);
                    return info1;
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    ex = new FisException("CHK830", erpara);
                    throw ex;
                }
            }

            //return info;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="prodid"></param>
        /// <returns></returns>
        public IList<DefectCodeStationInfo> GetReturnStationForAdd(string code, string cause, string prodid)
        {
            var stationRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Station.IStationRepository, IMES.FisObject.Common.Station.IStation>();
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IProduct currentProduct = null;
            IList<DefectCodeStationInfo> info = null;

            currentProduct = productRepository.Find(prodid);
            if (currentProduct != null)
            {
                if (!String.IsNullOrEmpty(cause))
                {
                    DefectCodeStationInfo cond = new DefectCodeStationInfo();
                    cond.defect = code;
                    cond.pre_stn = currentProduct.Status.StationId;
                    cond.crt_stn = "45";
                    cond.cause = cause;
                    info = stationRepository.GetDefectCodeStationList(cond);
                }
                else
                {
                    DefectCodeStationInfo cond1 = new DefectCodeStationInfo();
                    cond1.defect = code;
                    cond1.pre_stn = currentProduct.Status.StationId;
                    cond1.crt_stn = "45";
                    DefectCodeStationInfo cond2 = new DefectCodeStationInfo();
                    cond2.cause = "";

                    info = stationRepository.GetDefectCodeStationList(cond1, cond2);
                }
            }

            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public IList<StationInfo> GetReturnStationList(string prodid, int status)
        {
            //IList<string> retList = null;
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
            //retList = productRepository.GetReturnStnListFromProductRepair(prodid, status);

            IList<StationInfo> retList = new List<StationInfo>();
            retList = productRepository.GetStationInfoListFromProductRepair(prodid, status);

            return retList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodid"></param>
        /// <returns></returns>
        public string GetProductMac(string prodid)
        {
            string mac = "";
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
            IProduct prod = productRepository.Find(prodid);
            if (prod != null)
            {
                mac = prod.MAC;
            }

            return mac;
        }

        /// <summary>
        /// Edit Repair logs
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="rll">改变的Repair log</param>
        public void Edit(string prodId, string macValue, RepairInfo rll, string repairStation)
        {
            logger.Debug("(FARepairImpl)Edit start, [prodId]:" + prodId
                + " [rll]: " + rll);                                                
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

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

                    ////Edit Check Return Station At Here!!!!!
                    IProduct productObj = (IProduct)session.GetValue(Session.SessionKeys.Product);
                    //IList<DefectCodeStationInfo> returnList = new List<DefectCodeStationInfo>();
                    //returnList = this.GetReturnStation(0, defect.DefectCodeID, productObj.Status.StationId, defect.Cause);
                    //defect.ReturnStation = returnList[0].nxt_stn;
                    ////Edit Check Return Station At Here!!!!!

                    #region get next station
                    var stationRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Station.IStationRepository, IMES.FisObject.Common.Station.IStation>();
                    //IList<DefectCodeNextStationInfo> nextStationList = stationRepository.GetNextStationFromDefectStation(productObj.Status.StationId,
                    //                                                                              repairStation,
                    //                                                                             string.IsNullOrEmpty(defect.MajorPart) ? string.Empty : defect.MajorPart.Trim(),
                    //                                                                             string.IsNullOrEmpty(defect.Cause) ? string.Empty : defect.Cause.Trim(),
                    //                                                                             string.IsNullOrEmpty(defect.DefectCodeID) ? string.Empty : defect.DefectCodeID.Trim());
                    IList<DefectCodeNextStationInfo> nextStationList = stationRepository.GetNextStationFromDefectStation(productObj.Status.StationId,
                                                                                                  repairStation,
                                                                                                  string.IsNullOrEmpty(defect.MajorPart) ? string.Empty : defect.MajorPart.Trim(),
                                                                                                  string.IsNullOrEmpty(defect.Cause) ? string.Empty : defect.Cause.Trim(),
                                                                                                  string.IsNullOrEmpty(defect.DefectCodeID) ? string.Empty : defect.DefectCodeID.Trim(),
                                                                                                  productObj.Family,
                                                                                                  productObj.Model);
                    if (nextStationList == null || nextStationList.Count == 0)
                    {
                        List<string> errpara = new List<string>();
                        FisException e = new FisException("CHK950", errpara);
                        throw e;
                    }

                    defect.ReturnStation = nextStationList[0].NXT_STN;
                    #endregion

                    defect.Location = (defect.MajorPart + "   ").Substring(0, 3)
                                + (defect.Component + "  ").Substring(0, 2)
                                    + defect.Site.TrimEnd();

                    session.AddValue(Session.SessionKeys.CurrentRepairdefect, defect);
                    session.AddValue(Session.SessionKeys.MaintainAction, 1);
                    session.AddValue(Session.SessionKeys.MAC, macValue);
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
        public void Add(string prodId, string macValue, RepairInfo rll, string repairStation)
        {
            logger.Debug("(FARepairImpl)Add start, [prodId]:" + prodId
                + " [rll]: " + rll);  
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    RepairDefect defect = getFARepairDefect(rll);

                    #region get next station
                    IProduct productObj = (IProduct)session.GetValue(Session.SessionKeys.Product);

                    var stationRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Station.IStationRepository, IMES.FisObject.Common.Station.IStation>();
                    //IList<DefectCodeNextStationInfo> nextStationList = stationRepository.GetNextStationFromDefectStation(productObj.Status.StationId,
                    //                                                                             repairStation,
                    //                                                                             string.IsNullOrEmpty(defect.MajorPart) ? string.Empty : defect.MajorPart.Trim(),
                    //                                                                             string.IsNullOrEmpty(defect.Cause) ? string.Empty : defect.Cause.Trim(),
                    //                                                                             string.IsNullOrEmpty(defect.DefectCodeID) ? string.Empty : defect.DefectCodeID.Trim());
                    IList<DefectCodeNextStationInfo> nextStationList = stationRepository.GetNextStationFromDefectStation(productObj.Status.StationId,
                                                                                                  repairStation,
                                                                                                  string.IsNullOrEmpty(defect.MajorPart) ? string.Empty : defect.MajorPart.Trim(),
                                                                                                  string.IsNullOrEmpty(defect.Cause) ? string.Empty : defect.Cause.Trim(),
                                                                                                  string.IsNullOrEmpty(defect.DefectCodeID) ? string.Empty : defect.DefectCodeID.Trim(),
                                                                                                  productObj.Family,
                                                                                                  productObj.Model);
                    if (nextStationList == null || nextStationList.Count == 0)
                    {
                        List<string> errpara = new List<string>();
                        FisException e = new FisException("CHK950", errpara);
                        throw e;
                    }

                    defect.ReturnStation = nextStationList[0].NXT_STN;
                    #endregion

                    defect.Location = (defect.MajorPart + "   ").Substring(0, 3)
                                + (defect.Component + "  ").Substring(0, 2)
                                    + defect.Site.TrimEnd();

                    session.AddValue(Session.SessionKeys.CurrentRepairdefect, defect);
                    session.AddValue(Session.SessionKeys.MaintainAction, 0);
                    session.AddValue(Session.SessionKeys.MAC, macValue);

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
        public ArrayList Save(string prodId, string testStation, string type)
        {
            logger.Debug("(FARepairImpl)Save start, [prodId]:" + prodId
                + " [testStation]: " + testStation); 
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            ArrayList ret = new ArrayList();

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

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
                    session.AddValue("ReturnProduct", type);

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
                var currentProduct = (IProduct)session.GetValue(Session.SessionKeys.Product);
                string isReturnProduct = (string)session.GetValue("ReturnProduct");
                string nextStation = (string)session.GetValue(Session.SessionKeys.ReturnStation);
                string desc = "";
                IMES.FisObject.Common.Station.IStationRepository stationRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Station.IStationRepository>();
                IMES.FisObject.Common.Station.IStation iStation = stationRepository.Find(nextStation);
                if (iStation != null)
                {
                    desc = iStation.Descr;
                }

                ret.Add(currentProduct.CUSTSN);
                ret.Add(isReturnProduct);
                ret.Add(nextStation);
                ret.Add(desc);

                return ret;
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

        public int CheckReturnProduct(string prodid)
        {
            int ret = 0;
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IProduct currentProduct = productRepository.Find(prodid);
            //select @RepairID = ID from ProductRepair nolock where ProductID = @PrdID order by Udt desc
            int logID = productRepository.GetNewestProductRepairIdRegardlessStatus(prodid);
            if (logID != 0)
            {
                //Select @Station=Station from ProductStatus where ProductID=@PrdID
                string station = currentProduct.Status.StationId;
                //select distinct Cause from ProductRepair_DefectInfo where ProductRepairID = @RepairID
                IList<string> causeLst = new List<string>();
                causeLst = productRepository.GetCauseListByProductRepairId(logID);

                //若Cause 只有1条件记录，前2码为’CN’或WW’，@Station<>’7P’，则判定该Product为回流机器
                if (causeLst != null && causeLst.Count == 1)
                {
                    if (causeLst[0].Substring(0, 2) == "CN" || causeLst[0].Substring(0, 2) == "WW")
                    {
                        if (station != "7P")
                        {
                            ret = 1;
                        }
                    }
                }
            }
            return ret;
        }


        /// <summary>
        /// Cancel
        /// </summary>
        public void WFCancel(string prodId)
        {
            logger.Debug("(FARepair)Cancel start, [prodId]:" + prodId);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
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
                logger.Debug("(FARepair)Cancel end, [prodId]:" + prodId);
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

            defect.Location = repairLogInfo.location;
            defect.MTAID = repairLogInfo.mtaID;
            //add new
            defect.ReturnStation = repairLogInfo.returnStation;

            return defect;
        }
    }
}
