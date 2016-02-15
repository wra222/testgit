/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Implementation for BT Change Page
 * UI:CI-MES12-SPEC-PAK-BT_CHANGE.docx –2011/10/28 
 * UC:CI-MES12-SPEC-PAK-BT_CHANGE.docx –2011/10/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-28   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0016
*/

using System;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Linq;
namespace IMES.Station.Implementation
{

    public class BTChange : MarshalByRefObject, IBTChange
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        #region IKPPrint Members


        /// <summary>
        /// 检查输入Model的合法性
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param> 
        public void InputModel(string model, string pdLine, string editor, string stationId, string customerId)
		{
/*//test，测试流程是否通，需删除========
return;
//test，测试流程是否通，需删除========*/

            logger.Debug("(BTChange)InputModel start, model:" + model + " pdLine:" + pdLine + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = model;
             
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;

                    RouteManagementUtils.GetWorkflow(stationId, "BTChange.xoml", "BTChange.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.ModelName, model);

                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    
                    Session.WorkflowInstance.Start();
                    Session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }
                    throw Session.Exception;
                }
 
                return;
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
                logger.Debug("(BTChange)InputModel end, model:" + model + " pdLine:" + pdLine + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            }
            
        }


        /// <summary>
        /// 将属于所输Model的 符合条件的Product转成BT类型
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="BTToUnBT">BT To unBT Flag</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public void DoBTChange(string model, bool BTToUnBT, string pdLine, string editor, string stationId, string customerId)
        {
/*//test，测试流程是否通，需去掉========
return;
//test，测试流程是否通，需去掉========*/
            
            logger.Debug("(BTChange)BTChange start, model:" + model + " pdLine:" + pdLine + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

			FisException ex;
			List<string> erpara = new List<string>();
			
            try
            {
                string sessionKey = model;
                if (!BTToUnBT)
                { 
                   //
                    DateTime dNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                    IList<Delivery> dnList= DeliveryRepository.GetDeliveryListByModel(model, "00");
                    if (dnList.Any(x => x.ShipDate >= dNow && x.ShipDate < DateTime.Now.AddDays(14)))
                    {
                       throw new FisException("There are Delivery which status=00, can not do BT change!");
                    }
                 //
                
                }

                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.ModelName, model);
                    sessionInfo.AddValue("BTToUnBT", BTToUnBT);
                    sessionInfo.Exception = null;
                    sessionInfo.SwitchToWorkFlow();		
                }

                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }

                    throw sessionInfo.Exception;
                }

				return;
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
                logger.Debug("(BTChange)BTChange end, model:" + model + " pdLine:" + pdLine + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
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

         #endregion
    }
}
