/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:  ICT input interface implement
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2009-10-20  Zhao Meili(EB)        Create 
 * 2010-01-08  Zhao Meili(EB)        Modify: ITC-1103-0066、ITC-1103-0088、ITC-1103-0089、ITC-1103-0086
 * 200910-201005  itc207013          ITC-1103-0137,ITC-1103-0145
 *                                   ITC-1103-0148,ITC-1103-0224
 *                                   ITC-1103-0225,ITC-1103-0233
 *                                   ITC-1103-0245,ITC-1103-0246
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
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Station.Interface.CommonIntf;
using IMES.FisObject.PCA.MBModel;
using IMES.FisObject.PCA.MB;
using log4net;
using IMES.Infrastructure.Extend;
using IMES.Route;
using System.Collections;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// [ICT Input] 实现如下功能：
    /// 记录ICT测试结果:良品,则分配MAC号码并列印；不良,则记录不良信息.
    /// </summary>
    class ICTInput :MarshalByRefObject, IICTInput
    {
        private static readonly Session.SessionType theType = Session.SessionType.MB;
        private const string WF = "003ICTInput.xoml";
        private const string Rule = "003ICTInput.rules";
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 1.1	UC-PCA-ICTI-01 ICT Input
        /// 输入基本信息并进行卡站
        /// 异常情况：
        /// a.	如果用户没有选择[PdLine]，则报告错误：“请选择Product Line！！”
        /// b.	如果用户没有输入[ECR]，则报告错误：“请输入ECR!!”
        /// c.	如果[Cust Version]为空，则报告错误：“Please Entry Cust Version first !!”
        /// d.	如果用户输入的[Cust Version] 不满足Rule of Cust Version，则报告错误：“Format Error，Please Entry Cust Version again!!”
        /// e.	如果[IEC Version]为空，则报告错误：“Please Entry IEC Version first !!”
        /// f.	如果用户输入的[IEC Version] 不满足Rule of IEC Version，则报告错误：“Format Error，Please Entry IEC Version again!!”
        /// g.	如果用户没有输入[Multi Q’ty]，则报告错误：“Please Entry Multi Q’ty!!”
        /// h.	如果用户输入的[Multi Q’ty] 不在Range of Multi Q’ty 允许范围内，则报告错误：“Out of range. Please Entry Multi Q’ty again!! ”
        /// i.	如果没有输入[MB Sno]， 则报告错误：“Please Entry MB Sno!!”
        /// j.	如果用户没有选择[Batch Files/Template]，则报告错误：“Please Select Batch File/Template!!”
        /// k.	如果用户没有输入[Device No]，则报告错误：“Please Entry Device No!!”
        /// l.	如果用户没有输入[Date Code]，则报告错误：“Please Entry Date Code!!”
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="custVersion">Customer Version</param>
        /// <param name="IECVersion">IEC Version</param>
        /// <param name="multiQty">Multi-Quantity</param>
        /// <param name="MB_SNo">MB SNo</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <param name="ecr">ecr</param>
        /// <param name="IsQuantity">IsQuantity</param>
        public void InputMBSNo( string pdLine,
                                out string custVersion,
                                out string IECVersion,
                                int multiQty,
                                string MB_SNo,
                                string editor, 
                                string stationId, 
                                string customerId, 
                                string ecr, 
                                out bool IsQuantity)
        {
            logger.Debug("(ICTInput)InputMBSNo start,"
                + " [pdLine]:" + pdLine
                + " [multiQty]:" + multiQty
                + " [MB_SNo]:" + MB_SNo
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customerId);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                IsQuantity = false;
                string sessionKey = MB_SNo;
                custVersion = "";
                IECVersion = "";
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

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
                    //MB mainMB = new MB(MB_SNo,"","","","","","",IECVersion,custVersion,"",DateTime .MinValue ,DateTime .MinValue);
                    //sessionInfo.AddValue(Session.SessionKeys.MB, mainMB);

                    sessionInfo.AddValue(Session.SessionKeys.Qty, multiQty);
                    sessionInfo.AddValue(Session.SessionKeys.LoopCount, 0);
                    sessionInfo.AddValue(Session.SessionKeys.ECR, ecr);
                    
                    //Dean 20110330
                    
                    IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMES.FisObject.PCA.MB.IMB>();
                    IMES.FisObject.PCA.MB.IMB MBData = mbRepository.Find(MB_SNo);
                    custVersion = MBData.CUSTVER;
                    IECVersion = MBData.IECVER;                                                                         

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, WF, Rule, out wfName, out rlName);
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
                string mbcode = mb.ModelObj.Mbcode;
                //string family=mb.Family;
                IMBModelRepository mbmodelRepository = RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
                IList<EcrVersionInfo> ecrinfo = mbmodelRepository.getEcrVersionsByEcrAndMbcode(ecr, mbcode);


                //IList<string> ret=mbmodelRepository.GetMBFamilyList();
                
                //如果用户输入的[ECR]，在ECR 资料中没有找到，则报告错误：“ECR 资料不存在！！”
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

                #region Dean 20110329 Mark 试产/量产
                // Dean 20110329 Mark 试产/量产
                /*
                //MB SNo 所属的SMT Mo 的第一位，如果为P，则为试产；如果为M，则为量产
                if (mb.SMTMO.StartsWith("M"))
                {
                    IsQuantity = true;
                    //ITC-1103-0331
                   
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

                    if (string.IsNullOrEmpty(ecrinfo[0].IECVer) || ecrinfo[0].IECVer.Length != 4 || !ecrinfo[0].IECVer.Substring(1).StartsWith("."))
                    {
                        FisException ex1;
                        List<string> erpara1 = new List<string>();
                        ex1 = new FisException("CHK091", erpara1);
                        sessionInfo.ResumeWorkFlow();
                        throw ex1;
                    }

                    if (string.IsNullOrEmpty(ecrinfo[0].CustVer))
                    {
                        ecrinfo[0].CustVer = "";
                    }
                    else if (ecrinfo[0].CustVer.Length != 3)
                    {
                        FisException ex1;
                        List<string> erpara1 = new List<string>();
                        ex1 = new FisException("CHK092", erpara1);
                        sessionInfo.ResumeWorkFlow();
                        throw ex1;
                    }

                }
                else if (mb.SMTMO.StartsWith("P"))
                {
                    IsQuantity = false;

                }
                //ITC-1103-0336
                if (ecrinfo != null && ecrinfo.Count>0)
                {
                    custVersion = ecrinfo[0].CustVer;
                    IECVersion = ecrinfo[0].IECVer;
                }*/
                #endregion
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
                logger.Debug("(ICTInput)InputMBSNo End,"
                + " [pdLine]:" + pdLine
                + " [multiQty]:" + multiQty
                + " [MB_SNo]:" + MB_SNo
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customerId);
            }
        }


        /// <summary>
        /// 1.1	UC-PCA-ICTI-01 ICT Input
        /// 输入基本信息并进行卡站
        /// 异常情况：
        /// a.	如果用户没有选择[PdLine]，则报告错误：“请选择Product Line！！”
        /// b.	如果用户没有输入[ECR]，则报告错误：“请输入ECR!!”
        /// c.	如果[Cust Version]为空，则报告错误：“Please Entry Cust Version first !!”
        /// d.	如果用户输入的[Cust Version] 不满足Rule of Cust Version，则报告错误：“Format Error，Please Entry Cust Version again!!”
        /// e.	如果[IEC Version]为空，则报告错误：“Please Entry IEC Version first !!”
        /// f.	如果用户输入的[IEC Version] 不满足Rule of IEC Version，则报告错误：“Format Error，Please Entry IEC Version again!!”
        /// g.	如果用户没有输入[Multi Q’ty]，则报告错误：“Please Entry Multi Q’ty!!”
        /// h.	如果用户输入的[Multi Q’ty] 不在Range of Multi Q’ty 允许范围内，则报告错误：“Out of range. Please Entry Multi Q’ty again!! ”
        /// i.	如果没有输入[MB Sno]， 则报告错误：“Please Entry MB Sno!!”
        /// j.	如果用户没有选择[Batch Files/Template]，则报告错误：“Please Select Batch File/Template!!”
        /// k.	如果用户没有输入[Device No]，则报告错误：“Please Entry Device No!!”
        /// l.	如果用户没有输入[Date Code]，则报告错误：“Please Entry Date Code!!”
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="custVersion">Customer Version</param>
        /// <param name="IECVersion">IEC Version</param>
        /// <param name="multiQty">Multi-Quantity</param>
        /// <param name="MB_SNo">MB SNo</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <param name="ecr">ecr</param>
        /// <param name="IsQuantity">IsQuantity</param>
        public void InputMBSNo(string pdLine,
                            out string custVersion,
                            out string IECVersion,
                            int multiQty,
                            string MB_SNo,
                            string editor, 
                            string stationId, 
                            string customerId, 
                            string ecr,
                            string shipmode,
                            out bool IsQuantity)
        {
            logger.Debug("(ICTInput)InputMBSNo start,"
                + " [pdLine]:" + pdLine
                + " [multiQty]:" + multiQty
                + " [MB_SNo]:" + MB_SNo
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customerId);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                IsQuantity = false;
                string sessionKey = MB_SNo;
                custVersion = "";
                IECVersion = "";
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

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
                    //MB mainMB = new MB(MB_SNo,"","","","","","",IECVersion,custVersion,"",DateTime .MinValue ,DateTime .MinValue);
                    //sessionInfo.AddValue(Session.SessionKeys.MB, mainMB);

                    sessionInfo.AddValue(Session.SessionKeys.Qty, multiQty);
                    sessionInfo.AddValue(Session.SessionKeys.LoopCount, 0);
                    sessionInfo.AddValue(Session.SessionKeys.ECR, ecr);
                                        
                    //Dean 20110330
                    sessionInfo.AddValue(ExtendSession.SessionKeys.ShipMode, shipmode);
                  
                    IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMES.FisObject.PCA.MB.IMB>();
                    IMES.FisObject.PCA.MB.IMB MBData = mbRepository.Find(MB_SNo);
                    custVersion = MBData.CUSTVER;
                    IECVersion = MBData.IECVER;

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, WF, Rule, out wfName, out rlName);
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
                string mbcode = mb.ModelObj.Mbcode;
                //string family=mb.Family;
                IMBModelRepository mbmodelRepository = RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
                IList<EcrVersionInfo> ecrinfo = mbmodelRepository.getEcrVersionsByEcrAndMbcode(ecr, mbcode);


                //IList<string> ret=mbmodelRepository.GetMBFamilyList();

                //如果用户输入的[ECR]，在ECR 资料中没有找到，则报告错误：“ECR 资料不存在！！”
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

                #region Dean 20110329 Mark 试产/量产
                // Dean 20110329 Mark 试产/量产
                /*
                //MB SNo 所属的SMT Mo 的第一位，如果为P，则为试产；如果为M，则为量产
                if (mb.SMTMO.StartsWith("M"))
                {
                    IsQuantity = true;
                    //ITC-1103-0331
                   
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

                    if (string.IsNullOrEmpty(ecrinfo[0].IECVer) || ecrinfo[0].IECVer.Length != 4 || !ecrinfo[0].IECVer.Substring(1).StartsWith("."))
                    {
                        FisException ex1;
                        List<string> erpara1 = new List<string>();
                        ex1 = new FisException("CHK091", erpara1);
                        sessionInfo.ResumeWorkFlow();
                        throw ex1;
                    }

                    if (string.IsNullOrEmpty(ecrinfo[0].CustVer))
                    {
                        ecrinfo[0].CustVer = "";
                    }
                    else if (ecrinfo[0].CustVer.Length != 3)
                    {
                        FisException ex1;
                        List<string> erpara1 = new List<string>();
                        ex1 = new FisException("CHK092", erpara1);
                        sessionInfo.ResumeWorkFlow();
                        throw ex1;
                    }

                }
                else if (mb.SMTMO.StartsWith("P"))
                {
                    IsQuantity = false;

                }
                //ITC-1103-0336
                if (ecrinfo != null && ecrinfo.Count>0)
                {
                    custVersion = ecrinfo[0].CustVer;
                    IECVersion = ecrinfo[0].IECVer;
                }*/
                #endregion
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
                logger.Debug("(ICTInput)InputMBSNo End,"
                + " [pdLine]:" + pdLine
                + " [multiQty]:" + multiQty
                + " [MB_SNo]:" + MB_SNo
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customerId);
            }
        }
        /// <summary>
        /// 刷入Defect code
        /// </summary>
        /// <param name="MB_SNo">MB SNo</param>
        /// <param name="defectList">Defect IList</param>
        /// <param name="printItems">printItems</param>
        /// <param name="CustVersion">Customer Version</param>
        /// <param name="IECVersion">IEC Version</param>
        /// <param name="subMBSNs">sub MBSN list</param>
        public IList<PrintItem> InputDefectCodeList(
            string MB_SNo,
            IList<string> defectList, IList<PrintItem> printItems, out IList<string> subMBSNs,string IECVersion,string CustVersion)
        {
            string defstr = "";
            if (defectList != null && defectList.Count > 0)
            {                
                foreach (string singledef in defectList)
                {
                    defstr = defstr+","+singledef;
                }
                defstr ="{"+defstr.Substring(1) +  "}";
            }
            logger.Debug("(ICTInput)InputDefectCodeList start,"
             + " [MB_SNo]:" + MB_SNo
             + " [defectList]:" + defstr);
            FisException ex;
            List<string> erpara = new List<string>();
            //IList<PrintItem> printList;
            try
            {

                string sessionKey = MB_SNo;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.DefectList, defectList);
                    sessionInfo.AddValue(Session.SessionKeys.PrintItems, printItems);
 		            sessionInfo.AddValue(Session.SessionKeys.CustomVersion, CustVersion);
                    sessionInfo.AddValue(Session.SessionKeys.IECVersion, IECVersion);
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


               
                int mbqty=(int)sessionInfo.GetValue(Session.SessionKeys.Qty);
                if (mbqty == 1 || (defectList != null && defectList.Count > 0))
                {
                    subMBSNs = new List<string>() { MB_SNo };                    
                }
                else
                {
                    subMBSNs = (IList<string>)sessionInfo.GetValue(Session.SessionKeys.MBNOList);                  
                }
                   
                if (defectList != null && defectList.Count > 0)
                {
                    return new List<PrintItem>(); 
                }
                else
                {
                    return (IList<PrintItem>)sessionInfo.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug("(ICTInput)InputDefectCodeList End,"
                               + " [MB_SNo]:" + MB_SNo
                               + " [defectList]:" + defstr);
            }
        }



        public IList<PrintItem> InputDefectCodeList(
            string MB_SNo,
            IList<string> defectList, IList<PrintItem> printItems, out IList<string> subMBSNs, string IECVersion, string CustVersion, out IList<string> subMBCustSNs)
        {
            string defstr = "";
            if (defectList != null && defectList.Count > 0)
            {
                foreach (string singledef in defectList)
                {
                    defstr = defstr + "," + singledef;
                }
                defstr = "{" + defstr.Substring(1) + "}";
            }
            logger.Debug("(ICTInput)InputDefectCodeList start,"
             + " [MB_SNo]:" + MB_SNo
             + " [defectList]:" + defstr);
            FisException ex;
            List<string> erpara = new List<string>();
            //IList<PrintItem> printList;
            try
            {

                string sessionKey = MB_SNo;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.DefectList, defectList);
                    sessionInfo.AddValue(Session.SessionKeys.PrintItems, printItems);
                    sessionInfo.AddValue(Session.SessionKeys.CustomVersion, CustVersion);
                    sessionInfo.AddValue(Session.SessionKeys.IECVersion, IECVersion);
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



                int mbqty = (int)sessionInfo.GetValue(Session.SessionKeys.Qty);
                if (mbqty == 1 || (defectList != null && defectList.Count > 0))
                {
                    subMBSNs = new List<string>() { MB_SNo };
                    subMBCustSNs = new List<string>();
                    if (sessionInfo.GetValue(Session.SessionKeys.CustSN)!=null)
                        subMBCustSNs.Add(sessionInfo.GetValue(Session.SessionKeys.CustSN).ToString());
                    else
                        subMBCustSNs.Add("");
                    //subMBCustSNs.Add(sessionInfo.GetValue(Session.SessionKeys.CustSN));                    
                }
                else
                {
                    subMBSNs = (IList<string>)sessionInfo.GetValue(Session.SessionKeys.MBNOList);
                    subMBCustSNs = new List<string>();
                                     
                    for (int i = 0; i <= subMBSNs.Count - 1; i++)
                    {
                        IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMES.FisObject.PCA.MB.IMB>();
                        IMES.FisObject.PCA.MB.IMB MBData = mbRepository.Find(subMBSNs[i]);
                        if (MBData != null)
                            subMBCustSNs.Add(MBData.CustSn);
                        else
                            subMBCustSNs.Add("");
                    }                                       
                }

                if (defectList != null && defectList.Count > 0)
                {
                    return new List<PrintItem>();
                }
                else
                {
                    return (IList<PrintItem>)sessionInfo.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug("(ICTInput)InputDefectCodeList End,"
                               + " [MB_SNo]:" + MB_SNo
                               + " [defectList]:" + defstr);
            }
        }



        ///// <summary>
        ///// 取得Customer Version和IEC Version
        ///// </summary>
        ///// <param name="MB_CODE">MB_CODE</param>
        ///// <param name="ECR">ECR</param>
        ///// <param name="IECVersion">IEC Version</param>
        ///// <returns>Customer Version</returns>
        //public string GetCustVersion(string MB_CODE, string ECR, out string IECVersion)
        //{
        //    logger.Debug("(ICTInput)GetCustVersion start,"
        //        + " [MB_CODE]:" + MB_CODE
        //        + " [ECR]: " + ECR);
        //    try
        //    {
        //        IMBModelRepository mbmodelRepository = RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
        //        IList<EcrVersionInfo> ecrinfo = mbmodelRepository.getEcrVersionsByEcrAndMbcode(ECR, MB_CODE);
        //        if (ecrinfo != null && ecrinfo.Count > 0)
        //        {
        //            IECVersion = ecrinfo[0].IECVer;
        //            return ecrinfo[0].CustVer;
        //        }
        //        else
        //        {
        //            IECVersion = "";
        //            return "";
        //        }
        //    }
        //    catch (FisException e)
        //    {
        //        logger.Error(e.mErrmsg);
        //        throw e;
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e.Message);
        //        throw new SystemException(e.Message);
        //    }
        //    finally
        //    {
        //        logger.Debug("(ICTInput)GetCustVersion End,"
        //           + " [MB_CODE]:" + MB_CODE
        //           + " [ECR]: " + ECR);
        //    }
        //}

        /// <summary>
        /// 1.2	UC-PCA-ICTI-02 Reprint
        /// 1.	标签损坏时，重印
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="MB_SNo">MB SNo</param>
        /// <param name="editor">operator</param>
        /// <returns>Print Items</returns>
  public IList<PrintItem> Reprint(
            string pdLine,
            string MB_SNo,
            string editor, string stationId, string customerId,
            string reason, IList<PrintItem> printItems)
        {
            throw new NotImplementedException();
        }



  /// <summary>
  /// 输入设备号
  /// </summary>
  /// <param name="MB_SNo">MB_SNo</param>
  /// <param name="deviceNo">设备号</param>
        public void InputDeviceNo(string MB_SNo, string deviceNo)
        {
            logger.Debug("(ICTInput)InputDeviceNo start," 
                + " [MB_SNo]: " + MB_SNo
                + " [deviceNo]:" + deviceNo);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {

                string sessionKey = MB_SNo;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.MBSN, MB_SNo);
                    sessionInfo.AddValue(Session.SessionKeys.FixtureID, deviceNo);
                    sessionInfo.Exception = null;
                    //sessionInfo.SwitchToWorkFlow();

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
                logger.Debug("(ICTInput)InputDeviceNo End,"
                               + " [MB_SNo]: " + MB_SNo
                               + " [deviceNo]:" + deviceNo);
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
