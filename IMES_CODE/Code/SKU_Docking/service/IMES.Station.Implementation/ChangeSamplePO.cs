// INVENTEC corporation (c)2012 all rights reserved. 
// Description: Change PO
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.DataModel;
using System.Data;

namespace IMES.Station.Implementation
{
    /// <summary>
    ///
    /// </summary>
    public class ChangeSamplePO : MarshalByRefObject, IChangeSamplePO
    {
		private const Session.SessionType TheType = Session.SessionType.Product;
        #region IChangeSamplePO Members

        public ArrayList InputSN1(string sn1, string editor, string line, string station, string customer)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
			logger.DebugFormat("BEGIN: {0}(sn:{1})", methodName, sn1);
			try
			{
                ArrayList retLst = new ArrayList();
                
                string sessionKey = sn1;
                Dictionary<string,object> sessionKeyValueList = new Dictionary<string,object>();
                Session currentSession = null;

                string wfName = "ChangeSamplePO.xoml";
                string wfRule = "ChangeSamplePO.rules";
				//sessionKeyValueList.Add(IMES.Infrastructure.Session.SessionKeys.IsComplete, false);
				
				//executing workflow, if fail then throw error 
                currentSession = WorkflowUtility.InvokeWF(sessionKey, station, line, customer, editor, TheType, wfName, wfRule, sessionKeyValueList);

                IProduct prod = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);
                S_ChangeSamplePO_Prod sProd = Get_ChangeSamplePO_Prod(prod);
                retLst.Add(sProd);

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
				logger.DebugFormat("END: {0}()", methodName);
			}
        }

        public ArrayList InputSN2(string sn1, string sn2)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(sn1={1}, sn2={2})", methodName, sn1, sn2);
            ArrayList retLst = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = sn1;
            Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            try
            {
                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.ProductIDOrCustSN, sn2);
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
                
				IProduct prod = (IProduct)sessionInfo.GetValue(Session.SessionKeys.Product);
                S_ChangeSamplePO_Prod sProd = Get_ChangeSamplePO_Prod(prod);
                retLst.Add(sProd);

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
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public ArrayList Change(string sn1, string sn2)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(sn1={1}, sn2={2})", methodName, sn1, sn2);
            ArrayList retLst = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = sn1;
            Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            try
            {
                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.IsComplete, true);
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

                IProduct prodSource = CommonImpl.GetProductByInput(sn1, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                S_ChangeSamplePO_Prod sProdSource = Get_ChangeSamplePO_Prod(prodSource);
                retLst.Add(sProdSource);

                IProduct prodDest = CommonImpl.GetProductByInput(sn2, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                S_ChangeSamplePO_Prod sProdDest = Get_ChangeSamplePO_Prod(prodDest);
                retLst.Add(sProdDest);

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
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

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

        private S_ChangeSamplePO_Prod Get_ChangeSamplePO_Prod(IProduct prod)
        {
            S_ChangeSamplePO_Prod sProd = new S_ChangeSamplePO_Prod();
            sProd.ProdId = prod.ProId;
            sProd.CustSN = prod.CUSTSN;
            sProd.Model = prod.Model;
            sProd.PoNo = prod.MOObject.PoNo;
            sProd.MO = prod.MO;
            sProd.Station = prod.Status.StationId + " - " + CommonImpl.GetInstance().GeStationDescr(prod.Status.StationId);
            return sProd;
        }

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion
    }
}
