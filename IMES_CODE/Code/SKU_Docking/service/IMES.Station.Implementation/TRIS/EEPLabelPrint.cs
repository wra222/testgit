/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Generate Customer SN
* UI:CI-MES12-SPEC-FA-UC Generate Customer SN.docx –2011/11/11 
* UC:CI-MES12-SPEC-FA-UI Generate Customer SN.docx –2011/11/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-01   Du.Xuan               Create   
* ITC-1360-1020 增加zzzz上限防护
* Known issues:
* TODO：
* 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Route;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.UnitOfWork;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// IGenerateCustomerSN接口的实现类
    /// </summary>
    public class EEPLabelPrint : MarshalByRefObject, IEEPLabelPrint 
    {
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region EEPLabelPrint members

        /// <summary>
        /// 输入Product Id和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        public ArrayList InputProdId(string pdLine, string prodId, string editor, string stationId, string PrintLogName, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(EEPLabelPrint)InputProdId start, [pdLine]:" + pdLine
                + " [prodId]: " + prodId
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, ProductSessionType, editor, stationId, pdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "EEPLabelPrint.xoml", "EEPLabelPrint.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, prodId);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, prodId);
                    session.AddValue(Session.SessionKeys.PrintLogDescr, "");
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);

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
                UnitOfWork uow = new UnitOfWork();
                ArrayList retList = new ArrayList();
                IProductRepository iProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                Product tempProduct = (Product)session.GetValue(Session.SessionKeys.Product);
                ProductModel curProduct = new ProductModel();
                if (string.IsNullOrEmpty(tempProduct.CUSTSN))
                {
                    tempProduct.CUSTSN = tempProduct.ProId;
                }
                iProductRepository.Update(tempProduct, uow);
                uow.Commit();
                curProduct.CustSN = tempProduct.CUSTSN;
                curProduct.ProductID = tempProduct.ProId;
                curProduct.Model = tempProduct.Model;
                IList<PrintItem> retprintitem = (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
                retList.Add(curProduct);
                retList.Add(retprintitem);
                return retList;
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
                logger.Debug("(EEPLabelPrint)InputProdId end, [pdLine]:" + pdLine
                    + " [prodId]: " + prodId
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        public ArrayList Reprint(string prodid, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            ArrayList retrunValue = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                IPrintLogRepository printLogRepository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository>();
                PrintLog condition = new PrintLog();
                condition.Name = printItems[0].LabelType;
                condition.BeginNo = prodid;
                IList<PrintLog> printLogList = printLogRepository.GetPrintLogListByCondition(condition);
                if (printLogList.Count == 0)
                {
                    throw new FisException("CHK270",new string[]{});
                }

                logger.Debug("(EEPLabelPrint)Reprint start, ProdId:" + prodid + " line:" + line + " editor:" + editor + " customerId:" + customer);
                string sessionKey = prodid;
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
                    RouteManagementUtils.GetWorkflow(station, "EEPLabelRePrintl.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.Reason, reason);
                    Session.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, prodid);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, prodid);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, prodid);
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
                if (prodid.Length != 9)
                { 
                    Product tempProduct = (Product)Session.GetValue(Session.SessionKeys.Product);
                    if (!string.IsNullOrEmpty(tempProduct.CUSTSN))
                    {
                        prodid = tempProduct.ProId;
                    }
                }
                
                IList<PrintItem> returnList = (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);
                retrunValue.Add(prodid);
                retrunValue.Add(returnList);
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
                logger.Debug("(EEPLabelPrint)Reprint end, ProdId:" + prodid + " line:" + line + " editor:" + editor + " customerId:" + customer);
            }
        }


        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string prodId)
        {
            logger.Debug("(EEPLabelPrint)Cancel start, [prodId]:" + prodId);
            //FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
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
                logger.Debug("(EEPLabelPrint)Cancel end, [prodId]:" + prodId);                
            }
        }

        #endregion
    }
}
