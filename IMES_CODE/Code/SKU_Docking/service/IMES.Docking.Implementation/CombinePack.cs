﻿/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Combine Pack
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-5-28    itc200052           Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Linq;
using System.Text;
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
using IMES.Docking.Interface.DockingIntf;
using log4net;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Warranty;
using IMES.FisObject.PCA.MB;


using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.DN;
//using IMES.FisObject.PAK.StandardWeight;

using IMES.Station.Implementation;

//using IProduct = IMES.FisObject.FA.Product.IProduct;
//using ModelInfo = IMES.FisObject.Common.Model.ModelInfo;

using IMES.Infrastructure.Repository.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PAK.Pizza;




namespace IMES.Docking.Implementation
{
    public class CombinePack : MarshalByRefObject, ICombinePack
    {


        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    //    private string OriSessionkey;

        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
 
        #region ICombinePack Members
        public IList<BomItemInfo> InputProdIdorCustsn(string input, string editor, string stationId, string customerId, out string realProdID, out string model, out string CustomerSN, out bool IsPAQC)
        {
            logger.Debug("(CombinePack)InputProdId start, input:" + input + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            
            var currentProduct = productRepository.GetProductByIdOrSn(input);

            //var currentProduct = CommonImpl.GetProductByInput(input, CommonImpl.InputTypeEnum.ProductIDOrCustSN);

            FisException ex;
            List<string> erpara = new List<string>();
            if (currentProduct == null)
            {

                erpara.Add(input);
                throw new FisException("SFC002", erpara);
            }

            string sessionKey = currentProduct.ProId;
            realProdID = sessionKey;
            model = currentProduct.Model;
            CustomerSN = currentProduct.CUSTSN;

            IDeliveryRepository iDeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            ProductQCStatus qcStatus = iDeliveryRepository.GetQCStatus(realProdID, "PAQC");
            IsPAQC = false;
            if (qcStatus != null && qcStatus.Status == "8")
            {
                IsPAQC = true;
            }
            

            try
            {
                //ArrayList retLst = new ArrayList();
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {


                    Session = new Session(sessionKey, TheType, editor, stationId, "" , customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    // WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("_TEST.xoml", "_TEST.rules", wfArguments);
                //    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("021BoardInput.xoml", "021BoardInput.rules", wfArguments); 
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "CombinePack.xoml", "CombinePack.rules", out wfName, out rlName);
               //   RouteManagementUtils.GetWorkflow(stationId, "021BoardInput_TEST.xoml", "021BoardInput_TEST.rules", out wfName, out rlName);
                  
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
             
                    Session.AddValue(Session.SessionKeys.IsComplete, false);
                    //Session.AddValue(Session.SessionKeys.ifElseBranch, false);
                    Session.AddValue(Session.SessionKeys.CustSN, sessionKey);

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

                //get bom
                return PartCollection.GeBOM(sessionKey, TheType);

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
                logger.Debug("(CombinePack)InputProdId end, input:" + input + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            }
        }

        public IMES.DataModel.MatchedPartOrCheckItem InputMBSn(string prodId, string item)
        {
            logger.Debug("(CombinePack)InputMBSn start, prodId:" + prodId + " item:" + item);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
          //  string sessionKey = OriSessionkey;

            Session Session = SessionManager.GetInstance.GetSession(prodId, TheType);
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

                ArrayList returnItem = new ArrayList();

                Session.Exception = null;
                Session.AddValue(Session.SessionKeys.ValueToCheck, item);
                Session.InputParameter = item;
                Session.SwitchToWorkFlow();

                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }

                    throw Session.Exception;
                }

                //2010-02-03   207006     ITC-1122-0079
                //IList<MatchedPartOrCheckItem> MatchedList = new List<MatchedPartOrCheckItem>();
                //get matchedinfo
                PartUnit matchedPart = (PartUnit)Session.GetValue(Session.SessionKeys.MatchedParts);

                if (matchedPart != null)
                {
                    MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                    tempMatchedPart.PNOrItemName = matchedPart.Pn;
                    tempMatchedPart.CollectionData = item;
                    tempMatchedPart.ValueType = matchedPart.ValueType;
                    return tempMatchedPart;
                }


                return null;
        
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
                logger.Debug("(CombinePack)InputMBSn end, prodId:" + prodId + " item:" + item); 
            }

        }

        public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue)
        {
            return PartCollection.TryPartMatchCheck(sessionKey, TheType, checkValue);
        }

 
        //modify 1053 bug
        public void Save(string prodId, out string custsn)
        {
            logger.Debug("(CombinePack)Save start, prodId:" + prodId);
        //    custsn = "";
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            //if (prodId.Trim().Length == 10)
            //{ prodId = prodId.Trim().Substring(0, 9); }  
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
                Session.SwitchToWorkFlow();
                var prod = (IProduct)Session.GetValue(Session.SessionKeys.Product);

                custsn = "";

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
                if (e.mErrcode.ToString().Trim() == "GEN021")
                {
                    ex = new FisException("GEN047", erpara);
                    throw ex;
                }
                else
                {
                    throw e;
                }
           
            }
            catch (Exception e) 
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(CombinePack)Save end, prodId:" + prodId);
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

        #endregion
    }
}