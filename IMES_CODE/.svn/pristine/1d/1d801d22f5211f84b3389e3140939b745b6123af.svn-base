/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:IMES service implement for AFTMVS Page
 *             
 * UI:CI-MES12-SPEC-FA-UI AFT MVS.docx –2011/10/10 
 * UC:CI-MES12-SPEC-FA-UC AFT MVS.docx –2011/10/25            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-28  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using log4net;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for AFT MVS.
    /// </summary>
    public class _AFTMVS : MarshalByRefObject, IAFTMVS
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        private IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        private IMiscRepository miscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();

        #region IAFTMVS Members
        
        /// <summary>
        /// 获取Product的Model和BOM信息
        /// </summary>
        public IList<BomItemInfo> InputProductIDorCustSN(string input, string line, string editor, string station, string customer, out string prodID, out string model)
        {
            logger.Debug("(_AFTMVS)InputProductIDorCustSN start, input:" + input + " pdLine:" + line + " editor:" + editor + " stationId:" + station + " customerId:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();


            try
            {
                string sessionKey = input;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("SaveProductQC.xoml", "SaveProductQC.rules", wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
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

                prodID = ((Product)currentSession.GetValue(Session.SessionKeys.Product)).ProId;
                model = ((Product)currentSession.GetValue(Session.SessionKeys.Product)).Model;
                IList<BomItemInfo> bom = currentSession.GetValue(Session.SessionKeys.SessionBom) as List<BomItemInfo>;
                IList<BomItemInfo> ret = new List<BomItemInfo>();

                BomItemInfo item = new BomItemInfo();
                item.type = "Customer SN";
                item.description = "Customer SN";
                item.PartNoItem = "";
                item.qty = 1;
                ret.Add(item);

                BomItemInfo item1 = new BomItemInfo();
                item1.type = "Product ID";
                item1.description = "Product ID";
                item1.PartNoItem = "";
                item1.qty = 1;
                ret.Add(item1);

                foreach (BomItemInfo tmp in bom)
                {
                    ret.Add(tmp);
                }
                return ret;
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
                logger.Debug("(_AFTMVS)InputProductIDorCustSN end, input:" + input + " pdLine:" + line + " editor:" + editor + " stationId:" + station + " customerId:" + customer);
            }
        }

        /// <summary>
        /// 处理输入资产标签，返回结合的PartNo
        /// </summary>
        public string InputASTLabel(string sesKey, string ast)
        {
            logger.Debug("(_AFTMVS)InputASTLabel start, ast:" + ast);

            FisException ex;
            List<string> erpara = new List<string>();


            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(sesKey, TheType);

                if (currentSession == null)
                {
                    erpara.Add(sesKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }

                currentSession.AddValue(Session.SessionKeys.VCode, ast);

                currentSession.SwitchToWorkFlow();

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                return currentSession.GetValue(Session.SessionKeys.MatchedCheckItem) as string;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_AFTMVS)InputASTLabel end, ast:" + ast);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        public void Cancel(string sesKey)
        {
            logger.Debug("(_AFTMVS)Cancel start.");

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sesKey, TheType);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(_AFTMVS)Cancel end.");
            }
        }

        /// <summary>
        /// 获取当前Product的QC方法信息
        /// </summary>
        public string GetQCMethod(string sesKey, out string wc)
        {
            FisException ex;
            List<string> erpara = new List<string>();


            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(sesKey, TheType);

                if (currentSession == null)
                {
                    erpara.Add(sesKey);
                    ex = new FisException("CHK194", erpara);
                    throw ex;
                }

                currentSession.AddValue(Session.SessionKeys.IsComplete, true);

                currentSession.SwitchToWorkFlow();

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                string strNewStation = currentSession.GetValue(Session.SessionKeys.RandomInspectionStation) as string;
                wc = strNewStation;
                FisException normalException;
                if (strNewStation == "73") //ret = 1; //Real EPIA
                {
                    normalException = new FisException("QCM073", new string[] { });
                }
                else if (strNewStation == "6A") //ret = 2; //EPIA
                {
                    normalException = new FisException("QCM06A", new string[] { });
                }
                else if (strNewStation == "79") //ret = 3; //Notcheck
                {
                    normalException = new FisException("QCM079", new string[] { });
                }
                else
                {
                    normalException = new FisException("QCMERR", new string[] { });
                }
                return normalException.mErrmsg;
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
        }

        /// <summary>
        /// 获取指定PdLine的QC统计信息
        /// </summary>
        public void GetQCStatics(string pdLine, out int piaCnt, out int epiaCnt, out int passCnt)
        {
            try
            {
                int sum1 = 0;
                int sum2 = 0;
                int sum3 = 0;

                IList<QCStatisticInfo> infoLst = miscRepository.GetQCStatisticList(pdLine); 
                
                foreach (QCStatisticInfo ele in infoLst)
                {
                    sum1 += ele.noCheck;
                    sum2 += ele.piaIn;
                    sum3 += ele.epiaIn;
                }

                passCnt = sum1;
                piaCnt = sum2;
                epiaCnt = sum3;
                return;
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
        }

        /// <summary>
        /// 获取ESOP文件名列表
        /// </summary>
        public IList<string> GetESOPList(string model)
        {
            try
            {
                IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                return bomRep.GetESOPListByModel(model);
                /*
                IList<string> ret = new List<string>();
                ret.Add("esop1.jpg");
                ret.Add("esop2.jpg");
                ret.Add("esop3.jpg");
                return ret;
                */
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
        }
        #endregion
    }
}
