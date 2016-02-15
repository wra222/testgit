/*
 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM; 
using IMES.FisObject.Common.Part;   
//using System.Linq;
using System.Data;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
using IMES.DataModel;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.Extend;
using IMES.FisObject.PAK.Pizza;

namespace IMES.Station.Implementation
{

    public class WHInspection : MarshalByRefObject, IWHInspection
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public IList<ConstValueTypeInfo> GetMaterialType(string type)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                IList<ConstValueTypeInfo> retLst = new List<ConstValueTypeInfo>();
                retLst = itemRepository.GetConstValueTypeList(type);
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

    
		/// <summary>
        /// InputMaterialCT
		/// </summary>
        /// <param name="materialct">string</param>
        /// <param name="materialtype">string</param>
        /// <param name="pdLine">string</param>
        /// <param name="editor">string</param>
        /// <param name="stationId">string</param>
        /// <param name="customerId">string</param>
		/// <returns></returns>
        public ArrayList InputMaterialCT(string materialct, string materialtype, string pdLine, string editor, string stationId, string customerId)
		{


            logger.Debug("(WHInspection)InputMaterialCT start, materialct:" + materialct + " materialtype:" + materialtype + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = materialct;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "WHInspection.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.MaterialCT, materialct);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }
                    throw currentSession.Exception;
                }
                ArrayList arr = new ArrayList();
                arr.Add(materialct);
                return arr;
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
                logger.Debug("(WHInspection)InputMaterialCT end, materialct:" + materialct + " materialtype:" + materialtype + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
            
        }

        //public void Save(string materialct)
        //{
        //    logger.Debug("(WHInspection)Save Start,"
        //        + " [materialct]:" + materialct);
        //    FisException ex;
        //    List<string> erpara = new List<string>();
        //    try
        //    {
        //        Session currentSession = SessionManager.GetInstance.GetSession(materialct, Session.SessionType.Product);

        //        if (currentSession == null)
        //        {
        //            erpara.Add(materialct);
        //            ex = new FisException("CHK021", erpara);
        //            logger.Error(ex.Message, ex);
        //            throw ex;
        //        }
        //        else
        //        {

                   
        //            currentSession.AddValue(Session.SessionKeys.IsComplete, true);
        //            currentSession.Exception = null;
        //            currentSession.SwitchToWorkFlow();
        //        }

        //        if (currentSession.Exception != null)
        //        {
        //            if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
        //            {
        //                currentSession.ResumeWorkFlow();
        //            }

        //            throw currentSession.Exception;
        //        }

        //    }
        //    catch (FisException e)
        //    {
        //        logger.Error(e.mErrmsg);
        //        throw e;
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e.Message);
        //        throw new SystemException(e.Message);
        //    }
        //    finally
        //    {
        //        logger.Debug("(WHInspection)Save End,"
        //            + " [materialct]:" + materialct);
        //    }
        //}

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

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
                logger.Debug("Cancel end, sessionKey:" + sessionKey);
            }
        }
    }
}
