/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006            Create 
 * 2009-11-03   207006            ITC-1122-0064
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using log4net;
using IMES.Route;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.FA.Product;

namespace IMES.Station.Implementation
{
    public class AssignWHLocation : MarshalByRefObject, IAssignWHLocation
    {
        //private const string Station ="";
        //private bool hasDealWithOneFKU = false;
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IAssignWHLocation Members

        //public void InputProdId(string floor, string pdLine, string prodId, string editor, string stationId, string customerId)
        public ArrayList InputProdId(string floor, string pdLine, string customerSn, string editor, string stationId, string customerId, string pre_model)
        {
            logger.Debug("(_AssignWHLocation)InputProdId start, floor:" + floor + " pdLine:" + pdLine + " customerSn:" + customerSn + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            ArrayList this_array_list_return = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();
            //string sessionKey = prodId;
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(customerSn, CommonImpl.InputTypeEnum.CustSN);
                if (currentProduct.ProId == "")
                {
                    throw new FisException("CHK020", new string[] { customerSn });
                }

                if ((pre_model.Length > 0) && (currentProduct.Model != pre_model))
                {
                    throw new FisException("CHK252", new string[] { });
                }

                string sessionKey = currentProduct.ProId;
                string this_model = currentProduct.Model;
                string this_line = currentProduct.Status.Line;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                
                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey); // key must be prodId.
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "AssignWHLocation.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue("_product_line", this_line);
                    Session.AddValue(Session.SessionKeys.Floor, floor);
                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }


                    //Release The XXX: 2012.04.24 LiuDong
                    try
                    {
                        Session.WorkflowInstance.Start();
                        Session.SetHostWaitOne();
                    }
                    finally
                    {
                        IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                        productRepository.ReleaseLockByTransThread("BTLoc", (Guid)Session.GetValue<Guid>(Session.SessionKeys.lockToken_Loc));
                    }
                    //Release The XXX: 2012.04.24 LiuDong


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

                //if (hasDealWithOneFKU == false) hasDealWithOneFKU = true;
                this_array_list_return.Add(customerSn); // customerId
                this_array_list_return.Add(sessionKey); // ProdId
                this_array_list_return.Add(this_model); // model
                PakBtLocMasInfo CurrentLocation = (PakBtLocMasInfo)Session.GetValue(Session.SessionKeys.WHLocationObj);
                this_array_list_return.Add(CurrentLocation.snoId);
                //this_array_list_return.Add(CurrentLocation.cmbQty);//locQty);

                var currentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                SnoDetBtLocInfo condition = new SnoDetBtLocInfo();
                /*
                condition.snoId = currentProduct.ProId;
                //condition.sno = CurrentLocation.snoId;
                //condition.status = "In";
                 */
                //modified into following
                condition.sno = CurrentLocation.snoId;
                condition.status = "In";

                IList<SnoDetBtLocInfo> nGotSnoDetBtLocInfoList = currentRepository.GetSnoDetBtLocInfosByCondition(condition);
                int x = 0;

                foreach (SnoDetBtLocInfo node in nGotSnoDetBtLocInfoList)
                {
                    if ((node.sno == CurrentLocation.snoId) && (node.status == "In"))
                    {
                        x++;
                    }
                }

                this_array_list_return.Add(x);

                return this_array_list_return;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                logger.Debug("(_AssignWHLocation)InputProdId end, floor:" + floor + " pdLine:" + pdLine + " customerSn:" + customerSn + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            }
        }

        // (string Floor, string station, string Location)
        public void closeLocation(string floor, string pdLine, string customerSn, string editor, string stationId, string customerId, string location)
        {
            logger.Debug("(_AssignWHLocation)closeLocation start, floor:" + floor + " pdLine:" + pdLine + " customerSn:" + customerSn + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId + " location:" + location);
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                var currentProduct = CommonImpl.GetProductByInput(customerSn, CommonImpl.InputTypeEnum.CustSN);
                if (currentProduct.ProId == "")
                {
                    throw new FisException("CHK020", new string[] { customerSn });
                }

                string sessionKey = currentProduct.ProId;
                string this_model = currentProduct.Model;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey); // key must be prodId.
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "CloseLocation.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.Floor, floor);
                    Session.AddValue(Session.SessionKeys.WHLocation, location);
                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    //Comment out the ConcurrentLocks Mechanism: 2012.04.24 LiuDong
                    ////Lock The XXX: 2012.04.24 LiuDong
                    //IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    //var identity = new ConcurrentLocksInfo();
                    //identity.clientAddr = "N/A";
                    //identity.customer = Session.Customer;
                    //identity.descr = string.Format("ThreadID: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                    //identity.editor = Session.Editor;
                    //identity.line = Session.Line;
                    //identity.station = Session.Station;
                    //identity.timeoutSpan4Hold = new TimeSpan(0, 0, 8).Ticks;
                    //identity.timeoutSpan4Wait = new TimeSpan(0, 0, 10).Ticks;
                    //Guid gUiD = Guid.Empty;
                    //try
                    //{
                    //  gUiD = productRepository.GrabLockByTransThread("BTLoc", "ALL", identity);

                        Session.WorkflowInstance.Start();
                        Session.SetHostWaitOne();
                    //}
                    //finally
                    //{
                    //    productRepository.ReleaseLockByTransThread("BTLoc", gUiD);
                    //}
                    ////Release The XXX: 2012.04.24 LiuDong
                    //Comment out the ConcurrentLocks Mechanism: 2012.04.24 LiuDong
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
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                logger.Debug("(_AssignWHLocation)closeLocation end, floor:" + floor + " pdLine:" + pdLine + " customerSn:" + customerSn + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId + " location:" + location);
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

                if (sessionKey.StartsWith("custSn:"))
                {
                    string customerSn = sessionKey.Substring(7);
                    string current_ = "";
                    try
                    {
                        var currentProduct = CommonImpl.GetProductByInput(customerSn, CommonImpl.InputTypeEnum.CustSN);
                        if (currentProduct.ProId == "")
                        {
                            return;
                        }
                        current_ = currentProduct.ProId;
                    }
                    catch (FisException )
                    {
                    }
                    catch (Exception )
                    {
                    }
                    if (current_ == "") return;
                    sessionKey = current_;
                }

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
#if false
        public void InputProdId(string floor, string pdLine, string prodId, string editor, string stationId, string customerId)
        {
            logger.Debug("(_AssignWHLocation)InputProdId start, floor:" + floor + " pdLine:" + pdLine + " prodId:" + prodId + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            //2009-11-03   207006            ITC-1122-0064
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            try
            {
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "AssignWHLocation.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.Floor, floor);
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

            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                logger.Debug("(_AssignWHLocation)InputProdId end, floor:" + floor + " pdLine:" + pdLine + " prodId:" + prodId + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            }
        } 
#endif

        #endregion
    }
}
