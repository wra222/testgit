/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: Unit Label Print
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-03-18   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IUnitLabelPrint接口的实现类
    /// </summary>
    public class UnitLabelPrintImpl : MarshalByRefObject, IUnitLabelPrint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;

        #region IUnitLabelPrint members

        /// <summary>
        /// 刷productID，启动工作流，检查输入的productID，卡站，获取Model,ProductID,
        /// 调用成功后调用GetNeedCheckPartAndItem方法获取需要检料的列表
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        public ProductModel InputProductID(string productID, string line, string editor, string station, string customer)
        {
            logger.Debug("(UnitLabelPrintImpl)InputProductID start, productID:" + productID);

            try
            {

                var currentProduct = CommonImpl.GetProductByInput(productID, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
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
                    RouteManagementUtils.GetWorkflow(station, "035UnitLabelPrint.xoml", "035UnitLabelPrint.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
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
                logger.Debug("(UnitLabelPrintImpl)InputProductID end, productID:" + productID);
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
        public IList<MatchedPartOrCheckItem> CheckPartAndItem(string productID, string checkValue)
        {
            logger.Debug("(UnitLabelPrintImpl)GetNeedCheckPartAndItem start, ProductID:" + productID
                 + " [checkValue]:" + checkValue);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(productID, SessionType);

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
                MatchedList = new List<MatchedPartOrCheckItem>();
                //get matchedinfo
                IList<IBOMPart> bomPartlist = (IList<IBOMPart>)currentSession.GetValue(Session.SessionKeys.MatchedParts);
                if ((bomPartlist != null) && (bomPartlist.Count > 0))
                {
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
                    ICheckItem citem = (ICheckItem)currentSession.GetValue(Session.SessionKeys.MatchedCheckItem);
                    if (citem == null)
                    {
                        throw new FisException("MAT010", new string[] { checkValue });
                    }
                    else
                    {
                        MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                        tempMatchedPart.PNOrItemName = citem.ItemDisplayName;
                        tempMatchedPart.CollectionData = citem.CheckValue;
                        tempMatchedPart.ValueType = "";
                        MatchedList.Add(tempMatchedPart);
                        return MatchedList;
                    }
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
                logger.Debug("(UnitLabelPrintImpl)GetNeedCheckPartAndItem end, ProductID:" + productID
                     + " [checkValue]:" + checkValue);
            }
        }

        /// <summary>
        /// 扫描完本站应该扫描的所有Parts后调用本方法存储扫入的所有Parts信息，结束工作流。
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        public IList<PrintItem> Save(string productID, IList<PrintItem> printItems)
        {
            logger.Debug("(UnitLabelPrintImpl)Save start, ProductID:" + productID);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(productID, SessionType);

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
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
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

                return (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug("(UnitLabelPrintImpl)Save end, ProductID:" + productID);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="productID"></param>
        public void Cancel(string productID)
        {
            try
            {
                logger.Debug("(UnitLabelPrintImpl)Cancel start, sessionKey:" + productID);

                Session currentSession = SessionManager.GetInstance.GetSession(productID, SessionType);

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
                logger.Debug("(UnitLabelPrintImpl)Cancel end, sessionKey:" + productID);
            }
        }

        /// <summary>
        /// 获取需要刷料的列表,从SessionBom中获取要刷的料,从CheckItem中获取要刷的CheckItem
        /// 本站为SN和COA，用Product获取Session
        /// 调用ICollectionData.GetCheckItemList 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public IList<BomItemInfo> GetNeedCheckPartAndItem(string productID)
        {
            logger.Debug("(UnitLabelPrintImpl)GetNeedCheckPartAndItem start, ProductID:" + productID);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(productID, SessionType);

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
                logger.Debug("(UnitLabelPrintImpl)GetNeedCheckPartAndItem end, ProductID:" + productID);
            }
        }

        /// <summary>
        /// 重印标签,调用Product的CanReprintUnitLabel方法判断是否能重印
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        public IList<PrintItem> ReprintLabel(string productID, string line, string editor, string station, string customer, string reason, IList<PrintItem> printItems, out string backProID, out string model)
        {
            logger.Debug("(UnitLabelPrintImpl)ReprintLabel start, [productID]:" + productID
                + " [line]: " + line
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = productID;

            try
            {
                IProduct product = CommonImpl.GetProductByInput(productID, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                sessionKey = product.ProId;

                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, SessionType, editor, station, string.Empty, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", string.Empty);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("035UnitLabelRePrint.xoml", string.Empty, wfArguments);

                    session.AddValue(Session.SessionKeys.Product, product);
                    session.AddValue(Session.SessionKeys.Reason, reason);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, product.ProId);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, product.ProId);
                    session.AddValue(Session.SessionKeys.PrintLogDescr, "");
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

                backProID = product.ProId;
                model = product.Model;
                return (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug("(UnitLabelPrintImpl)ReprintLabel end, [productID]:" + productID
                    + " [line]: " + line
                    + " [editor]:" + editor
                    + " [station]:" + station
                    + " [customer]:" + customer);
            }            
        }

        #endregion
    }
}
