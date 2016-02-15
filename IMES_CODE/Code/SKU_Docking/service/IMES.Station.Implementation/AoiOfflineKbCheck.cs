﻿/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: AoiOfflineKbCheck
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-10-20   zhu lei           Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.PCA.MB;
using log4net;
using IMES.Route;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Line;
using IMES.Infrastructure.Extend;
using System.Collections;
using IMES.DataModel;



namespace IMES.Station.Implementation
{
    /// <summary>
    /// IAoiOfflineKbCheck 接口的实现类
    /// </summary>
    public class AoiOfflineKbCheck : MarshalByRefObject, IAoiOfflineKbCheck
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region IAoiOfflineKbCheck members

        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="custsn">Cust sn</param>
        /// <param name="editor">Editor</param>
        /// <param name="stationId">Station</param>
        /// <param name="customer">Customer</param>
        /// <returns>prestation</returns>
        public ArrayList InputCustSn(string pdLine, string custsn, string editor, string stationId, string customer)
        {
            logger.Debug("(AoiOfflineKbCheck)InputCustSn start, [pdLine]:" + pdLine
                + " [custsn]: " + custsn
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custsn;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, ProductSessionType, editor, stationId, pdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "AoiOfflineKbCheck.xoml", "AoiOfflineKbCheck.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.IsComplete, false);
                    session.AddValue(Session.SessionKeys.CustSN, sessionKey);
                    session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(session))
                    {
                        session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    session.WorkflowInstance.Start();
                    session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }
                IProduct product = (IProduct)session.GetValue(Session.SessionKeys.Product);
                ArrayList arr = new ArrayList();
                arr.Add(product.ProId);
                arr.Add(product.Model);
                CommonImpl cmm = new CommonImpl();
                IList<DefectInfo> defectList  =cmm.GetDefectList("PRD");
                arr.Add(defectList);
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
                throw e;
            }
            finally
            {
                logger.Debug("(AoiOfflineKbCheck)InputCustSn end, [pdLine]:" + pdLine
                    + " [custsn]: " + custsn
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// 不良品信息。
        /// </summary>
        /// <param name="custsn">Cust Sn</param>
        /// <param name="defectList">Defect IList</param>
      //  public void Save(string custsn, IList<string> defectList,string reason)

        public void Save(string custsn, IList<string> defectList, string reason)
       {
            logger.Debug("(AoiOfflineKbCheck)InputDefectCodeList start,"
                + " [custsn]: " + custsn
                + " [defect]:" + defectList
                );
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custsn;
            //string qcMethod = "None";
            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                else
                {
                    session.AddValue(ExtendSession.SessionKeys.TestLogActionName, "AOI Offline KB Check");
                    //IList<string> defectList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.DefectList);
          
                    if (defectList.Count == 0)
                    {
                        defectList = null;
                        session.AddValue(ExtendSession.SessionKeys.TestLogDescr,reason);
                    }
                    else
                    {
                        session.AddValue(ExtendSession.SessionKeys.TestLogDescr, "");
                    }
                      
                    session.AddValue(Session.SessionKeys.DefectList, defectList);

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
                logger.Debug("(AoiOfflineKbCheck)InputDefectCodeList end,"
                   + " [custsn]: " + custsn
                   + " [defectList]:" + defectList);
            }
        }


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            logger.Debug("(AoiOfflineKbCheck)Cancel start, [custsn]:" + sessionKey);
            //FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
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
                logger.Debug("(AoiOfflineKbCheck)Cancel end, [custSn]:" + sessionKey);
            }
        }

     

        #endregion

    }
}