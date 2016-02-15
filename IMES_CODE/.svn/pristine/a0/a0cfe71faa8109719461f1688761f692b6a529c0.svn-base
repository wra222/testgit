/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-01-06   zhu lei           Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Docking.Interface.DockingIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.DataModel;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.PCA.MB;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBModel;
using IMES.Infrastructure.Repository;
using IMES.Infrastructure.Repository.PCA;
using log4net;
using System.Collections;
using IMES.Route;
using IMES.Infrastructure.Repository._Schema;
using System.Data.SqlClient;
using System.Data;
namespace IMES.Docking.Implementation
{
    /// <summary>
    /// MBLabelPrint
    /// </summary>
    public class _MBLabelPrint : MarshalByRefObject, IMBLabelPrint, ISMTMO
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private static readonly string station;
        private static readonly Session.SessionType theType = Session.SessionType.Common;
        #region IMBLabelPrint Members

        /// <summary>
        /// Print
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="isNextMonth"></param>
        /// <param name="mbCode"></param>
        /// <param name="mo"></param>
        /// <param name="qty"></param>
        /// <param name="dateCode"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        /// <param name="startProdIdAndEndProdId"></param>
        /// <param name="_111"></param>
        /// <param name="factor"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public IList<PrintItem> Print(
            string pdLine,
            bool isNextMonth,
            string mbCode,
            string mo,
            int qty,
            string dateCode,
            string editor,
            string stationId, string customerId,
            out IList<string> startProdIdAndEndProdId, string _111, string factor, IList<PrintItem> printItems)
        {
            logger.Debug("(MBLabelPrint)Print start, pdLine:" + pdLine + " isNextMonth:" + isNextMonth.ToString() + " mo:" + mo + " qty:" +qty.ToString()+" dateCode:"+dateCode+ " editor:" + editor +" stationId:"+stationId+ " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            //IList<PrintItem> printList;
            try
            {
                string sessionKey = mo;

                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, theType,editor,stationId,pdLine,customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", theType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "002MBLabelPrint.xoml",  string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.IsNextMonth, isNextMonth);
                    Session.AddValue(Session.SessionKeys.Qty, qty);
                    Session.AddValue(Session.SessionKeys.DateCode, dateCode);
                    Session.AddValue(Session.SessionKeys.motherOrChild, "0");
                    Session.AddValue(Session.SessionKeys.MBMONO, mo);
                    Session.AddValue(Session.SessionKeys.ModelName,_111);
                    Session.AddValue(Session.SessionKeys.FamilyName, factor);
                    Session.AddValue(Session.SessionKeys.MBCode, mbCode);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.PrintLogName, "MB");

                    //MB mb = new MB();
                    //mb.Model = _111;

                    var mbModelRepository = (IMBModelRepository)RepositoryFactory.GetInstance().GetRepository<IMBModelRepository,IMBModel>();
                    MBModel model = (MBModel)mbModelRepository.Find(_111);

                    Session.AddValue(Session.SessionKeys.MBCode, model.Mbcode);
                    Session.AddValue(Session.SessionKeys.MBType, model.Type);

                    //Add 2012/08/13 区分整机和Docking
                    Session.AddValue("IsDocking", "Docking");
                     
                    Session.SetInstance(instance);
                    //for generate MB no

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

                //startProdIdAndEndProdId = (IList<string>)Session.GetValue(Session.SessionKeys.MBNOList);
                IList<string> MBNOList = new List<string>();

                string BegNo = (string)Session.GetValue(Session.SessionKeys.PrintLogBegNo);
                string EndNo = (string)Session.GetValue(Session.SessionKeys.PrintLogEndNo);
                MBNOList.Add(BegNo);
                MBNOList.Add(EndNo);

                startProdIdAndEndProdId = MBNOList;

                IList<PrintItem> returnList = this.getPrintList(Session);
                return returnList;
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
                logger.Debug("(MBLabelPrint)Print end, pdLine:" + pdLine + " isNextMonth:" + isNextMonth.ToString() + " mo:" + mo + " qty:" + qty.ToString() + " dateCode:" + dateCode + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
                
            }
        }

        #endregion

        private IList<PrintItem> getPrintList(Session session)
        {
            try
            {
                object printObject = session.GetValue(Session.SessionKeys.PrintItems);
                session.RemoveValue(Session.SessionKeys.PrintItems);
                if (printObject == null)
                {
                    return null;
                }
                IList<PrintItem> printItems = (IList<PrintItem>)printObject;

                return printItems;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Reprint
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="customerId"></param>
        /// <param name="reason"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public IList<PrintItem> RePrint(string mbSno, string customerId, string reason, string editor, string stationId, IList<PrintItem> printItems)
        {
            logger.Debug("(MBLabelReprint)Reprint start, MBSno:" + mbSno + " Reason:" + reason + " editor:" + editor + " station:" + stationId + " customerId:" + customerId);
            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList arr1 = new ArrayList();

            try
            {
                string sessionKey = mbSno;
                //       int Qty = Convert.ToInt32(endMBSNo) - Convert.ToInt32(startMBSNo) + 1;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, theType, editor, stationId, "", customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", theType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "002MBLabelReprint.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    //WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("002MBLabelReprint.xoml", null, wfArguments);
                    Session.AddValue(Session.SessionKeys.MBMONO, mbSno);
                    Session.AddValue(Session.SessionKeys.MBSN, mbSno);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.PrintLogName, "MBLabel");
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, mbSno);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, "");
                    Session.AddValue(Session.SessionKeys.Reason, reason);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, "Reprint");

                    //Add 2012/08/13 区分整机和Docking
                    Session.AddValue("IsDocking", "Docking");
       
                    Session.SetInstance(instance);
                    //for generate MB no

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

                return (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug("(MBLabelReprint)Reprint end, MBSno:" + mbSno + " Reason:" + reason + " editor:" + editor + " customerId:" + customerId);
            }
        }

        public IList<SMTMOInfo> GetSMTMOList(string _111LevelId)
        {
            IMBMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
            return (List<SMTMOInfo>)moRepository.GetSMTMOListFor002(_111LevelId);
        }

        public SMTMOInfo GetSMTMOInfo(string SMTMOId)
        {
            throw new NotImplementedException();
        }

        public IList<SMTMOInfo> GetSmtMoListByPno(string partNo)
        {
            throw new NotImplementedException();
        }

        public SMTMOInfo GetSmtmoInfoList(string SMTMO)
        {
            throw new NotImplementedException();
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

