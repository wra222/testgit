// INVENTEC corporation (c)2010 all rights reserved. 
// Description:  InitialPVS bll
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-10   Yuan XiaoWei                 create
// Known issues:
using System;
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
    /// 实现IInitialPVS接口，Initial PVS BLL实现类,实现Initial PVS站检料功能
    /// </summary>
    public class InitialPVS : MarshalByRefObject, IInitialPVS
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Product;

        #region IInitialPVS Members

        /// <summary>
        /// 此站输入的是SN，需要先根据SN获取Product调用CommonImpl.GetProductByInput()
        /// 将获得的Product放到Session.Product
        /// 再用ProductID启动工作流
        /// 卡站，获取Model,SN,ProductID
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        IMES.DataModel.ProductModel IInitialPVS.InputUUT(string custSN, string line, string editor, string station, string customer)
        {
            logger.Debug(" InputUUT start, custSN:" + custSN);
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
                    RouteManagementUtils.GetWorkflow(station, "033InitialPVS.xoml", "033InitialPVS.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(custSN);
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
                logger.Debug(" InputUUT end, custSN:" + custSN);
            }
        }

        /// <summary>
        /// 输入料或者要检查的CheckItem进行检查
        /// 如果没有抛出Match异常，从Session.SessionKeys.MatchedParts中把当前Match的料取出
        /// 如果没有match到Part，从Session.SessionKeys.MatchedCheckItem取出match的CheckItem
        /// 返回给前台
        /// 检料全部完成后调用Save
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        IList<MatchedPartOrCheckItem> IInitialPVS.CheckPartAndItem(string productID, string checkValue)
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
                    currentSession.AddValue(Session.SessionKeys.PartID, checkValue);
                    currentSession.AddValue(Session.SessionKeys.ValueToCheck, checkValue);
                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();
                }

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                IList<MatchedPartOrCheckItem> MatchedList = null;
                //get matchedinfo
                IList<IBOMPart> bomPartlist = (IList<IBOMPart>)currentSession.GetValue(Session.SessionKeys.MatchedParts);
                if ((bomPartlist != null) && (bomPartlist.Count > 0))
                {
                    MatchedList = new List<MatchedPartOrCheckItem>();
                    foreach (IBOMPart bompartitem in bomPartlist)
                    {
                        MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                        tempMatchedPart.PNOrItemName = bompartitem.PN;
                        tempMatchedPart.CollectionData = bompartitem.MatchedSn;
                        tempMatchedPart.ValueType = bompartitem.ValueType;
                        MatchedList.Add(tempMatchedPart);
                    }
                    return MatchedList;
                }
                else
                {
                    throw new FisException("MAT010", new string[] { checkValue });
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw e;
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

        /// <summary>
        /// 扫描完本站应该扫描的所有Parts后调用本方法存储扫入的所有Parts信息，结束工作流。
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        void IInitialPVS.Save(string productID)
        {
            logger.Debug("Save start, ProductID:" + productID);

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
                    currentSession.AddValue(Session.SessionKeys.IsComplete, true);
                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();
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
                logger.Debug("Save end, ProductID:" + productID);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void IInitialPVS.Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

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
                logger.Debug("Cancel end, sessionKey:" + sessionKey);
            }
        }

        /// <summary>
        /// 获取所有需要扫描的PizzaParts，在调用inputUUT成功开启工作流之后调用
        /// 用productID获取Session
        /// 调用ICollectionData.GetCheckItemList 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        IList<IMES.DataModel.BomItemInfo> IInitialPVS.GetNeedCheckPartAndItem(string productID)
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
