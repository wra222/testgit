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

    public class CheckDoorSn : MarshalByRefObject, ICheckDoorSn
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        


    
		/// <summary>
        /// SN Check初次输入SN处理
        /// </summary>
        /// <param name="inputSn">custsn</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>ProductID</returns>
        public ArrayList InputCustSN(string custsn, string pdLine, string editor, string stationId, string customerId)
		{


            logger.Debug("(CheckDoorSn)InputCustSNOnProduct start, inputCustsn:" + custsn + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                //bool IsChinaEnergyLabel = false;
                string sessionKey = custsn;
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
                    RouteManagementUtils.GetWorkflow(stationId, "CheckDoorSn.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    currentSession.AddValue(Session.SessionKeys.CustSN, custsn);
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

                Product product = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                //IList<string> lstPn=
                //        product.ProductParts.Where(x => x.CheckItemType == "ServiceDoor").Select(x=>x.PartSn).ToList();
                //// txtName.Text.Trim().Length == 0 ? null : txtName.Text.Trim()
                //string doorSn = lstPn.Count > 0 ? lstPn[0].Trim() : "";
                //ArrayList arr = new ArrayList();
                //arr.Add(doorSn);
                //return arr;
                IList<string> lstPn =
                 product.ProductParts.Where(x => x.CheckItemType == "ServiceDoor").Select(x => x.PartSn).ToList();
                // txtName.Text.Trim().Length == 0 ? null : txtName.Text.Trim()
                string doorSn = lstPn.Count > 0 ? lstPn[0].Trim() : "";
                ArrayList arr = new ArrayList();
                if (doorSn != "")
                {
                    arr.Add(doorSn);
                }
                IList<string> lstPn_HDD =
                     product.ProductParts.Where(x => x.CheckItemType == "HDDFrame").Select(x => x.PartSn).ToList();
                string FrameSn = lstPn_HDD.Count > 0 ? lstPn_HDD[0].Trim() : "";
                if (FrameSn != "")
                {
                    arr.Add(FrameSn);
                }
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
                logger.Debug("(SNCheck)InputCustSNOnProduct end, inputSn:" + custsn + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
            
        }

        

        public void Save(string custsn)
        {
            logger.Debug("(CheckDoorSn)Save Start,"
                + " [Save]:" + custsn);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(custsn, Session.SessionType.Product);

                if (currentSession == null)
                {
                    erpara.Add(custsn);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {

                   
                    currentSession.AddValue(Session.SessionKeys.IsComplete, true);
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
                logger.Debug("(CheckDoorSn)Save End,"
                    + " [CUSTSN]:" + custsn);
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
