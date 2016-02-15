

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Station.Implementation;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMaterialReturn接口的实现类
    /// </summary>
    public class MaterialReturn : MarshalByRefObject, IMaterialReturn
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Common;
        private const string WFfile = "MaterialReturn.xoml";
        private const string Rulesfile = "MaterialReturn.rules";

        public IList<ConstValueTypeInfo> GetMaterialType(string type, string value)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                IList<ConstValueTypeInfo> retLst = new List<ConstValueTypeInfo>();
                if (string.IsNullOrEmpty(value))
                {
                    retLst = itemRepository.GetConstValueTypeList(type);
                }
                else
                {
                    retLst = itemRepository.GetConstValueTypeList(type,value);
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ArrayList InputMaterialCTFirst(string materialCT, string materialType, string line, string editor, string station, string customer)
        {
            logger.Debug("(MaterialReturn)InputMaterialCTFirst start, MaterialCT:" + materialCT + "MaterialType:" + materialType
                                        + "Line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            try 
            {
                ArrayList retLst = new ArrayList();

                string sessionKey = Guid.NewGuid().ToString();
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
                    RouteManagementUtils.GetWorkflow(station, WFfile, Rulesfile, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    IList<string> materialCTList = new List<string>();
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.AddValue(Session.SessionKeys.MaterialCT, materialCT);
                    currentSession.AddValue(Session.SessionKeys.MaterialCTList, materialCTList);
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
                retLst.Add(materialCT);
                retLst.Add(sessionKey);

                return retLst;
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
                logger.Debug("(MaterialReturn)InputMaterialCTFirst end,  MaterialCT:" + materialCT + "MaterialType:" + materialType
                                            + "Line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }

        public ArrayList InputMaterialCT(string materialCT, string sessionKey)
        {
            logger.Debug("(MaterialReturn)InputMaterialCT start, MaterialCT:" + materialCT);
            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retLst = new ArrayList();

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                session.AddValue(Session.SessionKeys.MaterialCT, materialCT);
                session.AddValue(Session.SessionKeys.IsComplete, false);
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
                retLst.Add(materialCT);
                retLst.Add(sessionKey);
                return retLst;
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
                logger.Debug("(MaterialReturn)InputMaterialCT end,  MaterialCT:" + materialCT);
            }
        }

        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="prodId"></param>
        public void Save(string sessionKey)
        {
            logger.Debug("(MaterialReturn)save start," + " [sessionKey]: " + sessionKey);
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }

                session.AddValue(Session.SessionKeys.IsComplete, true);
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
                logger.Debug("(MaterialReturn)save end," + " [sessionKey]: " + sessionKey);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        public void Cancel(string sessionKey)
        {
            logger.Debug("(MaterialReturn)Cancel start, [sessionKey]:" + sessionKey);
            

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
                throw;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(MaterialReturn)Cancel end, [sessionKey]:" + sessionKey);
            }
        }

    }
}
