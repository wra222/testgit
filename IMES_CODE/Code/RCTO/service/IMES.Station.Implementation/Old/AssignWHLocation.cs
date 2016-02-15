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
        public ArrayList InputProdId(string floor, string pdLine, string customerSn, string editor, string stationId, string customerId)
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
                    //if (hasDealWithOneFKU == false) hasDealWithOneFKU = true;
                    this_array_list_return.Add(customerSn); // customerId
                    this_array_list_return.Add(sessionKey); // ProdId
                    this_array_list_return.Add(this_model); // model
                    this_array_list_return.Add(Session.SessionKeys.WHLocation); // location
                    this_array_list_return.Add(Session.SessionKeys.Qty); // Qty.
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
