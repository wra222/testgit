/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
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
using IMES.Infrastructure.Repository.PCA;
using IMES.FisObject.PCA.MBModel;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Extend;

namespace IMES.Station.Implementation
{
    public class _PCATestStation : MarshalByRefObject, IPCATestStation
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType TheType = Session.SessionType.MB;

        #region IPCATestStation Members

        public string InputMBSNo(string pdLine, string testStation, string MB_SNo, string editor, string customerId, out string AllowPass, out string DefectStation)
        {
            logger.Debug("(PCATestStation)InputMBSNo start, pdLine:" + pdLine + " testStation:" + testStation + " MB_SNo:" + MB_SNo + " editor:" + editor + " customerId:" + customerId);
            AllowPass = "";
            DefectStation = "";
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = MB_SNo;
            try
            {
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);


               // 2011/06/07 Jiali add
              //  Begin
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMES.FisObject.PCA.MB.IMB>();
                var mbModelRepository = (IMBModelRepository)RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
                Nullable<IMES.DataModel.MBInfo> NullAblembInfo=mbRepository.GetMBInfo(MB_SNo);
                 if (NullAblembInfo == null || NullAblembInfo.Value.id ==null)
                {
                    var ex1 = new FisException("SFC001", new string[] { MB_SNo });
                    throw ex1;
                }
                IMES.DataModel.MBInfo mbInfo = NullAblembInfo.Value;
                MBModel model = (MBModel)mbModelRepository.Find(mbInfo._111LevelId);
               Char[] MB_SNotoChar = MB_SNo.ToCharArray();


                 if (model.MultiQty != 1 && MB_SNotoChar[5] == '0')
                 {
                  
                     erpara.Add(sessionKey);
                     ex = new FisException("CHK164", erpara);
                     throw ex;
                 }

               //  end

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, testStation, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", testStation);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(testStation, "006PCATest.xoml", "006PCATest.rules", out wfName, out rlName);
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
        //6.25 jiali Add

                    //object obj1 = Session.GetValue(ExtendSession.SessionKeys.AllowPass);
                    //if (obj1 != null)
                    //{ AllowPass = obj1.ToString(); }

                    //object obj2 = Session.GetValue(ExtendSession.SessionKeys.DefectStation);
                    //if (obj2 != null)
                    //{ DefectStation = obj2.ToString(); }

                   //AllowPass =Session.GetValue(ExtendSession.SessionKeys.AllowPass).ToString();
                   // DefectStation =Session.GetValue(ExtendSession.SessionKeys.DefectStation).ToString();

                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //Session.SwitchToWorkFlow();

                //check workflow exception
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }
                    throw Session.Exception;
                }

                IMB mb = (MB)(Session.GetValue(Session.SessionKeys.MB));
                //6.25 jiali Add

                AllowPass = Session.GetValue(ExtendSession.SessionKeys.AllowPass).ToString();
                DefectStation = Session.GetValue(ExtendSession.SessionKeys.DefectStation).ToString();

                return mb.Model;

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
                logger.Debug("(PCATestStation)InputMBSNo end, pdLine:" + pdLine + " testStation:" + testStation + " MB_SNo:" + MB_SNo + " editor:" + editor + " customerId:" + customerId);
            }
        }

        public void InputDefectCodeList(string MB_SNo, IList<string> defectList)
        {
            logger.Debug("(PCATestStation)InputDefectCodeList start, MB_SNo:" + MB_SNo);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = MB_SNo;
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
                logger.Debug("(PCATestStation)InputDefectCodeList end, MB_SNo:" + MB_SNo);
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

        public string InputMBSNo(
           string pdLine,
           string testStation,
           string MB_SNo,
           string editor, string customerId)
        {
            return "1";
        }
       

        #endregion
    }
}
