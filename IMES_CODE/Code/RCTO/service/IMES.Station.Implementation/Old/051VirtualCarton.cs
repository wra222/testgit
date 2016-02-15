/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006            Create 
 * 2010-04-13   207006            ITC-1155-0036
 * 2009-04-13   207006            ITC-1155-0038
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.FA.Product;
using IMES.DataModel;
using log4net;
using IMES.Route;

namespace IMES.Station.Implementation
{
    class _051VirtualCarton : MarshalByRefObject, IVirtualCarton
    {
//2010-04-13   207006            ITC-1155-0036
//2009-04-13   207006            ITC-1155-0038
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType TheType = Session.SessionType.Common ;
        private string WFName = "051VirtualCarton.xoml";
        private string WFRule = "051virtualcarton.rules";

        #region IVirtualCarton Members

        public IMES.DataModel.ProductModel StartWorkFlow(string firstCustSN, string line, string editor, string station, string customer)
        {
            logger.Debug("(_051VirtualCarton)StartWorkFlow start, line:" + line + " custSN:" + firstCustSN + " editor:" + editor + " station:" + station + " customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = firstCustSN;
            try
            {
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor,station , line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, WFName, WFRule, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.CustSN , firstCustSN);
                    Session.AddValue(Session.SessionKeys.IsComplete, false );
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

                IProduct  prod = (Product)(Session.GetValue(Session.SessionKeys.Product));
                Session.AddValue(Session.SessionKeys.FirstProductModel , prod.Model);

                ProductModel ProductModel = new ProductModel();

                ProductModel.CustSN = prod.CUSTSN ;
                ProductModel.Model = prod.Model;
                ProductModel.ProductID = prod.ProId;

                return ProductModel;

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
                logger.Debug("(_051VirtualCarton)StartWorkFlow end, line:" + line + " custSN:" + firstCustSN + " editor:" + editor + " station:" + station + " customer:" + customer);

            }
        }

        public IMES.DataModel.ProductModel ScanSN(string firstCustSN, string custSN)
        {

            logger.Debug("(_051VirtualCarton)ScanSN start, firstCustSN:" + firstCustSN + " custSN:" + custSN);

            try
            {
                FisException ex;
                List<string> erpara = new List<string>();
                string sessionKey = firstCustSN;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);


                if (Session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    Session.AddValue(Session.SessionKeys.CustSN, custSN);
                    Session.Exception = null;
                    Session.SwitchToWorkFlow();

                    //check workflow exception
                    if (Session.Exception != null)
                    {
                        if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            Session.ResumeWorkFlow();
                        }
                        throw Session.Exception;
                    }

                    IProduct prod = (Product)(Session.GetValue(Session.SessionKeys.Product));
                    
                    ProductModel ProductModel = new ProductModel();

                    ProductModel.CustSN = prod.CUSTSN;
                    ProductModel.Model = prod.Model;
                    ProductModel.ProductID = prod.ProId;

                    return ProductModel;
                }
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
                logger.Debug("(_051VirtualCarton)ScanSN end, firstCustSN:" + firstCustSN + " custSN:" + custSN);
            }
        }

        public IList<IMES.DataModel.PrintItem> Save(string firstCustSN, IList<IMES.DataModel.PrintItem> printItems, out string virtualCartonNo)
        {
            logger.Debug("(_051VirtualCarton)Save start, firstCustSN:" + firstCustSN);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = firstCustSN;
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            if (Session == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                throw ex;
            }
            try
            {

                Session.Exception = null;
                Session.AddValue(Session.SessionKeys.IsComplete, true);
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

                IList<PrintItem> returnList = this.getPrintList(Session);
                virtualCartonNo = ((List<string>)Session.GetValue(Session.SessionKeys.VirtualCartonNoList))[0];
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
                logger.Debug("(_051VirtualCarton)Save end, firstCustSN:" + firstCustSN);
            }
        }

        public void Cancel(string uutSn)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + uutSn);

                Session currentSession = SessionManager.GetInstance.GetSession(uutSn, TheType);

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
                logger.Debug("Cancel end, sessionKey:" + uutSn);
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
                //throw new  SysException(e);
            }
        }
    }
}
