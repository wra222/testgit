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
using IMES.FisObject.FA.Product;
using IMES.Route;

namespace IMES.Station.Implementation
{
    public class PizzaWeight : MarshalByRefObject, IPizzaWeight 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Product;


        #region PizzaWeight

       /// <summary>
        /// 启动工作流，根据输入productID获取Model和标准重量,成功后调用NeedCheckSN
        /// 如果需要检查的SN，调用CheckSN，否则调用Save
        /// 将actualWeight放到Session.ActuralWeight中，用于检查是否和标准重量相符
        /// 将actualWeight放到Session.CartonWeight中，用于保存CartonWeight重量用
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="actualWeight"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
       public StandardWeight InputUUT(string productID, decimal actualWeight, string line, string editor, string station, string customer,out bool needCheckSN)
        {
            logger.Debug("(PizzaWeight)InputUUT Start,"
                + " [productID]:" + productID
                + " [line]:" + line
                + " [editor]:" + editor
                + " [station]:" + station
                 + " [actualWeight]:" + actualWeight.ToString() 
                + " [customer]:" + customer);
            try
            {
                string sessionKey = productID;
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
                    RouteManagementUtils.GetWorkflow(station, "036PizzaWeight.xoml", "036pizzaweight.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                   // currentSession.AddValue(Session.SessionKeys., currentProduct);
                    currentSession.AddValue(Session.SessionKeys.ActuralWeight, actualWeight);
 
                    currentSession.AddValue(Session.SessionKeys.CartonWeight, actualWeight);
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
                Product currentProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                string modelTolerance = currentSession.GetValue (Session.SessionKeys.Tolerance).ToString();
                decimal standardWeight =(decimal)currentSession.GetValue(Session.SessionKeys.StandardWeight);
                //out parameter
                StandardWeight currentStandardWeight = new StandardWeight();
                currentStandardWeight.Model = currentProduct.Model;
                currentStandardWeight.Weight = standardWeight;
                currentStandardWeight.Tolerance = modelTolerance;

                needCheckSN = false;
                if (String.Compare(currentProduct.ModelObj.Region, "JPN",true) == 0)
                {
                    needCheckSN = true;
                }
             
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
                logger.Debug("(PizzaWeight)InputUUT End,"
                    + " [productID]:" + productID
                    + " [line]:" + line
                    + " [editor]:" + editor
                    + " [station]:" + station
                     + " [actualWeight]:" + actualWeight.ToString()
                    + " [customer]:" + customer);
            }
           
        }

       ///// <summary>
       ///// 根据Model的Region判断是否是JPN
       ///// </summary>
       ///// <param name="productID"></param>
       ///// <returns></returns>
       //public bool NeedCheckSN(string productID)
       //{

       //}

       /// <summary>
       /// 检查CustSn,成功后调用save
       /// </summary>
       /// <param name="productID"></param>
       /// <param name="sn"></param>
       public void CheckSN(string productID, string sn)
       {
           logger.Debug("(PizzaWeight)CheckSN Start,"
             + " [productID]:" + productID
             + " [CustSN]:" + sn);
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
                    currentSession.AddValue(Session.SessionKeys.CustSN, sn);
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

                currentSession.AddValue(Session.SessionKeys.IsComplete, true);
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
                logger.Debug("(PizzaWeight)CheckSN End,"
                  + " [productID]:" + productID
                  + " [CustSN]:" + sn);
            }
       }

        /// <summary>
        /// 保存机器重量，更新机器状态，返回打印重量标签的PrintItem，结束工作流。
        /// 将printItems放到Session.PrintItems中
        /// 将actualWeight放到Session.ActuralWeight中
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public void Save(string productID)
        {
            logger.Debug("(PizzaWeight)Save Start,"
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
                logger.Debug("(PizzaWeight)Save End,"
                    + " [productID]:" + productID);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="productID"></param>
        public void Cancel(string productID)
        {
            logger.Debug("(PizzaWeight)Cancel Start,"
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
                logger.Debug("(PizzaWeight)Cancel End,"
                    + " [productID]:" + productID);
            }

        }
        #endregion


    }
}
