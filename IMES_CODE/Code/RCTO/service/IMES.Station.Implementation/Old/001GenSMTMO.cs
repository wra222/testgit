/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:  Generate SMT MO interface implement
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2009-10-20  Zhao Meili(EB)        Create 
 * 200910-201005  itc207013          ITC-1103-0011
 *                                   ITC-1103-0066,ITC-1103-0074
 *                                   ITC-1103-0079,ITC-1103-0086
 *                                   ITC-1103-0088,ITC-1103-0089
 * Known issues:
 */

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

using IMES.Infrastructure.Utility.Generates.impl;
using IMES.Infrastructure.Extend;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// SMT Mo 不同于成制由SAP 系统Download 的Mo。半制根据产销排程表，建立SMT Mo。
    /// </summary>
    public class GenSMTMO : MarshalByRefObject, IGenSMTMO
    {
        //undetermined
        private static readonly Session.SessionType theType = Session.SessionType.Common;
        private const string GenSMTWF = "001GenSMTMO.xoml";
        private const string DeleteSMTWF = "001DeleteSMTMO.xoml";

        private const string GenSMTRule = "001GenSMTMO.rules";
        private const string DeleteSMTRule = "";
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 1.1	UC-PCA-GSM-01 Generate SMT MO
        /// 根据产销排程生成相关机型Mo
        /// 1.	实现后续通过Mo管控MB S/N。
        /// 2.	实现生产订单追踪。
        /// 
        /// 异常情况：
        /// 1. 检查参数，报告错误情况；
        /// 2. 如果产生的Mo已经在数据库中存在，则报告错误：“MO NO is duplicate, please re-scan.”
        /// </summary>
        /// <param name="_111_PN">111阶Part Number</param>
        /// <param name="quantity">Product数量</param>
        /// <param name="isMassProduction">是否为量产</param>---2011/03/12 JialiUPdate <param name="IsMassProduction">生产类型（量产0/试产1/Special2）</param>
        /// <param name="remark">remark</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public string  GenerateSMTMO(string _111_PN,
            int quantity,
            bool  IsMassProduction,
            string remark,
            string editor, string stationId, string customerId)
        {
            logger.Debug("(GenerateSMTMO)GenerateSMTMO start, [_111_PN]:" + _111_PN 
                            + " [Qty]: " + quantity
                            + " [IsMassProduction]:" + IsMassProduction
                            + " [remark]:" + remark 
                            + " [editor]:" + editor 
                            + " [station]:" + stationId 
                            + " [customer]:" + customerId);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                //产生sessionKey
                string sessionKey = Guid.NewGuid().ToString() ;
                //通过sessionKey 链接seesion
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (sessionInfo == null)
                {
                    sessionInfo = new Session(sessionKey, theType,editor,stationId,"",customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", sessionInfo);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("SessionType", theType);



                    sessionInfo.AddValue(Session.SessionKeys.PN111, _111_PN);
                    sessionInfo.AddValue(Session.SessionKeys.Qty, quantity);
                    sessionInfo.AddValue(Session.SessionKeys.IsMassProduction,false);
                    sessionInfo.AddValue(Session.SessionKeys.Remark, remark);
                    sessionInfo.AddValue(Session.SessionKeys.IsExperiment, !IsMassProduction);
                    //Remoting开始,调用workflow
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(GenSMTWF, GenSMTRule, wfArguments);
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

                //取session值.返回
                string SMTMONo = (string)sessionInfo.GetValue(Session.SessionKeys.MBMONO);
                string SessKey = sessionInfo.Key;
                return SMTMONo + "," + sessionKey; ;
                
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
                logger.Debug("(GenerateSMTMO)GenerateSMTMO End, [_111_PN]:" + _111_PN
                            + " [Qty]: " + quantity
                            + " [isMassProduction]:" + IsMassProduction
                            + " [remark]:" + remark
                            + " [editor]:" + editor
                            + " [station]:" + stationId
                            + " [customer]:" + customerId);
            }
        }
        //Add by Jiali 2011/03/14
        public string GenerateSMTMO(string _111_PN,
         int quantity,
         string ModelType,
         string remark,
         string editor, string stationId, string customerId)
        {
            logger.Debug("(GenerateSMTMO)GenerateSMTMO start, [_111_PN]:" + _111_PN
                            + " [Qty]: " + quantity
                            + " [ModelType]:" + ModelType
                            + " [remark]:" + remark
                            + " [editor]:" + editor
                            + " [station]:" + stationId
                            + " [customer]:" + customerId);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                //产生sessionKey
                string sessionKey = Guid.NewGuid().ToString();
                //通过sessionKey 链接seesion
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);
                  
                if (sessionInfo == null)
                {
                    sessionInfo = new Session(sessionKey, theType, editor, stationId, "", customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", sessionInfo);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("SessionType", theType);

                    sessionInfo.AddValue(Session.SessionKeys.PN111, _111_PN);
                    sessionInfo.AddValue(Session.SessionKeys.Qty, quantity);
                    sessionInfo.AddValue(ExtendSession.SessionKeys.ModelType, ModelType);
                    sessionInfo.AddValue(Session.SessionKeys.Remark, remark);
                    sessionInfo.AddValue(ExtendSession.SessionKeys.isCreate, "");
                   
                    //Remoting开始,调用workflow
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(GenSMTWF, GenSMTRule, wfArguments);
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

                //取session值.返回
                string SMTMONo = (string)sessionInfo.GetValue(Session.SessionKeys.MBMONO);
                string SessKey = sessionInfo.Key;
                return SMTMONo + "," + sessionKey; ;

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
                logger.Debug("(GenerateSMTMO)GenerateSMTMO End, [_111_PN]:" + _111_PN
                            + " [Qty]: " + quantity
                            + " [ModelType]:" + ModelType
                            + " [remark]:" + remark
                            + " [editor]:" + editor
                            + " [station]:" + stationId
                            + " [customer]:" + customerId);
            }
        }
        //新添页面跳转后继续执行workflow的方法
        public void   CreateSMTMO(string key,bool isCreated)
        {
        try
            {
                Session CurrentsessionInfo = SessionManager.GetInstance.GetSession(key, theType);
                CurrentsessionInfo.AddValue(ExtendSession.SessionKeys.isCreate, isCreated);
                  
               if (CurrentsessionInfo != null)
                {
                    //if (isCreated)
                    //{
                         CurrentsessionInfo.SwitchToWorkFlow();
                        
                    //}
                    //else
                    //{



                    //    CurrentsessionInfo.WorkflowInstance.Terminate("no need to create these mo!");
                    ////    CurrentsessionInfo.WorkflowInstance.Suspend("不需要创建MO");
                    //    SessionManager.GetInstance.RemoveSession(CurrentsessionInfo);
                       
                        
                    //}
                    

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
              
            }
        }

        /// <summary>
        /// 1.2	UC-PCA-GSM-02 Query
        /// 查询并显示当天产生的，尚未列印完MB Label 的SMT MO。
        /// </summary>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>SMT MO列表</returns>
        public IList<SMTMOInfo> Query(string editor, string stationId, string customerId)
        {
            logger.Debug("(GenerateSMTMO)Query Start, "
                           + " [editor]:" + editor
                           + " [station]:" + stationId
                           + " [customer]:" + customerId);
            try
            {
                IMBMORepository mbmoRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
                 IList<SMTMOInfo> unprintedMOlist  = mbmoRepository.GetSMTMOInfos();
                return unprintedMOlist; 
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
                logger.Debug("(GenerateSMTMO)Query End, "
                         + " [editor]:" + editor
                         + " [station]:" + stationId
                         + " [customer]:" + customerId);
            }
           
        }


        /// <summary>
        /// 1.3	UC-PCA-GSM-03 Delete
        /// 删除用户选择的SMT Mo
        /// 
        /// 异常情况：
        /// 1. 如果欲删除的Mo，已经开始列印MB Label，则需要报告错误：“The Mo has combined MBNo or SBNO or VBNO, Can't delete!”
        /// </summary>
        /// <param name="SMT_MOs">待删除的SMT MO列表</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public void Delete(IList<string> SMT_MOs, string editor, string stationId, string customerId)
        {
            string smtmo = "";
            if ((SMT_MOs != null) && (SMT_MOs.Count > 0))
            {
                foreach (string singlemo in SMT_MOs)
                {
                    smtmo = smtmo+","+singlemo;
                }
                smtmo = "{" + smtmo.Substring(1) + "}";
            }
                
            logger.Debug("(GenerateSMTMO)Delete Start, "
                           + " [SMT_MOs]:" + smtmo 
                           + " [editor]:" + editor
                           + " [station]:" + stationId
                           + " [customer]:" + customerId);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {

                string sessionKey = Guid.NewGuid().ToString(); 
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (sessionInfo == null)
                {
                    sessionInfo = new Session(sessionKey, theType, editor, stationId, "", customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("CurrentFlowSession", sessionInfo);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", theType);
                    //wfArguments.Add("Pdline", pdLine);

                    sessionInfo.AddValue(Session.SessionKeys.MBMONOList, SMT_MOs);


                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(DeleteSMTWF, DeleteSMTRule, wfArguments);
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
                logger.Debug("(GenerateSMTMO)Delete End, "
                           + " [SMT_MOs]:" + smtmo
                           + " [editor]:" + editor
                           + " [station]:" + stationId
                           + " [customer]:" + customerId);
            }
        }

        /// <summary>
        /// 1.4	UC-PCA-GSM-04 Report
        /// 查询当天产生的，尚未列印完MB Label 的SMT Mo
        /// </summary>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>SMTMO结构列表</returns>
       public IList<SMTMOInfo> Report(string editor, string stationId, string customerId)
        {
            return Query(editor, stationId, customerId); 
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

               Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, theType);

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
