/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: MacPrintImpl
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================
 * 2010-03-10   ChenXu(itc-208014)  Create
 *  
 * Known issues:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.CommonIntf;
using System.Collections;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using log4net;
using IMES.FisObject.FA.Product;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// MAC Print
    /// </summary>
    public class MacPrint : MarshalByRefObject, IMacPrint
    {
       
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType ProductSessionType = Session.SessionType.Product;
       
        #region IMacPrint Members

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn"></param>

        void IMacPrint.Cancel(string uutSn)
        {
            string sessionKey = uutSn;
            try
            {
                logger.Debug("(MacPrintImpl)Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

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
                logger.Debug("(MacPrintImpl)Cancel end, sessionKey:" + sessionKey);
            }
        }


        /// <summary>
        /// 启动工作流，根据输入productID获取ProductModel,成功后调用CheckSN
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns>返回Model</returns>
        
        ProductModel IMacPrint.InputUUT(string productID, string line, string editor, string station, string customer)
        {
            logger.Debug("(MacPrintImpl)InputUUT start, productID:" + productID + "line:" + line + "editor:" + editor +"station:" +station + "customer:" + customer);
            string sessionKey = productID;
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, ProductSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "046MacPrint.xoml", "046MacPrint.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, sessionKey);
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

                ProductModel modelInfo = new ProductModel();
                Product tempProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                modelInfo.ProductID = tempProduct.ProId;
                modelInfo.Model = tempProduct.Model;
                modelInfo.CustSN =tempProduct.CUSTSN;
               
                return modelInfo;
                

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
                logger.Debug("(MacPrintImpl)InputUUT end, productID:" + productID + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }



        /// <summary>
        /// 将custSn放到Session.CustSN中
        /// 将Session.IsComplete设为False
        /// 检查CustSn,成功后调用save
        /// </summary>
        
        void IMacPrint.CheckSN(string productID, string custSn)
        {

            logger.Debug("(MacPrintImpl)CheckSN start, productID:" + productID + " custSn:" + custSn);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = productID;
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {

                    currentSession.AddValue(Session.SessionKeys.CustSN, custSn);

                    currentSession.Exception = null;
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
                logger.Debug("(MacPrintImpl)CheckSN end, productID:" + productID + " custSn:" + custSn);
            }

        }


        /// <summary>
        /// 记录过站Log，更新机器状态，返回打印重量标签的PrintItem，结束工作流。
        /// 将Session.IsComplete设为True
        /// </summary>

        IList<PrintItem> IMacPrint.Save(string productID, IList<PrintItem> printItems, out string mac)
        {
            logger.Debug("(MacPrintImpl)Save start, prodId:" + productID);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = productID;
           
            try
            {
                 Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, true);

                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();


                    if (currentSession.Exception != null)
                    {
                        if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            currentSession.ResumeWorkFlow();
                        }

                        throw currentSession.Exception;
                    }
                    
                    //mac = (string)currentSession.GetValue(Session.SessionKeys.MAC);
                   
                    Product tempProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                    mac = tempProduct.MAC;
                    IList<PrintItem> returnList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                    return returnList;
                                                        
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
                logger.Debug("(MacPrintImpl)Save end, prodId:" + productID);
            }
        }


        /// <summary>
        /// 重印标签
        /// </summary>

        IList<PrintItem> IMacPrint.ReprintLabel(string prodOrsn, string line, string editor, string station, string customer, string reason, IList<PrintItem> printItems, out string model, out string curProductID)
        {
            logger.Debug("(MacPrintImpl)ReprintLabel start, ProductIDOrCustSN:" + prodOrsn + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer + "reason:" + reason);
            try
            {
                /// ProductIDOrCustSN类型的InputType
                /// 为了确保一个Product不会同时被两个操作员刷入，需要用ProductID做主键启动工作流
                /// 此站若输入的是SN，需要先根据SN获取Product调用CommonImpl.GetProductByInput(),将获得的Product放到Session.Product,再用ProductID启动工作流

                var currentProduct = CommonImpl.GetProductByInput(prodOrsn, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                string sessionKey = currentProduct.ProId;
                curProductID = sessionKey;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, ProductSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station",station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("046MacRePrint.xoml", "", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, sessionKey);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, sessionKey);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "Mac");

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(prodOrsn);
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
                    erpara.Add(prodOrsn);
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

                currentModel.ProductID = currentProduct.ProId;
                currentModel.CustSN = currentProduct.CUSTSN;
                currentModel.Model = currentProduct.Model;

                model = currentModel.Model;
               
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
                logger.Debug("(MacPrintImpl)ReprintLabel end, ProductIDOrCustSN:" + prodOrsn + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer + "reason:" + reason);
            }
        }


        #endregion
    }
}
