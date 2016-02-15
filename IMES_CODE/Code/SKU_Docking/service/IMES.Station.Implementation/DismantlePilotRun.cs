using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.PrintLog;
using IMES.Route;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.MO;



namespace IMES.Station.Implementation
{

    /// <summary>
    /// </summary>
    public partial class DismantlePilotRun : MarshalByRefObject, IDismantlePilotRun
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region IDismantlePilotRunt Members
        /// <summary>
        /// </summary>                
        public ArrayList GetPilotMoInfo(string sn, string stage, string customer, string station, string user)
        {
            logger.Debug("(DismantlePilotRun)CheckWC Start:" + sn);

            ArrayList retValue = new ArrayList();
            string currentSessionKey = sn;

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Common);
                if (currentSession == null)
                {
                    currentSession = new Session(currentSessionKey, Session.SessionType.Common, user, station, "", customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", user);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Common);
                    currentSession.AddValue("Stage", stage);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("DismantlePilotRun.xoml", "", wfArguments);
               

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
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
                    erpara.Add(currentSessionKey);
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
                PilotMoInfo poIn =(PilotMoInfo) currentSession.GetValue(Session.SessionKeys.PilotMo);
                retValue.Add(poIn);
                retValue.Add((string)currentSession.GetValue(Session.SessionKeys.MBSN) ?? "");
                retValue.Add((string)currentSession.GetValue(Session.SessionKeys.ProductID) ?? "");
                retValue.Add((string)currentSession.GetValue(Session.SessionKeys.CustSN) ?? "");
                return retValue;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(DismantlePilotRun)GetPilotMoInfo End:" + sn);
            }
        }


        public void Dismantle(string sn)
        {
            logger.Debug("(DismantlePilotRun)Dismantle, [key]:" + sn);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = sn;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    session.Exception = null;
                    session.SwitchToWorkFlow();
                    if (session.Exception != null)
                    {
                        if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            session.ResumeWorkFlow();
                        }

                        throw session.Exception;
                    }
                }
                IMORepository iMORepository = RepositoryFactory.GetInstance().GetRepository<IMORepository>();
                PilotMoInfo poInfo = (PilotMoInfo)session.GetValue(Session.SessionKeys.PilotMo);
                poInfo.combinedQty = poInfo.combinedQty - 1;
                if (poInfo.combinedQty == 0)
                {
                    poInfo.combinedState = "Empty";
                }
                else
                {
                    poInfo.combinedState = "Partial";
                }
                poInfo.udt = DateTime.Now;
                poInfo.editor = session.Editor;
                iMORepository.UpdatePilotMo(poInfo);
                string stage = (string)session.GetValue("Stage");
                IList<string> lst = new List<string>();
                lst.Add("PilotMo");
                if (stage == "SA" || stage == "SMT")
                {
                    IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                    mbRepository.RemovePCBInfosByType(sn, lst);
                }
                else
                {
                    var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    productRepository.RemoveProductInfosByType((String)session.GetValue(Session.SessionKeys.ProductID), lst);
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
                logger.Debug("(DismantlePilotRun)Dismantle end, [key]:" + sn);
            }
        
        
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public IList<string> GetStage()
        {
            ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
           return   lineRepository.GetAllStage();
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn">uutSn</param>
        public void Cancel(string sn)
        {
            logger.Debug("(DismantlePilotRun)Cancel Start," + "SN:" + sn);
            try
            {
                string sessionKey =sn;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);

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
                logger.Debug("(DismantlePilotRun)Cancel End," + "SN:" + sn);
            }

        }


        #endregion
    }

}
