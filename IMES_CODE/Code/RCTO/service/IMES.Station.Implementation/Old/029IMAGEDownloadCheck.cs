/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 2010-02-03   207006     ITC-1122-0023 
 * 2010-02-03   207006     ITC-1122-0044 
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
using log4net;
using IMES.Route;

namespace IMES.Station.Implementation
{
    public class _IMAGEDownloadCheck : MarshalByRefObject, IIMAGEDownloadCheck
    {
        //private const string Station = "66";
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region IIMAGEDownloadCheck Members

        public void InputProdId(string pdLine, string prodId, string editor, string stationId, string customerId)
        {
            logger.Debug("(_IMAGEDownloadCheck)InputProdId start, pdLine:" + pdLine + " prodId:" + prodId + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            //2010-02-03   207006     ITC-1122-0023 
            //2010-02-03   207006     ITC-1122-0044 
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            try
            {
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType,editor,stationId,pdLine,customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "029IMAGEDownloadCheck.xoml", "029IMAGEDownloadCheck.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    //Session.AddValue(Session.SessionKeys., false);
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
                logger.Debug("(_IMAGEDownloadCheck)InputProdId end, pdLine:" + pdLine + " prodId:" + prodId + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            }
        }

        public void InputDefectCodeList(string prodId, IList<string> defectList)
        {
            logger.Debug("(_IMAGEDownloadCheck)InputDefectCodeList start, prodId:" + prodId);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            if (Session == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                //ex.logErr("", "", "", "", "83");
                //logger.Error(ex);
                throw ex;
            }
            try
            {
                Session.Exception = null;

                if (!(defectList == null || defectList.Count == 0))
                {
                    Session.AddValue(Session.SessionKeys.DefectList, defectList);
                }
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
                logger.Debug("(_IMAGEDownloadCheck)InputDefectCodeList end, prodId:" + prodId);

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
