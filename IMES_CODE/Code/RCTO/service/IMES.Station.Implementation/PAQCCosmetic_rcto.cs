using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
//using IMES.Station.Interface.CommonIntf;
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
using IMES.FisObject.Common.Defect;


namespace IMES.Station.Implementation
{

    /// <summary>
    /// </summary>
    public partial class PAQCCosmetic_rcto : MarshalByRefObject, IPAQCCosmetic_rcto
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region IPAQCCosmetic_rcto Members

        /// <summary>
        /// </summary>                
        public ArrayList ProcessInput(string prodid, string flag, string pdline, string station, string editor, string customer)
        {
            logger.Debug("PAQCCosmetic(ProcessInput) start:" + pdline + "," + prodid);
            ArrayList retValue = new ArrayList();
            
            string currentSessionKey = prodid.Substring(0, 9);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Product);
                if (currentSession == null)
                {
                    currentSession = new Session(currentSessionKey, Session.SessionType.Product, editor, station, pdline, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PAQCCosmetic_rcto.xoml", "PAQCCosmetic_rcto.rules", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.ifElseBranch, flag);
                    currentSession.AddValue(Session.SessionKeys.InfoValue, "0");
                    currentSession.AddValue(Session.SessionKeys.DefectList, new List<string>());
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

                var currentProduct = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);

                string isPaqc = (string)currentSession.GetValue("GoodSample");

                retValue.Add(currentProduct.CUSTSN);
                retValue.Add(currentProduct.Model);
                retValue.Add(currentProduct.ProId);
                retValue.Add("0");
                retValue.Add(isPaqc);

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
                logger.Debug("PAQCCosmetic(ProcessInput) end:" + pdline + "," + prodid);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("PAQCCosmetic(Cancel) start, sessionKey:" + sessionKey);
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
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
                logger.Debug("PAQCCosmetic(Cancel) start, sessionKey:" + sessionKey);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ArrayList GetDefectInfo(string code)
        {
            logger.Debug("(PAQCCosmetic)GetDefectInfo start, [DefectCode]:" + code);
            ArrayList retValue = new ArrayList();
            try
            {
                IDefectRepository defectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository, Defect>();

                string desc = string.Empty;
                IList<DefectCodeInfo> defectcodeLst = new List<DefectCodeInfo>();
                DefectCodeInfo cond = new DefectCodeInfo();
                cond.Defect = code;
                defectcodeLst = defectRepository.GetDefectCodeInfoList(cond);
                if (defectcodeLst != null && defectcodeLst.Count > 0)
                {
                    desc = defectcodeLst[0].Descr;
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    ex = new FisException("CHK908", erpara);
                    throw ex;
                }

                retValue.Add(desc);
                return retValue;
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
                logger.Debug("(PAQCCosmetic)GetDefectInfo end, [DefectCode]:" + code);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="list"></param>
        public ArrayList Save(string key, IList<string> list)
        {
            logger.Debug("(PAQCCosmetic)Save start, [key]:" + key);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = key;

            try
            {
                ArrayList ret = new ArrayList();
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    if (list != null && list.Count > 0)
                    {
                        session.AddValue(Session.SessionKeys.InfoValue, "1");
                    }
                    else
                    {
                        session.AddValue(Session.SessionKeys.InfoValue, "0");
                    }
                    session.AddValue(Session.SessionKeys.DefectList, list);
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
                string isGood = (string)session.GetValue(Session.SessionKeys.InfoValue);

                string isPaqc = (string)session.GetValue("GoodSample");

                ret.Add(isGood);
                ret.Add(isPaqc);
                return ret;
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
                logger.Debug("(PAQCCosmetic)Save end, [key]:" + key);
            }
        }

        #endregion
    }
}
