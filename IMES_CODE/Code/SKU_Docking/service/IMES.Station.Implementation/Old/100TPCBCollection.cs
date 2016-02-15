/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: TPCBCollectionImpl
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================
 * 2010-04-10   Chen Xu (eB1-4)     create
 * Known issues:
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
using IMES.FisObject.Common.TPCB;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;
using IMES.Route;


namespace IMES.Station.Implementation
{

    /// <summary>
    /// TPCB Collection
    /// </summary>
    public class TPCBCollection : MarshalByRefObject, ITPCBCollection
    {

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType currentSessionType = Session.SessionType.Common;

        #region ITPCBCollection Members

        /// <summary>
        /// 【保存】或【更新】TPCBInfo信息
        /// </summary>
        public IList<TPCBInfo> SaveTPCB(string family, string pdline, string type, string partno, string vendor, string dcode, string editor, string station, string customer)
        {
            logger.Debug("(TPCBCollectionImpl)SaveTPCB start, family:" + family + "pdline:" + pdline + "type:" + type + "partno:" + partno + "vendor" + vendor + "dcode" + dcode + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(family, currentSessionType);

                string sessionkey = family;

                if (currentSession == null)
                {
                    currentSession = new Session(sessionkey, currentSessionType, editor, station, pdline, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionkey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "100TPCBCollection.xoml", "100TPCBCollection.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.FamilyName, family);
                    currentSession.AddValue(Session.SessionKeys.LineCode, pdline);
                    currentSession.AddValue(Session.SessionKeys.TPCBType, type);
                    currentSession.AddValue(Session.SessionKeys.PartNo, partno);
                    currentSession.AddValue(Session.SessionKeys.VendorSN, vendor);
                    currentSession.AddValue(Session.SessionKeys.DCode, dcode);
                    currentSession.AddValue(Session.SessionKeys.Editor, editor);
                    currentSession.AddValue(Session.SessionKeys.ifElseBranch, 1);

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionkey + " Exists.");
                        erpara.Add(sessionkey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionkey);
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
               //return ((DateTime)currentSession.GetValue(Session.SessionKeys.CreateDateTime));
                return (IList<TPCBInfo>)currentSession.GetValue(Session.SessionKeys.TPCBInfoLst);
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
                logger.Debug("(TPCBCollectionImpl)SaveTPCB end, family:" + family + "pdline:" + pdline + "type:" + type + "partno:" + partno + "vendor" + vendor + "dcode" + dcode + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }

        /// <summary>
        /// 删除TPCB数据相关信息
        /// </summary>
        public void DeleteTPCB(string family, string pdline, string partno, string editor, string station, string customer)
        {
            logger.Debug("(TPCBCollectionImpl)DeleteTPCB start, family:" + family + "pdline:" + pdline + "partno" + partno + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(family, currentSessionType);

                string sessionkey = family;

                if (currentSession == null)
                {
                    currentSession = new Session(sessionkey, currentSessionType, editor, station, pdline, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionkey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "100TPCBCollection.xoml", "100TPCBCollection.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.FamilyName, family);
                    currentSession.AddValue(Session.SessionKeys.LineCode, pdline);
                    currentSession.AddValue(Session.SessionKeys.PartNo, partno);
                    currentSession.AddValue(Session.SessionKeys.ifElseBranch, 2);

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionkey + " Exists.");
                        erpara.Add(sessionkey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionkey);
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
                logger.Debug("(TPCBCollectionImpl)DeleteTPCB end, family:" + family + "pdline:" + pdline + "partno" + partno + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }

        /// <summary>
        /// 【保存】TPCBDet信息
        /// </summary>
        public void SaveTPCBDet(string tpcbCode, string family, string pdline, string editor, string station, string customer)
        {
            logger.Debug("(TPCBCollectionImpl)SaveTPCBDet start, tpcbCode:" + tpcbCode + "family:" + family + "pdline:" + pdline + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(tpcbCode, currentSessionType);

                string sessionkey = tpcbCode;

                if (currentSession == null)
                {
                    currentSession = new Session(sessionkey, currentSessionType, editor, station, pdline, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionkey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "100TPCBCollection.xoml", "100TPCBCollection.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.TPCB, tpcbCode);
                    currentSession.AddValue(Session.SessionKeys.FamilyName, family);
                    currentSession.AddValue(Session.SessionKeys.LineCode, pdline);
                    currentSession.AddValue(Session.SessionKeys.Editor, editor);
                    currentSession.AddValue(Session.SessionKeys.ifElseBranch, 3);

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionkey + " Exists.");
                        erpara.Add(sessionkey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionkey);
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
                logger.Debug("(TPCBCollectionImpl)SaveTPCBDet end, tpcbCode:" + tpcbCode + "family:" + family + "pdline:" + pdline + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }

        /// <summary>
        /// 获得FamilyInfo
        /// </summary>
        public IList<FamilyInfo> GetFamilyList()
        {
            logger.Debug("(TPCBCollectionImpl)GetFamilyList start");

            try
            {
                ITPCBInfoRepository TPCBInfoRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository, TPCB_Info>();
                IList<FamilyInfo> currentFamilyLst = TPCBInfoRepository.GetFamilyList();

                return currentFamilyLst;
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
                logger.Debug("(TPCBCollectionImpl)GetFamilyList start");
            }
        }

        // <summary>
        /// 根据下拉框选择的family，取得Type下拉框信息
        /// </summary>
        public IList<string> GetTypeList(string family)
        {
            logger.Debug("(TPCBCollectionImpl)GetFamilyList start, family:" + family);

            try
            {
                ITPCBInfoRepository TPCBInfoRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository, TPCB_Info>();
                IList<string> currentTPCBTypeLst = TPCBInfoRepository.GetTypeList(family);

                return currentTPCBTypeLst;
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
                logger.Debug("(TPCBCollectionImpl)GetFamilyList start, family:" + family);
            }
        }

        /// <summary>
        /// 根据下拉框选择的family和type, 取得PartNo下拉框信息
        /// </summary>
        public IList<string> GetPartNoList(string family, string type)
        {
            logger.Debug("(TPCBCollectionImpl)GetFamilyList start, family:" + family + "type:" + type);

            try
            {
                ITPCBInfoRepository TPCBInfoRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository, TPCB_Info>();
                IList<string> currentPartNoLst = TPCBInfoRepository.GetPartNoList(family, type);

                return currentPartNoLst;
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
                logger.Debug("(TPCBCollectionImpl)GetFamilyList start, family:" + family + "type:" + type);
            }
        }

        /// <summary>
        /// 根据下拉框选择的family和partno, 取得DCode信息
        /// </summary>
        public string GetDCode(string family, string partno)
        {
            logger.Debug("(TPCBCollectionImpl)GetDCode start, family:" + family + "partno:" + partno);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                ITPCBInfoRepository TPCBInfoRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository, TPCB_Info>();
                IList<string> currentDCode = TPCBInfoRepository.GetDCode(family, partno);

                if (currentDCode.Count > 1)
                {
                    erpara.Add(family);
                    erpara.Add(partno);
                    ex = new FisException("CHK120", erpara);
                    throw ex;
                }

                else
                {
                    return currentDCode[0];
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(TPCBCollectionImpl)GetDCode start, family:" + family + "partno:" + partno);
            }
        }

        /// <summary>
        /// 根据下拉框选择的family和partno, 取得VendorSN信息
        /// </summary>
        public string GetVendorSN(string family, string partno)
        {
            logger.Debug("(TPCBCollectionImpl)GetVendorSN start, family:" + family + "partno:" + partno);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                ITPCBInfoRepository TPCBInfoRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository, TPCB_Info>();
                IList<string> currentVendorSN = TPCBInfoRepository.GetVendorSN(family,partno);

                if (currentVendorSN.Count > 1)
                {
                    erpara.Add(family);
                    erpara.Add(partno);
                    ex = new FisException("CHK121", erpara);
                    throw ex;
                }

                else
                {
                    return currentVendorSN[0];
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(TPCBCollectionImpl)GetVendorSN start, family:" + family + "partno:" + partno);
            }
        }

        /// <summary>
        /// 显示全部TPCBInfo数据相关信息
        /// </summary>
        public IList<TPCBInfo> Query(string family, string pdline)
        {
            logger.Debug("(TPCBCollectionImpl)Query start, family:" + family + "pdline:" + pdline);
            
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                ITPCBInfoRepository TPCBInfoRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository, TPCB_Info>();
                IList<TPCBInfo> currentTPCBInfoLst = TPCBInfoRepository.Query(family, pdline);

                if (currentTPCBInfoLst.Count > 0)
                {
                    return currentTPCBInfoLst;
                }

                else
                {
                    erpara.Add(family);
                    erpara.Add(pdline);
                    ex = new FisException("CHK116", erpara);
                    throw ex;
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(TPCBCollectionImpl)Query end, family:" + family + "pdline:" + pdline);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("(TPCBCollectionImpl)Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(TPCBCollectionImpl)Cancel end, sessionKey:" + sessionKey);
            }     
        }

        #endregion
    }
}
