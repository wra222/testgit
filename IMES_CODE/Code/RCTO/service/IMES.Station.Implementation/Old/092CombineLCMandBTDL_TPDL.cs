using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.FisObject.PCA.MB;
using log4net;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.Common.Model;
using System.Workflow.Runtime;
using IMES.Route;

namespace IMES.Station.Implementation
{
    public class CombineLCMandBTDL_TPDL : MarshalByRefObject, ICombineLCMandBTDL_TPDL
    {
        // 5. Check LCM CT#	检查规则：
        //在PartSN表里根据 CT#找到对应的记录，得到Part Type=LCM

        //以下是TSB特有业务需求：
        //若Vendor=LG并且family前7码=POTOMAC，则提示user以下信息：”需贴rubber.”
        //Family获取方法：根据PartNo得到第一个包含此part的MOBom，得到model，再找到对应的Family
        //Vendor取值参考语句：
        //Part.Vendor where PartNo=pn

        //异常情况：
        //A.	若输入的CT#已绑定灯下（LCMBind），则提示”此CT已与其它灯下结合，不能再结合.”
        //B.	若输入的CT#已绑定Vendor SN(PartSN)，则提示”此CT已与其它vendor SN结合，不能再结合.”
        //C.	若CT#没在PartSN中存在，则提示”此CT不存在，重新输入.”
        //D.	若CT#对应的Type不等于LCM，则提示”此CT不是LCM，请重新输入.”
        private static readonly Session.SessionType theType = Session.SessionType.Common ;
        private const string BTDLWF = "092CombineLCMBTDL.xoml";
 
        private const string BTDLRule = "092CombineLCMBTDL.rules";

        private const string TPDLWF = "092CombineLCMTPDL.xoml";
 
        private const string TPDLRule = "092CombineLCMTPDL.rules";

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Check BtdlLcmCT#
        /// </summary>
        /// <param name="lcmCTNum"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        public void CheckBtdlLcmCT(string lcmCTNum, string editor, string stationId, string customerId)
        {
            logger.Debug("(CombineLCMandBTDL_TPDL)CheckBtdlLcmCT start,"
                + " [lcmCTNum]:" + lcmCTNum);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                string sessionKey = lcmCTNum;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);
                string pdLine = "";
                if (sessionInfo == null)
                {
                    sessionInfo = new Session(sessionKey, theType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("CurrentFlowSession", sessionInfo);
                    wfArguments.Add("SessionType", theType);
                    sessionInfo.AddValue(Session.SessionKeys.BTDLLCMCTNO, lcmCTNum);
                    sessionInfo.AddValue(Session.SessionKeys.IsComplete, false);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, BTDLWF, BTDLRule, out wfName, out rlName);
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
                logger.Debug("(CombineLCMandBTDL_TPDL)CheckBtdlLcmCT End,"
                + " [lcmCTNum]:" + lcmCTNum);
            }
        }

        /// <summary>
        /// Check TpdlLcmCT#
        /// </summary>
        /// <param name="lcmCTNum"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        public void CheckTpdlLcmCT(string lcmCTNum, string editor, string stationId, string customerId)
        {
            logger.Debug("(CombineLCMandBTDL_TPDL)CheckBtdlLcmCT start,"
                + " [lcmCTNum]:" + lcmCTNum);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                string sessionKey = lcmCTNum;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);
                string pdLine = "";
                if (sessionInfo == null)
                {
                    sessionInfo = new Session(sessionKey, theType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("CurrentFlowSession", sessionInfo);
                    wfArguments.Add("SessionType", theType);
                    sessionInfo.AddValue(Session.SessionKeys.TPDLLCMCTNO, lcmCTNum);
                    sessionInfo.AddValue(Session.SessionKeys.IsComplete, false);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, TPDLWF, TPDLRule, out wfName, out rlName);
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
                logger.Debug("(CombineLCMandBTDL_TPDL)CheckBtdlLcmCT End,"
                + " [lcmCTNum]:" + lcmCTNum);
            }
        }

        /// <summary>
        /// Check Lcm VendorSn
        /// </summary>
        /// <param name="lcmCTNum"></param>
        /// <param name="lcmVendorSn"></param>
        public void CheckLcmVendorSn(string lcmCTNum,
            string lcmVendorSn)
        {
            logger.Debug("(CombineLCMandBTDL_TPDL)CheckLcmVendorSn start,"
                + " [lcmCTNum]: " + lcmCTNum
                + " [lcmVendorSn]:" + lcmVendorSn);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {

                string sessionKey = lcmCTNum;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.VendorSN , lcmVendorSn);
                    sessionInfo.AddValue(Session.SessionKeys.IsComplete , false);
 
                     sessionInfo.AddValue(Session.SessionKeys.BTDLSN, "");
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
                logger.Debug("(CombineLCMandBTDL_TPDL)CheckLcmVendorSn End,"
                + " [lcmCTNum]: " + lcmCTNum
                + " [lcmVendorSn]:" + lcmVendorSn);
            }
        }
   //     public void CheckBtdlSn(string lcmCTNum, string btdlSn)
   //     {
   //         logger.Debug("(CombineLCMandBTDL_TPDL)CheckBtdlSn start,"
   //            + " [lcmCTNum]: " + lcmCTNum
   //            + " [btdlSn]:" + btdlSn);
   //         FisException ex;
   //         List<string> erpara = new List<string>();
   //         try
   //         {

   //             string sessionKey = lcmCTNum;
   //             Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

   //             if (sessionInfo == null)
   //             {
   //                 erpara.Add(sessionKey);
   //                 ex = new FisException("CHK021", erpara);
   //                 throw ex;
   //             }
   //             else
   //             {
   //                 sessionInfo.AddValue(Session.SessionKeys.BTDLSN, btdlSn);
   //                 sessionInfo.Exception = null;
   //                 sessionInfo.SwitchToWorkFlow();

   //             }

   //             if (sessionInfo.Exception != null)
   //             {
   //                 if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
   //                 {
   //                     sessionInfo.ResumeWorkFlow();
   //                 }

   //                 throw sessionInfo.Exception;
   //             }
   //         }
   //         catch (FisException e)
   //         {
   //             logger.Error(e.mErrmsg);
   //             throw e;
   //         }
   //         catch (Exception e)
   //         {
   //             logger.Error(e.Message);
   //             throw new SystemException(e.Message);
   //         }
   //         finally
   //         {
   //             logger.Debug("(CombineLCMandBTDL_TPDL)CheckBtdlSn End,"
   //            + " [lcmCTNum]: " + lcmCTNum
   //            + " [btdlSn]:" + btdlSn);
   //         }
   //     }
   //     public void CheckTpdlSn(string lcmCTNum, string tpdlSn)
   //     {
   //         logger.Debug("(CombineLCMandBTDL_TPDL)CheckTpdlSn start,"
   //+ " [lcmCTNum]: " + lcmCTNum
   //+ " [tpdlSn]:" + tpdlSn);
   //         FisException ex;
   //         List<string> erpara = new List<string>();
   //         try
   //         {

   //             string sessionKey = lcmCTNum;
   //             Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

   //             if (sessionInfo == null)
   //             {
   //                 erpara.Add(sessionKey);
   //                 ex = new FisException("CHK021", erpara);
   //                 throw ex;
   //             }
   //             else
   //             {
   //                 sessionInfo.AddValue(Session.SessionKeys.TPDLSN, btdlSn);
   //                 sessionInfo.Exception = null;
   //                 sessionInfo.SwitchToWorkFlow();

   //             }

   //             if (sessionInfo.Exception != null)
   //             {
   //                 if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
   //                 {
   //                     sessionInfo.ResumeWorkFlow();
   //                 }

   //                 throw sessionInfo.Exception;
   //             }
   //         }
   //         catch (FisException e)
   //         {
   //             logger.Error(e.mErrmsg);
   //             throw e;
   //         }
   //         catch (Exception e)
   //         {
   //             logger.Error(e.Message);
   //             throw new SystemException(e.Message);
   //         }
   //         finally
   //         {
   //             logger.Debug("(CombineLCMandBTDL_TPDL)CheckTpdlSn End,"
   //            + " [lcmCTNum]: " + lcmCTNum
   //            + " [tpdlSn]:" + tpdlSn);
   //         }
   //     }

        /// <summary>
        /// Combine BTDL
        /// </summary>
        /// <param name="lcmCTNum"></param>
        /// <param name="btdlSn"></param>
        public void CombineBTDL(string lcmCTNum, string btdlSn, string pCode)
        {
            logger.Debug("(CombineLCMandBTDL_TPDL)CombineBTDL start,"
   + " [lcmCTNum]: " + lcmCTNum
   + " [btdlSn]:" + btdlSn);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {

                string sessionKey = lcmCTNum;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.BTDLSN, btdlSn);
                    sessionInfo.AddValue(Session.SessionKeys.IsComplete, false);
                    sessionInfo.AddValue(Session.SessionKeys.PCode, pCode);
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
                logger.Debug("(CombineLCMandBTDL_TPDL)CombineBTDL End,"
   + " [lcmCTNum]: " + lcmCTNum
   + " [btdlSn]:" + btdlSn);
            }
        }

        /// <summary>
        /// Combine TPDL
        /// </summary>
        /// <param name="lcmCTNum"></param>
        /// <param name="tpdlSn"></param>
        public void CombineTPDL(string lcmCTNum, string tpdlSn, string pCode)
        {
            logger.Debug("(CombineLCMandBTDL_TPDL)CombineTPDL start,"
+ " [lcmCTNum]: " + lcmCTNum
+ " [tpdlSn]:" + tpdlSn);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {

                string sessionKey = lcmCTNum;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.TPDLSN, tpdlSn);
                    sessionInfo.AddValue(Session.SessionKeys.IsComplete, false);
                    sessionInfo.AddValue(Session.SessionKeys.PCode, pCode);
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
                logger.Debug("(CombineLCMandBTDL_TPDL)CombineTPDL End,"
   + " [lcmCTNum]: " + lcmCTNum
   + " [tpdlSn]:" + tpdlSn);
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
