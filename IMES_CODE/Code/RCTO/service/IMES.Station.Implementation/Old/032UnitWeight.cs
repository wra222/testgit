/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:  UnitWeight interface implement
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2010-03-20  Zhao Meili(EB)        Create 
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
using IMES.Route;

namespace IMES.Station.Implementation
{
    public class UnitWeight : MarshalByRefObject, IUnitWeight
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Product;


        #region UnitWeight

        /// <summary>
        /// 此站输入的是SN，需要在BLL中先根据SN获取Product调用CommonImpl.GetProductByInput()
        /// 如果获取不到，报CHK079！
        /// 用ProductID启动工作流
        /// 将获得的Product放到Session.Product中
        /// 获取Model和标准重量和ProductID
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="actualWeight"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="productID"></param>
        /// <returns>StandardWeight对象</returns>
        //public StandardWeight InputUUT(string custSN, string line, string editor, string station, string customer, out string productID)
      //   IList<PrintItem> InputUUT(string custSN, string line, string editor, string station, string customer, decimal actualWeight, IList<PrintItem> printItems, out string productID,out StandardWeight currentStandardWeight)
       public  StandardWeight InputUUT(string custSN, decimal actualWeight, string line, string editor, string station, string customer, out string productID)
        {
            logger.Debug("(UnitWeight)InputUUT Start,"
                + " [custSN]:" + custSN
                + " [line]:" + line
                + " [editor]:" + editor
                + " [station]:" + station
                 + " [actualWeight]:" + actualWeight.ToString() 
                + " [customer]:" + customer);
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                if (currentProduct == null)
                {
                    FisException fe = new FisException("CHK079", new string[] { custSN });
                    throw fe;
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

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "032UnitWeight.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.ActuralWeight, actualWeight);
                    
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

                string modelTolerance = currentSession.GetValue (Session.SessionKeys.Tolerance).ToString();
                decimal standardWeight =(decimal)currentSession.GetValue(Session.SessionKeys.StandardWeight);
                //out parameter
                StandardWeight currentStandardWeight = new StandardWeight();
                currentStandardWeight.Model = currentProduct.Model;
                currentStandardWeight.Weight = standardWeight;
                currentStandardWeight.Tolerance = modelTolerance;
                
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
            logger.Debug("(UnitWeight)InputUUT End,"
                + " [custSN]:" + custSN
                + " [line]:" + line
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);
            }
           
        }

        /// <summary>
        /// 保存机器重量，更新机器状态，返回打印重量标签的PrintItem，结束工作流。
        /// 将printItems放到Session.PrintItems中
        /// 将actualWeight放到Session.ActuralWeight中
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        //public IList<PrintItem> Save(string productID,  IList<PrintItem> printItems)
       public void Save(string productID)
        {
            logger.Debug("(UnitWeight)Save Start,"
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
                    //currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
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

               // return (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

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
                logger.Debug("(UnitWeight)Save End,"
                    + " [productID]:" + productID);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="productID"></param>
        public void Cancel(string productID)
        {
            logger.Debug("(UnitWeight)Cancel Start,"
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
                logger.Debug("(UnitWeight)Cancel End,"
                    + " [productID]:" + productID);
            }

        }
        #endregion


        #region Reprint

        /// <summary>
        /// 重印Unit Weight Label
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public IList<PrintItem> ReprintLabel(string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItemList,out string productID,out string model)
        {
            logger.Debug("(UnitWeight)ReprintLabel Start,"
                            + " [custSN]:" + custSN
                            + " [line]:" + line
                            + " [editor]:" + editor
                            + " [station]:" + station
                            + " [customer]:" + customer);
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                if (currentProduct == null)
                {
                    FisException fe = new FisException("CHK079", new string[] { custSN });
                    throw fe;
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

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("032UnitWeightReprint.xoml", "", wfArguments);
                    currentSession.AddValue(Session.SessionKeys.CustSN, custSN);
                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "");
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItemList);
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
                productID = currentProduct.ProId;
                model = currentProduct.Model;
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
            + " [custSN]:" + custSN
            + " [line]:" + line
            + " [editor]:" + editor
            + " [station]:" + station
            + " [customer]:" + customer);
            }
        }

        #endregion


        #region Modify StandardWeight

        /// <summary>
        /// 输入Model，获取标准重量ModelWeight的UnitWeight栏位
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

                if (currentModelWeight == null || currentModelWeight.UnitWeight == null || currentModelWeight.UnitWeight.Equals(0))
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(Model);

                    throw new FisException("CHK003", errpara);
                }
                else
                {
                    return currentModelWeight.UnitWeight;
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

        /// <summary>
        /// 启动工作流，修改Unit称重的标准重量，对应与ModelWeight表中的UnitWeight栏位
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="NewStandardWeight"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        public void ModifyStandardWeight(string Model, decimal NewStandardWeight, string line, string editor, string station, string customer)
        {
            logger.Debug("(UnitWeight)ReprintLabel Start,"
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

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("032ModifyUnitWeight.xoml", "", wfArguments);
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
                logger.Debug("(UnitWeight)ReprintLabel End,"
                  + " [Model]:" + Model
                     + " [NewStandardWeight]:" + NewStandardWeight.ToString()
                  + " [line]:" + line
                  + " [editor]:" + editor
                  + " [station]:" + station
                  + " [customer]:" + customer);
            }
        }

        #endregion


    }
}
