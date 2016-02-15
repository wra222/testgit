/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-12-01   200050            Create 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using log4net;
using IMES.Route;

namespace IMES.Station.Implementation
{
    public class KittingInput : MarshalByRefObject, IKittingInput
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IKittingInput Members

        /// <summary>
        /// 1st step. -- All Step: 1st step:input prodId;  2nd step:input BoxId, 
        /// Func: Input ProdId, to check if maintain kitting data 
        ///       (correspoding <--> ProdId-product-family ).
        /// SQL:      If exist(SELECT [Code],[PartNo]  FROM [FA].[dbo].[WipBuffer] where [Code] = @family)
        /// ErrorTip: If not found, give tip in interface: "沒有maintain 資料"
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="prodId"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public ArrayList InputProdId(string pdLine, string prodId, string editor, string stationId, string customerId)
        {
            logger.Debug("(_KittingInput)InputProdId start, pdLine:" + pdLine + " prodId:" + prodId + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            ArrayList this_array_list_return = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(prodId, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                if (currentProduct.ProId == "")
                {
                    throw new FisException("CHK020", new string[] { prodId });
                }

                string sessionKey = currentProduct.ProId;
                string this_model = currentProduct.Model;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey); // key must be prodId.
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "KittingInput.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue("_prodId", currentProduct.ProId);
                    Session.AddValue("inter_pdline", pdLine);
                    //Session.AddValue(Session.SessionKeys.Floor, floor);
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

                //if (hasDealWithOneFKU == false) hasDealWithOneFKU = true;
                this_array_list_return.Add(sessionKey); // ProdId
                this_array_list_return.Add(this_model); // model
                return this_array_list_return;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                logger.Debug("(_KittingInput)InputProdId end, pdLine:" + pdLine + " prodId:" + prodId + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            }
        }

#if false
		public void InputBoxId(string prodId, string boxId)
        {
            logger.Debug("(_KittingInput)InputBoxId start, prodId:" + prodId + " boxId:" + boxId);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            if (Session == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                throw ex;
            }
            try
            {
                Session.Exception = null;
                Session.AddValue(Session.SessionKeys.boxId, boxId);
                Session.SwitchToWorkFlow();
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }

                    throw Session.Exception;
                }
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
                logger.Debug("(_KittingInput)InputBoxId end, prodId:" + prodId + " boxId:" + boxId);
            }
        }
#else
        /// <summary>
        /// 2ndt step. -- All Step: 1st step:input prodId;  2nd step:input BoxId, 
        /// Func: Input BoxId, 
        ///       Interface: 6.	对BoxId做4码检查，不做重复检查
        ///                     异常情况：
        ///                         A.	若没有输入ProdId，则提示”请首先输入ProdId !”
        ///                         B.	当BoxId不是4码时，则提示”请输入正确的BoxId !”
        ///       Service:  6. Get Bom
        ///                    根据Model得到此unit的part BOM和SELECT [Code]  ,
        ///                    [PartNo]  FROM [FA].[dbo].[WipBuffer] where [Code] = @family 的交集
        ///                 7.	将bom中需要检料的part通过kitting middleware传至Kitting Line Server
        ///                 8.	绑定ProdId和BoxId
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="boxId"></param>
		public void InputBoxId(string prodId, string boxId)
        {
            logger.Debug("(_KittingInput)InputBoxId start, prodId:" + prodId + " boxId:" + boxId);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }

                session.AddValue(Session.SessionKeys.boxId, boxId);
                session.AddValue("_boxId", boxId);
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
                logger.Debug("(_KittingInput)InputBoxId end, prodId:" + prodId + " boxId:" + boxId);
            }
        }
#endif

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
