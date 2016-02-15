/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 2010-02-03   207006     ITC-1122-0079
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.FisObject.Common.Part;
namespace IMES.Station.Implementation
{
    public class CombineCarton_CR : MarshalByRefObject, ICombineCarton_CR
    {

        private IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        private const Session.SessionType TheType = Session.SessionType.Product;

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //    private string OriSessionkey;

        #region ICombineKeyParts Members
        public ArrayList inputProductFirst(string pdLine, string input, string Station, string editor, string customerId)
        {
            logger.Debug("(CombineCarton_CR)InputProdIdFirst start, pdLine:" + pdLine + " input:" + input + "Station:" + Station + " editor:" + editor + " customerId:" + customerId);

            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            
            var currentProduct = productRepository.GetProductByIdOrSn(input);
            FisException ex;
            List<string> erpara = new List<string>();
            if (currentProduct == null)
            {
                erpara.Add(input);
                throw new FisException("SFC002", erpara);
            }

            string sessionKey = currentProduct.ProId;
            //realProdID = sessionKey;
            //model = currentProduct.Model;

            try
            {
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {


                    Session = new Session(sessionKey, TheType, editor, Station, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", Station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    Session.AddValue(Session.SessionKeys.IsComplete, false);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(Station, "CombineCarton_CR.xoml", "CombineCarton_CR.rules", out wfName, out rlName);
                  
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
             
                    Session.AddValue(Session.SessionKeys.IsComplete, false);
                    Session.SetInstance(instance);
                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");

                        erpara.Add(input);
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
                       
                var prod = (IProduct)Session.GetValue(Session.SessionKeys.Product);
                ArrayList ret = new ArrayList();
                ret.Add("SUCCESSRET");
                ret.Add(prod.ProId);
                ret.Add(prod.Model);
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
                logger.Debug("(CombineCarton_CR)InputProdIdFirst end, pdLine:" + pdLine + " input:" + input + " editor:" + editor + " stationId:" + Station + " customerId:" + customerId);

            }
        }

        public ArrayList inputProductOther(string input, string FirstProductID)
        {
            logger.Debug("(CombineCarton_CR)InputProdIdOther start, input:" + input);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = FirstProductID;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    IProduct product = CommonImpl.GetProductByInput(input, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                    if (product == null)
                    {
                        erpara.Add(input);
                        ex = new FisException("CHK152", erpara);    //this Customer SN %1 is invalid , please rescan！
                        throw ex;
                    }
                    sessionInfo.AddValue(Session.SessionKeys.Product, product);
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
                var prod = (IProduct)sessionInfo.GetValue(Session.SessionKeys.Product);
                ArrayList ret = new ArrayList();
                ret.Add("SUCCESSRET");
                ret.Add(prod.ProId);
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(CombineCarton_CR)InputProdIdOther end, input:" + input);
            }
            
        }

        public ArrayList Reprint(string cartonsn, string reason, string line, string editor, string station, string customer, string pCode, IList<PrintItem> printItems)
        {
            ArrayList retrunValue = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();
            string mo = "";
            try
            {
                logger.Debug("(CombineCarton_CR)Reprint start, CartonSN:" + cartonsn + " line:" + line + " editor:" + editor + " customerId:" + customer);
                // Check Prodid....
                // Check Prodid....

                string sessionKey = cartonsn;
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
                    RouteManagementUtils.GetWorkflow(station, "CombineCartonReprint_CR.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.Reason, reason);
                    Session.AddValue(Session.SessionKeys.PrintLogName, "CR_CartonLabel");
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
                string id = (string)Session.GetValue(Session.SessionKeys.ProductIDOrCustSN);
                string action = "0";

                retrunValue.Add(action);
                retrunValue.Add(id);
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
                logger.Debug("(CombineCarton_CR)Reprint end, CartonSN:" + cartonsn + " line:" + line + " editor:" + editor + " customerId:" + customer);
            }
        }

        public IList<PrintItem> Save(string FirstProductID,IList<PrintItem> printItemLst, out string cartonsn)
        {
            logger.Debug("(CombineCarton_CR)Save start," + " [prodId]: " + FirstProductID);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = FirstProductID;
            Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            try
            {
                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.IsComplete, true);
                    sessionInfo.AddValue(Session.SessionKeys.PrintItems, printItemLst);
                    sessionInfo.Exception = null;
                    sessionInfo.SwitchToWorkFlow();
                    var prod = (IProduct)sessionInfo.GetValue(Session.SessionKeys.Product);
                    cartonsn = prod.CartonSN;


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
                logger.Debug("(CombineCarton_CR)Save  End," + " [prodId]: " + FirstProductID);
            }
            return (IList<PrintItem>)sessionInfo.GetValue(Session.SessionKeys.PrintItems);
        }

        public void ClearPart(string sessionKey)
        {
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                logger.Debug("(CombineCarton_CR)Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    var currentProduct = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);

                    IFlatBOM bom = null;
                    bom = (IFlatBOM)currentSession.GetValue(Session.SessionKeys.SessionBom);
                    bom.ClearCheckedPart();
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
                logger.Debug("(CombineCarton_CR)Cancel end, sessionKey:" + sessionKey);
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
                logger.Debug("(CombineCarton_CR)Cancel start, sessionKey:" + sessionKey);

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
                logger.Debug("(CombineCarton_CR)Cancel end, sessionKey:" + sessionKey);
            }
        }

        public IList<ConstValueTypeInfo> GetQtySelect()
        {
            return partRepository.GetConstValueTypeList("CleanCartonQty");
        }

      #endregion
    }
}
