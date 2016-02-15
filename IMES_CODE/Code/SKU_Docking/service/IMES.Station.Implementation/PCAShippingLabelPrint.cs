/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PCAShippingLabelPrint
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-12-01   zhu lei           Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.PCA.MB;
using log4net;
using IMES.Route;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Warranty;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// 实现IPCAShippingLabelPrint接口，PCAShippingLabelPrint实现类,实现PCAShippingLabelPrint打印和重印功能
    /// </summary>
    public class PCAShippingLabelPrint : MarshalByRefObject, IPCAShippingLabelPrint 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType TheType = Session.SessionType.MB;

        #region IPCAShippingLabelPrint Members
        /// <summary>
        /// InputMBSNo
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="MBSNo"></param>
        /// <param name="checkPCMBRCTOMB"></param>
        /// <param name="station"></param>
        /// <param name="editor"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IList InputMBSNo(string pdLine, string MBSNo, string checkPCMBRCTOMB, string station, string editor, string customerId)
        {
            logger.Debug("(PCAShippingLabelPrint)InputMBSNo start, pdLine:" + pdLine + " MBSNo:" + MBSNo + " editor:" + editor + " station:" + station + " customerId:" + customerId);

            FisException ex;
            IList ret = new ArrayList();
            List<string> erpara = new List<string>();
            string sessionKey = MBSNo;
            try
            {
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, station, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "PCAShippingLabelPrint.xoml", "PCAShippingLabelPrint.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.CN, checkPCMBRCTOMB);
                    Session.AddValue(Session.SessionKeys.PrintLogName, "MBSNO");
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, sessionKey);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, sessionKey);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, "");
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
               
                ret.Add(mb.ModelObj.Family);
                ret.Add(mb.PCBModelID);
                ret.Add(mb.MAC);
                ret.Add(mb.IECVER);
                ret.Add(mb.ECR);

                return ret;

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
                logger.Debug("(PCAShippingLabelPrint)InputMBSNo end, pdLine:" + pdLine + " MBSNo:" + MBSNo + " editor:" + editor + " station:" + station + " customerId:" + customerId);
            }
        }

        /// <summary>
        /// save
        /// </summary>
        /// <param name="MBno"></param>
        /// <param name="model"></param>
        /// <param name="dcode"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public ArrayList save(string MBno, string model, string dcode, string region, IList<PrintItem> printItems)
        {
            logger.Debug("(PCAShippingLabelPrint)save start, MBno:" + MBno + "model:" + model + "dcode:" + dcode);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = MBno;
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            ArrayList ret = new ArrayList();
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

                Session.AddValue(Session.SessionKeys.ModelName, model);
                Session.AddValue(Session.SessionKeys.WarrantyCode, dcode);
                Session.AddValue(Session.SessionKeys.InfoValue, region);
                Session.AddValue(Session.SessionKeys.PrintItems, printItems);

                Session.SwitchToWorkFlow();


                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }

                    throw Session.Exception;
                }

                var retDCode = (string)Session.GetValue(Session.SessionKeys.DCode);
                var printLst = (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);

                ret.Add(retDCode);
                ret.Add(printLst);

                return ret;
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
                logger.Debug("(PCAShippingLabelPrint)save end, MBno:" + MBno + "model:" + model　+ "dcode:" + dcode);
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

        /// <summary>
        /// 重印标签
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="reason"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        public ArrayList ReprintLabel(string mbSno, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(PCAShippingLabelReprint)ReprintLabel start, [mbSno]:" + mbSno
                + " [reason]: " + reason
                + " [line]: " + line
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            bool isProduct = false;

            if (mbSno.Length == 9)
            {
                var objProduct = CommonImpl.GetProductByInput(mbSno, CommonImpl.InputTypeEnum.ProductIDOrCustSN);

                if (String.IsNullOrEmpty(objProduct.PCBID))
                {
                    erpara.Add(mbSno);
                    ex = new FisException("CHK400", erpara);
                    throw ex;
                }

                mbSno = objProduct.PCBID;
                isProduct = true;
            }

            string sessionKey = mbSno;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (session == null)
                {
                    session = new Session(sessionKey, TheType, editor, station, string.Empty, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", string.Empty);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PCAShippingLabelReprint.xoml", string.Empty, wfArguments);

                    session.AddValue(Session.SessionKeys.MBSN, mbSno);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.PrintLogName, "MBSNO");
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, mbSno);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, mbSno);
                    session.AddValue(Session.SessionKeys.PrintLogDescr, "");
                    session.AddValue(Session.SessionKeys.Reason, reason);
                    session.AddValue("isProduct", isProduct);
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

                //session.SwitchToWorkFlow();

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }

                ArrayList ret = new ArrayList();
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                var mb = mbRepository.Find(mbSno);
                string dCode = mb.DateCode;

                var printLst = (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);

                ret.Add(dCode);
                ret.Add(printLst);
                ret.Add(isProduct);
                ret.Add(mbSno);

                return ret;
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
                logger.Debug("(PCAShippingLabelReprint)ReprintLabel end, [mbSno]:" + mbSno
                    + " [reason]: " + reason
                    + " [line]: " + line
                    + " [editor]:" + editor
                    + " [station]:" + station
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// SetDataCodeValue
        /// </summary>
        /// <param name="model"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public string SetDataCodeValue(string model, string customer)
        {
            string value = "";
            string str = "";
            string ret = "";

            try
            {
                logger.Debug("SetDataCodeValue start, model:" + model + "customer:" + customer);

                IWarrantyRepository wr = RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository, Warranty>();
                IList<Warranty> warrantys = wr.GetDCodeRuleListForMB("Customer");
                
                IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                Model mdl = modelRepository.Find(model);
                if (mdl == null || mdl.Status != "1")
                {
                    throw new FisException("CHK038", new List<string>() { model });
                }

                value = (string)mdl.GetAttribute("WRNT");
                if (value != "")
                {
                    str = value.Substring(0, 1);
                }

                if (str == "1")
                {
                    var val = (from w in warrantys
                                where (w.WarrantyCode == "4")
                                select w.Descr.ToString()).ToArray();
                    ret = val[0].ToString();
                }
                else if (str == "0")
                {
                    var val = (from w in warrantys
                               where w.WarrantyCode == "0"
                               select w.Descr.ToString()).ToArray();
                    ret = val[0].ToString();

                }
                else if (str == "3")
                {
                    var val = (from w in warrantys
                               where w.WarrantyCode == "5"
                               select w.Descr.ToString()).ToArray();
                    ret = val[0].ToString();

                }
                else if (str == "9")
                {
                    var val = (from w in warrantys
                               where w.WarrantyCode == "3"
                               select w.Descr.ToString()).ToArray();
                    ret = val[0].ToString();

                }
                return ret;
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
                logger.Debug("SetDataCodeValue end, model:" + model + "customer:" + customer);
            }
        }

        #endregion
    }
}
