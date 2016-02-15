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

using System.Data;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.Extend;

namespace IMES.Station.Implementation
{
    public class CheckAst : MarshalByRefObject, ICheckAst
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        private IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        private IMiscRepository miscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
        /// <summary>
        /// 获取Product的Model和BOM信息
        /// </summary>
        public ArrayList InputProductIDorCustSN(string input, string line, string editor, string station, string customer, bool bquery, string iamge1ATtype, string iamge2ATtype, out string prodID, out string model)
        {
            logger.Debug("(_ASTCheck)InputProductIDorCustSN start, input:" + input + " pdLine:" + line + " editor:" + editor + " stationId:" + station + " customerId:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();


            try
            {
                var currentProduct = CommonImpl.GetProductByInput(input, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                string sessionKey = currentProduct.ProId;
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
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("CheckAST.xoml", "CheckAST.rules", wfArguments);
                    currentSession.AddValue("bOnlyQuery", false);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.AddValue("Image1ASTType", iamge1ATtype);
                    currentSession.AddValue("Image2ASTType", iamge2ATtype);

                    if (bquery)
                    {
                        currentSession.AddValue(Session.SessionKeys.IsComplete, true);
                        currentSession.AddValue("bOnlyQuery", true);
                    }
                    currentSession.SetInstance(instance);
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
                 string   image1scr = (string)currentSession.GetValue("Image1Src");
                 string   image2scr = (string)currentSession.GetValue("Image2Src");
                IFlatBOM CurrenBom = (IFlatBOM)currentSession.GetValue(Session.SessionKeys.SessionBom);
               // IList<IMES.DataModel.BomItemInfo> ret = new List<BomItemInfo>();
                ArrayList ret = new ArrayList();
                ret.Add(image1scr);
                ret.Add(image2scr);
                if (CurrenBom != null)
                {
                    ret.Add(CurrenBom.ToBOMItemInfoList());
                }
                else
                {
                    ret.Add(null);
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
                logger.Debug("(_ASTCheck)InputProductIDorCustSN end, input:" + input + " pdLine:" + line + " editor:" + editor + " stationId:" + station + " customerId:" + customer);
            }
        }

        public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue)
        {
            return PartCollection.TryPartMatchCheck(sessionKey, TheType, checkValue);
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
        /// Save
        /// </summary>
        public void Save(string sesKey)
        {
            logger.Debug("(_CheckAst)save start");
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sesKey, TheType);
                if (session == null)
                {
                    erpara.Add(sesKey);
                    ex = new FisException("CHK194", erpara);
                    throw ex;
                }
                session.AddValue(Session.SessionKeys.IsComplete, true);

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
                //CheckEPIA = (string)session.GetValue(ExtendSession.SessionKeys.FAQCStatus);
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
                logger.Debug("(_CheckAst)save end.");
            }
        }
    }
}
