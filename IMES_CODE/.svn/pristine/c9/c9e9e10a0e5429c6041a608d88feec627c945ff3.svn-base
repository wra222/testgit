/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: FACosmetic
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-03-30   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.PCA.MB;
using log4net;
using IMES.Route;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// IFACosmetic接口的实现类
    /// </summary>
    public class FACosmeticImpl : MarshalByRefObject, IFACosmetic
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region IFACosmetic members

        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>prestation</returns>
        public string InputProdId(string pdLine, string custsn, string editor, string stationId, string customer)
        {
            logger.Debug("(FACosmeticImpl)InputProdId start, [pdLine]:" + pdLine
                + " [custsn]: " + custsn
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            // Add in 05/04 Convert Custsn to ProID
               //var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
               //var currentProduct = productRepository.GetProductByIdOrSn(custsn);
               // string prodId ;
               // //==================================
               // if (currentProduct == null)
               // {
               //     erpara.Add(custsn);
               //     throw new FisException("SFC011", erpara);
               // }
               // else
               // {
               //     prodId = currentProduct.ProId;
               // }
            // Add in 05/04
             string sessionKey = custsn;
             
            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                var currentProduct = productRepository.GetProductByIdOrSn(custsn);

                if (currentProduct == null)
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(custsn);

                    throw new FisException("SFC013", errpara);


                }

                if (session == null)
                {
                    session = new Session(sessionKey, ProductSessionType, editor, stationId, pdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "099FACosmetic.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);


                    session.AddValue(Session.SessionKeys.IsComplete, false);
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

                return string.Empty;
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
                logger.Debug("(FACosmeticImpl)InputProdId end, [pdLine]:" + pdLine
                    + " [prodId]: " + custsn
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// 不良品信息。
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="defectList">Defect IList</param>
        public void InputDefectCodeList(string prodId, IList<string> defectList)
        {
            logger.Debug("(FACosmeticImpl)InputDefectCodeList start,"
                + " [prodId]: " + prodId
                + " [defectList]:" + defectList);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                else
                {
                    session.AddValue(Session.SessionKeys.DefectList, defectList);

                    session.Exception = null;
                    session.SwitchToWorkFlow();

                    //check workflow exception
                    if (session.Exception != null)
                    {
                        if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            session.ResumeWorkFlow();
                        }

                        throw session.Exception;
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
                logger.Debug("(FACosmeticImpl)InputDefectCodeList end,"
                   + " [prodId]: " + prodId
                   + " [defectList]:" + defectList);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            logger.Debug("(FACosmeticImpl)Cancel start, [prodId]:" + sessionKey);
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
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
                logger.Debug("(FACosmeticImpl)Cancel end, [prodId]:" + sessionKey);
            }
        }

        #endregion
    }
}