using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
//using IMES.Station.Interface.CommonIntf;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.PrintLog;
using IMES.Route;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.PCA.MB;


namespace IMES.Station.Implementation
{

    /// <summary>
    /// </summary>
    public partial class RCTOLabelPrint : MarshalByRefObject, IRCTOLabelPrint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region IRCTOLabelPrint Members

        /// <summary>
        /// </summary>                
        public ArrayList ProcessProd(string prodid, string existProd, string pdline, string station, string editor, string customer)
        {
            logger.Debug("RCTOLabelPrint(ProcessProd) start:" + pdline + "," + prodid);
            ArrayList retValue = new ArrayList();

            try
            {
                string currentSessionKey = prodid.Substring(0, 9);
                string existSessionKey;
                if (!String.IsNullOrEmpty(existProd) && existProd.Length >= 9)
                {
                    existSessionKey = existProd.Substring(0, 9);
                    Cancel(existSessionKey);
                }

                Session currentSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Product);
                if (currentSession == null)
                {
                    currentSession = new Session(currentSessionKey, Session.SessionType.Product, editor, station, pdline, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("RCTOLabelPrint.xoml", "RCTOLabelPrint.rules", wfArguments);

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(currentSessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }
                    throw currentSession.Exception;
                }

                var currentProduct = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);
                
                retValue.Add(currentProduct.CUSTSN);
                retValue.Add(currentProduct.Model);
                retValue.Add(currentProduct.ProId);

                return retValue;
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
                logger.Debug("RCTOLabelPrint(ProcessProd) end:" + pdline + "," + prodid);
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
                logger.Debug("RCTOLabelPrint(Cancel) start, sessionKey:" + sessionKey);
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
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
                logger.Debug("RCTOLabelPrint(Cancel) end, sessionKey:" + sessionKey);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="mbsn"></param>
        /// <returns></returns>
        public ArrayList ProcessMB(string key, string mbsn, IList<PrintItem> printItems)
        {
            logger.Debug("(RCTOLabelPrint)ProcessMB start, [key]:" + key);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = key;

            ArrayList retValue = new ArrayList();

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                    IMB mb = mbRepository.Find(mbsn);
                    var currentModel = (IProduct)session.GetValue(Session.SessionKeys.Product);
                    string Model = currentModel.Model;

                    session.AddValue(Session.SessionKeys.MBCode, mbsn);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.MB, mb);
                    session.AddValue(Session.SessionKeys.CN, "");
                    session.AddValue(Session.SessionKeys.ModelName, Model);

                    session.AddValue(Session.SessionKeys.Reason, "");
                    session.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, key);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, key);
                    session.AddValue(Session.SessionKeys.PrintLogDescr, key);

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

                    IList<PrintItem> para = (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
                    var currentProduct = (IProduct)session.GetValue(Session.SessionKeys.Product);
                    retValue.Add(para);
                    retValue.Add(currentProduct.ProId);
                    return retValue;
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
                logger.Debug("(RCTOLabelPrint)ProcessMB end, [key]:" + key);
            }
        }
		
		public ArrayList RePrint(string sn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            try
            {
                logger.Debug("(RCTOLabelPrint)Reprint start, sn:" + sn + " line:" + line + " editor:" + editor + " customerId:" + customer);
                List<string> erpara = new List<string>();
                FisException ex;
                string key = sn;
                
                //Check Print Log
                var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();


                bool bFlag = false;

                bFlag = repository.CheckExistPrintLogByLabelNameAndDescr(printItems[0].LabelType, key);
                if (!bFlag)
                {

                    ex = new FisException("CHK270", erpara);
                    throw ex;
                }

                //Check Print Log
                string sessionKey = key;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (Session == null)
                {
                    Session = new Session(sessionKey, Session.SessionType.Product, editor, station, line, customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "RePrint.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    //Get Product data

                    Session.AddValue(Session.SessionKeys.ProductIDOrCustSN, sn);
                    Session.AddValue(Session.SessionKeys.Reason, reason);

                    Session.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);

                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, sn);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, sn);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, sn);



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


                IList<PrintItem> returnList = (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);
                ArrayList arr = new ArrayList();
                arr.Add(returnList);
                return arr;
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
                logger.Debug("(RCTOLabelPrint)Reprint end, sn:" + sn + " line:" + line + " editor:" + editor + " customerId:" + customer);
            }
        }

        #endregion

        #region IRCTOLabelReprint Members

        public ArrayList Reprint(string prodid, string reason, string editor, string station, string customer, string pCode, IList<PrintItem> printItems)
        {
            ArrayList retrunValue = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();
            string mo = "";
            try
            {
                logger.Debug("(RCTOLabelReprint)Reprint start, startProdId:" + prodid + " endProdId:" + prodid + " editor:" + editor + " customerId:" + customer);
                
                string sessionKey = prodid;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (Session == null)
                {
                    Session = new Session(sessionKey, Session.SessionType.Product, editor, station, "", customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "RCTOLabelReprint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    //ITC-1360-1265
                    Session.AddValue(Session.SessionKeys.PrintLogName, "CT Label");
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, prodid);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, prodid);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, prodid);
                    Session.AddValue(Session.SessionKeys.Reason, reason);

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


                IList<PrintItem> returnList = (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);
                
                retrunValue.Add(returnList);
                retrunValue.Add(prodid);

                return retrunValue;
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
                logger.Debug("(RCTOLabelReprint)Reprint end, mo:" + mo + " startProdId:" + prodid + " endProdId:" + prodid + " editor:" + editor + " station:" + station + " customerId:" + customer);

            }
        }
        #endregion
    }
}
