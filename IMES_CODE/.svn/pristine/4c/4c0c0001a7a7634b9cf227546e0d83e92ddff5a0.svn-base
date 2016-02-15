using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PCA.MBMO;
using log4net;
using IMES.FisObject.PCA.MBModel;
using IMES.Route;
namespace IMES.Station.Implementation
{
    public class MBReflow : MarshalByRefObject, IMBReflow 
    {
        private static readonly Session.SessionType theType = Session.SessionType.MB;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// mb reflow
        /// </summary>
        /// <param name="MBSN"></param>
        /// <param name="ECR"></param>
        /// <param name="DateCode"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        public IList<PrintItem> Reflow(string MBSN, string ECR, string DateCode, string line, string editor, string station, string customer, IList<PrintItem> printItems, out bool IsQuantity,out string iecVer,out string CustVer,out string MECR)
        {

            logger.Debug("(MBReflow)Reflow start,"
                      + " [MBSN]:" + MBSN
                      + " [ECR]: " + ECR
                      + " [DateCode]:" + DateCode
                       + " [line]:" + line
                      + " [editor]:" + editor
                      + " [station]:" + station
                      + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                IsQuantity = false;
                iecVer = "";
                CustVer = "";
                MECR = "";
                string sessionKey = MBSN;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (sessionInfo == null)
                {
                    sessionInfo = new Session(sessionKey, theType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("CurrentFlowSession", sessionInfo);
                    wfArguments.Add("SessionType", theType);
                    sessionInfo.AddValue(Session.SessionKeys.ECR, ECR);
                    sessionInfo.AddValue(Session.SessionKeys.DateCode, DateCode);

                    //mac reprint
                    sessionInfo.AddValue(Session.SessionKeys.Reason, "MB Reflow");
                    sessionInfo.AddValue(Session.SessionKeys.PrintLogBegNo, MBSN);
                    sessionInfo.AddValue(Session.SessionKeys.PrintLogEndNo, MBSN);
                    sessionInfo.AddValue(Session.SessionKeys.PrintItems, printItems);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "096MBReflow.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    sessionInfo.SetInstance(instance);
                    //for generate MB no

                    if (!SessionManager.GetInstance.AddSession(sessionInfo))
                    {
                        sessionInfo.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    sessionInfo.WorkflowInstance.Start();
                    sessionInfo.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }
                    throw sessionInfo.Exception;
                }
                var mb = sessionInfo.GetValue(Session.SessionKeys.MB) as IMES.FisObject.PCA.MB.IMB;
                if (string.IsNullOrEmpty(mb.ECR))
                {
                    FisException ex1;
                    List<string> erpara1 = new List<string>();
                    erpara1.Add(mb.Sn);
                    ex1 = new FisException("CHK087", erpara1);
                    throw ex1;
                }
                //对于试产刷入MB Sno 后，用户需要手动点击[Save] 按钮进行保存；对于量产刷入MB Sno 后，系统自动保存
               if (mb.SMTMO.StartsWith("M"))
                {
                    IsQuantity = true;
                    sessionInfo.AddValue(Session.SessionKeys.IsMassProduction, true);
                    string mbcode = mb.ModelObj.Mbcode;
                    IMBModelRepository mbmodelRepository = RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
                    IList<EcrVersionInfo> ecrinfo = mbmodelRepository.getEcrVersionsByEcrAndMbcode(ECR, mbcode);
                    if (ecrinfo == null || ecrinfo.Count < 1)
                    {
                        //'ECR 数据错误'
                        //CHK086
                        FisException ex1;
                        List<string> erpara1 = new List<string>();
                        ex1 = new FisException("CHK086", erpara1);
                        sessionInfo.ResumeWorkFlow();
                        throw ex1;
                    }

                    iecVer = ecrinfo[0].IECVer;
                    //CustVer = ecrinfo[0].CustVer;
                    sessionInfo.AddValue(Session.SessionKeys.IECVersion, ecrinfo[0].IECVer);
                    //sessionInfo.AddValue(Session.SessionKeys.CustomVersion, ecrinfo[0].CustVer);
                   //<<CI-MES10-SPEC-096-UC MB Reflow.docx>>进行如下修改：

                    //1.    对于试产量产都需要支持刷入9999 或者点击 [确定]按钮两种方式，完成MB Reflow；取消原量产MB 自动完成MB Reflow 的功能

                    //sessionInfo.Exception = null;
                    //sessionInfo.SwitchToWorkFlow();
                    //if (sessionInfo.Exception != null)
                    //{
                    //    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    //    {
                    //        sessionInfo.ResumeWorkFlow();
                    //    }
                    //    throw sessionInfo.Exception;
                    //}

                }
                else if (mb.SMTMO.StartsWith("P"))
                {
                    IsQuantity = false;
                    sessionInfo.AddValue(Session.SessionKeys.IsMassProduction, false );
                    iecVer = mb.IECVER;
                    CustVer = mb.CUSTVER;
                    MECR = mb.ECR ;
                    //sessionInfo.AddValue(Session.SessionKeys.IECVersion, mb.IECVER);
                    //sessionInfo.AddValue(Session.SessionKeys.CustomVersion, mb.CUSTVER);
                    //sessionInfo.AddValue(Session.SessionKeys.ECR, mb.ECR);
                }
                return (IList<PrintItem>)sessionInfo.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug("(MBReflow)Reflow End,"
                          + " [MBSN]:" + MBSN
                          + " [ECR]: " + ECR
                          + " [DateCode]:" + DateCode
                           + " [line]:" + line
                          + " [editor]:" + editor
                          + " [station]:" + station
                          + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// save data
        /// </summary>
        /// <param name="MBSN"></param>
        /// <param name="ECR"></param>
        /// <param name="IECVerson"></param>
        /// <param name="CustVersion"></param>
        /// <param name="DateCode"></param>
        /// <returns></returns>
        public IList<PrintItem> Save(string MBSN, string ECR, string IECVerson, string CustVersion, string DateCode)
        {

            logger.Debug("(MBReflow)Save start,"
                      + " [MBSN]:" + MBSN
                       + " [IECVerson]:" + IECVerson
                      + " [CustVersion]:" + CustVersion);
            try
            {
                FisException ex;
                List<string> erpara = new List<string>();
                string sessionKey = MBSN;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);
                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {

                    if (string.IsNullOrEmpty(IECVerson) || IECVerson.Length != 4 || !IECVerson.Substring(1).StartsWith("."))
                    {
                        FisException ex1;
                        List<string> erpara1 = new List<string>();
                        ex1 = new FisException("CHK091", erpara1);
                        ex1.stopWF = false;
                        throw ex1;
                    }

                    if (string.IsNullOrEmpty(CustVersion))
                    {
                        CustVersion = "";
                    }
                    else if (CustVersion.Length != 3)
                    {
                        FisException ex1;
                        List<string> erpara1 = new List<string>();
                        ex1 = new FisException("CHK092", erpara1);
                        ex1.stopWF = false;
                        throw ex1;
                    }
                    sessionInfo.AddValue(Session.SessionKeys.ECR, ECR);
                    sessionInfo.AddValue(Session.SessionKeys.IECVersion, IECVerson);
                    sessionInfo.AddValue(Session.SessionKeys.CustomVersion, CustVersion);
                    sessionInfo.AddValue(Session.SessionKeys.DateCode, DateCode);
                                      
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

                return (IList<PrintItem>)sessionInfo.GetValue(Session.SessionKeys.PrintItems);

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
                logger.Debug("(MBReflow)Save end,"
                          + " [MBSN]:" + MBSN
                           + " [IECVerson]:" + IECVerson
                          + " [CustVersion]:" + CustVersion);

            }
        }
        
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="MBSN"></param>
        public void Cancel(string MBSN)
 
        {  
         logger.Debug("(MBReflow)Cancel Start,"
               + " [MBSN]:" + MBSN);
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(MBSN, theType);

                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
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
                  logger.Debug("(MBReflow)Cancel End,"
               + " [MBSN]:" + MBSN);

            }
	
        }
  

    }
}
