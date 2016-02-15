/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Initial parts collection interface implement
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2009-10-20  Zhao Meili(EB)        Create 
 * 200910-201005  itc207013          ITC-1122-0083,ITC-1122-0087
 *                                   ITC-1122-0088,ITC-1122-0096
 *                                   ITC-1122-0099,ITC-1122-0119
 *                                   ITC-1122-0149,ITC-1122-0151
 *                                   
 * Known issues:
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;

namespace IMES.Station.Implementation
{
    public class InitialPartsCollection : MarshalByRefObject, IInitialPartsCollection
    {
        private static readonly Session.SessionType theType = Session.SessionType.Product;
        private const string WFfile = "017InitialPartsCollection.xoml";
        private const string Rulesfile = "017InitialPartsCollection.rules";

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 输入Product Id和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="scanQty">Scan Qty</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public IList<BomItemInfo> InputProdId(
            string pdLine,
            string prodId,
            string editor, string stationId, string customerId)
        {
            logger.Debug("(InitialPartsCollection)InputProdId start,"
                          + " [prodId]:" + prodId
                          + " [pdLine]:" + pdLine
                          + " [editor]:" + editor
                          + " [station]:" + stationId
                          + " [customer]:" + customerId);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                string sessionKey = prodId;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (sessionInfo == null)
                {
                    sessionInfo = new Session(sessionKey, theType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", sessionInfo);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("SessionType", theType);
                    sessionInfo.AddValue(Session.SessionKeys.IsComplete, false);


                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, WFfile, Rulesfile, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    sessionInfo.SetInstance(instance);
                    //for generate MB no

                    if (!SessionManager.GetInstance.AddSession(sessionInfo))
                    {
                        sessionInfo.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    sessionInfo.WorkflowInstance.Start();
                    sessionInfo.SetHostWaitOne();


                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }
                    throw sessionInfo.Exception;
                }
                //get bominfo
                IList<BomItemInfo> ret = CommonImpl.GetCheckItemList(sessionInfo);
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(InitialPartsCollection)InputProdId End,"
                                          + " [prodId]:" + prodId
                                          + " [pdLine]:" + pdLine
                                          + " [editor]:" + editor
                                          + " [station]:" + stationId
                                          + " [customer]:" + customerId);
            }
        }

        /// <summary>
        /// 输入PPID
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="ppid">PPID</param>
        /// <returns>主料的Part No</returns>
        public IList<MatchedPartOrCheckItem> InputPPID(
            string prodId,
            string ppid)
        {
            logger.Debug("(InitialPartsCollection)InputPPID start,"
               + " [prodId]: " + prodId
               + " [ppid]:" + ppid);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                
                string sessionKey = prodId;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.PartID, ppid);
                    sessionInfo.AddValue(Session.SessionKeys.ValueToCheck, ppid);
                    sessionInfo.Exception = null;
                    sessionInfo.SwitchToWorkFlow();
                }

                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }

                    throw sessionInfo.Exception;
                }

                IList<MatchedPartOrCheckItem> RetList = new List<MatchedPartOrCheckItem>(); 

                //get matchedinfo
                IList<IBOMPart> bomPartlist = (IList<IBOMPart>)sessionInfo.GetValue(Session.SessionKeys.MatchedParts);
                if ((bomPartlist != null) && (bomPartlist.Count > 0))
                {
                    foreach (IBOMPart bompartitem in bomPartlist)
                    {
                        MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                        tempMatchedPart.PNOrItemName = bompartitem.PN;
                        tempMatchedPart.CollectionData = bompartitem.MatchedSn;
                        tempMatchedPart.ValueType = bompartitem.ValueType;
                        RetList.Add(tempMatchedPart); 
                    }
                    return RetList;
                }
                else
                {
                    ICheckItem citem = (ICheckItem)sessionInfo.GetValue(Session.SessionKeys.MatchedCheckItem);
                    if (citem == null)
                    {
                        erpara.Add(ppid);
                        throw new FisException("MAT010", erpara);
                    }
                    else
                    {
                        MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                        tempMatchedPart.PNOrItemName = citem.ItemDisplayName;
                        tempMatchedPart.CollectionData = citem.ValueToCollect;
                        tempMatchedPart.ValueType = "";
                        RetList.Add(tempMatchedPart);                                      
                        return RetList;
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(InitialPartsCollection)InputPPID  End,"
                               + " [prodId]: " + prodId
                               + " [ppid]:" + ppid);
            }
        }




        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="prodId"></param>
        /// 
        public void Save(string prodId)
        {
            logger.Debug("(InitialPartsCollection)Save start,"
               + " [prodId]: " + prodId);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {

                string sessionKey = prodId;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.IsComplete, true);
                    sessionInfo.Exception = null;
                    sessionInfo.SwitchToWorkFlow();
                }

                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }

                    throw sessionInfo.Exception;
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(InitialPartsCollection)Save  End,"
                               + " [prodId]: " + prodId);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, theType);

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
                logger.Debug("Cancel end, sessionKey:" + sessionKey);
            }
        }

    }
}
