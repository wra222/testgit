using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.PrintLog;
using IMES.Route;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.MO;
using IMES.FisObject.Common.Part;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.Common.Misc;
using IMES.Infrastructure.Repository._Metas;
using IMES.Common;



namespace IMES.Station.Implementation
{

    /// <summary>
    /// </summary>
    public partial class DefectComponentPrint : MarshalByRefObject, IDefectComponentPrint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IMiscRepository miscRep = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
        #region DefectComponentPrint Members
        public IList<string> GetVendorList()
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                IList<string> defectList = miscRep.GetDataDistinct<DefectComponent, DefectComponentInfo, string>(new DefectComponentInfo { }, "Vendor");
                return defectList;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        

        public IList<string> GetReturnLineList()
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                IList<string> ret = new List<string>();
                IList<ConstValueTypeInfo> constList = ActivityCommonImpl.Instance.ConstValueType("SAPReturnLine", ""); //CommonUtl.ConstValueType("SAPReturnLine", "");
                foreach (ConstValueTypeInfo item in constList)
                {
                    ret.Add(item.value);
                }
                return ret;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public ArrayList GetDefectComponentInfo(string vendor, string customer, string station, string user)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            ArrayList retValue = new ArrayList();
            string currentSessionKey = Guid.NewGuid().ToString();
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Common);
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                if (currentSession == null)
                {
                    currentSession = new Session(currentSessionKey, Session.SessionType.Common, user, station, "", customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", user);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Common);
                    currentSession.AddValue("Vendor", vendor);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("DefectComponentPrint.xoml", "", wfArguments);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
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
                    erpara.Add(currentSessionKey);
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
                IList<DefectComponentInfo> retList = (IList<DefectComponentInfo>)currentSession.GetValue("DefectComponentInfo");
                IList<DefectComponentInfo> saveList = retList.Where(x => x.Status == "11" && x.BatchID == null || x.BatchID == "" && x.Vendor == vendor).ToList();
                currentSession.AddValue("DefectComponentInfo", saveList);
                IList<DefectComponentPrintGV1> gvList = retList
                                .Where(x => x.Status == "11" && x.BatchID == null || x.BatchID == "" && x.Vendor == vendor)
                                .GroupBy(item => new
                                {
                                    Family = item.Family,
                                    IECPn = item.IECPn,
                                    PartType = item.PartType,
                                    Vendor = item.Vendor,
                                    DefectCode = item.DefectCode,
                                    DefectDesc = item.DefectDescr,
                                    PartSn = item.PartSn
                                })
                                .Select(group => new DefectComponentPrintGV1
                                {
                                    ReturnType = "Z45",
                                    Family = group.Key.Family,
                                    IECPn = group.Key.IECPn,
                                    PartType = group.Key.PartType,
                                    Vendor = group.Key.Vendor,
                                    NPQty = group.Count(),
                                    DefectCode = group.Key.DefectCode,
                                    DefectDesc = group.Key.DefectDesc
                                })
                                .OrderBy(item => item.Family).ThenBy(item => item.IECPn).ThenBy(item => item.Vendor).ThenBy(item => item.DefectCode).ToList();

                int total = gvList.Count();
                if (gvList.Count == 0)
                {
                    throw new FisException("CQCHK50114", new string[] { });
                }
                retValue.Add(currentSessionKey);
                retValue.Add(gvList);
                retValue.Add(total);
                return retValue;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public ArrayList GetDefectComponentDetailInfo(string guid ,string vendor, string family, string iecpn, string defectcode)
        {
            ArrayList retValue = new ArrayList();
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string sessionKey = guid;
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);
                FisException ex;
                List<string> erpara = new List<string>();
                IList<DefectComponentInfo> defectComponentInfoList = new List<DefectComponentInfo>();
                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    IMiscRepository miscRep = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                    defectComponentInfoList = miscRep.GetData<DefectComponent, DefectComponentInfo>(new DefectComponentInfo { Family = family,
                                                                                                                              Vendor = vendor,
                                                                                                                              IECPn=iecpn,
                                                                                                                              DefectCode=defectcode});
                    IList<DefectComponentPrintGV2> gvList = defectComponentInfoList
                                    .Where(x => x.Status == "11" && x.BatchID == null || x.BatchID == "")
                                    .Select(group => new DefectComponentPrintGV2
                                    {
                                        Vendor = group.Vendor,
                                        Family = group.Family,
                                        PartNo = group.PartNo,
                                        PartType = group.PartType,
                                        IECPn = group.IECPn,
                                        PartSN = group.PartSn,
                                        Defect = group.DefectCode + "-" + group.DefectDescr
                                    })
                                    .OrderBy(item => item.Family).ThenBy(item => item.IECPn).ThenBy(item => item.Vendor).ThenBy(item => item.Defect).ToList();
                    retValue.Add(gvList);
                }
                return retValue;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public ArrayList Save(string guid, string returnLine, int totalQty, List<PrintItem> printItems)
        {
            ArrayList retValue = new ArrayList();
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                string sessionKey = guid;
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);
                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    session.Exception = null;
                    session.AddValue("ReturnLine", returnLine);
                    session.AddValue("TotalQty", totalQty);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
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
                List<PrintItem> printItemList = (List<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
                string batchID = (string)session.GetValue("BatchID");
                retValue.Add(printItemList);
                retValue.Add(batchID);
                return retValue;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }
        
        public void Cancel(string sn)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string sessionKey = sn;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);

                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }
        #endregion

        #region DefectComponent RePrint Members
        public IList<DefectComponentPrintGV_Batch> GetDefectComponentBatch(string vendor, string printDate)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            IList<DefectComponentPrintGV_Batch> ret = new List<DefectComponentPrintGV_Batch>();
            try
            {
                DefectComponentBatchStatusInfo item = new DefectComponentBatchStatusInfo();
                item.Vendor = vendor;
                DateTime startDate,endDate;
                IList<DefectComponentBatchStatusInfo> defectBatchList = miscRep.GetData<DefectComponentBatchStatus, DefectComponentBatchStatusInfo>(item);
                if (!string.IsNullOrEmpty(printDate))
                {
                    startDate = DateTime.Parse(printDate);
                    endDate = DateTime.Parse(printDate + " 23:59:59");
                    defectBatchList = defectBatchList.Where(m => m.PrintDate >= startDate && m.PrintDate <= endDate).ToList();
                }
                
                
                IList<ConstValueInfo> constValueList = miscRep.GetData<ConstValue, ConstValueInfo>(new ConstValueInfo { type = "DCBatchStatus" });
                ret = (from a in defectBatchList
                       join b in constValueList on a.Status equals b.name
                       select new DefectComponentPrintGV_Batch
                       {
                           BatchID = a.BatchID,
                           PrintDate = a.PrintDate,
                           Status = a.Status + "-" + b.value,
                           ReturnLine = a.ReturnLine,
                           TotalQty = a.TotalQty
                       }).ToList();

                return ret;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public IList<DefectComponentPrintGV1> GetDefectComponent_RePrint(string batchID)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                DefectComponentInfo items = new DefectComponentInfo();
                items.BatchID = batchID;
                IList<DefectComponentInfo> defectList = miscRep.GetData<DefectComponent, DefectComponentInfo>(items);
                IList<DefectComponentPrintGV1> gvList = defectList
                                .GroupBy(item => new
                                {
                                    Family = item.Family,
                                    IECPn = item.IECPn,
                                    PartType = item.PartType,
                                    Vendor = item.Vendor,
                                    DefectCode = item.DefectCode,
                                    DefectDesc = item.DefectDescr
                                })
                                .Select(group => new DefectComponentPrintGV1
                                {
                                    ReturnType = "Z45",
                                    Family = group.Key.Family,
                                    IECPn = group.Key.IECPn,
                                    PartType = group.Key.PartType,
                                    Vendor = group.Key.Vendor,
                                    NPQty = group.Count(),
                                    DefectCode = group.Key.DefectCode,
                                    DefectDesc = group.Key.DefectDesc
                                })
                                .OrderBy(item => item.Family).ThenBy(item => item.IECPn).ThenBy(item => item.Vendor).ThenBy(item => item.DefectCode).ToList();

                return gvList;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public IList<DefectComponentPrintGV2> GetDefectComponentDetailInfo_RePrint(string batchID, string vendor, string family, string iecpn, string defectcode)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                IList<DefectComponentInfo> defectComponentInfoList = miscRep
                    .GetData<DefectComponent, DefectComponentInfo>(new DefectComponentInfo
                {
                    BatchID = batchID,
                    Family = family,
                    Vendor = vendor,
                    IECPn = iecpn,
                    DefectCode = defectcode
                });
                IList<DefectComponentPrintGV2> gvList = defectComponentInfoList
                                .Select(group => new DefectComponentPrintGV2
                                {
                                    Vendor = group.Vendor,
                                    Family = group.Family,
                                    PartNo = group.PartNo,
                                    PartType = group.PartType,
                                    IECPn = group.IECPn,
                                    PartSN = group.PartSn,
                                    Defect = group.DefectCode + "-" + group.DefectDescr
                                })
                                .OrderBy(item => item.Family).ThenBy(item => item.IECPn).ThenBy(item => item.Vendor).ThenBy(item => item.Defect).ToList();
                return gvList;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public ArrayList RePrint(string batchID, string customer, string station, string user, List<PrintItem> printItems)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            ArrayList retValue = new ArrayList();
            string currentSessionKey = batchID;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Common);
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                if (currentSession == null)
                {
                    currentSession = new Session(currentSessionKey, Session.SessionType.Common, user, station, "", customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", user);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Common);
                    currentSession.AddValue("BatchID", batchID);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("DefectComponentRePrint.xoml", "", wfArguments);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
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
                    erpara.Add(currentSessionKey);
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
                List<PrintItem> printItemList = (List<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                retValue.Add(printItemList);
                retValue.Add(batchID);
                return retValue;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        #endregion
    }

}
