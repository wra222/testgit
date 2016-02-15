/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Implementation for Generate Customer SN For Docking Page
* UI:CI-MES12-SPEC-FA-UI Generate Customer SN For Docking.docx –2012/5/18 
* UC:CI-MES12-SPEC-FA-UC Generate Customer SN For Docking.docx –2012/5/18           
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-05-18   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using IMES.Station.Interface.StationIntf;
using IMES.Docking.Interface.DockingIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Route;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using System.Data;

namespace IMES.Docking.Implementation
{
    /// <summary>
    /// IGenerateCustomerSNForDocking接口的实现类
    /// </summary>
    public class GenerateCustomerSNForDockingImpl : MarshalByRefObject, IGenerateCustomerSNForDocking 
    {
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IGenerateCustomerSNForDocking members

        /// <summary>
        /// 输入Product Id和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        public void InputProdId(string pdLine, string prodId, string editor, string stationId, string customer,
                                out IMES.DataModel.ProductModel curProduct)
        {
            logger.Debug("(GenerateCustomerSNForDockingImpl)InputProdId start, [pdLine]:" + pdLine
                + " [prodId]: " + prodId
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);

            try
            {
                DoWfGenerateCustomerSNForDocking(pdLine, prodId, editor, stationId, customer,
                    out curProduct, "");

                return;
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
                logger.Debug("(GenerateCustomerSNForDockingImpl)InputProdId end, [pdLine]:" + pdLine
                    + " [prodId]: " + prodId
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public IList<PrintItem> Print(string prodId, IList<PrintItem> printItems)
        {
            logger.Debug("(GenerateCustomerSNForDockingImpl)print start, [prodId]:" + prodId);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);
                Product curProduct = (Product)session.GetValue(Session.SessionKeys.Product);
  
                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                else
                {
                    session.AddValue(Session.SessionKeys.IsComplete, true);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    session.AddValue(Session.SessionKeys.PrintLogName, curProduct.Customer + "SNO");
                    /*
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, curProduct.CUSTSN);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, curProduct.CUSTSN);
                    session.AddValue(Session.SessionKeys.PrintLogDescr, curProduct.ProId);
                    */
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, curProduct.ProId);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, curProduct.ProId);
                    session.AddValue(Session.SessionKeys.PrintLogDescr, curProduct.CUSTSN);

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

                    IList<PrintItem> returnList = this.getPrintList(session);
                    return returnList;
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
                logger.Debug("(GenerateCustomerSNForDockingImpl)InputECR end, [prodId]:" + prodId);
            }
        }

        /// <summary>
        /// 重新列印Customer SN
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>Print Items</returns>
        public ArrayList Reprint(string prodId, string editor, string stationId, string customer, IList<PrintItem> printItems, string reason, out string customerSN)
        {
            logger.Debug("(GenerateCustomerSNForDockingImpl)Reprint start,"
                + " [prodId]: " + prodId
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            IMES.FisObject.FA.Product.IProduct product = null;

            try
            {
                ArrayList retList = new ArrayList();
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, ProductSessionType, editor, stationId, string.Empty, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);

                    //session.AddValue(Session.SessionKeys.PrintLogBegNo, prodId);
                    //session.AddValue(Session.SessionKeys.PrintLogEndNo, prodId);
                    session.AddValue(Session.SessionKeys.Reason, reason);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("GenerateCustomerSNForDockingReprint.xoml", null, wfArguments);

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

                product = (IMES.FisObject.FA.Product.IProduct)session.GetValue(Session.SessionKeys.Product);

                customerSN = (product == null ? string.Empty : product.CUSTSN);
                IList<PrintItem> printList = this.getPrintList(session);

                ProductModel curproc = new ProductModel();
                curproc.CustSN = product.CUSTSN;
                curproc.ProductID = product.ProId;
                curproc.Model = product.Model;

                retList.Add(printList);
                retList.Add(curproc);
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
                logger.Debug("(GenerateCustomerSNForDockingImpl)Reprint end,"
                    + " [prodId]: " + prodId
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

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

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string prodId)
        {
            logger.Debug("(GenerateCustomerSNForDockingImpl)Cancel start, [prodId]:" + prodId);
            FisException ex;
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
                logger.Debug("(GenerateCustomerSNForDockingImpl)Cancel end, [prodId]:" + prodId);                
            }
        }
		
		public string GetModelFamily(string model)
		{
			logger.Debug("(GenerateCustomerSNForDockingImpl)GetModelFamily start, [model]:" + model);
			FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
				Model curModel = modelRep.Find(model);
				if (null == curModel){
					erpara.Add(model);
                    ex = new FisException("CHK038", erpara);
                    throw ex;
				}
				string family = curModel.FamilyName;
				return family;
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
                logger.Debug("(GenerateCustomerSNForDockingImpl)GetModelFamily end, [model]:" + model);
            }
		}

        private void DoWfGenerateCustomerSNForDocking(string pdLine, string prodId, string editor, string stationId, string customer,
                                out IMES.DataModel.ProductModel curProduct, string newmodel)
		{
			FisException ex;
            List<string> erpara = new List<string>();
			string sessionKey = prodId;
			Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

			if (session == null)
			{
                if (!string.IsNullOrEmpty(newmodel))
                {
                    IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                    Model newModel = modelRep.Find(newmodel);
                    if (null == newModel)
                    {
                        erpara.Add(newmodel);
                        ex = new FisException("CHK038", erpara);
                        throw ex;
                    }
                    string family = newModel.FamilyName;

                    var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    IProduct currentProduct = productRepository.Find(prodId);
                    if (null == currentProduct)
                    {
                        erpara.Add(prodId);
                        ex = new FisException("CHK079", erpara);
                        throw ex;
                    }

                    Model curModel = modelRep.Find(currentProduct.Model);
                    if (null == curModel)
                    {
                        erpara.Add(currentProduct.Model);
                        ex = new FisException("CHK038", erpara);
                        throw ex;
                    }

                    if (!family.Equals(curModel.FamilyName))
                    {
                        ex = new FisException("DCK001", erpara); //Family不同，不能转换
                        throw ex;
                    }

                }

				// begin wf
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
				RouteManagementUtils.GetWorkflow(stationId, "GenerateCustomerSNForDocking.xoml", "GenerateCustomerSNForDocking.rules", out wfName, out rlName);
				WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                session.AddValue("ChangeToModel", newmodel);
				session.AddValue(Session.SessionKeys.IsComplete, false);
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

			Product tempProduct = (Product)session.GetValue(Session.SessionKeys.Product);
			//IMES.DataModel.ProductModel 
			curProduct = new IMES.DataModel.ProductModel();

			curProduct.CustSN = tempProduct.CUSTSN;
			curProduct.ProductID = tempProduct.ProId;
			curProduct.Model = tempProduct.Model;
		}
		
		public void InputProdIdAndChangeModel(string pdLine, string prodId, string editor, string stationId, string customer,
                                out IMES.DataModel.ProductModel curProduct, string newmodel)
        {
            logger.Debug("(GenerateCustomerSNForDockingImpl)InputProdIdAndChangeModel start, [pdLine]:" + pdLine
                + " [prodId]: " + prodId
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer
                + " [newmodel]:" + newmodel);
            
            try
            {
                DoWfGenerateCustomerSNForDocking(pdLine, prodId, editor, stationId, customer,
                    out curProduct, newmodel);

                return;
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
                logger.Debug("(GenerateCustomerSNForDockingImpl)InputProdIdAndChangeModel end, [pdLine]:" + pdLine
                    + " [prodId]: " + prodId
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer
                    + " [newmodel]:" + newmodel);
            }
        }

        #endregion
    }
}
