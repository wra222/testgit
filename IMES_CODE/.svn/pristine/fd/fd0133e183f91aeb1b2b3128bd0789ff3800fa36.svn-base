/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: CombinePOInCartonImpl
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================
 * 2010-03-31   ChenXu (itc-208014) Create
 * 2010-04-07   ChenXu (itc-208014) Modify: ITC-1155-0002
 * 2010-04-20   ChenXu              Modify: ITC-1155-0034   (Workflow中增加记录Unpack的机器记录到ProductLog表中 )
 * 2010-05-05   YuanXiaoWei         Modify: ITC-1155-0062   2个workflow，sessionkey有争议,合并Workflow，并修改dll
 * 2010-06-24   ChenXu              Modify: ITC-1155-0202   去掉station=8S的需求定义
 * 2011-03-14   Lucy Liu            Modify: for BN新需求
 * Known issues:
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.MO;
using IMES.FisObject.Common.Model;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.FisObject.PAK.DN;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// CombinePOInCarton
    /// </summary>
    public class CombinePOInCarton : MarshalByRefObject, ICombinePOInCarton
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Common;
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region ICombinePOInCarton Members

        /// <summary>
        /// 刷第一个SN时，调用该方法启动工作流053CombinePOInCarton.xoml，根据输入CustSN获取Model,
        /// 将SN放到Session.CustSN里
        /// 成功后将ProductModel.Model放到Session.FirstProductModel中
        /// 调用getDNList
        /// </summary>
        public IMES.DataModel.ProductModel ScanFirstSN(string firstSN, string line, string editor, string station, string customer)
        {
            logger.Debug("(CombinePOInCartonImpl)ScanFirstSN start, firstSN:" + firstSN + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            logger.Error("------------------ScanFirstSN-----------------firstSN=" + firstSN);
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {

                var currentProduct = CommonImpl.GetProductByInput(firstSN, CommonImpl.InputTypeEnum.CustSN);

                if (!string.IsNullOrEmpty(currentProduct.CartonSN))
                {
                    //需要保证Product的CartonSN无值
                    FisException fe = new FisException("CHK151", new string[] { firstSN });
                    throw fe;
                }
                string sessionKey = currentProduct.ProId;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);
                station = "82";

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
                    RouteManagementUtils.GetWorkflow(station, "053CombinePOInCarton.xoml", "053CombinePOInCarton.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.CustSN, firstSN);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
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
                currentSession.AddValue(Session.SessionKeys.FirstProductModel, currentProduct.Model);

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
                logger.Debug("(CombinePOInCartonImpl)ScanFirstSN end, firstSN:" + firstSN + "line:" + line + "editor:" + editor + "station:" + station + "customer" + customer);
            }
        }

        /// <summary>
        /// 除第一个SN外，每次刷SN都调用该方法，进行SFC,判断Model是否和第一个相同,刷SN返回ProductID
        /// 将uutSN放到Session.CustSN里
        /// </summary>
        public IMES.DataModel.ProductModel ScanSN(string productID, string uutSN)     //接口改为productID
        {
            logger.Debug("(CombinePOInCartonImpl)ScanSN start, productID:" + productID + "uutSN:" + uutSN);
            logger.Error("------------------ScanSN-----------------uutSN=" + uutSN);
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = productID;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.CustSN, uutSN);
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



                    IMES.DataModel.ProductModel currentModel = new IMES.DataModel.ProductModel();

                    var uutProduct = CommonImpl.GetProductByInput(uutSN, CommonImpl.InputTypeEnum.CustSN);
                    currentModel.CustSN = uutProduct.CUSTSN;
                    currentModel.ProductID = uutProduct.ProId;
                    currentModel.Model = uutProduct.Model;

                    return currentModel;
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
                logger.Debug("(CombinePOInCartonImpl)ScanSN end, productID:" + productID + "uutSN:" + uutSN);
            }
        }

        /// <summary>
        /// 刷满了当前Carton，扫入并检查通过的Product数量与设定的PCS相等。调用该方法
        /// 创建Carton号码，更新所有机器状态，记录所有机器Log，返回打印重量标签的PrintItem，结束工作流。
        /// 将deliveryNo放到Session.DeliveryNo里
        /// 将NewScanedProductIDList放到Session.NewScanedProductIDList里
        /// 以deliveryNo为SessionKey创建工作流053CombinePOInCarton.xoml
        /// cartonNo从(List<string>)CurrentSession.GetValue(Session.SessionKeys.CartonNoList)[0]中获得
        /// </summary>
        public void Save(IList<string> NewScanedProductIDList, IList<string> NewScanedModelList, IList<string> custSNList, string line, string editor, string station, string customer)
        {
            logger.Debug("(CombinePOInCartonImpl)Save start, NewScanedProductIDList:" + NewScanedProductIDList +  "line:" + line + "editor:" + editor + "station:" + station + "customer" + customer);
            string[] strs = NewScanedProductIDList.ToArray();          
            string ret = string.Format("'{0}'", string.Join("','", strs));

            string[] strs1 = custSNList.ToArray();
            string ret1 = string.Format("'{0}'", string.Join("','", strs1));
            logger.Error("------------------Save-----------------NewScanedProductIDList=" + ret);
            logger.Error("------------------Save-----------------custSNList=" + ret1);
            logger.Error("------------------Save-----------------PCS=" + NewScanedProductIDList.Count);
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                
               
                string sessionKey = NewScanedProductIDList[0];
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                int pcs = NewScanedProductIDList.Count;

               
                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;

                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.IsComplete, true);
                    currentSession.AddValue(Session.SessionKeys.PCS, pcs);
                    currentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, NewScanedProductIDList);
                    currentSession.AddValue(Session.SessionKeys.NewScanedProductModelList, NewScanedModelList);
                    currentSession.AddValue(Session.SessionKeys.NewScanedProductCustSNList, custSNList);
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

                    
                    //cartonNo = ((List<string>)currentSession.GetValue(Session.SessionKeys.CartonNoList))[0];
                    //currentSession.AddValue(Session.SessionKeys.Carton, cartonNo);
                    //return returnList;
                    
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
                logger.Debug("(CombinePOInCartonImpl)Save end, NewScanedProductIDList:" + NewScanedProductIDList +  "line:" + line + "editor:" + editor + "station:" + station + "customer" + customer);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        public void Cancel(string productID)
        {
            try
            {
                logger.Debug("(CombinePOInCartonImpl)Cancel start, productID:" + productID);

                string sessionKey = productID;
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
                logger.Debug("(CombinePOInCartonImpl)Cancel end, productID:" + productID);
            }
        }

        /// <summary>
        /// 输入的可以是CartonNo，也可以是ProductID,也可以是CustSN
        /// 使用工作流053CombinePOInCartonRePrint.xoml
        /// </summary>
        public IList<IMES.DataModel.PrintItem> ReprintCartonLabel(string ProductIDOrSNOrCartonNo, string line, string editor, string station, string customer, string reason, IList<IMES.DataModel.PrintItem> printItems, out string cartonNo)
        {

            logger.Debug("(CombinePOInCartonImpl)ReprintCartonLabel start, ProductIDOrSNOrCartonNo:" + ProductIDOrSNOrCartonNo + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer + "printItems:" + printItems);

            FisException ex;
            List<string> erpara = new List<string>();

            /// <summary>
            /// 输入的可以是CartonNo，也可以是ProductID,也可以是CustSN
            /// </summary>
            /// 
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(ProductIDOrSNOrCartonNo, CommonImpl.InputTypeEnum.ProductIDOrCustSNOrCarton);
                string sessionKey = currentProduct.ProId;

                ///检查输入合法性,如果是SN,判断Product表对应记录的CartonSN是否为空; 如果是CARTON，Product表是否有CartonSN等于输入Carton号码的记录
                ///业务异常 CHK101: 对应的Product%1 还没有绑定Carton!!
                if (string.IsNullOrEmpty(currentProduct.CartonSN))
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK101", erpara);
                    throw ex;
                }


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
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("053CombinePOInCartonRePrint.xoml", "", wfArguments);


                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.Carton, currentProduct.CartonSN);
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currentProduct.CartonSN);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currentProduct.CartonSN);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "CombinePOInCarton");
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
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

                cartonNo = (string)currentProduct.CartonSN;
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
                logger.Debug("(CombinePOInCartonImpl)ReprintCartonLabel end,  ProductIDOrSNOrCartonNo:" + ProductIDOrSNOrCartonNo + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer + "printItems:" + printItems);
            }
        }

        
        #endregion

       
    }
}
