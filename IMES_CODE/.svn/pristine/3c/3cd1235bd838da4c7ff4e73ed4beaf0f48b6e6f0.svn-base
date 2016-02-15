/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Implementation for QC Repair Add/Edit Page
 * UI:CI-MES12-SPEC-FA-UI QC Repair.docx –2011/10/19 
 * UC:CI-MES12-SPEC-FA-UC QC Repair.docx –2011/10/19            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-14   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0434, Jessica Liu, 2012-2-28
* ITC-1360-1577, Jessica Liu, 2012-3-27
* ITC-1360-1672, Jessica Liu, 2012-4-11
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.DataModel;
using System.Workflow.Runtime;
using log4net;
using IMES.Route;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IPAQCRepair接口的实现类
    /// </summary>
    public class PAQCRepairImpl : MarshalByRefObject, IPAQCRepair 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region IPAQCRepair members


        /// <summary>
        /// 取得OQC Product Repair列表
        /// </summary>
        /// <param name="ProdId">Product标识</param>
        /// <param name="status">status</param>
        /// <param name="defectCodeType">defectCode Type</param>
        /// <returns>Product Repair列表</returns>
        //ITC-1360-0434, Jessica Liu, 2012-2-28
        public IList<RepairInfo> GetOQCProdRepairList(string ProdId, int status, string defectCodeType)
        {
            IList<RepairInfo> retLst = null;

            try
            {
                if (!String.IsNullOrEmpty(ProdId))
                {
                    IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                    retLst = productRepository.GetOQCProdRepairList(ProdId, status, defectCodeType);
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 输入Product Id和相关信息, 
        /// 初次进入Repair 的时候，会基于Test Log 生成Repair Record
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>Repair Logs</returns>
        public IList<RepairInfo> InputProdId(string pdLine, string prodId, string editor, string stationId, string customer)
        {
            logger.Debug("(PAQCRepairImpl)InputProdId start, [pdLine]:" + pdLine
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
                    RouteManagementUtils.GetWorkflow(stationId, "PAQCRepair.xoml", "PAQCRepair.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    //ITC-1360-1577, Jessica Liu, 2012-3-27
                    session.AddValue(Session.SessionKeys.CN, "ALL");
                    session.AddValue(Session.SessionKeys.IsComplete, false);
                    session.SetInstance(instance);

                    //ITC-1360-1672, Jessica Liu, 2012-4-11
                    session.AddValue(Session.SessionKeys.Pallet, true);

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
                logger.Debug("(PAQCRepairImpl)InputProdId end, [pdLine]:" + pdLine
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
            logger.Debug("(PAQCRepairImpl)Edit start, [prodId]:" + prodId
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
                    RepairDefect defect = getPAQCRepairDefect(rll);

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
                logger.Debug("(PAQCRepairImpl)Edit end, [prodId]:" + prodId
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
            logger.Debug("(PAQCRepairImpl)Add start, [prodId]:" + prodId
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
                    RepairDefect defect = getPAQCRepairDefect(rll);

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
                logger.Debug("(PAQCRepairImpl)Add end, [prodId]:" + prodId
                    + " [rll]: " + rll);             
            }
        }

        /// <summary>
        /// Delete Repair logs
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="rll">删除的Repair logs</param>
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

            //        //session.AddValue(Session.SessionKeys.RepairDefectID, repairLogId);
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

        /* 2012-7-5, 新需求
        /// <summary>
        /// 完成维修并保存
        /// </summary>
        /// <param name="prodId">Product Id</param>
        public void Save(string prodId)
        {
            logger.Debug("(PAQCRepairImpl)Save start, [prodId]:" + prodId);
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

                    //2012-7-24, Jessica Liu, for Mantis:0001175
                    session.AddValue(Session.SessionKeys.Pallet, false);

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
                logger.Debug("(PAQCRepairImpl)Save end, [prodId]:" + prodId);            
            }
        }
        */
        /* 2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
        /// <summary>
        /// 完成维修并保存，返回excel打印信息
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <returns>excel里7个参数的值</returns>
        public List<string> Save(string prodId)
        */
        /// <summary>
        /// 完成维修并保存，返回excel打印信息
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="returnStation">return Station</param>
        /// <param name="returnStationText">return Station Text</param>
        /// <returns>excel里7个参数的值</returns>
        public List<string> Save(string prodId, string returnStation, string returnStationText)
        {
            logger.Debug("(PAQCRepairImpl)Save start, [prodId]:" + prodId);
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

                    //2012-7-24, Jessica Liu, for Mantis:0001175
                    session.AddValue(Session.SessionKeys.Pallet, false);

                    //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
                    session.AddValue("ReturnStation", returnStation);
                    session.AddValue("ReturnStationText", returnStationText);

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

                    //获取excel所需内容
                    IProduct currentProduct = (IProduct)session.GetValue(Session.SessionKeys.Product);
                    string param0 = DateTime.Now.ToString("yyyy-MM-dd HH:ss") + "(" + currentProduct.Status.Line + ")";
                    string param3 = "";
                    string param7 = "";
                    int repairID = 0;

                    IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

                    repairID = productRepository.GetNewestProductRepairId(currentProduct.ProId);

                    int param3Count = 0;
                    IList<string> retList = productRepository.GetDefectForProductRepair(repairID);
                    foreach (string tempStr in retList)
                    {
                        if (param3Count > 0)
                        {
                            param3 += ";";
                        }

                        param3 += tempStr;
                        param3Count++;
                    }

                    int param7Count = 0;
                    IList<string> retDefectList = productRepository.GetRemarkListOfProductRepairDefectInfo(repairID);
                    foreach (string tempDefectStr in retDefectList)
                    {
                        if (param7Count > 0)
                        {
                            param7 += "; ";
                        }

                        param7 += tempDefectStr;
                        param7Count++;
                    }

                    //2012-7-18
                    string stationStation = (string)session.GetValue("StationStation");
                    string stationDescr = (string)session.GetValue("StationDescr");
                    //2012-9-12, Jessica Liu, UC变更/* 2012-8-3
                    //2012-7-19
                    bool isBackflowProduct = (bool)session.GetValue("IsBackflowProduct");
                    string BackflowProductFlag = "";
                    if (isBackflowProduct == true)
                    {
                        BackflowProductFlag = "TRUE";
                    }
                    else
                    {
                        BackflowProductFlag = "FALSE";
                    }
                    //2012-9-12, Jessica Liu, UC变更*/
                    /* 2012-8-16
                    string BackflowProductFlag = "FALSE";
                    if (returnStation == "PO" || returnStation == "6A")
                    {
                        BackflowProductFlag = "TRUE";
                    }
                    */
                    //2012-9-12, Jessica Liu, UC变更
                    //string BackflowProductFlag = "TRUE";


                    List<string> ret = new List<string>();
                    ret.Add(param0);                        //参数0 
                    ret.Add(currentProduct.Status.Line);    //参数1
                    ret.Add(currentProduct.ProId);          //参数2
                    ret.Add(param3);                        //参数3 
                    ret.Add(currentProduct.PCBID);          //参数4
                    ret.Add(currentProduct.CUSTSN);         //参数5
                    ret.Add(currentProduct.Model);          //参数6
                    ret.Add(param7);                        //参数7 
                    //2012-7-18
                    ret.Add(currentProduct.CUSTSN);
                    ret.Add(stationStation);
                    ret.Add(stationDescr);
                    //2012-7-19
                    ret.Add(BackflowProductFlag);

                    return ret;
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
                logger.Debug("(PAQCRepairImpl)Save end, [prodId]:" + prodId);
            }
        }


        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string prodId)
        {
            logger.Debug("(PAQCRepairImpl)Cancel start, [prodId]:" + prodId);
            //FisException ex;
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
                logger.Debug("(PAQCRepairImpl)Cancel end, [prodId]:" + prodId);
            }
        }

        //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
        /// <summary>
        /// GetReturnStation
        /// </summary>
        /// <param name="code"></param>
        /// <param name="preStn"></param>
        /// <param name="station"></param>
        /// <param name="cause"></param>
        /// <returns></returns>
        public IList<DefectCodeStationInfo> GetReturnStation(int id, string code, string preStn, string station, string cause)
        {
            var stationRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Station.IStationRepository, IMES.FisObject.Common.Station.IStation>();
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            DefectCodeStationInfo cond = new DefectCodeStationInfo();
            IList<DefectCodeStationInfo> info = null;
            cond.defect = code;
            cond.pre_stn = preStn;
            cond.crt_stn = station;
            cond.cause = cause;
            info = stationRepository.GetDefectCodeStationList(cond);
            if (info != null && info.Count > 0)
            {
                RepairInfo setValue = new RepairInfo();
                RepairInfo c1 = new RepairInfo();

                c1.Identity = id;
                setValue.returnStation = info[0].nxt_stn;

                productRepository.UpdateProductRepairDefectInfo(setValue, c1);
            }
            else
            {
                DefectCodeStationInfo cond1 = new DefectCodeStationInfo();
                IList<DefectCodeStationInfo> info1 = null;
                cond1.defect = code;
                cond1.pre_stn = preStn;
                cond.crt_stn = station;
                info1 = stationRepository.GetDefectCodeStationList(cond1);

                if (info1 != null && info1.Count > 0)
                {
                    RepairInfo setValue = new RepairInfo();
                    RepairInfo c1 = new RepairInfo();

                    c1.Identity = id;
                    setValue.returnStation = info[0].nxt_stn;

                    productRepository.UpdateProductRepairDefectInfo(setValue, c1);
                }
                else
                {
                    List<string> errpara = new List<string>();
                    FisException e = new FisException("CHK830", errpara);
                    throw e;
                }
            }

            return info;
        }

        /// <summary>
        /// GetReturnStationList
        /// </summary>
        /// <param name="prodid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public IList<StationInfo> GetReturnStationList(string prodid, int status)
        {
            IList<StationInfo> retList = null;
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
            //2012-8-21, for mantis
            //retList = productRepository.GetStationInfoListFromProductRepair(prodid, status);
            List<string> erpara = new List<string>();
            IProduct currentProduct = CommonImpl.GetProductByInput(prodid, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
            if (currentProduct == null)
            {
                erpara.Add(prodid);
                throw new FisException("CHK079", erpara);
            }
            else if (string.IsNullOrEmpty(currentProduct.ProId))
            {
                erpara.Add(prodid);
                throw new FisException("SFC002", erpara);
            }
            else
            {
                retList = productRepository.GetStationInfoListFromProductRepair(currentProduct.ProId, status);
            }

            return retList;
        }
        #endregion

        private RepairDefect getPAQCRepairDefect(RepairInfo repairLogInfo)
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

            //2012-5-4
            defect.Location = repairLogInfo.location;
            defect.MTAID = repairLogInfo.mtaID;

            return defect;
        }
    }
}
