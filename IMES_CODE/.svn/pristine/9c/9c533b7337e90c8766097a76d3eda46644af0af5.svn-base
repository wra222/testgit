/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Implementation for PAQC Output For RCTO Page
* UI:CI-MES12-SPEC-PAK-UI PAQC Output_RCTO.docx –2012/6/11 
* UC:CI-MES12-SPEC-PAK-UC PAQC Output_RCTO.docx –2012/7/10            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-07-25   Jessica Liu           Create
* Known issues:
* TODO：
* ITC-1428-0065, Jessica Liu, 2012-9-17
*/

using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Line;
using IMES.Route;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// PAQCOutputForRCTO接口的实现类
    /// </summary>
    public class PAQCOutputForRCTO : MarshalByRefObject, IPAQCOutputForRCTO
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;

        #region members

        /// <summary>
        /// 刷uutSn，启动工作流，检查输入的uutSn，卡站，获取ProductModel
        /// </summary>
        /// <param name="uutSn"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>ProductModel</returns>
        public ArrayList InputSN(string uutSn, string line, string editor, string station, string customer)
        {
            logger.Debug("(PAQCOutputImpl)InputSN start, uutSn:" + uutSn);

            try
            {
                //var currentProduct = CommonImpl.GetProductByInput(uutSn, CommonImpl.InputTypeEnum.CustSN);
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                var currentProduct = productRepository.GetProductByIdOrSn(uutSn);
               
                FisException ex;
                List<string> erpara = new List<string>();
                if (currentProduct == null)
                {
                    erpara.Add(uutSn);
                    throw new FisException("SFC002", erpara);
                }

                string needCheckMB = "Y";
                if (string.IsNullOrEmpty(currentProduct.PCBID)) // currentProduct.Model.StartsWith("146")
                {
                    //throw new FisException("CQCHK1060", new string[] { });
                    needCheckMB = "N";
                }

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
                    RouteManagementUtils.GetWorkflow(station, "PAQCOutputForRCTO.xoml", "PAQCOutputForRCTO.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        //FisException ex;
                        //List<string> erpara = new List<string>();
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    //FisException ex;
                    //List<string> erpara = new List<string>();
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
                //==============================================================================
                ArrayList retList = new ArrayList();
                //Get infomation
                Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                IMES.DataModel.ProductModel currentModel = new IMES.DataModel.ProductModel();
                
                //a.	如果Product 在QCStatus 中不存在记录，则报告错误：“此Product 在QCStatus 中不存在记录，请联系相关人员”

                if (curProduct.QCStatus.Count == 0)
                {
                    //ITC-1414-0077, Jessica Liu, 2012-6-6
                    cancel(sessionKey);

                    //FisException ex;
                    //List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("PAK110", erpara);//此Product 在QCStatus 中不存在记录，请联系相关人员
                    throw ex;

                }
                
                currentModel.CustSN = curProduct.CUSTSN;
                currentModel.ProductID = curProduct.ProId;
                currentModel.Model = curProduct.Model;

                retList.Add(currentModel);


                //select b.Line + ' ' + b.Descr from ProductStatus a (nolock), Line b (nolock)
	            //WHERE a.ProductID = @ProductId
		        //AND a.Line = b.Line
                ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
                Line pdline = lineRepository.Find(curProduct.Status.Line);

                string strline ="";
                strline = pdline.Id + " " + pdline.Descr;
                retList.Add(strline);

                retList.Add(needCheckMB);

                //===============================================================================

                return retList;

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
                logger.Debug("(PAQCOutputImpl)InputSN end, uutSn:" + uutSn);
            }
        }


        /// <summary>
        /// check MBSno
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="prodId"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        //2012-7-20
        public void checkMBSno(string mbSno, string prodId, string line, string editor, string station, string customer)
        {
            logger.Debug("(PAQCOutputImpl)checkMBSno start, [mbSno]:" + mbSno + " [prodId]: " + prodId);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                //Jessica Liu, 2012-9-17
                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    Product curProduct = (Product)session.GetValue(Session.SessionKeys.Product);

                    if (mbSno != curProduct.PCBID && mbSno != curProduct.MAC)
                    {
                        //ITC-1428-0065, Jessica Liu, 2012-9-17
                        //cancel(curProduct.ProId);

                        erpara.Add(sessionKey);
                        throw new FisException("CHK933", erpara);   //刷入的Product Id与MB S/N不符合，请检查!!
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
                logger.Debug("(PAQCOutputImpl)checkMBSno end, [mbSno]:" + mbSno + " [prodId]: " + prodId);
            }
        }


        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="prodId"></param>
        public void save(string prodId, IList<string> defectCodeList)
        {
            logger.Debug("(PAQCOutputImpl)save start,"
                + " [prodId]: " + prodId
                + " [defectList]:" + defectCodeList);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

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
                    //ITC-1428-0021, Jessica Liu, 2012-9-12
                    //session.AddValue(Session.SessionKeys.DefectList, defectCodeList);
                    if (defectCodeList == null || defectCodeList.Count == 0)
                    {
                        session.AddValue(Session.SessionKeys.DefectList, new List<string>());
                    }
                    else
                    {
                        session.AddValue(Session.SessionKeys.DefectList, defectCodeList);
                    }
                    session.AddValue(Session.SessionKeys.HasDefect, (defectCodeList != null && defectCodeList.Count != 0) ? true : false);
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
                logger.Debug("(PAQCOutputImpl)save end,"
                   + " [prodId]: " + prodId
                   + " [defectList]:" + defectCodeList);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        public void cancel(string prodId)
        {
            logger.Debug("(PAQCOutputImpl)Cancel start, [prodId]:" + prodId);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

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
                logger.Debug("(PAQCOutputImpl)Cancel end, [prodId]:" + prodId);
            } 
        }
        #endregion

    }
}
