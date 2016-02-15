/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: FGShippingLabelTRO Impl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-03-09   LuycLiu     Create 

 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections;
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

    public class _FGShippingLabelTRO : MarshalByRefObject, IFGShippingLabelTRO
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region IFGShippingLabelTRO Members
        #endregion


        /// <summary>
        /// 启动工作流，根据输入productID获取Model,成功后调用CheckSN
        /// </summary>
        /// <param name="productID">product id</param>
        /// <param name="line">product line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <returns>返回ProductModel</returns>
        public ProductModel InputUUT(string prodId, string pdLine, string editor, string stationId, string customerId)
        {
            logger.Debug("(_FGShippingLabelTRO)InputUUT start, pdLine:" + pdLine + " prodId:" + prodId + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            try
            {
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "050FGShippingLabelTRO.xoml", "050FGShippingLabelTRO.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.IsComplete, false);
                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    Session.WorkflowInstance.Start();
                    Session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                //check workflow exception
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }
                    throw Session.Exception;
                }
                IProduct product = (IProduct)Session.GetValue(Session.SessionKeys.Product);
                ProductModel currentModel = new ProductModel();
                if (product != null)
                {
                    currentModel.CustSN = product.CUSTSN;
                    currentModel.ProductID = product.ProId;
                    currentModel.Model = product.Model;
                }
                return currentModel;

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
                logger.Debug("(_FGShippingLabelTRO)InputUUT end, pdLine:" + pdLine + " prodId:" + prodId + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            }
        }

        /// <summary>
        /// 检查CustSn,成功后调用save
        /// 将Session.IsComplete设为false
        /// </summary>
        /// <param name="productID">product id</param>
        /// <param name="sn">sn</param>
        public void CheckSN(string prodId, string sn)
        {
            logger.Debug("(_FGShippingLabelTRO)CheckSN start, prodId:" + prodId + " sn:" + sn);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            if (Session == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                throw ex;
            }
            try
            {

               
                Session.Exception = null;
                //将sn保存至session，供CheckSN Activity校验时使用
                Session.AddValue(Session.SessionKeys.CustSN, sn);
              
                Session.SwitchToWorkFlow();

                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }

                    throw Session.Exception;
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
                logger.Debug("(_FGShippingLabelTRO)CheckSN end, prodId:" + prodId + " sn:" + sn);
            }
        }

        /// <summary>
        /// 记录过站Log，更新机器状态，返回打印重量标签的PrintItem，结束工作流。
        /// 将Session.IsComplete设为True
        /// </summary>
        /// <param name="productID">product id</param>
        /// <param name="printItems">打印列表</param> 
        /// <returns>打印列表</returns>
        public IList<PrintItem> Save(string prodId, IList<PrintItem> printItems, out string custSn)
        {
            logger.Debug("(_FGShippingLabelTRO)Save start, prodId:" + prodId);

            FisException ex;
            string sessionKey = prodId;
            List<string> erpara = new List<string>();
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            if (Session == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                throw ex;
            }
            try
            {

                Session.Exception = null;
                Session.AddValue(Session.SessionKeys.IsComplete, true);
                Session.AddValue(Session.SessionKeys.PrintItems, printItems);

                Session.SwitchToWorkFlow();

                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }

                    throw Session.Exception;
                }


                IList<PrintItem> returnList = this.getPrintList(Session);
                custSn = (string)Session.GetValue(Session.SessionKeys.CustSN);
                return returnList;
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
                logger.Debug("(_FGShippingLabelTRO)Save end, prodId:" + prodId);
            }
        }

        /// <summary>
        /// 从Sessin里获取打印列表
        /// </summary>
        /// <param name="session">session</param>
        /// <returns></returns>
        private IList<PrintItem> getPrintList(Session session)
        {

            try
            {
                object printObject = session.GetValue(Session.SessionKeys.PrintItems);
                session.RemoveValue(Session.SessionKeys.PrintItems);
                if (printObject == null)
                {
                    return null;
                }

                IList<PrintItem> printItems = (IList<PrintItem>)printObject;

                return printItems;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;

            }
        }
        /// <summary>
        /// 重印标签
        /// </summary>
        /// <param name="productID">product id</param>
        /// <param name="line">product line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="printItems">打印列表</param> 
        /// <returns>打印列表</returns>
        public IList<PrintItem> ReprintLabel(string prodId, string pdLine, string editor, string stationId, string customerId, IList<PrintItem> printItems, out string custSn)
        {
            logger.Debug("(_FGShippingLabelTRO)ReprintLabel start, prodId:" + prodId + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            
            try
            {
                //因为输入的可能是ProductId或者SN,GetProuct Activity不能做到验证，因此这里采用CommonImpl里面的方法
                var currentProduct = CommonImpl.GetProductByInput(prodId, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                //如果没抛异常，证明输入合法，将ProcuctId作为sessionKey
                string sessionKey = currentProduct.ProId;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("050FGShippingLabelTROReprint.xoml", null, wfArguments);

                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, prodId);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, prodId);
                    //保存product对象至session,为工作流中的activity使用
                    Session.AddValue(Session.SessionKeys.Product, currentProduct);
                    Session.WorkflowInstance.Start();
                    Session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }
                    throw Session.Exception;
                }

                IList<PrintItem> returnList = this.getPrintList(Session);
                custSn = currentProduct.CUSTSN;
                return returnList;
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
                logger.Debug("(_FGShippingLabelTRO)ReprintLabel end, prodId:" + prodId + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
        }

        /// <summary>
        /// 解除DN的绑定关系
        /// </summary>
        /// <param name="productID">product id</param>
        /// <param name="line">product line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <returns></returns>
        public void Unpack(string prodId, string pdLine, string editor, string stationId, string customerId)
        {
            logger.Debug("(_FGShippingLabelTRO)Unpack start, prodId:" + prodId + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {

                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("050FGShippingLabelTROUnpack.xoml", "050FGShippingLabelTROUnpack.rules", wfArguments);

                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    Session.WorkflowInstance.Start();
                    Session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }
                    throw Session.Exception;
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
                logger.Debug("(_FGShippingLabelTRO)Unpack end, prodId:" + prodId + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn">uut sn</param>
        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

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