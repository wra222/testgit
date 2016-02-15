/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: JPNPVSImpl
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================
 * 2010-03-15   ChenXu(itc-208014)  Create
 * 2010-05-06   ChenXu              Modify:ITC-1155-0068
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
    /// <summary>
    /// Japanese PVS
    /// </summary>
    public class JPNPVS : MarshalByRefObject, IJPNPVS
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType currentSessionType = Session.SessionType.Product;

        #region IJPNPVS Members

        /// <summary>
        /// 启动工作流，根据输入productID获取ProductModel,成功后调用getExplicitCheckItem
        /// 如果有需要刷料检查的CheckItem，调用checkExplicitCheckItem，否则调用Save
        /// </summary>
        ProductModel IJPNPVS.InputUUT(string custSN, string line, string editor, string station, string customer)
        {
            logger.Debug("(JPNPVSImpl)InputUUT start, custSN:" + custSN + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                string sessionKey = currentProduct.ProId;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "048JPNPVS.xoml", "048JPNPVS.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(custSN);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
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
                logger.Debug("(JPNPVSImpl)InputUUT end, custSN:" + custSN + "line:" + line + "editor:" + editor + "station:" + station + "customer" + customer);
            }
        }

        /// <summary>
        /// 刷料检查的一些属性,全部检查成功后调用save
        /// 将Session.IsComplete设为False
        /// </summary>
        IList<MatchedPartOrCheckItem> IJPNPVS.CheckExplicitCheckItem(string productID, string checkValue)
        {
            logger.Debug("(JPNPVSImpl)CheckExplicitCheckItem start, productID:" + productID + " checkValue:" + checkValue);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = productID;
            Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

            if (currentSession == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                throw ex;
            }
            try
            {
                currentSession.Exception = null;
                currentSession.AddValue(Session.SessionKeys.ValueToCheck, checkValue);
                currentSession.InputParameter = checkValue;
                currentSession.SwitchToWorkFlow();

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }


                IList<MatchedPartOrCheckItem> MatchedList = new List<MatchedPartOrCheckItem>();
                //get matchedinfo

                ICheckItem citem = (ICheckItem)currentSession.GetValue(Session.SessionKeys.MatchedCheckItem);
                if (citem == null)
                {
                    throw new FisException("MAT010", new string[] { checkValue });
                }
                else
                {
                    MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                   // tempMatchedPart.PNOrItemName = citem.CheckItem;
					tempMatchedPart.PNOrItemName = citem.ItemDisplayName;
                    tempMatchedPart.CollectionData = citem.ValueToCollect;
                    MatchedList.Add(tempMatchedPart);
                    return MatchedList;
                }

            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(JPNPVSImpl)CheckExplicitCheckItem end, productID:" + productID + " checkValue:" + checkValue);
            }
        }

        /// <summary>
        /// 记录过站Log，更新机器状态，结束工作流。
        /// 将Session.IsComplete设为True
        /// </summary>
        void IJPNPVS.Save(string productID)
        {
            logger.Debug("(JPNPVSImpl)Save start, productID:" + productID);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = productID;
            Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

            if (currentSession == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                throw ex;
            }
            try
            {

                currentSession.Exception = null;
                currentSession.AddValue(Session.SessionKeys.IsComplete, true);

                currentSession.SwitchToWorkFlow();

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
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(JPNPVSImpl)Save end, productID:" + productID);
            }

        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        void IJPNPVS.Cancel(string uutSn)
        {
            string sessionKey = uutSn;
            try
            {
                logger.Debug("(JPNPVSImpl)Cancel start, sessionKey:" + sessionKey);

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
                logger.Debug("(JPNPVSImpl)Cancel end, sessionKey:" + sessionKey);
            }
        }

        /// <summary>
        /// 获取所有需要扫描的Parts和CheckItem，在调用inputUUT成功开启工作流之后调用
        /// 本站原定义要刷的内容为Model的TSBPN,UPCCODE
        /// 用productID获取Session
        /// 调用ICollectionData.GetCheckItemList 
        /// </summary>
        IList<BomItemInfo> IJPNPVS.GetNeedCheckPartAndItem(string productID)
        {
            logger.Debug("GetNeedCheckPartAndItem start, ProductID:" + productID);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(productID, currentSessionType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(productID);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    return CommonImpl.GetCheckItemList(currentSession);
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
                logger.Debug("GetNeedCheckPartAndItem end, ProductID:" + productID);
            }
        }

        #endregion
    }
}
