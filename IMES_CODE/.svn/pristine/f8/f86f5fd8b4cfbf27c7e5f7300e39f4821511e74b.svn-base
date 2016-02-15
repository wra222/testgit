/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: OQCOutputImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-08-21   itc202017     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using log4net;
using IMES.Route;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Line;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IOQCOutput接口的实现类
    /// </summary>
    public class _OQCOutputImpl : MarshalByRefObject, IOQCOutput 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region IOQCOutput members

        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        public List<string> InputProduct(string input, bool isID, string editor, string station, string customer)
        {
            logger.Debug("(_OQCOutputImpl)InputProdId start, [input]: " + input);
            FisException ex;
            List<string> errpara = new List<string>();
            List<string> retvaluelist = new List<string>();
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            string sessionKey = "";

            if (!isID)
            {
                var prod = productRepository.GetProductInfoByCustomSn(input);
                if (prod.id == null || prod.id == "")
                {
                    errpara.Add(input);
                    throw new FisException("CHK079", errpara);
                }
                sessionKey = prod.id;
            }
            else
            {
                sessionKey = input;
            }

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, ProductSessionType, editor, station, "", customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "OQCOutput.xoml", "OQCOutput.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(session))
                    {
                        session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        errpara.Add(sessionKey);
                        ex = new FisException("CHK020", errpara);
                        throw ex;
                    }

                    session.WorkflowInstance.Start();
                    session.SetHostWaitOne();
                }
                else
                {
                    errpara.Add(sessionKey);
                    ex = new FisException("CHK020", errpara);
                    throw ex;
                }

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }

                ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
                IProduct product = (IProduct)session.GetValue(Session.SessionKeys.Product);
                string qcStatus = (string)session.GetValue(Session.SessionKeys.QCStatus);
                retvaluelist.Add(product.ProId);
                retvaluelist.Add(product.Model);
                retvaluelist.Add(product.Status.Line + " " + lineRepository.Find(product.Status.Line).Descr);
                retvaluelist.Add(qcStatus);
                return retvaluelist;  
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
                logger.Debug("(_OQCOutputImpl)InputProdId end, [input]: " + input);
            }
        }

        /// <summary>
        /// 记录良品/不良品信息
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="defectList">Defect IList</param>
        public void InputDefectCodeList(string prodId, IList<string> defectList)
        {
            logger.Debug("(_OQCOutputImpl)InputDefectCodeList start, [prodId]: " + prodId);
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
                    throw ex;
                }
                else
                {
                    session.AddValue(Session.SessionKeys.DefectList, defectList);
                    session.AddValue(Session.SessionKeys.HasDefect, defectList.Count > 0);

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
                logger.Debug("(_OQCOutputImpl)InputDefectCodeList end, [prodId]: " + prodId);           
            }
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string prodId)
        {
            logger.Debug("(_OQCOutputImpl)Cancel start, [prodId]:" + prodId);
            string sessionKey = prodId;

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
                logger.Debug("(_OQCOutputImpl)Cancel end, [prodId]:" + prodId);
            } 
        }

        #endregion
    }
}
