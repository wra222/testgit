/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:  MB dismantle interface implement
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2009-10-20  Zhao Meili(EB)        Create 
 * 200910-201005  itc207013          ITC-1103-0255,ITC-1103-0277
 *                                   ITC-1103-0330,ITC-1103-0331
 *                                   ITC-1103-0332,ITC-1103-0336
 *                                   
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
namespace IMES.Station.Implementation
{
    public class MBPrintDismantle : MarshalByRefObject, IMBLabelPrint
    {
        private static readonly Session.SessionType theType = Session.SessionType.Common;
        private const string WF = "002Dismantle.xoml";
        private const string Rule = "002Dismantle.rules";
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 1.3	UC-PCA-MLP-03 Dismantle
        /// 打断MB SNo与Mo的归属关系
        /// 异常情况：
        /// a.	如果没有输入[Start MB SNo]，则报告错误：“请输入Start MB/SB/VB NO !!”
        /// b.	如果没有输入[Dismantle Reason]，则报告错误：“必须输入Dismantle Reason”
        /// c.	如果用户输入的[End MB SNo]<[Start MB SNo]，则报告错误：“End MB SNo 必须大于 Start MB SNo"
        /// d.	如果用户输入的[End MB SNo]的Mo与[Start MB SNo] 的Mo 不同，则报告错误：“Start MB SNo 与 End MB SNo 的Mo 不同！”
        /// 异常情况：
        /// a.	如果在用户输入的[Start MB SNo]，[End MB SNo] 范围内在[SnoDetPCB] 表中没有记录存在，则报告错误：“MB NO is not exist !!”；注意需要明确的报告是MB NO，VB NO ，还是SB NO 不存在。
        /// b.	如果指定的MB SNo 已经投入生产，则报告错误“MB SNo: @mbsno 已经投入生产，不能进行Dismantle!!”
        /// </summary>
        /// <param name="startMBSNo">开始MB SNo</param>
        /// <param name="endMBSNo">结束MB SNo</param>
        /// <param name="reason">打断原因</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public void Dismantle(
            string startMBSNo,
            string endMBSNo,
            string reason,
            string editor, string stationId, string customerId)
        {

            logger.Debug("(Dismantle)Dismantle start,"
                      + " [startMBSNo]:" + startMBSNo
                      + " [endMBSNo]: " + endMBSNo
                      + " [reason]:" + reason
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
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("CurrentFlowSession", sessionInfo);
                    wfArguments.Add("SessionType", theType);
                    sessionInfo.AddValue(Session.SessionKeys.startProdId, startMBSNo);
                    sessionInfo.AddValue(Session.SessionKeys.endProdId, endMBSNo);
                    sessionInfo.AddValue(Session.SessionKeys.Reason, reason);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(WF, Rule, wfArguments);
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
                logger.Debug("(Dismantle)Dismantle End,"
                  + " [startMBSNo]:" + startMBSNo
                  + " [endMBSNo]: " + endMBSNo
                  + " [reason]:" + reason
                  + " [editor]:" + editor
                  + " [station]:" + stationId
                  + " [customer]:" + customerId);
            }
        }
        #region "just for inherts methods"
        public IList<PrintItem> Print(
            string pdLine,
            bool isNextMonth, 
            string mo,
            int qty,
            string dateCode,
            string editor, string stationId, string customerId,
            out IList<string> startProdIdAndEndProdId, string _111, IList<PrintItem> printItems)
        {
            throw new NotImplementedException(); 
        }
        public string Find(
            string mo,
            string isNextMonth,
            string editor, string stationId, string customerId)
        {
            throw new NotImplementedException();
        }
        #endregion

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
     

        #region IMBLabelPrint Members


        public void Reprint(string startMBSNo, string endMBSNo, string reason, string editor, string stationId, string customerId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IMBLabelPrint Members


        public IList<PrintItem> RePrintMbLabel(string beginNo, string endNo, string customerId, string reason, string editor, string stationId, IList<PrintItem> printItems, out List<string> lstMBNo, out List<string> lstParttNo)
        {
            throw new NotImplementedException();
        }

        #endregion



        #region IMBLabelPrint Members


        public bool CheckIsProduct(string beginNo, string endNo, string SA1StationName, out string ExistMB)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
