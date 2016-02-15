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
    public partial class SKULabelPrint : MarshalByRefObject, ISKULabelPrint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IRCTOLabelReprint Members

        public ArrayList GetPCBID(string ProductID)
        {
            logger.Debug("(SKULabelPrint)GetPCBID start, ProdId:" + ProductID);
            ArrayList retrunValue = new ArrayList();
            try
            {
                IProductRepository iProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                //IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                IProduct Product = iProductRepository.Find(ProductID);
                string paqcStatus = Product.GetAttributeValue("PAQC_QCStatus") ?? "";
                switch (paqcStatus)
                {
                    case "":
                        throw new FisException("PAK014", new string[] { });
                    case "8":
                    case "B":
                    case "C":
                        throw new FisException("PAK014", new string[] { });
                    case "A":
                        throw new FisException("PAK015", new string[] { });
                    default:
                        break;
                }
                retrunValue.Add(ProductID);
                retrunValue.Add(Product.PCBID);
                retrunValue.Add(Product.MAC);
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
                logger.Debug("(SKULabelPrint)GetPCBID end, ProdId:" + ProductID);
            }
        }

        public ArrayList Print(string prodid, string PCBID, string reason, string editor, string station, string customer, string pCode, IList<PrintItem> printItems)
        {
            ArrayList retrunValue = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();
            string mo = "";
            try
            {
                logger.Debug("(SKULabelPrint)Reprint start, startProdId:" + prodid + " endProdId:" + prodid + " editor:" + editor + " customerId:" + customer);

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
                    RouteManagementUtils.GetWorkflow(station, "SKULabelPrint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    //ITC-1360-1265
                    Session.AddValue(Session.SessionKeys.PrintLogName, "SKU Label");
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
                string MBCT2 = (string)Session.GetValue("MBCT2");
                retrunValue.Add(returnList);
                retrunValue.Add(prodid);
                retrunValue.Add(MBCT2);
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
                logger.Debug("(SKULabelPrint)Reprint end, mo:" + mo + " startProdId:" + prodid + " endProdId:" + prodid + " editor:" + editor + " station:" + station + " customerId:" + customer);

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
    }
}
