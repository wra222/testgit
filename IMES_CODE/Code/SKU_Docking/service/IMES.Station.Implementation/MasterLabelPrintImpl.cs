﻿/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: OQCOutputImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-24   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using System.Workflow.Runtime;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using log4net;
using IMES.Route;
using IMES.DataModel;
using System.Collections;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IOQCOutput接口的实现类
    /// </summary>
    public class MasterLabelPrintImpl : MarshalByRefObject, MasterLabelPrint 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

        #region MasterLabelPrintImpl members

        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>prestation</returns>
        public ArrayList InputProdId(string pdLine, string prodId, IList<PrintItem> printItems, string editor, string stationId, string customer)
        {
            logger.Debug("(MasterLabelPrintImpl)InputProdId start, [pdLine]:" + pdLine
                + " [prodId]: " + prodId
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            //List<string> retvaluelist = new List<string>();
            ArrayList retvaluelist = new ArrayList();

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
                    RouteManagementUtils.GetWorkflow(stationId, "MasterLabelPrint.xoml", "masterlabelprint.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    //session.AddValue(Session.SessionKeys.PrintLogDescr, sessionKey);  //应该是model
 
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

                IProduct product = (IProduct)session.GetValue(Session.SessionKeys.Product);
                IList<ProductQCStatus> lstQCStatus = product.QCStatus;
                retvaluelist.Add(product.ProId);

                retvaluelist.Add(product.Status.StationId);
                IList<PrintItem> resultPrintItems = session.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;

                retvaluelist.Add(resultPrintItems);
                return retvaluelist; 

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
                logger.Debug("(MasterLabelPrintImpl)InputProdId end, [pdLine]:" + pdLine
                    + " [prodId]: " + prodId
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string prodId)
        {
            logger.Debug("(MasterLabelPrintImpl)Cancel start, [prodId]:" + prodId);
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
                logger.Debug("(MasterLabelPrintImpl)Cancel end, [prodId]:" + prodId);
            }
        }


        public ArrayList rePrint(string pdLine, string prodId, string reason, IList<PrintItem> printItems, string editor, string stationId, string customer)
        {
            logger.Debug("(MasterLabelPrintImpl)InputProdId start, [pdLine]:" + pdLine
                + " [prodId]: " + prodId
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retvaluelist = new ArrayList();

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
                    RouteManagementUtils.GetWorkflow(stationId, "MasterLabelrePrint.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.Reason, reason);
                    session.AddValue(Session.SessionKeys.PrintLogName, "MasterLabel");
                    
                   

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

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }


                IList<PrintItem> resultPrintItems = session.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;
                IList<string> Product = new List<string>();
                Product.Add((session.GetValue(Session.SessionKeys.Product) as IProduct).ProId);
                retvaluelist.Add(resultPrintItems);
                //retvaluelist.Add((session.GetValue(Session.SessionKeys.Product) as IProduct).ProId);
                //A24300534
                retvaluelist.Add(Product);
                return retvaluelist; 

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
                logger.Debug("(MasterLabelPrintImpl)rePrint end, [pdLine]:" + pdLine
                    + " [prodId]: " + prodId
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }


        private bool isJamestownConstValue(string val)
        {
            bool isFound = false;
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueTypeInfo> lstConstValueType = partRepository.GetConstValueTypeList("MasterLabelCheckSNFamily");
            if (null != lstConstValueType)
                foreach (ConstValueTypeInfo vi in lstConstValueType)
                {
                    if (val.Equals(vi.value))
                    {
                        isFound = true;
                        break;
                    }
                }
            return isFound;
        }

        private bool isCheckDDRCTConstValue(string val)
        {
            bool isFound = false;
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueTypeInfo> lstConstValueType = partRepository.GetConstValueTypeList("NoCheckDDRCT");
            if (null != lstConstValueType)
                foreach (ConstValueTypeInfo vi in lstConstValueType)
                {
                    if (val.Equals(vi.value))
                    {
                        isFound = true;
                        break;
                    }
                }
            return isFound;
        }

        /// <summary>
        /// CheckJamestown
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <returns>prestation</returns>
        public ArrayList CheckJamestown(
            string prodId,
            string pdLine, string editor, string stationId, string customer)
        {
            logger.Debug("(MasterLabelPrintImpl)CheckJamestown start, [pdLine]:" + pdLine
                + " [prodId]: " + prodId
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            //FisException ex;
            List<string> errpara = new List<string>();
            try
            {
                ArrayList ret = new ArrayList();

                bool isJamestownModel = false;
                //IProduct currentProduct = productRepository.Find(prodId);
                IProduct currentProduct = productRepository.GetProductByIdOrSn(prodId);
                if (currentProduct == null)
                {
                    errpara.Add(prodId);
                    throw new FisException("SFC002", errpara);
                }
                //if ("JC".Equals(currentProduct.Model.Substring(0, 2)))
				if (CommonImpl.GetInstance().CheckModelByProcReg(currentProduct.Model, "ThinClient"))
                    isJamestownModel = true;

                if (!isJamestownModel)
                {
                    if (isCheckDDRCTConstValue(currentProduct.Family))
                    {
                        ret.Add("notJC"); // Jamestown
                        ret.Add(""); // ConstValue
                        ret.Add(""); // model
                        ret.Add("notCheckDDRCT"); //DDRCT
                        return ret;
                    }
                    else
                    {
                        ret.Add("notJC");
                        ret.Add("");
                        ret.Add(currentProduct.Model);
                        ret.Add("isCheckDDRCT");
                        return ret;
                    }
                }

                if (isJamestownConstValue(currentProduct.Family))
                {
                    if (isCheckDDRCTConstValue(currentProduct.Family))
                    {
                        ret.Add("isJC");
                        ret.Add("isCheckSN");
                        ret.Add(currentProduct.Model);
                        ret.Add("notCheckDDRCT");
                        return ret;
                    }
                    else
                    {
                        ret.Add("isJC");
                        ret.Add("isCheckSN");
                        ret.Add(currentProduct.Model);
                        ret.Add("");
                        return ret;
                    }
                }
                else
                {
                    if (isCheckDDRCTConstValue(currentProduct.Family))
                    {
                        ret.Add("isJC");
                        ret.Add("notCheckSN");
                        ret.Add(currentProduct.Model);
                        ret.Add("notCheckDDRCT");
                        return ret;
                    }
                    else
                    {
                        ret.Add("isJC");
                        ret.Add("notCheckSN");
                        ret.Add(currentProduct.Model);
                        ret.Add("isCheckDDRCT");
                        return ret;
                    }
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
                logger.Debug("(MasterLabelPrintImpl)CheckJamestown end, [pdLine]:" + pdLine
                    + " [prodId]: " + prodId
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }


        /// <summary>
        /// CheckDDRCT
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="ddrct">ddrct</param>
        /// <param name="prodId">Product Id</param>
        /// <returns>prestation</returns>
        public ArrayList CheckDDRCT(
            string prodId, string ddrct,
            string pdLine, string editor, string stationId, string customer)
        {
            logger.Debug("(MasterLabelPrintImpl)CheckDDRCT start, [pdLine]:" + pdLine
                + " [prodId]: " + prodId
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
           // FisException ex;
            List<string> errpara = new List<string>();
            try
            {
                ArrayList ret = new ArrayList();

                bool existDDRCT = false;
                //bool isJamestownModel = false;
                IProduct currentProduct = productRepository.Find(prodId);
                if (currentProduct == null)
                {
                    errpara.Add(prodId);
                    throw new FisException("SFC002", errpara);
                }

                if (null != ddrct)
                {
                    IList<IProductPart> productParts = currentProduct.ProductParts;
                    foreach (ProductPart part in productParts)
                    {
                        if ("DDR".Equals(part.CheckItemType) && ddrct.Equals(part.PartSn))
                        {
                            existDDRCT = true;
                            break;
                        }
                    }
                }
                if (!existDDRCT)
                {
                    errpara.Add(prodId);
                    FisException e = new FisException("CHK1081", errpara); // DDR CT不匹配
                    throw e;
                }

                if (isJamestownConstValue(currentProduct.Family))
                {
                    ret.Add("");
                    ret.Add("isCheckSN");
                    ret.Add(currentProduct.Model);
                    return ret;
                }
                else
                {
                    ret.Add("");
                    ret.Add("notCheckSN");
                    ret.Add(currentProduct.Model);
                    return ret;
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
                logger.Debug("(MasterLabelPrintImpl)CheckDDRCT end, [pdLine]:" + pdLine
                    + " [prodId]: " + prodId
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// CheckCustsn
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="sn">sn</param>
        /// <returns>prestation</returns>
        public bool CheckCustsn(
            string prodId,
            string sn,
            string pdLine, string editor, string stationId, string customer)
        {
            logger.Debug("(MasterLabelPrintImpl)CheckCustsn start, [pdLine]:" + pdLine
                + " [prodId]: " + prodId
                + " [sn]: " + sn
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
           // FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                List<string> errpara = new List<string>();
                IProduct currentProduct = productRepository.Find(prodId);
                if (currentProduct == null)
                {
                    errpara.Add(prodId);
                    throw new FisException("SFC002", errpara);
                }

                if (!sn.Equals(currentProduct.CUSTSN))
                {
                    throw new FisException("CHK1014", errpara); // 请核对流程卡!
                }
                return true;
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
                logger.Debug("(MasterLabelPrintImpl)CheckCustsn end, [pdLine]:" + pdLine
                    + " [prodId]: " + prodId
                    + " [sn]: " + sn
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

		/// <summary>
        /// GetCustsn
        /// </summary>
        public string GetCustsn(
            string prodId,
            string pdLine, string editor, string stationId, string customer)
        {
            logger.Debug("(MasterLabelPrintImpl)GetCustsn start, [pdLine]:" + pdLine
                + " [prodId]: " + prodId
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            //FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                List<string> errpara = new List<string>();
                IProduct currentProduct = productRepository.Find(prodId);
                if (currentProduct == null)
                {
                    errpara.Add(prodId);
                    throw new FisException("SFC002", errpara);
                }

                return currentProduct.CUSTSN;
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
                logger.Debug("(MasterLabelPrintImpl)GetCustsn end, [pdLine]:" + pdLine
                    + " [prodId]: " + prodId
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        #endregion
    }
}
