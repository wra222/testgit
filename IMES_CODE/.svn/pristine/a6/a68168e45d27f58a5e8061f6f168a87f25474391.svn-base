/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:  UnitWeight interface implement
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2010-03-20  Zhao Meili(EB)        Create 
 * 2010-04-14  Lucy Liu              Modify:ITC-1268-0104
 * Known issues:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure;
using IMES.FisObject.PAK.StandardWeight;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using System.Collections.Generic;
using IMES.Route;

namespace IMES.Station.Implementation
{
    public class CartonWeight : MarshalByRefObject, ICartonWeight 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Product;


        /// <summary>
        /// 因为Carton需要根据其中任意一个SN进行SFC，
        /// 所以在本方法中需要先根据Carton号码获取一个Product对像放到Session中
        /// CommonImpl.GetProductByInput(cartonNumber, CommonImpl.InputTypeEnum.Carton)
        /// 将cartonNumber放到Session.Carton
        /// 将CartonWeight放到Session.CartonWeight，用于保存CartonWeight重量用
        /// ProductRepository.GetProductIDListByCarton(string cartonSN)可以获取属于该Carton的所有的ProductID
        /// 根据CartonWeight/Carton中的机器数量获得平均每台机器的ActuralWeight，并放到Session.ActuralWeight，用于检查是否和标准重量相符
        /// 启动工作流，根据输入cartonNumber获取Model和标准重量
        /// </summary>
        /// <param name="cartonNumber"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>StandardWeight对象</returns>
        public StandardWeight InputCarton(string custSN, decimal cartonWeight, string line, string editor, string station, string customer, out string productID,out string cartonSN)
        {
            logger.Debug("(CartonWeight)InputCarton Start,"
                + " [custSN]:" + custSN
                + " [line]:" + line
                + " [editor]:" + editor
                + " [station]:" + station
                 + " [cartonWeight]:" + cartonWeight.ToString()
                + " [customer]:" + customer);
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                if (currentProduct == null)
                {
                    FisException fe = new FisException("CHK079", new string[] { custSN });
                    throw fe;
                }
                //ITC-1268-0104
                //判断cartonsn是否不为空
                if ((currentProduct.CartonSN == null) || (currentProduct.CartonSN.Trim() == string.Empty))
                {
                    throw new FisException("CHK101", new string[] { currentProduct.ProId });
                }
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

                    IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository,  IMES.FisObject.FA.Product.IProduct>();
                    IList<string> productlist = productRepository.GetProductIDListByCarton(currentProduct.CartonSN);
                    decimal actweight = cartonWeight / productlist.Count;
 
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "036CartonWeight.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    currentSession.AddValue(Session.SessionKeys.Carton, currentProduct.CartonSN);
                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.CartonWeight, cartonWeight);
                    currentSession.AddValue(Session.SessionKeys.ActuralWeight, actweight);


                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentProduct.ProId);
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

                string modelTolerance = currentSession.GetValue(Session.SessionKeys.Tolerance).ToString();
                decimal standardWeight = (decimal)currentSession.GetValue(Session.SessionKeys.StandardWeight);
                //out parameter
                StandardWeight currentStandardWeight = new StandardWeight();
                currentStandardWeight.Model = currentProduct.Model;
                currentStandardWeight.Weight = standardWeight;
                currentStandardWeight.Tolerance = modelTolerance;

                cartonSN = currentProduct.CartonSN;
                productID = currentProduct.ProId;
                return currentStandardWeight;
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
                logger.Debug("(CartonWeight)InputCarton End,"
                 + " [custSN]:" + custSN
                 + " [line]:" + line
                 + " [editor]:" + editor
                 + " [station]:" + station
                  + " [cartonWeight]:" + cartonWeight.ToString()
                 + " [customer]:" + customer);
            }
           
        }

        /// <summary>
        /// 保存Carton重量，更新机器状态，返回打印重量标签的PrintItem，结束工作流。
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="printItems"></param>
        /// <param name="cartonSn"></param>
        /// <returns></returns>
        public IList<PrintItem> save(string productID, IList<PrintItem> printItems,out string cartonSn)
        {
            logger.Debug("(CartonWeight)Save Start,"
                + " [productID]:" + productID);
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
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
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
                 var currentProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                 cartonSn = currentProduct.CartonSN;

                return (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

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
                logger.Debug("(CartonWeight)Save End,"
                    + " [productID]:" + productID);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="cartonNumber"></param>
        public void cancel(string productID)
        {
            logger.Debug("(CartonWeight)Cancel Start,"
               + " [productID]:" + productID);
            try
            {

                Session currentSession = SessionManager.GetInstance.GetSession(productID, currentSessionType);

                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
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
                logger.Debug("(CartonWeight)Cancel End,"
                    + " [productID]:" + productID);
            }

        }

        /// <summary>
        /// 重印Carton Weight Label
        /// </summary>
        /// <param name="uutSn"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        public IList<PrintItem> ReprintLabel(string custSNorCartonSn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems, ref string cartonSn)
        {
            logger.Debug("(UnitWeight)ReprintLabel Start,"
                            + " [custSNorCartonSn]:" + custSNorCartonSn
                            + " [line]:" + line
                            + " [editor]:" + editor
                            + " [station]:" + station
                            + " [customer]:" + customer);
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSNorCartonSn, CommonImpl.InputTypeEnum.ProductIDOrCustSNOrCarton);
                if (currentProduct == null)
                {
                    FisException fe = new FisException("CHK079", new string[] { custSNorCartonSn });
                    throw fe;
                }
                //获取cartonsno,设置打印参数
                cartonSn = currentProduct.CartonSN;
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

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("036CartonWeightReprint.xoml", "", wfArguments);
                    //currentSession.AddValue(Session.SessionKeys.CustSN, custSN);
                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "");
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(custSNorCartonSn);
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

                return (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug("(UnitWeight)ReprintLabel End,"
            + " [custSNorCartonSn]:" + custSNorCartonSn
            + " [line]:" + line
            + " [editor]:" + editor
            + " [station]:" + station
            + " [customer]:" + customer);
            }
        }


        /// <summary>
        /// 启动工作流，修改Carton称重的标准重量，对应与ModelWeight表中的CartonWeight栏位
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="NewStandardWeight"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        public void ModifyStandardWeight(string Model, decimal NewStandardWeight, string line, string editor, string station, string customer)
        {
            logger.Debug("(CartonWeight)ReprintLabel Start,"
             + " [Model]:" + Model
                + " [NewStandardWeight]:" + NewStandardWeight.ToString()
             + " [line]:" + line
             + " [editor]:" + editor
             + " [station]:" + station
             + " [customer]:" + customer);
            try
            {
                string sessionKey = Model;
                var sestype = Session.SessionType.Common;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, sestype);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, sestype, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", sestype);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("036ModifyCartonWeight.xoml", "", wfArguments);
                    currentSession.AddValue(Session.SessionKeys.ActuralWeight, NewStandardWeight);
                    currentSession.AddValue(Session.SessionKeys.ModelName, Model);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(Model);
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
                logger.Debug("(CartonWeight)ReprintLabel End,"
                  + " [Model]:" + Model
                     + " [NewStandardWeight]:" + NewStandardWeight.ToString()
                  + " [line]:" + line
                  + " [editor]:" + editor
                  + " [station]:" + station
                  + " [customer]:" + customer);
            }
        }


        /// <summary>
        /// 输入Model，获取标准重量ModelWeight的CartonWeight栏位
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public decimal GetStandardWeight(string Model)
        {
            logger.Debug("(UnitWeight)GetStandardWeight Start,"
               + " [Model]:" + Model);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                IModelWeightRepository currentModelWeightRepository = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository, ModelWeight>();
                ModelWeight currentModelWeight = currentModelWeightRepository.Find(Model);

                if (currentModelWeight == null || currentModelWeight.CartonWeight == null || currentModelWeight.CartonWeight.Equals(0))
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(Model);

                    throw new FisException("CHK003", errpara);
                }
                else
                {
                    return currentModelWeight.CartonWeight ;
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
                logger.Debug("(UnitWeight)GetStandardWeight End,"
                    + " [Model]:" + Model);
            }
        }


    }
}
